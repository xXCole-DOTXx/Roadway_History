﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Roadway_History.Models;
using PagedList;
using System.Security.Principal;
using System.Security;


namespace Roadway_History.Controllers
{
    public class StatewidesController : Controller
    {
        //public static bool IsInGroup(string groupName)
        //{
        //    var myIdentity = GetUserIDWithDomain();
        //}
        public static WindowsIdentity GetUserIdWithDomain()
        {
            var myIdentity = WindowsIdentity.GetCurrent();
            return myIdentity;
        }

        public static string GetUserId()
        {
            var id = GetUserIdWithDomain().Name.Split('\\');
            return id[1];
        }


        private RoadWay_HistoryEntities db = new RoadWay_HistoryEntities();

        // GET: Statewides
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.IDSortParm = String.IsNullOrEmpty(sortOrder) ? "ID" : "";
            ViewBag.countySortParm = sortOrder == "county" ? "county_desc" : "county";
            ViewBag.routeSortParm = sortOrder == "route" ? "route_desc" : "route";
            ViewBag.OGrouteSortParm = sortOrder == "OGroute" ? "OGroute_desc" : "OGroute";
            ViewBag.SignSortParm = sortOrder == "sign" ? "sign_desc" : "sign";
            ViewBag.SuppSortParm = sortOrder == "supp" ? "supp_desc" : "supp";
            ViewBag.LocalNameSortParm = sortOrder == "local" ? "local_desc" : "local";
            ViewBag.StatusSortParm = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var statewides = from s in db.Statewides
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                statewides = statewides.Where(s => s.COUNTY.Contains(searchString)
                                             || s.RouteNoOrigImport.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ID":
                    statewides = statewides.OrderBy(s => s.ID);
                    break;
                case "county":
                    statewides = statewides.OrderBy(s => s.COUNTY);
                    break;
                case "county_desc":
                    statewides = statewides.OrderByDescending(s => s.COUNTY);
                    break;
                case "route":
                    statewides = statewides.OrderBy(s => s.RouteNo);
                    break;
                case "route_desc":
                    statewides = statewides.OrderByDescending(s => s.RouteNo);
                    break;
                case "OGroute":
                    statewides = statewides.OrderBy(s => s.RouteNoOrigImport);
                    break;
                case "OGroute_desc":
                    statewides = statewides.OrderByDescending(s => s.RouteNoOrigImport);
                    break;
                case "sign":
                    statewides = statewides.OrderBy(s => s.SignSys);
                    break;
                case "sign_desc":
                    statewides = statewides.OrderByDescending(s => s.SignSys);
                    break;
                case "supp":
                    statewides = statewides.OrderBy(s => s.SuppDes);
                    break;
                case "supp_desc":
                    statewides = statewides.OrderByDescending(s => s.SuppDes);
                    break;
                case "local":
                    statewides = statewides.OrderBy(s => s.LocalName);
                    break;
                case "local_desc":
                    statewides = statewides.OrderByDescending(s => s.LocalName);
                    break;
                case "status":
                    statewides = statewides.OrderBy(s => s.Current_Status);
                    break;
                case "status_desc":
                    statewides = statewides.OrderByDescending(s => s.Current_Status);
                    break;
                default:
                    statewides = statewides.OrderByDescending(s => s.ID);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);
            return View(statewides.ToPagedList(pageNumber, pageSize));
        }

        // GET: Statewides/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Statewide statewide = db.Statewides.Find(id);
            if (statewide == null)
            {
                return HttpNotFound();
            }
            return View(statewide);
        }
        [Authorize(Users = "EXECUTIVE\\E072340, EXECUTIVE\\E096752")]
        // GET: Statewides/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statewides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,COUNTY,SignSys,RouteNoOrigImport,RouteNo,SuppDes,LocalName,DateNumber,Method,OriginalL,OriginalD,Projects,Documents,Comments,District,Duplicate_OK,RightofWay,ReservedRoute,ReservedBy,ReservedDate,CP_WorkCompleted,Work_Comments,CP_ProjectNo,ReservedCat,CurrentStatus,Add_User,Date_Added")] Statewide statewide)
        {
            if (ModelState.IsValid)
            {
                var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                statewide.Add_User = userName;
                statewide.Date_Added = DateTime.Today;
                db.Statewides.Add(statewide);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statewide);
        }

        // GET: Statewides/Edit/5
        [Authorize(Users = "EXECUTIVE\\E072340, EXECUTIVE\\E096752")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Statewide statewide = db.Statewides.Find(id);
            if (statewide == null)
            {
                return HttpNotFound();
            }
            return View(statewide);
        }

        // POST: Statewides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,COUNTY,SignSys,RouteNoOrigImport,RouteNo,SuppDes,LocalName,DateNumber,Method,OriginalL,OriginalD,Projects,Documents,Comments,District,Duplicate_OK,RightofWay,ReservedRoute,ReservedBy,ReservedDate,CP_WorkCompleted,Work_Comments,CP_ProjectNo,ReservedCat,CurrentStatus")] Statewide statewide)
        {
            if (ModelState.IsValid)
            {
                db.Entry(statewide).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statewide);
        }

        // GET: Statewides/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Statewide statewide = db.Statewides.Find(id);
            if (statewide == null)
            {
                return HttpNotFound();
            }
            return View(statewide);
        }

        // POST: Statewides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Statewide statewide = db.Statewides.Find(id);
            db.Statewides.Remove(statewide);
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
