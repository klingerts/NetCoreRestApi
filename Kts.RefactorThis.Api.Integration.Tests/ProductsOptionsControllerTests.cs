using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentAssertions;
using AutoFixture;

using Kts.RefactorThis.DTO;
using Kts.RefactorThis.Application.Queries;
using Kts.RefactorThis.Api.Payloads;
using Kts.RefactorThis.Api.Controllers;
using Kts.RefactorThis.DataAccess.QueryHandlers;
using Kts.RefactorThis.Application.Abstractions;
using Kts.RefactorThis.Application.Services;
using Kts.RefactorThis.DataAccess.Repositories;
using Kts.RefactorThis.DataAccess;
using Kts.RefactorThis.Application.Validators;

namespace Kts.RefactorThis.Api.Integration.Tests
{
    [TestFixture]
    public class ProductsOptionsControllerTests : IntegrationTestBase
    {
        private ProductsOptionsController SetupSut(IFixture fixture, IConnectionProxy connectionProxy)
        {
            fixture.Inject<IConnectionProxy>(connectionProxy);
            fixture.Inject<IValidator<CreateProductOptionDTO>>(fixture.Create<CreateProductOptionValidator>());
            fixture.Inject<IValidator<UpdateProductOptionDTO>>(fixture.Create<UpdateProductOptionValidator>());

            fixture.Inject<IProductQueryHandler>(fixture.Create<ProductQueryHandler>());
            fixture.Inject<IProductOptionQueryHandler>(fixture.Create<ProductOptionQueryHandler>());

            fixture.Inject<IProductOptionRepository>(fixture.Create<ProductOptionRepository>());
            fixture.Inject<IProductOptionService>(fixture.Create<ProductOptionService>());
            var sut = fixture.Build<ProductsOptionsController>().Create();
            return sut;
        }

        [Test]
        public async Task Given_ThereAreNoOptionForProduct_WhenGetIsInvoked_ReturnsOkResult_WithAnEmptyListOfOptions()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var fixture = BuildFixture();
            var productExpected = fixture.Create<QueryProductDTO>();
            var expectedList = new List<QueryProductOptionDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQuery(connectionProxy, SQLStatements.GetProductOptionByProductId, expectedList);
            SetupDapperQueryFirstOrDefault(connectionProxy, SQLStatements.GetProductById, productExpected);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.GetByProductId(productId, _pagination);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var productOptionsFound = (result as OkObjectResult).Value as OutboundProductOptions;

            productOptionsFound.Items.Should().BeEmpty();
        }

        [Test]
        public async Task Given_ProductOptionDoesNotExistForProductId_WhenGetIsInvoked_ReturnsNotFound_WithProductId()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var fixture = BuildFixture();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQuery<QueryProductOptionDTO>(connectionProxy, SQLStatements.GetProductOptionByProductId, null);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.GetByProductId(productId, _pagination);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            dynamic output = (result as NotFoundObjectResult)?.Value;
            var id = (Guid)output.GetType().GetProperty("id").GetValue(output, null);

            id.Should().Be(productId);
        }

        [Test]
        public async Task Given_ThereAreProductOptionsForProduct_WhenGetIsInvokedWithValidProductId_ReturnsOkResult_WithListOfProductOptions()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var fixture = BuildFixture();
            var expectedList = fixture.CreateMany<QueryProductOptionDTO>();
            var productExpected = fixture.Create<QueryProductDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQuery(connectionProxy, SQLStatements.GetProductOptionByProductId, expectedList);
            SetupDapperQueryFirstOrDefault(connectionProxy, SQLStatements.GetProductById, productExpected);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.GetByProductId(productId, _pagination);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var productOptionsFound = (result as OkObjectResult)?.Value as OutboundProductOptions;

            productOptionsFound.Should().BeOfType<OutboundProductOptions>();
            productOptionsFound.Items.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_ProductOptionExists_WhenGetByIdIsInvokedWithValidId_ReturnsOkResult_WithProductOption()
        {
            // Arrange            
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            var fixture = BuildFixture();
            var expectedProduct = fixture.Create<QueryProductOptionDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQueryFirstOrDefault(connectionProxy, SQLStatements.GetProductOptionByIdAndProductId, expectedProduct);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.GetByProductIdAndOptionId(productId, optionId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var output = (result as OkObjectResult)?.Value as OutboundProductOption;

            output.Should().BeOfType<OutboundProductOption>();
        }

        [Test]
        public async Task Given_ProductOptionDoesNotExist_WhenPostIsInvokedWithValidValues_ReturnsOkResult_WithNewId()
        {
            // Arrange       
            var productId = Guid.NewGuid();
            var newId = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundValues = fixture.Create<InboundProductOption>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperCreate(connectionProxy, SQLStatements.CreateProductOption, new[] { newId });

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Post(productId, inboundValues);


            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            dynamic output = (result as CreatedAtActionResult)?.Value;
            var optionId = (Guid)output.GetType().GetProperty("optionId").GetValue(output, null);

            optionId.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_ProductOptionDoesNotExist_WhenPostIsInvokedWithInvalidValues_ReturnsBadRequest()
        {
            // Arrange       
            var productId = Guid.NewGuid();
            var newId = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundValues = fixture.Create<InboundProductOption>();
            inboundValues.Name = null;
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperCreate(connectionProxy, SQLStatements.CreateProductOption, new[] { newId });

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Post(productId, inboundValues);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_ProductOptionExists_WhenPutIsInvokedWithValidDataAndId_ReturnsNoContent()
        {
            // Arrange     
            var productId = Guid.NewGuid();
            var idToUpdate = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundValues = fixture.Create<InboundProductOption>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.UpdateProductOptionById, 1);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.Put(productId, idToUpdate, inboundValues)) as NoContentResult;

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task Given_ProductOptionExists_WhenPutIsInvokedWithInvalidData_ReturnsBadRequest()
        {
            // Arrange  
            var productId = Guid.NewGuid();
            var idToUpdate = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundValues = fixture.Create<InboundProductOption>();
            inboundValues.Name = null;
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.UpdateProductOptionById, 1);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Put(productId, idToUpdate, inboundValues);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_ProductOptionExists_WhenDeleteIsInvokedWithValidId_ReturnsNoContent()
        {
            // Arrange   
            var productId = Guid.NewGuid();
            var idToDelete = Guid.NewGuid();
            var fixture = BuildFixture();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.DeleteProductOptionByIdAndProductId, 1);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Delete(productId, idToDelete);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task Given_ProductOptionDoesNotExists_WhenDeleteIsInvoked_ReturnsNotFound()
        {
            // Arrange     
            var productId = Guid.NewGuid();
            var idToDelete = Guid.NewGuid();
            var fixture = BuildFixture();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.DeleteProductOptionByIdAndProductId, 0);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Delete(productId, idToDelete) as NotFoundObjectResult;

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            dynamic output = (result as NotFoundObjectResult)?.Value;
            var id = (Guid)output.GetType().GetProperty("id").GetValue(output, null);
            var optionId = (Guid)output.GetType().GetProperty("optionId").GetValue(output, null);

            id.Should().Be(productId);
            optionId.Should().Be(idToDelete);
        }
    }
}
