using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(_categoryRepository.GetAll());
        }


        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            if (id == null)
            {
                ViewBag.PageTitle = "Create Category";
                //ilk defa kayıt
                return View(new Category());
            }
            ViewBag.PageTitle = "Edit Category";
            return View(_categoryRepository.GetById((int)id));

        }

        [HttpPost]
        public IActionResult AddOrUpdate(Category entity)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.SaveCategory(entity);
                TempData["message"] = "Kayıt başarıyla güncellendi";
                return RedirectToAction("List");
            }

            return View(entity);
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetById(id);
            _categoryRepository.DeleteCategory(id);
            TempData["message"] = $"{category.Name} successfully deleted.";
            return RedirectToAction("List");
        }

    }
}