namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class CategoryFactoryProvider
    {
        public CategoryFactory GetFactory(int? categoryId)
        {
            switch (categoryId)
            {
                case (byte)EnumCategories.Tools:
                    return new ToolsCategoryFactory();
                case (byte)EnumCategories.Materials:
                    return new MaterialsCategoryFactory();
                case (byte)EnumCategories.Plumbing:
                    return new PlumbingCategoryFactory();
                case (byte)EnumCategories.Electrical:
                    return new ElectricalCategoryFactory();
                case (byte)EnumCategories.Roofing:
                    return new RoofingCategoryFactory();
                default:
                    return new AllProductsFactory();
            }
        }
    }
}