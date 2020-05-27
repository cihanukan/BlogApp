using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfBlogRepository : IBlogRepository
    {
        private readonly BlogContext _blogContext;

        public EfBlogRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public Blog GetById(int blogId)
        {
            return _blogContext.Blogs.FirstOrDefault(p => p.BlogId == blogId);
        }

        public IQueryable<Blog> GetAll()
        {
            return _blogContext.Blogs;
        }
    }
}
