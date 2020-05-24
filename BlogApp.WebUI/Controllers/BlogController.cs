using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Index(int? id, string q)
        {
            var query = _blogRepository.GetAll().Where(p => p.isApproved);

            if (id != null)
            {
                // id bos degilse kategorisel süzme var demektir.
                query = query.Where(p => p.CategoryId == id);
            }

            if (!string.IsNullOrEmpty(q))
            {
                //İlk Arama Şekli
                //query = query.Where(p => p.Title.Contains(q) || p.Description.Contains(q) || p.Body.Contains(q));

                //EF CORE ILE GELEN OZELLIK - DAHA OZEL ARAMALAR YAPABILIRIZ
                query = query.Where(p => EF.Functions.Like(p.Title, "%" + q + "%") ||
                EF.Functions.Like(p.Description, "%"+q+"%") || 
                EF.Functions.Like(p.Body, "%"+q+"%"));
            }

            return View(query.OrderByDescending(p => p.Date));

        }
        public IActionResult List()
        {
            return View(_blogRepository.GetAll());
        }

        public IActionResult Details(int id)
        {
            var blog = _blogRepository.GetById(id);
            if (blog == null || blog.isApproved==false)
                return View("Error");

            return View(_blogRepository.GetById(id));

        }

        public IActionResult Delete(int id)
        {
            var blogTitle = _blogRepository.GetById(id).Title;
            _blogRepository.DeleteBlog(id);
            TempData["message"] = $"{blogTitle} successfully deleted.";
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog entity)
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(_blogRepository.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Blog entity, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    entity.Image = file.FileName;
                }

                _blogRepository.SaveBlog(entity);
                TempData["message"] = "Kayıt başarıyla güncellendi";
                return RedirectToAction("List");

            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }
    }
}