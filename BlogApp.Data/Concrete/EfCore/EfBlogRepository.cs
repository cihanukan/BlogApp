using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void AddBlog(Blog blog)
        {
            _blogContext.Blogs.Add(blog);
            _blogContext.SaveChanges();
        }

        public void UpdateBlog(Blog blog)
        {
            _blogContext.Entry(entity: blog).State = EntityState.Modified;
            _blogContext.SaveChanges();
        }

        public void DeleteBlog(int blogId)
        {
            var blog = _blogContext.Blogs.FirstOrDefault(p => p.BlogId == blogId);

            if (blog != null)
            {
                _blogContext.Blogs.Remove(blog);
                _blogContext.SaveChanges();
            }
        }
    }
}
