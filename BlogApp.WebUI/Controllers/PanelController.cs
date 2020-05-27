using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IPanelRepository _panelRepository;
        private ICategoryRepository _categoryRepository;

        public PanelController(IPanelRepository panelRepository, ICategoryRepository categoryRepository)
        {
            _panelRepository = panelRepository;
            _categoryRepository = categoryRepository;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult List()
        {
            return View(_panelRepository.GetAll());
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blogTitle = _panelRepository.GetById(id).Title;
            _panelRepository.DeleteBlog(id);
            try
            {
                if (await _panelRepository.SaveChangesAsync())
                {
                    TempData["message"] = $"{blogTitle} başarıyla silindi.";
                }
            }
            catch (Exception error)
            {
                TempData["message"] = $"{blogTitle} adlı blog silinemedi. Hata mesajı : {error.Message}";

            }
            return RedirectToAction("List");

        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog entity)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _panelRepository.SaveBlog(entity);
                    if (await _panelRepository.SaveChangesAsync())
                    {
                        TempData["message"] = "Kayıt başarıyla güncellendi";
                        return RedirectToAction("List");
                    }
                }
                catch (Exception error)
                {
                    TempData["message"] = $"Blog kaydedilirken hata oluştu. Hata mesajı : {error.Message}";
                    return View(entity);
                }
            }

            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(_panelRepository.GetById(id));
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

                _panelRepository.SaveBlog(entity);
                try
                {
                    if (await _panelRepository.SaveChangesAsync())
                    {
                        TempData["message"] = "Kayıt başarıyla güncellendi";
                        return RedirectToAction("List");
                    }
                }
                catch (Exception error)
                {
                    TempData["message"] = $"Blog kaydedilirken hata oluştu. Hata mesajı : {error.Message}";
                    return View(entity);
                }
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            return View(entity);
        }

    }
}