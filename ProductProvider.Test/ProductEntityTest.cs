using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Entities;
using ProductProvider.Test.Data;

namespace ProductProvider.Test
{
    public class ProductEntityTest
    {
        private TestDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new TestDbContext(options);
        }





        [Fact]
        public void CanAddProduct()
        {

        }
    }
}