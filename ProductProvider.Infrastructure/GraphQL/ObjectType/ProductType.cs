using ProductProvider.Infrastructure.Data.Entities;

namespace ProductProvider.Infrastructure.GraphQL.ObjectType;

public class ProductType : ObjectType<ProductEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ProductEntity> descriptor)
    {
        descriptor.Field(p => p.Id).Type<NonNullType<IdType>>(); // Primary key for Cosmos DB
        descriptor.Field(p => p.Title).Type<StringType>();
        descriptor.Field(p => p.Brand).Type<StringType>();
        descriptor.Field(p => p.Size).Type<StringType>();
        descriptor.Field(p => p.Color).Type<StringType>();
        descriptor.Field(p => p.Price).Type<DecimalType>();
        descriptor.Field(p => p.Description).Type<StringType>();
        descriptor.Field(p => p.StockStatus).Type<BooleanType>();
        descriptor.Field(p => p.SKU).Type<StringType>();
        descriptor.Field(p => p.Ratings).Type<DecimalType>();
        descriptor.Field(p => p.ProductImage).Type<StringType>();

        // Directly reference Categories and Materials without intermediate classes
        descriptor.Field(p => p.Categories).Type<ListType<CategoryType>>().Name("categories");
        descriptor.Field(p => p.Materials).Type<ListType<MaterialType>>().Name("materials");
    }

    public class CategoryType : ObjectType<CategoryEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<CategoryEntity> descriptor)
        {
            descriptor.Field(c => c.CategoryName).Type<StringType>();
            descriptor.Field(c => c.SubCategories).Type<ListType<CategoryType>>();
        }
    }

    public class MaterialType : ObjectType<MaterialEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<MaterialEntity> descriptor)
        {
            descriptor.Field(m => m.MaterialName).Type<StringType>();
        }
    }
}
