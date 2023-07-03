using Microsoft.Ajax.Utilities;
using MVC課程.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC課程.Controllers
{
    public class UserDBController : Controller
    {
        private MVC_UserDBContext _db = new MVC_UserDBContext(); //開啟資料庫
        protected override void Dispose(bool disposing) //關閉資料庫
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: UserDB
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create() 
        {
            return View();
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]   //避免XSS XCSRF 攻擊

        public ActionResult CreateConfirm(UserTable _userTable)
        {
            if ((_userTable != null) && (ModelState.IsValid))
            {
                _db.UserTables.Add(_userTable);
                _db.SaveChanges();
                return RedirectToAction("List");
            }
            else
            {
                ModelState.AddModelError("Value1", "自訂錯誤訊息(1)");
                ModelState.AddModelError("Value2", "自訂錯誤訊息(2)");
                return View();
            }
        }
        public ActionResult List()
        {
            IQueryable<UserTable> ListAll = from _userTable in _db.UserTables
                                            select _userTable;

            if (ListAll == null)
            {
                return HttpNotFound();
            }
            else { 
            return View(ListAll.ToList());
        }





        }
    
     [HttpGet]
      public ActionResult Details(int _ID = 1)
    {
        if (_ID == null)
        {
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }
        UserTable ut = _db.UserTables.Find(_ID);
        if (ut == null)
        {
            return HttpNotFound();
        }
        else {
            return View(ut);
        }
    }

        public ActionResult Delete(int _ID = 1)
        {
            if (_ID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            UserTable ut = _db.UserTables.Find(_ID);
            if (ut == null)
            {
                return HttpNotFound();
            }
            else {
                return View(ut);
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]   //避免XSS XCSRF 攻擊
        public ActionResult DeleteConfirm(int _ID)
        {
            UserTable ut = _db.UserTables.Find(_ID);
            _db.UserTables.Remove(ut);
            _db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Edit(int _ID = 1)
        {
            if (_ID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            UserTable ut = _db.UserTables.Find(_ID);
            if (ut == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ut);
            }
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]   //避免XSS XCSRF 攻擊
        public ActionResult EditConfirm(UserTable _userTable)
        { 
        _db.Entry(_userTable).State = System.Data.Entity.EntityState.Modified;//確定被修改(狀態:Modified)
        _db.SaveChanges();
          return RedirectToAction("List");
        }

        public ActionResult Search(string _SearchWord = "MVC")
        {
            ViewData["SW"] = _SearchWord;

            IQueryable<UserTable> ListAll = from _usertable in _db.UserTables
                                            select _usertable;
            if (!String.IsNullOrEmpty(_SearchWord))
            {
                return View(ListAll.Where(s => s.UserName.Contains(_SearchWord)));
            }
            else 
            {
                return HttpNotFound();
            }
        }
        public ActionResult Search4_Multi()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search4_Multi(UserTable _userTable)
        { 
        string uName = _userTable.UserName;
        string uMobilePhone = _userTable.UserMobilePhone;

        var ListAll = _db.UserTables.Select(s => s);
            if(!string.IsNullOrWhiteSpace(uName))
            {
                ListAll = ListAll.Where(s => s.UserName.Contains(uName));
            }
            if (!string.IsNullOrWhiteSpace(uMobilePhone))
            {
                ListAll = ListAll.Where(s => s.UserMobilePhone.Contains(uMobilePhone));
            }
            if ((_userTable != null) && (ModelState.IsValid))
            {
                return View("Search4_Result", ListAll.ToList());
            }
            else 
            {
                return HttpNotFound();
            }

          

        }

    }

}