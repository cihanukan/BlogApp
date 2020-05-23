using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;
        private ICategoryRepository _categoryRepository;

        public BlogController(IBlogRepository blogRepository, ICategoryRepository categoryRepository)
        {
            _blogRepository= blogRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            return View(_blogRepository.GetAll());
        }

        public IActionResult Delete(int id)
        {
            var blogTitle = _blogRepository.GetById(id).Title;
            _blogRepository.DeleteBlog(id);
            TempData["message"] = $"{blogTitle} successfully deleted.";
            return RedirectToAction("List");
        }


        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");

            if (id == null)
            {
                //ilk defa kayıt
                ViewBag.PageTitle = "Create Blog";
                return View(new Blog());
            }
            else
            {
                ViewBag.PageTitle = "Edit Blog";
                return View(_blogRepository.GetById(((int) id)));
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Blog entity)
        {

            if (ModelState.IsValid)
            {
                _blogRepository.SaveBlog(entity);
                TempData["message"] = "Kayıt başarıyla güncellendi";
                return RedirectToAction("List");

            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }
    }
}