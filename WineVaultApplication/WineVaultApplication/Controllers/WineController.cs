using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WineVaultApplication.Models;

namespace WineVaultApplication.Controllers
{
    public class WineController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();
        

        [Authorize]
        public ActionResult MyList()
        {
            var userId = User.Identity.GetUserId();
            var wines = db.wines
                .Where(w => w.UserId == userId)
                .ToList();
            return View(wines);
        }

        [Authorize]
        //public ActionResult WinePosts()
        //{


        //    var currentUserId = User.Identity.Name;
        //    var wines = db.wines
        //        .Where(w => w.UserId != currentUserId)
        //        .OrderByDescending(w => w.CreatedAt)
        //        .ToList();

        //    var userLikes = db.WineLikes
        //        .Where(l => l.UserId == currentUserId)
        //        .Select(l => l.WineId)
        //        .ToList();



        //    foreach (var wine in wines)
        //    {

        //        //var isLiked = db.WineLikes.Any(l => l.WineId == wine.Id && l.UserId == currentUserId);
        //        var isLiked = userLikes.Contains(wine.Id);
        //        wine.IsLikedByCurrentUser = isLiked;
        //        Console.WriteLine($"Wine {wine.Id} is liked: {isLiked}"); 
        //    }
        //    return View(wines);


        //}

        public ActionResult WinePosts()
        {
            var currentUserId = User.Identity.GetUserId();
            System.Diagnostics.Debug.WriteLine($"Current User ID: {currentUserId}");

            // Дебагирање на WineLikes состојба
            var allWineLikes = db.WineLikes.ToList();
            System.Diagnostics.Debug.WriteLine($"Total likes in database: {allWineLikes.Count}");

            // Земи ги сите лајкови за тековниот корисник
            var userLikes = db.WineLikes
                .Where(l => l.UserId == currentUserId)
                .ToList();  // Експлицитно земи ги како листа

            System.Diagnostics.Debug.WriteLine($"Found {userLikes.Count} likes for current user");
            foreach (var like in userLikes)
            {
                System.Diagnostics.Debug.WriteLine($"User liked wine with ID: {like.WineId}");
            }

            // Земи ги вината
            var wines = db.wines
                .Include(w => w.User)
                .OrderByDescending(w => w.CreatedAt)
                .ToList();

            // Провери за секое вино дали е лајкнато
            foreach (var wine in wines)
            {
                // Експлицитна проверка со debugging
                var isLiked = userLikes.Any(l => l.WineId.Equals(wine.Id, StringComparison.OrdinalIgnoreCase));
                wine.IsLikedByCurrentUser = isLiked;
                System.Diagnostics.Debug.WriteLine($"Wine {wine.Id} IsLikedByCurrentUser: {isLiked}");
            }

            return View(wines);
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            var wine = db.wines
                //.Include(w => w.User)
                .FirstOrDefault(w => w.Id == id);

            if (wine == null)
                return HttpNotFound();

            return View(wine);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(Wine wine)
        {
            if (ModelState.IsValid)
            {
                wine.Id = Guid.NewGuid().ToString();
                wine.CreatedAt = DateTime.Now;
                wine.UserId = User.Identity.GetUserId();
                db.wines.Add(wine);
                db.SaveChanges();
                return RedirectToAction("MyList");
            }
            return View(wine);
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id == null)
                return HttpNotFound();

            var wine = db.wines.Find(id);
            if (wine == null || wine.UserId != User.Identity.GetUserId())
                return HttpNotFound();

            return View(wine);
        }

       


        [HttpPost]
        public ActionResult Edit(Wine wine)
        {
            if (ModelState.IsValid)
            {
                var existingWine = db.wines.Find(wine.Id);
                if (existingWine == null)
                    return HttpNotFound();

                
                var originalCreatedAt = existingWine.CreatedAt;
                var originalUserId = existingWine.UserId;
                existingWine.Name = wine.Name;
                existingWine.Description = wine.Description;
                existingWine.Category = wine.Category;
                existingWine.ImagePath = wine.ImagePath;
                existingWine.CreatedAt = originalCreatedAt;
                existingWine.UserId = originalUserId;

                db.SaveChanges();
                return RedirectToAction("MyList");
            }
            return View(wine);
        }


        
        public ActionResult Delete(string id)
        {
            var wine = db.wines.Find(id);
            if (wine == null)
                return HttpNotFound();

            if (wine.UserId != User.Identity.GetUserId())
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);

