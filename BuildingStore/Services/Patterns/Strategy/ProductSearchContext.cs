using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Strategy
{
    public class ProductSearchContext
    {
        private IProductSearchStrategy strategy;

        public ProductSearchContext(string searchName, int? categoryId)
        {
            bool hasName = !string.IsNullOrWhiteSpace(searchName);
            bool hasCategory = categoryId.HasValue && categoryId > 0;

            if (hasName && hasCategory)
            {
                strategy = new CombinedSearchStrategy();
            }
            else if (hasName)
            {
                strategy = new SearchByNameStrategy();
            }
            else if (hasCategory)
            {
                strategy = new SearchByCategoryStrategy();
            }
            else
            {
                strategy = new DefaultSearchStrategy();
            }
        }
        public List<Product> ExecuteSearch(List<Product> products, string searchName, int? categoryId)
        {
            return strategy.Search(products, searchName, categoryId);
        }
    }
}