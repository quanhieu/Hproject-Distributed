using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HProjectStaff.Models;

namespace HProjectStaff.Controllers
{
    public class ProductsController : ApiController
    {
        private HCosmeticEntities db = new HCosmeticEntities(true);

        // GET: api/Products
        public IHttpActionResult GetProducts()
        {
            var data = db.Products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.Description,
                p.Price,
                Category = new {p.Category.CategoryId,p.Category.CategoryName}
            });
            return Ok(data);
            //return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product p = db.Products.Find(id);
            if (p == null)
            {
                return NotFound();
            }

            var data = new
            {
                p.ProductId,
                p.ProductName,
                p.Description,
                p.Price,
                Category = new { p.Category.CategoryId, p.Category.CategoryName }
            };
            //db.Entry(product).Reference(p => p.Category).Load();
            return Ok(data);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            db.Entry(product.Category).State = EntityState.Unchanged;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(product.Category).State = EntityState.Unchanged;
            db.Products.Add(product);
            db.SaveChanges();

            var data = new
            {
                product.ProductId,
                product.ProductName,
                product.Description,
                product.Price,
                Category = new { product.Category.CategoryId, product.Category.CategoryName }
            };

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, data);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}