namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Model.Paging;
    using AllSopFoodService.ViewModels;
    using Boxed.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly FoodDBContext _db;
        private readonly IMapper<Product, FoodProductVM> productMapper;

        public CategoryService(FoodDBContext dbcontext, IMapper<Product, FoodProductVM> productMapper)
        {
            this._db = dbcontext;
            this.productMapper = productMapper;
        }


        public void CreateCategory(CategoryVM cartegory)
        {
            var newCategory = new Category()
            {
                Label = cartegory.Label,
                IsAvailable = cartegory.IsAvailable
            };

            this._db.Categories.Add(newCategory);
            this._db.SaveChanges();
        }

        public List<Category> GetAllCategories(string? sortBy, string? searchString, int? pageNum)
        {
            var allCategories = this._db.Categories.OrderBy(n => n.Label).ToList();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "label_desc":
                        allCategories = allCategories.OrderByDescending(n => n.Label).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allCategories = allCategories.Where(n => n.Label.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            //Server Side Paging
            //var pageSize = 3;
            //allCategories = PaginatedList<Category>.Create(allCategories.AsQueryable(), pageNum ?? 1, pageSize);

            return allCategories;
        }

        public CategoryWithProducts GetCategoryData(int categoryId)
        {
            var categoryData = this._db.Categories.Include(c => c.FoodProducts.Where(p => p.CategoryId == categoryId)).ThenInclude(fp => fp.Category)
                                                    .Select(c => new CategoryWithProducts()
                                                    {
                                                        Label = c.Label,
                                                        Products = c.FoodProducts.Select(fp => this.productMapper.Map(fp)).ToList()
                                                    }).FirstOrDefault();

#pragma warning disable CS8603 // Possible null reference return.
            return categoryData;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void DeleteCategoryById(int id)
        {
            var category = this._db.Categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                this._db.Remove(category);
                this._db.SaveChanges();
            }
        }

    }
}