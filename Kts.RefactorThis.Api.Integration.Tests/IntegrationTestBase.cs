using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using AutoMapper;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using AutoMapper.Configuration;

using Kts.RefactorThis.Common;
using Kts.RefactorThis.DataAccess;
using Kts.RefactorThis.Api.Payloads;

namespace Kts.RefactorThis.Api.Integration.Tests
{
    [TestFixture]
    public class IntegrationTestBase
    {
        protected IMapper _mapper;
        protected AppConfiguration _appConfig = new AppConfiguration();
        protected PaginationParams _pagination = new PaginationParams();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<MappingProfileApi>();
            cfg.AddProfile<MappingProfileApplication>();
            var mapperConfig = new MapperConfiguration(cfg);
            _mapper = new Mapper(mapperConfig);
        }

        protected IFixture BuildFixture(bool configureMembers = true,
                                        AppConfiguration appConfig = null)
        {
            var cust = new AutoMoqCustomization { ConfigureMembers = configureMembers };
            var fixture = new Fixture().Customize(cust);
            fixture.Inject(_mapper);
            fixture.Inject(appConfig ?? _appConfig);
            return fixture;
        }

        protected void SetupDapperExecute(Mock<IConnectionProxy> connectionProxy, string sql, int valueToReturn)
        {
            connectionProxy.Setup(proxy => proxy.ExecuteAsync(It.Is<string>(s => s == sql),
                                                              It.IsAny<object>(),
                                                              It.Is<IDbTransaction>((v) => v == null || v is IDbTransaction)))
                           .Returns(Task.FromResult(valueToReturn));
        }

        // TODO: Fix setup for sql param = null to avoid false positives
        // Uses a query but returns the Guid
        protected void SetupDapperCreate(Mock<IConnectionProxy> connectionProxy, 
                                                        string sql, IEnumerable<Guid> valuesToReturn)
        {
            connectionProxy.Setup(proxy => proxy.CreateAsync(sql, It.IsAny<object>(), 
                                        It.Is<IDbTransaction>((v) => v == null || v is IDbTransaction)))
                           .Returns(Task.FromResult(valuesToReturn));
        }

        protected void SetupDapperQuery<T>(Mock<IConnectionProxy> connectionProxy,
                                           string sql, IEnumerable<T> valuesToReturn)
        {
            connectionProxy.Setup(proxy => proxy.QueryAsync<T>(sql, It.IsAny<object>()))
                           .Returns(Task.FromResult(valuesToReturn));
        }

        protected void SetupDapperQueryFirst<T>(Mock<IConnectionProxy> connectionProxy, 
                                                string sql, T valueToReturn)
        {
            connectionProxy.Setup(proxy => proxy.QueryFirstAsync<T>(sql, It.IsAny<object>()))
                           .Returns(Task.FromResult(valueToReturn));
        }

        protected void SetupDapperQueryFirstOrDefault<T>(Mock<IConnectionProxy> connectionProxy, 
                                                         string sql, T valueToReturn, T defaultWhenNotFound = default(T))
        {
            connectionProxy.Setup(proxy => proxy.QueryFirstOrDefaultAsync<T>(sql, It.IsAny<object>()))
                           .Returns(Task.FromResult(valueToReturn));
        }

    }
}

