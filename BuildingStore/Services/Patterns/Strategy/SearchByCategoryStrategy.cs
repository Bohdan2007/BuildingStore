using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public class SearchByCategoryStrategy : IProductSearchStrategy
    {
        public List<Product> Search(List<Product> products, string searchName, int? categoryId)
        {
            return products.Where(p => p.CategoryId == categoryId).ToList();
        }
    }
}
