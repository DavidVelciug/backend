using AutoMapper;
using MyFullstackApp.DataAccess.Context;
using MyFullstackApp.Domains.Entities.Category;
using MyFullstackApp.Domains.Entities.Product;
using MyFullstackApp.Domains.Models.Base;
using MyFullstackApp.Domains.Models.Category;

namespace MyFullstackApp.BusinessLogic.Core.Categories;

public class CategoryAction
{
    protected readonly IMapper Mapper;

    protected CategoryAction(IMapper mapper)
    {
        Mapper = mapper;
    }

    protected List<CategoryDto> ExecuteGetAllCategoriesAction()
    {
        using var db = new AppDbContext();
        var data = db.Categories.ToList();
        return Mapper.Map<List<CategoryDto>>(data);
    }

    protected CategoryDto? GetCategoryDataByIdAction(int id)
    {
        using var db = new AppDbContext();
        var data = db.Categories.FirstOrDefault(x => x.Id == id);
        return data == null ? null : Mapper.Map<CategoryDto>(data);
    }

    protected ResponceMsg ExecuteCategoryUpdateAction(CategoryDto category)
    {
        using var db = new AppDbContext();
        var data = db.Categories.FirstOrDefault(x => x.Id == category.Id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Category not found." };
        }

        var nameTaken = db.Categories.Any(c => c.Id != category.Id && c.Name.Equals(category.Name));
        if (nameTaken)
        {
            return new ResponceMsg { IsSuccess = false, Message = "A category with this name already exists." };
        }

        data.Name = category.Name;
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Category updated successfully." };
    }

    protected ResponceMsg ExecuteCategoryDeleteAction(int id)
    {
        using var db = new AppDbContext();
        var data = db.Categories.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            return new ResponceMsg { IsSuccess = false, Message = "Category not found." };
        }

        if (db.Products.Any(p => p.CategoryId == id))
        {
            return new ResponceMsg
            {
                IsSuccess = false,
                Message = "Cannot delete category while products are assigned to it."
            };
        }

        db.Categories.Remove(data);
        db.SaveChanges();

        return new ResponceMsg { IsSuccess = true, Message = "Category deleted successfully." };
    }

    protected ResponceMsg ExecuteCategoryCreateAction(CategoryDto category)
    {
        using var db = new AppDbContext();
        var exists = db.Categories.FirstOrDefault(x => x.Name.Equals(category.Name));
        if (exists != null)
        {
            return new ResponceMsg
            {
                IsSuccess = false,
                Message = "A category with this name already exists in the system."
            };
        }

        var entity = Mapper.Map<CategoryData>(category);
        entity.Id = 0;
        entity.Products = new List<ProductData>();

        db.Categories.Add(entity);
        db.SaveChanges();

        return new ResponceMsg
        {
            IsSuccess = true,
            Message = "Category was successfully added."
        };
    }
}