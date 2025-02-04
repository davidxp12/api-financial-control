# api-financial-control

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