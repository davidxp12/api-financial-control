using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.Runtime.Internal.Util;
using FinancialControl.Application.Commands;
using FinancialControl.Domain.BaseTypes;
using FinancialControl.Domain.Helper;
using FinancialControl.Domain.Queue;
using FluentAssertions;
using LambdaLogger;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using MultipleLambdas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialControl.Test
{
	public class UnitTestSetTransactionFuncion
	{
		private readonly Mock<LambdaLogger.ILogger> _loggerMock;
		private readonly Mock<IMediator> _mediatorMock;
		private readonly SetTransactionFuncion _function;
		private readonly Mock<ISQSManager> _sqsManagerMock;
		private readonly Mock<IConfigurationManager> _configManagerMock;

		public UnitTestSetTransactionFuncion()
		{
			Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "dev");
			_mediatorMock = new Mock<IMediator>();
			_function = new SetTransactionFuncion();
			_sqsManagerMock = new Mock<ISQSManager>();
			_configManagerMock = new Mock<IConfigurationManager>();

			_function = new SetTransactionFuncion();
		}
		[Fact]
		public async Task FunctionHandler_ShouldProcessAllMessages()
		{
			// Arrange
			var context = LambdaHelper.LambdaTestBuildContext();

			var sqsEvent = new SQSEvent
			{
				Records = new List<SQSEvent.SQSMessage>
			{
				new SQSEvent.SQSMessage
				{
					MessageId = "1",
					Body = JsonConvert.SerializeObject(new RegisterTransactionCommand(DateTime.Now, 100, "test" )),
					ReceiptHandle = "handle1"
				},
				new SQSEvent.SQSMessage
				{
					MessageId = "2",
					Body = JsonConvert.SerializeObject(new RegisterTransactionCommand( DateTime.Now, -50, "test" )),
					ReceiptHandle = "handle2"
				}
			}
			};


			// Act
			await Task.Run(() => _function.FunctionHandler(sqsEvent, context));

			// Assert
			_mediatorMock.Verify(m => m.Send(It.IsAny<RegisterTransactionCommand>(), default), Times.Exactly(2));
			_sqsManagerMock.Verify(m => m.DeleteMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
			
		}

		[Fact]
		public async Task ProcessMessage_ShouldReturnTrue_WhenMessageProcessedSuccessfully()
		{
			// Arrange
			var context = LambdaHelper.LambdaTestBuildContext();

			var message = new SQSEvent.SQSMessage
			{
				MessageId = "1",
				Body = JsonConvert.SerializeObject(new RegisterTransactionCommand(DateTime.Now, 100, "test")),
				ReceiptHandle = "handle1"
			};

			_mediatorMock
				.Setup(m => m.Send(It.IsAny<RegisterTransactionCommand>(), default))
				.ReturnsAsync(true);

			_sqsManagerMock
				.Setup(m => m.DeleteMessage(It.IsAny<string>(), It.IsAny<string>()))
				.Verifiable();

			// Act
			var result = await _function.ProcessMessage(message, context);

			// Assert
			result.Should().BeTrue();
			_sqsManagerMock.Verify(m => m.DeleteMessage(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			
		}

		[Fact]
		public async Task ProcessMessage_ShouldReturnFalse_WhenExceptionOccurs()
		{
			// Arrange
			var context = LambdaHelper.LambdaTestBuildContext();

			var message = new SQSEvent.SQSMessage
			{
				MessageId = "2",
				Body = JsonConvert.SerializeObject(new RegisterTransactionCommand(DateTime.Now, 550, "test")),
				ReceiptHandle = "handle2"
			};

			_mediatorMock
				.Setup(m => m.Send(It.IsAny<RegisterTransactionCommand>(), default))
				.ThrowsAsync(new Exception("Database error"));

			// Act
			var result = await _function.ProcessMessage(message, context);

			// Assert
			result.Should().BeFalse();
			
		}

		[Fact]
		public async Task FunctionHandler_ShouldHandleEmptySqsEvent()
		{
			// Arrange
			var context = LambdaHelper.LambdaTestBuildContext();
			var sqsEvent = new SQSEvent { Records = new List<SQSEvent.SQSMessage>() };

			// Act
			await Task.Run(() => _function.FunctionHandler(sqsEvent, context));

			// Assert
			_mediatorMock.Verify(m => m.Send(It.IsAny<RegisterTransactionCommand>(), default), Times.Never);
			
		}
	}
}
