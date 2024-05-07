using AutoFixture;
using Moonpig.PostOffice.Data.Entities;
using Moonpig.PostOffice.Data.Interfaces;
using Moq;
using Shouldly;

namespace Moonpig.PostOffice.Data.Tests
{
    public class DataProvidersTests
    {
        private Fixture _fixture;
        private readonly Mock<IDbContext> _mockDbContext;

        private readonly DataProvider _sut;

        public DataProvidersTests()
        {
            _fixture = new();
            var rep = new MockRepository(MockBehavior.Strict);
            _mockDbContext = rep.Create<IDbContext>();

            _sut = new(_mockDbContext.Object);
        }

        [Fact]
        public void GivenProductExists_ShouldReturnEntity()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var entity = _fixture
                .Build<Product>()
                .With(x => x.ProductId, id)
                .Create();

            var entities = new List<Product> { entity }.AsQueryable();

            _mockDbContext
                .Setup(x => x.Products)
                .Returns(entities);

            // Act
            var result = _sut.GetProduct(id);

            // Assert
            result.ShouldBe(entity);
        }

        [Fact]
        public void GivenProductDoesntExists_ShouldReturnNull()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var entities = _fixture.CreateMany<Product>().AsQueryable();

            _mockDbContext
                .Setup(x => x.Products)
                .Returns(entities);

            // Act
            var result = _sut.GetProduct(id);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GivenSupplierExists_ShouldReturnEntity()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var entity = _fixture
                .Build<Supplier>()
                .With(x => x.SupplierId, id)
                .Create();

            var entities = new List<Supplier> { entity }.AsQueryable();

            _mockDbContext
                .Setup(x => x.Suppliers)
                .Returns(entities);

            // Act
            var result = _sut.GetSupplier(id);

            // Assert
            result.ShouldBe(entity);
        }

        [Fact]
        public void GivenSupplierDoesntExists_ShouldReturnNull()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var entities = _fixture.CreateMany<Supplier>().AsQueryable();

            _mockDbContext
                .Setup(x => x.Suppliers)
                .Returns(entities);

            // Act
            var result = _sut.GetSupplier(id);

            // Assert
            result.ShouldBeNull();
        }
    }
}