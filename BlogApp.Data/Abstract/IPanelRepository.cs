using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPanelRepository
    {
        Blog GetById(int blogId);
        IQueryable<Blog> GetAll();
        void SaveBlog(Blog entity);
        void DeleteBlog(int blogId);
        Task<bool> SaveChangesAsync();
    }
}
