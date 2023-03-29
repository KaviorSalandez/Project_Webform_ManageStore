using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                List<Brand> brands = context.Brands.ToList();
                return View(brands);
            }
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new DbPrn211ProjectWebContext())
            {
                var brand = context.Brands.FirstOrDefault(x => x.Id == id);
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }
        }
        // GET: CarsController/Create
        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand b)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(b.imageFile1.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await b.imageFile1.CopyToAsync(fileStream);
                        }
                        context.Brands.Add(new Brand
                        {
                            Name = b.Name,
                            Image = b.imageFile1.FileName,
                        });
                        context.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(b);
                }
            }
        }

        [HttpGet]
        // GET: CarsController/Edit/5
        public ActionResult Edit(int? id)
        {

            using (var context = new DbPrn211ProjectWebContext())
            {

                var brand = context.Brands.FirstOrDefault(x => x.Id == id);
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }
        }


        // POST: CarsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand c)
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
                        var fileName = Path.GetFileName(c.imageFile1.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await c.imageFile1.CopyToAsync(fileStream);
                        }
                        context.Update(new Brand
                        {
                            Id = id,
                            Name = c.Name,
                            Image = c.imageFile1.FileName,
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
                var brand = context.Brands.FirstOrDefault(x => x.Id == id);
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }

        }

        // POST: CarsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                Brand c = context.Brands.FirstOrDefault(x => x.Id == id);
                if(c == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        context.Brands.Remove(c);
                        context.SaveChanges();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Không xóa được vì đang có sản phẩm trong thương hiệu này";
                        return View();
                    }
                }
               
            }
        }


    }
}
