# api-financial-control


------------------------------------------------------------------------------------------------------------

## Arquitetura da Aplicação
![Diagrama da Arquitetura](Documentos/architecture.png)


## Solution Review
📌 [Visualizar em PDF](Documentos/documentation.pdf)

------------------------------------------------------------------------------------------------------------

## Detalhes dos serveless lambda:

------------------------------------------------------------------------------------------------------------
Lambda de relatório consolidado por data (lambda-consolidated-report) - consolidated-report/{date}
📌 Responsabilidade: Fornecer o saldo consolidado de um determinado dia.

1⃣  Receber a requisição via API Gateway

O API Gateway recebe a requisição com a data informada ({date}).
2⃣  Consultar os dados no DynamoDB (ConsolidatedReports table)

O Lambda ConsolidationReport é acionado para buscar o saldo consolidado dessa data.
A consulta no DynamoDB retorna:
Total de Créditos do dia.
Total de Débitos do dia.
Saldo final (Créditos - Débitos).

3⃣  Retornar a resposta formatada

A API retorna um JSON com os dados consolidados do dia solicitado.
Exemplo de resposta:
{
  "date": "2024-06-01",
  "total_credits": 10500.75,
  "total_debits": 5230.25,
  "final_balance": 5270.50
}

------------------------------------------------------------------------------------------------------------
Lambda de Processamento de Transações (lambda-transaction)
📌 Responsabilidade: Processar mensagens da fila sqs-transactions, validar e armazenar no DynamoDB.

Trigger: SQS (sqs-transactions).
Tarefas:
Ler a mensagem da fila.	
Validar os dados da transação (exemplo: valor positivo para crédito, negativo para débito).
Armazenar a transação no DynamoDB (Transactions table).
Enviar um evento para a fila sqs-consolidation para atualizar o saldo diário.
Registrar logs no Datadog para rastrear possíveis erros.

Armazenar os dados no DynamoDB (Transactions table)

Insere a transação na tabela com os seguintes atributos:
{
  "transaction_id": "uuid-12345",
  "date": "2024-06-01",
  "amount": 150.50,
  "type": "credit",
  "category": "sales"
}

------------------------------------------------------------------------------------------------------------
Lambda de Consolidação Diária (lambda-consolidation)
📌 Responsabilidade: Processar mensagens da fila sqs-consolidation e atualizar o saldo diário.

Trigger: SQS (sqs-consolidation).
Tarefas:
Ler a mensagem da fila.
Consultar o DynamoDB (Transactions table) para calcular o saldo diário.
Atualizar o DynamoDB (ConsolidatedReports table) com o saldo consolidado.
Registrar logs no Datadog para monitorar o tempo de processamento.

------------------------------------------------------------------------------------------------------------

Monitoramento e Logs no Datadog

Tempo de resposta e erros de consulta são monitorados via Datadog.
Se houver falha na consulta, um alerta é gerado.

🔥 Benefícios desse design
✔️ Escalável: Como os dados consolidados são pré-calculados, a consulta no DynamoDB é rápida.
✔️ Baixa Latência: API Gateway + DynamoDB garantem alta performance.
✔️ Monitorado e Seguro: Logs e métricas no Datadog + regras de acesso via AWS WAF.
------------------------------------------------------------------------------------------------------------
📌 Como Implantar a Solução nos ambientes (dev,hml e prd)

✅ 1. Pré-requisitos
Antes de executar o template, certifique-se de ter:

AWS CLI instalado e configurado (aws configure)
AWS SAM CLI instalado (sam --version)
Docker (necessário para testes locais)

sh
brew install aws/tap/aws-sam-cli   # macOS
choco install aws-sam-cli          # Windows

via pip Linux
pip install aws-sam-cli

✅ 2. Compilar e Construir a Aplicação
sam build

✅ 3. Testar a Aplicação Localmente (Opcional)
sam local start-api

Isso simula o API Gateway chamando os Lambdas. Agora, teste enviando uma requisição:

curl -X POST http://127.0.0.1:3000/api/transactions -H "Content-Type: application/json" -d '{"transactionId": "123", "amount": 100.00, "type": "credit"}'

Se estiver testando um Lambda que processa SQS, você pode simular uma invocação:

Criar um arquivo event.json com um exemplo de mensagem SQS:
{
  "Records": [
    {
      "messageId": "1",
      "body": "{\"transactionId\": \"123\", \"amount\": 100.00, \"type\": \"credit\"}"
    }
  ]
}

✅ 4. Implantar a Aplicação na AWS

4.1 Fazer o Deploy pela Primeira Vez
sam deploy --guided
ou
sam deploy --parameter-overrides Environment=dev

Esse comando solicita configurações interativas, incluindo:

Nome do Stack (Ex: financial-control-app)
Região da AWS (Ex: us-east-1)
Ambiente (Ex: dev, hml, prd)
Criar ou usar um bucket S3 para armazenar artefatos do deployment

4.2 Deploys Futuros
sam deploy


✅ 5. Verificar os Recursos Criados
aws cloudformation describe-stacks --stack-name financial-control-app

✅ 6. Testar a API Implantada

Agora que a aplicação está na AWS, envie uma requisição:

curl -X POST https://your-api-gateway-url.amazonaws.com/dev/api/transactions -H "Content-Type: application/json" -d '{"transactionId": "123", "amount": 100.00, "type": "credit"}'

Se for um Lambda com trigger do SQS, envie uma mensagem para a fila:
aws sqs send-message --queue-url https://sqs.us-east-1.amazonaws.com/123456789012/sqs-transactions-dev --message-body '{"transactionId": "123", "amount": 100.00, "type": "credit"}'

E veja os logs da execução no CloudWatch:
aws logs describe-log-groups
aws logs tail /aws/lambda/SetTransactionFunction


------------------------------------------------------------------------------------------------------------

## Para teste local no visual studio

✔️ Configurar o projeto MultipleLambdas como set as startup project

✔️ Configurando o dynamodb local
no root do repositório existe a pasta dynamodb_local

abrir a pasta com o cmd e executar os seguintes comandos:

--- inicia o dynamodb local
java -Djava.library.path=./DynamoDBLocal_lib -jar DynamoDBLocal.jar -sharedDb

abrir outro cmd e execuar os comando abaixos para criar as tabelas:

aws dynamodb create-table --cli-input-json file://create_table_ConsolidatedReport.json --endpoint-url http://localhost:8000
aws dynamodb create-table --cli-input-json file://create_table_Transaction.json --endpoint-url http://localhost:8000


