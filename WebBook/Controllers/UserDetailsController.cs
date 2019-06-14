using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBook.Models;
using Microsoft.AspNet.Identity;

namespace WebBook.Controllers
{
    public class UserDetailsController : Controller
    {
        private UserDetailsModel db = new UserDetailsModel();
        private AppUserManagerModel appDB = new AppUserManagerModel();


        // GET: UserDetails
        public ActionResult Index(string msg)
        {
            string myId = User.Identity.GetUserId();
            List<FriendsView> model = new List<FriendsView>();
            foreach(var user in db.UserDetails.ToList())
            {
                FriendsView friend = new FriendsView();
                friend.friend = user;
                if(db.Friends.Any(x=>x.User1Id==user.Id && x.User2Id==myId) || db.Friends.Any(x => x.User1Id ==myId && x.User2Id == user.Id))
                {
                    friend.friends = true;
                }
                model.Add(friend);
            }

            if(!String.IsNullOrEmpty(msg))
            {
                TempData["msg"] = "<script>alert('"+msg+"');</script>";
            }

            return View(model.Where(x=>x.friend.OwnerID!=myId));
        }
        public ActionResult AddFriend(string id)
        {
            ViewBag.Id = id;
            ViewBag.Id2 = User.Identity.GetUserId();
            ViewBag.name = db.UserDetails.FirstOrDefault(x => x.Id == id).Name;
            return View();
        }

        [HttpPost]
        public ActionResult AddFriend(string id,string id2)
        {
            string name = db.UserDetails.FirstOrDefault(x => x.Id == id).Name;
            Friends friend = new Friends();
            friend.User1Id = id;
            friend.User2Id = User.Identity.GetUserId();
            db.Friends.Add(friend);
            db.SaveChanges();
            return RedirectToAction("Index", new { msg = "Pomyslnie dodano " + name + " do znajomych" }); 
        }

        public ActionResult RemoveFriend(string id)
        {
            ViewBag.Id = id;
            ViewBag.Id2 = User.Identity.GetUserId();
            ViewBag.name = db.UserDetails.FirstOrDefault(x => x.Id == id).Name;
            return View();
        }



        [HttpPost]
        public ActionResult RemoveFriend(string id, string id2)
        {
            string name = db.UserDetails.FirstOrDefault(x => x.Id == id).Name;
            Friends friend = new Friends();
            if (db.Friends.Any(x => x.User1Id == id && x.User2Id == id2))
            {
                friend = db.Friends.FirstOrDefault(x => x.User1Id == id && x.User2Id == id2);
            }
            else if(db.Friends.Any(x => x.User1Id == id2 && x.User2Id == id))
            {
                friend = db.Friends.FirstOrDefault(x => x.User1Id == id2 && x.User2Id == id);
            }
            if(friend!=null)
            {
                db.Friends.Remove(friend);
                db.SaveChanges();
            }
           
            return RedirectToAction("Index", new { msg = "Pomyslnie usunięto " + name + " ze znajomych" });
        }


        public ActionResult MyFriends(string id)
        {
            if(id == null)
            {
                id = User.Identity.GetUserId();
            }

            List<UserDetails> friends = new List<UserDetails>();
            foreach(var friends1 in db.Friends.Where(x=>x.User1Id==id))
            {
                friends.Add(db.UserDetails.FirstOrDefault(x => x.Id == friends1.User2Id));
            }
            foreach (var friends2 in db.Friends.Where(x => x.User2Id == id))
            {
                friends.Add(db.UserDetails.FirstOrDefault(x => x.Id == friends2.User1Id));
            }
            return View(friends.Distinct());
        }

        public ActionResult MyFriendsPartial() { 
        
            string id = User.Identity.GetUserId();
        

        List<UserDetails> friends = new List<UserDetails>();
            foreach(var friends1 in db.Friends.Where(x=>x.User1Id==id))
            {
                friends.Add(db.UserDetails.FirstOrDefault(x => x.Id == friends1.User2Id));
            }
            foreach (var friends2 in db.Friends.Where(x => x.User2Id == id))
            {
                friends.Add(db.UserDetails.FirstOrDefault(x => x.Id == friends2.User1Id));
            }
            return PartialView(friends);
        }

        // GET: UserDetails/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            UserDetails userDetails;
            if (id == null)
            {
                string uid = User.Identity.GetUserId<string>();
                userDetails = db.UserDetails.Where(d => d.OwnerID.Equals(uid)).SingleOrDefault();
            } else
            {
                userDetails = db.UserDetails.Find(id);
            }
            if (userDetails == null)
            {
                   string url = "Create";
                   return RedirectToAction(url);
            }
            return View(userDetails);
        }

        // GET: UserDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Name,Surname,Country,City,Birthday")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId<string>();
                userDetails.OwnerID = userId;
                userDetails.Id = userId;
                db.UserDetails.Add(userDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userDetails);
        }

        // GET: UserDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UserDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OwnerID,Name,Surname,Country,City,Birthday")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userDetails);
        }

        // GET: UserDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDetails userDetails = db.UserDetails.Find(id);
            if (userDetails == null)
            {
                return HttpNotFound();
            }
            return View(userDetails);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDetails userDetails = db.UserDetails.Find(id);
            db.UserDetails.Remove(userDetails);
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
