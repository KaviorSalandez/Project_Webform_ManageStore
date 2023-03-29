using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using System.IO;

namespace WebApplication2.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                List<Category> categories = context.Categories.ToList();
                ViewBag.Categories = categories;
            }
            return View();
        }
        // GET: CarsController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new DbPrn211ProjectWebContext())
            {
                var category = context.Categories.FirstOrDefault(x => x.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
        }

        // GET: CarsController/Create
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Category c)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(c.imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await c.imageFile.CopyToAsync(fileStream);
                        }
                        context.Categories.Add(new Category
                        {
                            Name = c.Name,
                            Image = c.imageFile.FileName,
                        }) ;
                        context.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(c);
                }
            }
        }
        [HttpGet]
        // GET: CarsController/Edit/5
        public ActionResult Edit(int? id)
        {

            using (var context = new DbPrn211ProjectWebContext())
            {

                var category = context.Categories.FirstOrDefault(x => x.Id == id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
        }


        // POST: CarsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Category c)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                try
                {
                    if (id != c.Id)
                    {
                        return NotFound();
                    }
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(c.imageFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await c.imageFile.CopyToAsync(fileStream);
                        }
                        context.Update(new Category
                        {
                            Id = id,
                            Name = c.Name,
                            Image = c.imageFile.FileName
                        });
                        context.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View();
                }
            }
        }

        // GET: CarsController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new DbPrn211ProjectWebContext())
            {
                var ca = context.Categories.FirstOrDefault(x => x.Id == id);
                if (ca == null)
                {
                    return NotFound();
                }
                return View(ca);
            }

        }

        // POST: CarsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                Category c = context.Categories.FirstOrDefault(x => x.Id == id);
                if(c == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        context.Categories.Remove(c);
                        context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Không xóa được vì đang có sản phẩm trong thể loại này";
                        return View();
                    }
                }
               
            }
        }

    }
}
