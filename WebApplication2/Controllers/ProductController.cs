using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {

            using (var context = new DbPrn211ProjectWebContext())
            {
                List<Product> products = context.Products.Include(p=>p.BidNavigation).Include(c=>c.CidNavigation).ToList();
                List<Brand> listB = context.Brands.ToList();
                List<Category> listC = context.Categories.ToList();
                ViewBag.Brands = listB;
                ViewBag.Categoiries = listC;
                return View(products);
            }
        }
        [HttpPost]
        public IActionResult Filter()
        {
            string cid = HttpContext.Request.Form["cid"];
            int c_id = 0;
            try
            {
                c_id = byte.Parse(cid);
            }
            catch (Exception)
            {
            }
            using (var context = new DbPrn211ProjectWebContext())
            {
                List<Product> products = new List<Product>();
                if (c_id == 0)
                {
                    products = context.Products.Include(p => p.BidNavigation).Include(c => c.CidNavigation).ToList();
                }
                else
                {
                     products = context.Products.Include(p => p.BidNavigation).Include(c => c.CidNavigation).Where(x=>x.Cid==c_id).ToList();
                }
                List<Brand> listB = context.Brands.ToList();
                List<Category> listC = context.Categories.ToList();
                ViewBag.Brands = listB;
                ViewBag.Categoiries = listC;
                ViewBag.cid = c_id;

                return View(products);
            }
        }
        [HttpPost]
        public IActionResult Search()
        {
            string name1 = HttpContext.Request.Form["nameS"];
            if(name1 == null)
            {
                name1 = "";
            }
            using (var context = new DbPrn211ProjectWebContext())
            {
                List<Product> products = new List<Product>();
                if (name1 == "")
                {
                    products = context.Products.Include(p => p.BidNavigation).Include(c => c.CidNavigation).ToList();
                }
                else
                {
                    products = context.Products.Include(p => p.BidNavigation).Include(c => c.CidNavigation).Where(x => x.Name.ToLower().Contains(name1.ToLower())).ToList();
                }
                List<Brand> listB = context.Brands.ToList();
                List<Category> listC = context.Categories.ToList();
                ViewBag.Brands = listB;
                ViewBag.Categoiries = listC;
                ViewBag.name1 = name1;

                return View(products);
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
                var product = context.Products.Include(p => p.BidNavigation).Include(c => c.CidNavigation).FirstOrDefault(x => x.Id == id);
                return View(product);
            }
        }


        public ActionResult Create()
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                
                List<Brand> listB = context.Brands.ToList();
                List<Category> listC = context.Categories.ToList();
                ViewBag.Brands = listB;
                ViewBag.Categoiries = listC;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product p)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(p.imageFileP.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await p.imageFileP.CopyToAsync(fileStream);
                        }
                        context.Products.Add(new Product
                        {
                            Name= p.Name,
                            Price= p.Price,
                            Descrip= p.Descrip,
                            Bid = p.Bid,
                            Cid= p.Cid,
                            Stock= p.Stock,
                            Image = p.imageFileP.FileName,
                        });
                        context.SaveChanges();
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                    return View(p);
                }
            }
        }

        [HttpGet]
        // GET: CarsController/Edit/5
        public ActionResult Edit(int? id)
        {

            using (var context = new DbPrn211ProjectWebContext())
            {

                var product = context.Products.FirstOrDefault(x => x.Id == id);

                if (product == null)
                {
                    return NotFound();
                }
                List<Brand> listB = context.Brands.ToList();
                List<Category> listC = context.Categories.ToList();
                ViewBag.Brands = listB;
                ViewBag.Categoiries = listC;
                ViewBag.IdBrand = product.Bid;
                ViewBag.IdCate = product.Cid;
                return View(product);
            }
        }


        // POST: CarsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Product p)
        {
            using (var context = new DbPrn211ProjectWebContext())
            {
                try
                {
                    if (id != p.Id)
                    {
                        return NotFound();
                    }
                    if (ModelState.IsValid)
                    {
                        var fileName = Path.GetFileName(p.imageFileP.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await p.imageFileP.CopyToAsync(fileStream);
                        }
                        context.Update(new Product
                        {
                            Id = id,
                            Name = p.Name,
                            Price = p.Price,
                            Descrip = p.Descrip,
                            Bid = p.Bid,
                            Cid = p.Cid,
                            Stock = p.Stock,
                            Image = p.imageFileP.FileName,

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


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new DbPrn211ProjectWebContext())
            {
                var ca = context.Products.FirstOrDefault(x => x.Id == id);
                if (ca == null)
                {
                    return NotFound();
                }
                List<Brand> listB = context.Brands.ToList();
                List<Category> listC = context.Categories.ToList();
                ViewBag.Brands = listB;
                ViewBag.Categoiries = listC;
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
                Product p = context.Products.FirstOrDefault(x => x.Id == id);
                if (p == null)
                {
                    return NotFound();
                }
                else
                {
                    try
                    {
                        context.Products.Remove(p);
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
