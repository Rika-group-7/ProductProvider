using ProductProvider.Infrastructure.Models;
using ProductProvider.Infrastructure.Services;

namespace ProductProvider.Infrastructure.GraphQL.Mutations;

public class ProductMutation(IProductService productService)
{
    private readonly IProductService _productService = productService;

    [GraphQLName("createProduct")]
    public async Task<Product> CreateProductAsync(ProductCreateRequest input)
    {
        return await _productService.CreateProductAsync(input);
    }

    [GraphQLName("updateProduct")]
    public async Task<Product> UpdateProductAsync(ProductUpdateRequest input)
    {
        return await _productService.UpdateProductAsync(input);
    }

    [GraphQLName("deleteProduct")]
    public async Task<bool> DeleteProductAsync(string id)
    {
        return await _productService.DeleteProductAsync(id);
    }
}
