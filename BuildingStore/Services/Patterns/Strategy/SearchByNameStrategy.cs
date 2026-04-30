using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public class SearchByNameStrategy : IProductSearchStrategy
    {
        public List<Product> Search(List<Product> products, string searchName, int? categoryId)
        {
            return products.Where(p => p.Name.ToLower().Contains(searchName.ToLower())).ToList();
        }
    }
}
