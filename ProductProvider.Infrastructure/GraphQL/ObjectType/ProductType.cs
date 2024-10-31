using ProductProvider.Infrastructure.Data.Entities;
using HotChocolate.Types;

namespace ProductProvider.Infrastructure.GraphQL.ObjectType;

public class ProductType : ObjectType<ProductEntity>
{
    protected override void Configure(IObjectTypeDescriptor<ProductEntity> descriptor)
    {
        descriptor.Field(p => p.ProductID).Type<NonNullType<IdType>>();
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
        descriptor.Field(p => p.Categories).Type<ListType<CategoryType>>();
        descriptor.Field(p => p.Materials).Type<ListType<MaterialType>>();
    }

    public class CategoryType : ObjectType<CategoryEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<CategoryEntity> descriptor)
        {
            descriptor.Field(c => c.CategoryID).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.CategoryName).Type<StringType>();
        }
    }

    public class MaterialType : ObjectType<MaterialEntity>
    {
        protected override void Configure(IObjectTypeDescriptor<MaterialEntity> descriptor)
        {
            descriptor.Field(m => m.MaterialID).Type<NonNullType<IntType>>();
            descriptor.Field(m => m.MaterialName).Type<StringType>();
        }
    }
}
