using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProductsControllerTests : IntegrationTestBase
    {
        private ProductsController SetupSut(IFixture fixture, IConnectionProxy connectionProxy)
        {
            fixture.Inject<IConnectionProxy>(connectionProxy);
            fixture.Inject<IValidator<CreateProductDTO>>(fixture.Create<CreateProductValidator>());
            fixture.Inject<IValidator<UpdateProductDTO>>(fixture.Create<UpdateProductValidator>());
            fixture.Inject<IProductQueryHandler>(fixture.Create<ProductQueryHandler>());
            fixture.Inject<IProductRepository>(fixture.Create<ProductRepository>());
            fixture.Inject<IProductService>(fixture.Create<ProductService>());
            var sut = fixture.Build<ProductsController>().Create();
            return sut;
        }

        [Test]
        public async Task Given_ThereAreNoProducts_WhenGetIsInvoked_ReturnsOkResult_WithAnEmptyListOfProducts()
        {
            // Arrange
            var fixture = BuildFixture();
            var expectedList = new List<QueryProductDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQuery(connectionProxy, SQLStatements.GetAllProducts, expectedList);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.Get(null, _pagination)) as OkObjectResult;
            var productsFound = result?.Value as OutboundProducts;

            // Assert
            productsFound.Items.Should().BeEmpty();
        }

        [Test]
        public async Task Given_ThereAreProducts_WhenGetIsInvokedWithoutName_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var fixture = BuildFixture();
            var expectedList = fixture.CreateMany<QueryProductDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQuery(connectionProxy, SQLStatements.GetAllProducts, expectedList);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.Get(null, _pagination)) as OkObjectResult;
            var productsFound = result?.Value as OutboundProducts;

            // Assert
            productsFound.Items.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_ThereAreProducts_WhenGetIsInvokedWithName_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            string nameToGet = "aname";
            var fixture = BuildFixture();
            var expectedList = fixture.CreateMany<QueryProductDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQuery(connectionProxy, SQLStatements.GetProductsByName, expectedList);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.Get(nameToGet, _pagination)) as OkObjectResult;
            var productsFound = (result?.Value as OutboundProducts).Items;
            // Should get the same items
            var count = expectedList.Where(dto => productsFound.Any(outbound => dto.Name == outbound.Name)).Count();

            // Assert
            count.Should().Be(expectedList.Count());
        }

        [Test]
        public async Task Given_ProductExists_WhenGetByIdIsInvokedWithValidId_ReturnsOkResult_WithProduct()
        {
            // Arrange            
            var fixture = BuildFixture();
            var expectedProduct = fixture.Create<QueryProductDTO>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperQueryFirstOrDefault(connectionProxy, SQLStatements.GetProductById, expectedProduct);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.GetById(Guid.NewGuid())) as OkObjectResult;
            var output = result?.Value as OutboundProduct;

            // Assert
            output.Should().NotBeNull();
        }

        [Test]
        public async Task Given_ProductDoesNotExist_WhenPostIsInvokedWithValidValues_ReturnsOkResult_WithNewId()
        {
            // Arrange       
            var newId = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundProduct = fixture.Create<InboundProduct>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperCreate(connectionProxy, SQLStatements.CreateProduct, new[] { newId });

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.Post(inboundProduct)) as CreatedAtActionResult;
            dynamic output = result?.Value;
            var id = (Guid)output.GetType().GetProperty("id").GetValue(output, null);

            // Assert
            id.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_ProductDoesNotExist_WhenPostIsInvokedWithInvalidValues_ReturnsBadRequest()
        {
            // Arrange       
            var newId = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundProduct = fixture.Create<InboundProduct>();
            inboundProduct.Name = null;
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperCreate(connectionProxy, SQLStatements.CreateProduct, new[] { newId });

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Post(inboundProduct);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_ProductExists_WhenPutIsInvokedWithValidDataAndId_ReturnsNoContent()
        {
            // Arrange     
            var idToUpdate = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundProduct = fixture.Create<InboundProduct>();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.UpdateProductById, 1);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Put(idToUpdate, inboundProduct);

            // Assert
            result.Should().BeOfType<NoContentResult>();

        }

        [Test]
        public async Task Given_ProductExists_WhenPutIsInvokedWithInvalidData_ReturnsBadRequest()
        {
            // Arrange     
            var idToUpdate = Guid.NewGuid();
            var fixture = BuildFixture();
            var inboundProduct = fixture.Create<InboundProduct>();
            inboundProduct.Name = null;
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.UpdateProductById, 1);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Put(idToUpdate, inboundProduct);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task Given_ProductExists_WhenDeleteIsInvokedWithValidId_ReturnsNoContent()
        {
            // Arrange     
            var idToDelete = Guid.NewGuid();
            var fixture = BuildFixture();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.DeleteProductById, 1);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = (await sut.Delete(idToDelete)) as NoContentResult;

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task Given_ProductDoesNotExists_WhenDeleteIsInvoked_ReturnsNotFound()
        {
            // Arrange     
            var idToDelete = Guid.NewGuid();
            var fixture = BuildFixture();
            var connectionProxy = new Mock<IConnectionProxy>();
            SetupDapperExecute(connectionProxy, SQLStatements.DeleteProductById, 0);

            var sut = SetupSut(fixture, connectionProxy.Object);

            // Act
            var result = await sut.Delete(idToDelete);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
            dynamic output = ((NotFoundObjectResult)result).Value;
            var id = (Guid)output.GetType().GetProperty("id").GetValue(output, null);

            id.Should().Be(idToDelete);
        }
    }
}
