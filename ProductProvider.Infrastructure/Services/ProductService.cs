using Microsoft.EntityFrameworkCore;
using ProductProvider.Infrastructure.Data.Context;

namespace ProductProvider.Infrastructure.Services;

public class ProductService(IDbContextFactory<DataContext> contextFactory)
{
}
