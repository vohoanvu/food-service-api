namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public interface ICategoryService
    {
        void CreateCategory(CategoryVM cartegory);

        List<Category> GetAllCategories(string? sortBy, string? searchString, int? pageNum);

        CategoryWithProductsAndCartsVM GetCategoryData(int categoryId);

        void DeleteCategoryById(int id);
    }
}
