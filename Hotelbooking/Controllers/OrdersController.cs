using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotelbooking.Models;
using Microsoft.AspNet.Identity;

namespace Hotelbooking.Controllers
{
    public class OrdersController : Controller
    {
        private HotelbookingContainer db = new HotelbookingContainer();

        // GET: Orders
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            
            var orders = db.OrderSet.ToList();

            int totalmarks = 0;
            double mark = 0.0;
            foreach (Order o in orders) {
                Order or = db.OrderSet.Find(o.Id);
                totalmarks += int.Parse(or.comments);
            }
            if (orders.Count() != 0)
            {
                mark = (double)totalmarks / orders.Count();
            }
            ViewBag.Message = mark;
            
            if (User.IsInRole("User"))
            {
                var order = db.OrderSet.Where(s => s.UserId == userId).ToList();
                return View(order);
            }
            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.OrderSet.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.RoomSet, "Id", "number");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,startdate,enddate,totalprice,comments,UserId,RoomId")] Order order)
        {
            var userId = User.Identity.GetUserId();
            order.UserId = userId;

            var date = db.RoomdateSet.Where(s => s.RoomId == order.RoomId).ToList();
            for (int i = 0; i < date.Count(); i++)
            {
                if (date[i].date != order.startdate.ToString("yyyy-MM-dd"))
                {
                    continue;
                }
                else
                {
                    for (int j = i; j < date.Count(); j++)
                    {
                        if (date[j].date == order.enddate.ToString("yyyy-MM-dd")) break;
                        if (date[j].status == "1")
                        {
                            return Content("<script>alert('Booking failed! Your room has been booked during this time!')</script>");
                        }
                        else
                        {
                            Roomdate rd = db.RoomdateSet.Find(date[j].Id);
                            rd.status = "1";
                        }
                    }
                    break;
                }
            }

            Room room = db.RoomSet.Find(order.RoomId);

            TimeSpan lasttime = -(Convert.ToDateTime(order.startdate) - Convert.ToDateTime(order.enddate));
            int days = lasttime.Days;
            int price = int.Parse(room.price);

            string totalPrice = (price * days).ToString();
            order.totalprice = totalPrice;


            if (ModelState.IsValid)
            {
                order.Id = Guid.NewGuid();
                db.OrderSet.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            ViewBag.RoomId = new SelectList(db.RoomSet, "Id", "number", order.RoomId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.OrderSet.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomId = new SelectList(db.RoomSet, "Id", "number", order.RoomId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,startdate,enddate,totalprice,comments,UserId,RoomId")] Order order)
        {
            var userId = User.Identity.GetUserId();
            order.UserId = userId;

            Room room = db.RoomSet.Find(order.RoomId);

            TimeSpan lasttime = -(Convert.ToDateTime(order.startdate) - Convert.ToDateTime(order.enddate));
            int days = lasttime.Days;
            int price = int.Parse(room.price);

            string totalPrice = (price * days).ToString();
            order.totalprice = totalPrice;

            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.RoomSet, "Id", "number", order.RoomId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.OrderSet.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.OrderSet.Find(id);
            db.OrderSet.Remove(order);
            var date = db.RoomdateSet.Where(s => s.RoomId == order.RoomId).ToList();
            for (int i = 0; i < date.Count(); i++)
            {
                if (date[i].date != order.startdate.ToString("yyyy-MM-dd"))
                {
                    continue;
                }
                else
                {
                    for (int j = i; j < date.Count(); j++)
                    {
                        if (date[j].date == order.enddate.ToString("yyyy-MM-dd")) break;
                        if (date[j].status == "0")
                        {
                            return Content("<script>alert('Unknown error!')</script>");
                        }
                        else
                        {
                            Roomdate rd = db.RoomdateSet.Find(date[j].Id);
                            rd.status = "0";
                        }
                    }
                    break;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
