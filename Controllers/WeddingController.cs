using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    [Route("wedding")]
    public class WeddingController : Controller
    {
        private MyContext dbContext;

        public WeddingController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            User userindb = LoggedIn();
            if(userindb == null)
            {
                return RedirectToAction("Logout");
            }

            // first is many-to-many relationship (Include and ThenInclude) then one-to-many (Include)
            List<Wedding> AllWeddings = dbContext.Weddings.Include(w=>w.GuestList).ThenInclude(r=>r.Guest).Include(w=>w.Planner).OrderBy(w=>w.Date).ToList(); 

            ViewBag.User = LoggedIn();
            
            return View(AllWeddings);
        }

        [HttpGet ("Logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index", "Home");
        }

        private User LoggedIn()
        {
            User LogIn = dbContext.Users.Include(u=>u.PlannedWeddings).FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("UserId"));

            return LogIn;
        }

        [HttpGet("new/wedding")]
        public IActionResult NewWedding()
        {
            User userInDb = LoggedIn();
            ViewBag.User = userInDb;
            return View();
        }

        [HttpPost("create/wedding")]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            User userInDb = LoggedIn();
            if(ModelState.IsValid)
            {
                dbContext.Weddings.Add(newWedding);
                dbContext.SaveChanges();
                return Redirect($"/wedding/{newWedding.WeddingId}");
            }
            else
            {
                ViewBag.User = userInDb;
                return View("NewWedding");
            }
        }

        [HttpGet("{weddingId}")]
        public IActionResult ShowWedding(int weddingId)
        {
            User userInDb = LoggedIn();

            Wedding show = dbContext.Weddings.Include(w=>w.GuestList).ThenInclude(r=>r.Guest).Include(w=>w.Planner).FirstOrDefault(w=>w.WeddingId == weddingId);

            ViewBag.User = userInDb;

            return View(show);
        }

        [HttpGet("delete/{weddingId}")]
        public IActionResult DeleteWedding(int weddingId)
        {
            User userInDb = LoggedIn();
            Wedding remove = dbContext.Weddings.FirstOrDefault(w=>w.WeddingId == weddingId);
            dbContext.Weddings.Remove(remove);
            dbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        // NOTE: can use LINQ in views if it's with model; in ViewBag have to use only for loops

        [HttpGet("response/{weddingId}/{userId}/{status}")]
        public IActionResult Rsvp(int weddingId, int userId, string status)
        {
            User userInDb = LoggedIn();
            if(status == "RSVP")
            {
                RSVP going = new RSVP();
                going.UserId = userId;
                going.WeddingId = weddingId;
                dbContext.RSVPs.Add(going);
                dbContext.SaveChanges();
            }
            else if(status == "Un-RSVP")
            {
                RSVP unrsvp = dbContext.RSVPs.FirstOrDefault(r=>r.WeddingId == weddingId && r.UserId == userId);
                dbContext.RSVPs.Remove(unrsvp);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        

        // [HttpGet("NewWedding")]
        // public IActionResult NewWedding()
        // {
        //     ViewBag.User = dbContext.Users.Include(u=>u.PlannedWeddings).FirstOrDefault (u => u.UserId == HttpContext.Session.GetInt32 ("UserId"));
        //     return View();
        // }

        // [HttpPost("Process")]
        // public IActionResult Process(Wedding addWedding)
        // {
        //     ViewBag.User = dbContext.Users.Include(u=>u.PlannedWeddings).FirstOrDefault (u => u.UserId == HttpContext.Session.GetInt32 ("UserId"));
        //     if(ModelState.IsValid)
        //     {
        //         dbContext.Weddings.Add(addWedding);
        //         dbContext.SaveChanges();
        //         return RedirectToAction("Index", "Wedding");
        //     }
        //     else
        //     {
        //         ViewBag.Weddings = dbContext.Weddings.ToList();
        //         return View("NewWedding");
        //     }
        // }

        // [HttpGet("OneWedding")]
        // public IActionResult OneWedding(int weddingId)
        // {
        //     ViewBag.Wedding = dbContext.Weddings.Include(p=>p.GuestList).ThenInclude(a=>a.UserId).FirstOrDefault(p=>p.WeddingId == weddingId);

        //     return View();
        // }

    }
}