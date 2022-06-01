﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Model.EF;

namespace DemoGridview.Controllers
{
    public class SchedulerController : Controller
    {
        private OnlineShopDbContext db = new OnlineShopDbContext();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EventSchedulers_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = db.EventSchedulers.ToList()
                        .Select(eventScheduler => new EventSchedulerViewModel(eventScheduler))
                        .AsQueryable();

            return Json(data.ToDataSourceResult(request));
        }

        public virtual JsonResult EventSchedulers_Create([DataSourceRequest] DataSourceRequest request, EventSchedulerViewModel eventScheduler)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty( eventScheduler.Title))
                {
                     eventScheduler.Title = "";
                }

                var entity = eventScheduler.ToEntity();
                db.EventSchedulers.Add(entity);
                db.SaveChanges();
                eventScheduler.ID = entity.ID;
            }

            return Json(new[] { eventScheduler }.ToDataSourceResult(request, ModelState));
        }
        public virtual JsonResult EventSchedulers_Update([DataSourceRequest] DataSourceRequest request, EventSchedulerViewModel eventScheduler)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty( eventScheduler.Title))
                {
                     eventScheduler.Title = "";
                }

                var entity = eventScheduler.ToEntity();
                db.EventSchedulers.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { eventScheduler }.ToDataSourceResult(request, ModelState));
        }
        public virtual JsonResult EventSchedulers_Destroy([DataSourceRequest] DataSourceRequest request, EventSchedulerViewModel eventScheduler)
        {
            if (ModelState.IsValid)
            {
                var entity = eventScheduler.ToEntity();
                db.EventSchedulers.Attach(entity);
                db.EventSchedulers.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { eventScheduler }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}