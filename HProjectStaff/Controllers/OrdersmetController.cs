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
    public class OrdersmetController : ApiController
    {
        private HCosmeticEntities db = new HCosmeticEntities(true);


        // GET: api/Orders
        //public IQueryable<Order> GetOrders()
        //{
        //    return db.Orders.Include(o => o.SalePersonId);
        //    //return db.Orders;
        //}
        public IHttpActionResult GetOrders()
        {
            var data = db.Orders.Select(p => new
            {
                p.OrderId,
                p.CustomerName,
                p.OrderDate,
                p.CustomerPhone,
                SalePerson = new { p.SalePerson.SalePersonId, p.SalePerson.SalePersonName }
            });
            return Ok(data);
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            var data = new
            {
                order.OrderId,
                order.CustomerName,
                order.OrderDate,
                order.CustomerPhone,
                SalePerson = new { order.SalePerson.SalePersonId, order.SalePerson.SalePersonName }
            };

            return Ok(data);
        }

        // PUT: api/Orders/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutOrder(int id, Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != order.OrderId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Entry(order.SalePerson).State = EntityState.Unchanged;
                
                foreach (var item in order.OrderDetails)
                {
                    db.Entry(item.Product).State = EntityState.Unchanged;
                    db.Entry(item.Product.Category).State = EntityState.Detached;
                }

                db.Orders.Add(order);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }

            return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, order);
        
        //public IHttpActionResult PostOrder(Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        db.Entry(order.SalePerson).State = EntityState.Unchanged;
        //        foreach (var item in order.OrderDetails)
        //        {
        //            db.Entry(item.Product).State = EntityState.Unchanged;
        //            db.Entry(item.Product.Category).State = EntityState.Detached;
        //        }

        //        db.Orders.Add(order);
        //        db.SaveChanges();


        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return NotFound();
        //    }
        //    var data = new
        //    {
        //        order.OrderId,
        //        order.CustomerName,
        //        order.OrderDate,
        //        order.CustomerPhone,
        //        SalePerson = new { order.SalePerson.SalePersonId, order.SalePerson.SalePersonName }
        //    };
        //    return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, data);
        //}


        //public IHttpActionResult PostOrder(Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    db.Entry(order.SalePerson).State = EntityState.Unchanged;
        //    db.Orders.Add(order);
        //    db.SaveChanges();
        //    var data = new
        //    {
        //        order.OrderId,
        //        order.CustomerName,
        //        order.OrderDate,
        //        order.CustomerPhone,
        //        SalePerson = new { order.SalePerson.SalePersonId, order.SalePerson.SalePersonName }
        //    };
        //    return CreatedAtRoute("DefaultApi", new { id = order.OrderId }, data);
    }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
       {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            db.Entry(order).Collection(o => o.OrderDetails).Load();
            List<OrderDetail> list = new List<OrderDetail>();
            foreach (var item in order.OrderDetails)
            {
                list.Add(item);
            }
            foreach (var item in list)
            {
                db.OrderDetails.Remove(item);
            }
            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }
    }
}