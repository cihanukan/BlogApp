using System.Linq;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
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

        public IActionResult Details(int id)
        {
            var blog = _blogRepository.GetById(id);
            if (blog == null || blog.isApproved==false)
                return View("Error");

            return View(_blogRepository.GetById(id));

        }
    }
}