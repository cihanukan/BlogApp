using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.WebUI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IBlogRepository _blogRepository;

        public HomeController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public IActionResult Index(int? id)
        {
            var viewmodel = new HomeBlogViewModel();

            var homeBlogs = _blogRepository.GetAll().Where(u => u.isApproved && u.isHome);
            viewmodel.SliderBlogs = _blogRepository.GetAll()
                .Where(u => u.isApproved && u.isSlider).OrderByDescending(p => p.Date).ToList();

            if (id != null)
                viewmodel.HomeBlogs = homeBlogs.Where(p => p.CategoryId == id)
                    .OrderByDescending(p => p.Date).ToList();
            else
                viewmodel.HomeBlogs = homeBlogs.OrderByDescending(p => p.Date).ToList();

            return View(viewmodel);
        }
        public IActionResult List()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var blog = _blogRepository.GetById(id);
            if(blog == null || blog.isApproved==false || blog.isHome==false)
                return View("Error");

            return View(_blogRepository.GetById(id));

        }
    }
}