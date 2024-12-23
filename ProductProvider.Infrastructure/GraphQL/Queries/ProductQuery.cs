﻿using ProductProvider.Infrastructure.Models;
using ProductProvider.Infrastructure.Services;

namespace ProductProvider.Infrastructure.GraphQL.Queries;

public class ProductQuery(IProductService productService)
{
    private readonly IProductService _productService = productService;

    [GraphQLName("getProducts")]
    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _productService.GetProductsAsync();
    }

    [GraphQLName("getProductById")]
    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _productService.GetProductByIdAsync(id);
    }
}
