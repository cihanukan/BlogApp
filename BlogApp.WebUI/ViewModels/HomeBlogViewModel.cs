using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entity;

namespace BlogApp.WebUI.ViewModels
{
    public class HomeBlogViewModel
    {
        public List<Blog> SliderBlogs { get; set; }
        public List<Blog> HomeBlogs { get; set; }
    }
}
