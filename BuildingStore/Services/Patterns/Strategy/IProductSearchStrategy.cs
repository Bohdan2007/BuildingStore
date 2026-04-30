using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public interface IProductSearchStrategy
    {
        List<Product> Search(List<Product> allProducts, string productName, int? categoryId);
    }
}
