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
        public IActionResult Index(int? id)
        {
            var query = _blogRepository.GetAll()
                .Where(u => u.isApproved && u.isHome);

            if (id != null)
            {
                query = query.Where(p => p.CategoryId == id);
            }

            return View(query.OrderByDescending(p=>p.Date));
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