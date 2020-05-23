using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private readonly BlogContext _context;

        public EfCategoryRepository(BlogContext context)
        {
            _context = context;
        }
        public Category GetById(int categoryId)
        {
            return _context.Categories.FirstOrDefault(p => p.CategoryId == categoryId);
        }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public void DeleteCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(p => p.CategoryId == categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public void SaveCategory(Category entity)
        {
            if (entity.CategoryId == 0)
            {
                _context.Categories.Add(entity);
            }
            else
            {
                var category = GetById(entity.CategoryId);
                if (category != null)
                {
                    category.Name = entity.Name;
                }
            }

            _context.SaveChanges();
        }
    }
}
