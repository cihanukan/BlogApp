using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
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
        public IActionResult Index()
        {
            return View(_blogRepository.GetAll().Where(p=>p.isApproved && p.isHome));
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