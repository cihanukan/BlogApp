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

        public void SaveBlog(Blog entity)
        {
            //fist time create
            if (entity.BlogId == 0)
            {
                entity.Date = DateTime.Now;
                _blogContext.Blogs.Add(entity);
            }
            else
            {
                var blog = GetById(entity.BlogId);
                if (blog != null)
                {
                    blog.Title = entity.Title;
                    blog.Body = entity.Body;
                    blog.Image = entity.Image;
                    blog.Description = entity.Description;
                    blog.isApproved = entity.isApproved;
                    blog.isHome = entity.isHome;
                    blog.isSlider = entity.isSlider;
                    blog.CategoryId = entity.CategoryId;
                }
            }
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
