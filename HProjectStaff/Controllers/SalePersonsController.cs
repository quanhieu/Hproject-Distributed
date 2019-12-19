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
    public class SalePersonsController : ApiController
    {
        private HCosmeticEntities db = new HCosmeticEntities(true);

        // GET: api/SalePersons
        //public IQueryable<SalePerson> GetSalePersons()
        //{
        //    return db.SalePersons.Include(s => s.Store);
        //}
        public IHttpActionResult GetSalePersons() {
            var data = db.SalePersons.Select(p => new
            {
                p.SalePersonId,
                p.SalePersonName,
                p.SalePersonPhone,
                Store = new {p.Store.StoreId, p.Store.StoreName }
            });
            return Ok(data);
        }

        // GET: api/SalePersons/5
        [ResponseType(typeof(SalePerson))]
        public IHttpActionResult GetSalePerson(int id)
        {
            SalePerson salePerson = db.SalePersons.Find(id);
            if (salePerson == null)
            {
                return NotFound();
            }

            var data = new
             {
                salePerson.SalePersonId,
                salePerson.SalePersonName,
                salePerson.SalePersonPhone,
                Store = new { salePerson.Store.StoreId, salePerson.Store.StoreName }
             };

            //db.Entry(salePerson).Reference(s => s.Store).Load();
            return Ok(data);
        }

        // PUT: api/SalePersons/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSalePerson(int id, SalePerson salePerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salePerson.SalePersonId)
            {
                return BadRequest();
            }

            db.Entry(salePerson).State = EntityState.Modified;
            db.Entry(salePerson.Store).State = EntityState.Unchanged;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalePersonExists(id))
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

        // POST: api/SalePersons
        [ResponseType(typeof(SalePerson))]
        public IHttpActionResult PostSalePerson(SalePerson salePerson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(salePerson.Store).State = EntityState.Unchanged;
            db.SalePersons.Add(salePerson);
            db.SaveChanges();

            var data = new
            {
                salePerson.SalePersonId,
                salePerson.SalePersonName,
                salePerson.SalePersonPhone,
                Store = new { salePerson.Store.StoreId, salePerson.Store.StoreName }
            };

            return CreatedAtRoute("DefaultApi", new { id = salePerson.SalePersonId }, data);
        }

        // DELETE: api/SalePersons/5
        [ResponseType(typeof(SalePerson))]
        public IHttpActionResult DeleteSalePerson(int id)
        {
            SalePerson salePerson = db.SalePersons.Find(id);
            if (salePerson == null)
            {
                return NotFound();
            }

            db.SalePersons.Remove(salePerson);
            db.SaveChanges();

            return Ok(salePerson);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalePersonExists(int id)
        {
            return db.SalePersons.Count(e => e.SalePersonId == id) > 0;
        }
    }
}