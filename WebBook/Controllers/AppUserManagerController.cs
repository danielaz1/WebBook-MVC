using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBook.Models;

namespace WebBook.Controllers
{
    public class AppUserManagerController : Controller
    {
        private AppUserManagerModel db = new AppUserManagerModel();

        // GET: AppUserManager
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                List<UserListViewModel> model = new List<UserListViewModel>();
                foreach (var user in db.AspNetUsers.ToList())
                {
                    UserListViewModel userView = new UserListViewModel();

                    userView.Id = user.Id;
                    userView.Login = user.UserName;
                    List<String> listRole = userManager.GetRoles(user.Id).ToList();
                    userView.Role = string.Join(",", listRole);
                    model.Add(userView);
                }

                return View(model);
            }
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult Edit(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                UserEditViewModel model = new UserEditViewModel();
                var uM = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                ViewBag.RoleId = new SelectList(db.AspNetRoles, "Name", "Name");
                model.roles = uM.GetRoles(id).ToList();
                model.Id = id;
                model.UserName = db.AspNetUsers.FirstOrDefault(x => x.Id == id).UserName;
                return View(model);
            }
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel model, string[] selectedRoles)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            if (User.Identity.IsAuthenticated) 
            {
                try
                {
                    var UserToUpdate = db.AspNetUsers.Find(model.Id);
                    var roles = userManager.GetRoles(model.Id).ToArray();
                    userManager.RemoveFromRoles(model.Id, roles);
                    userManager.AddToRoles(model.Id, selectedRoles);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
        // GET: Management/Delete/5
        public ActionResult Delete(string id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);
                if (aspNetUsers == null)
                {
                    return HttpNotFound();
                }
                return View(aspNetUsers);
            }
            else
                return RedirectToAction("Login", "Account");

        }

        // POST: Management/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUsers aspNetUsers = db.AspNetUsers.Find(id);

            foreach (var role in db.AspNetUserRoles.Where(x => x.UserId == id))
            db.AspNetUserRoles.Remove(role);
            db.AspNetUsers.Remove(aspNetUsers);
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
