using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public class ProductSearchContext
    {
        private IProductSearchStrategy strategy;

        public void SetStrategy(IProductSearchStrategy strategy)
        {
            this.strategy = strategy;
        }
        public List<Product> ExecuteSearch(List<Product> products, string searchName, int? categoryId)
        {
            return strategy.Search(products, searchName, categoryId);
        }
    }
}
