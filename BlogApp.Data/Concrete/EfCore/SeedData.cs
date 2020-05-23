using System;
using System.Collections.Generic;
using System.Text;
using BlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            BlogContext blogContext = app.ApplicationServices.GetRequiredService<BlogContext>();

            //if there is any migration waiting, this block will notify the db about migration change.
            blogContext.Database.Migrate();

            if (!blogContext.Categories.Any())
            {
                //adding seed data to the category table
                blogContext.Categories.AddRange(
                    new Category(){Name = "Category 1"},
                    new Category(){Name = "Category 2"},
                    new Category(){Name = "Category 3"}
                );

                blogContext.SaveChanges();
            }

            if (!blogContext.Blogs.Any())
            {
                //Adding seed data with range
                blogContext.Blogs.AddRange(

                    new Blog() { Title = "Blog title 1", Description = "Blog Description", Body = "Blog Body 1", Image = "1.jpeg", Date = DateTime.Now.AddDays(-5), isApproved = true, isHome = true, CategoryId = 5 },
                    new Blog() { Title = "Blog title 2", Description = "Blog Description", Body = "Blog Body 1", Image = "2.jpeg", Date = DateTime.Now.AddDays(-7), isApproved = true, isHome = false, CategoryId = 4 },
                    new Blog() { Title = "Blog title 3", Description = "Blog Description", Body = "Blog Body 1", Image = "3.jpeg", Date = DateTime.Now.AddDays(-8), isApproved = false, isHome = true, CategoryId = 2 },
                    new Blog() { Title = "Blog title 4", Description = "Blog Description", Body = "Blog Body 1", Image = "4.jpeg", Date = DateTime.Now.AddDays(-9), isApproved = true, isHome = false, CategoryId = 3 }
                );

                blogContext.SaveChanges();
            }
        }
    }
}
