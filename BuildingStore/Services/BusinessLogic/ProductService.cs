using BuildingStore.Models;
using BuildingStore.Services.Patterns.Strategy;

namespace BuildingStore.Services.BusinessLogic
{
    public class ProductService
    {
        private readonly AppDbContext appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public List<Product> FindProductsForAdmin(string searchName, int? categoryId)
        {
            var allProducts = appDbContext.Products.ToList();
            var searchContext = new ProductSearchContext();

            bool hasName = !string.IsNullOrWhiteSpace(searchName);
            bool hasCategory = categoryId.HasValue && categoryId > 0;

            if (hasName && hasCategory)
            {
                searchContext.SetStrategy(new CombinedSearchStrategy());
            }
            else if (hasName)
            {
                searchContext.SetStrategy(new SearchByNameStrategy());
            }
            else if (hasCategory)
            {
                searchContext.SetStrategy(new SearchByCategoryStrategy());
            }
            else
            {
                searchContext.SetStrategy(new DefaultSearchStrategy());
            }

            return searchContext.ExecuteSearch(allProducts, searchName, categoryId);
        }
    }
}
