using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public class DefaultSearchStrategy : IProductSearchStrategy
    {
        public List<Product> Search(List<Product> products, string searchName, int? categoryId)
        {
            return products; 
        }
    }
}
