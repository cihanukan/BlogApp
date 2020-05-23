using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ICategoryRepository
    {
        Category GetById(int categoryId);
        IQueryable<Category> GetAll();
        void DeleteCategory(int categoryId);
        void SaveCategory(Category entity);
    }
}
