using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public class CombinedSearchStrategy : IProductSearchStrategy
    {
        public List<Product> Search(List<Product> products, string searchName, int? categoryId)
        {
            return products.Where(p => p.CategoryId == categoryId && p.Name.ToLower().Contains(searchName.ToLower())).ToList();
        }
    }
}