            try
            {
              
                var comments = db.Comments.Where(c => c.WineId == id);
                db.Comments.RemoveRange(comments);
                var likes = db.WineLikes.Where(l => l.WineId == id);
                db.WineLikes.RemoveRange(likes);
                db.wines.Remove(wine);

                db.SaveChanges();
                return RedirectToAction("MyList");
            }
            catch (Exception ex)
            { 
                System.Diagnostics.Debug.WriteLine($"Error deleting wine: {ex.Message}");
                TempData["Error"] = "Настана грешка при бришење на виното.";
                return RedirectToAction("MyList");
            }
        }







        //[HttpPost]
        //public JsonResult Like(string id)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //        return Json(new { success = false });

        //    var userId = User.Identity.GetUserId();
        //    var wine = db.wines.Find(id);

        //    if (wine == null)
        //        return Json(new { success = false });

        //    var existingLike = db.WineLikes.FirstOrDefault(l => l.WineId == id && l.UserId == userId);
        //    bool isLiked;

        //    if (existingLike == null)
        //    {
        //        // Додавање лајк - ќе биде црвено
        //        db.WineLikes.Add(new WineLike { WineId = id, UserId = userId });
        //        wine.LikesCount++;
        //        isLiked = true;
        //    }
        //    else
        //    {
        //        // Бришење лајк - ќе биде сиво
        //        db.WineLikes.Remove(existingLike);
        //        wine.LikesCount = Math.Max(0, wine.LikesCount - 1);
        //        isLiked = false;
        //    }

        //    db.SaveChanges();
        //    return Json(new { success = true, isLiked = isLiked, likesCount = wine.LikesCount });
        //}

        //[HttpPost]
        //public JsonResult Like(string id)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //        return Json(new { success = false });

        //    var userId = User.Identity.GetUserId();
        //    var wine = db.wines.Find(id);

        //    if (wine == null)
        //        return Json(new { success = false });

        //    var existingLike = db.WineLikes.FirstOrDefault(l => l.WineId == id && l.UserId == userId);

        //    if (existingLike == null)
        //    {
        //        // Додавање лајк
        //        db.WineLikes.Add(new WineLike { WineId = id, UserId = userId });
        //        wine.LikesCount++;
        //        db.SaveChanges();
        //        return Json(new { success = true, isLiked = true, likesCount = wine.LikesCount });
        //    }
        //    else
        //    {
        //        // Бришење лајк
        //        db.WineLikes.Remove(existingLike);
        //        wine.LikesCount = Math.Max(0, wine.LikesCount - 1);
        //        db.SaveChanges();
        //        return Json(new { success = true, isLiked = false, likesCount = wine.LikesCount });
        //    }
        //}
        [HttpPost]
        public JsonResult Like(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false });

            var userId = User.Identity.GetUserId();
            var wine = db.wines.Find(id);

            if (wine == null)
                return Json(new { success = false });

            var existingLike = db.WineLikes.FirstOrDefault(l => l.WineId == id && l.UserId == userId);
            bool isLiked;

            if (existingLike == null)
            {
                // Додавање лајк
                db.WineLikes.Add(new WineLike { WineId = id, UserId = userId });
                wine.LikesCount++;
                isLiked = true;
            }
            else
            {
                // Бришење лајк
                db.WineLikes.Remove(existingLike);
                wine.LikesCount = Math.Max(0, wine.LikesCount - 1);
                isLiked = false;
            }

            db.SaveChanges();
            return Json(new { success = true, isLiked = isLiked, likesCount = wine.LikesCount });
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(string wineId, string content)
        {
            if (string.IsNullOrEmpty(content))
                return RedirectToAction("Details", new { id = wineId });

            var comment = new Comment
            {
                Content = content,
                WineId = wineId,
                UserId = User.Identity.GetUserId(),
                CreatedAt = DateTime.Now
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = wineId });
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
