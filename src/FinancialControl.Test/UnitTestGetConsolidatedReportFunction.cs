﻿using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using FluentAssertions;
using Newtonsoft.Json;
using FinancialControl.Domain.Consolidate;
using MediatR;
using MultipleLambdas;
using LambdaLogger;
using FinancialControl.Application.Queries;

public class GetConsolidatedReportFunctionTests
{
	private readonly Mock<ILogger> _loggerMock;
	private readonly Mock<IMediator> _mediatorMock;
	private readonly GetConsolidatedReportFunction _function;

	public GetConsolidatedReportFunctionTests()
	{
		_loggerMock = new Mock<ILogger>();
		_mediatorMock = new Mock<IMediator>();
	}

	[Fact]
	public async Task FunctionHandler_ShouldReturnOk_WhenValidDateIsProvided()
	{
		// Arrange
		var request = new APIGatewayProxyRequest
		{
			QueryStringParameters = new Dictionary<string, string> { { "date", "2024-06-01" } }
		};
		var context = new Mock<ILambdaContext>();

		var expectedResponse = new ConsolidatedReport
		{
			Date = "2024-06-01",
			TotalCredits = 10500.75m,
			TotalDebits = 5230.25m,
		};

		//_mediatorMock
		//	.Setup(m => m.Send(It.IsAny<GetConsolidatedReportQuery>(), default))
		//	.ReturnsAsync(expectedResponse);

		// Act
		var response = await _function.FunctionHandler(request, context.Object);

		// Assert
		response.StatusCode.Should().Be((int)HttpStatusCode.OK);
		response.Body.Should().NotBeNull();

		var result = JsonConvert.DeserializeObject<ConsolidatedReport>(response.Body);
		result.Should().BeEquivalentTo(expectedResponse);

	}

	[Fact]
	public async Task FunctionHandler_ShouldReturnInternalServerError_WhenExceptionOccurs()
	{
		// Arrange
		var request = new APIGatewayProxyRequest
		{
			QueryStringParameters = new Dictionary<string, string> { { "date", "2024-06-01" } }
		};
		var context = new Mock<ILambdaContext>();

		_mediatorMock
			.Setup(m => m.Send(It.IsAny<GetConsolidatedReportQuery>(), default))
			.ThrowsAsync(new Exception("Database connection failed"));

		// Act
		var response = await _function.FunctionHandler(request, context.Object);

		// Assert
		response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
		response.Body.Should().BeNullOrEmpty();

	}

	[Fact]
	public async Task FunctionHandler_ShouldReturnOk_WithDefaultDate_WhenNoDateProvided()
	{
		// Arrange
		var request = new APIGatewayProxyRequest
		{
			QueryStringParameters = new Dictionary<string, string>() // Sem `date` nos parâmetros
		};
		var context = new Mock<ILambdaContext>();

		var expectedResponse = new ConsolidatedReport
		{
			Date = "0001-01-01", // Default do DateTime.MinValue
			TotalCredits = 0,
			TotalDebits = 0
		};

		//_mediatorMock
		//	.Setup(m => m.Send(It.IsAny<GetConsolidatedReportQuery>(), default))
		//	.ReturnsAsync(expectedResponse);

		// Act
		var response = await _function.FunctionHandler(request, context.Object);

		// Assert
		response.StatusCode.Should().Be((int)HttpStatusCode.OK);
		response.Body.Should().NotBeNull();

		var result = JsonConvert.DeserializeObject<ConsolidatedReport>(response.Body);
		result.Should().BeEquivalentTo(expectedResponse);

	}

	[Fact]
	public async Task FunctionHandler_ShouldReturnInternalServerError_WhenRequestIsNull()
	{
		// Arrange
		var context = new Mock<ILambdaContext>();

		// Act
		var response = await _function.FunctionHandler(null, context.Object);

		// Assert
		response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
		response.Body.Should().BeNullOrEmpty();

	}
}
