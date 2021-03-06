﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController (MyContext context){
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost ("Register")]
        public IActionResult Register (User register) {
            if (ModelState.IsValid) {
                if (dbContext.Users.Any (u => u.Email == register.Email)) {
                    ModelState.AddModelError ("Email", "That email exists");
                    return View ("Index");
                } else {
                    PasswordHasher<User> hash = new PasswordHasher<User> ();
                    register.Password = hash.HashPassword (register, register.Password);

                    dbContext.Users.Add (register);
                    dbContext.SaveChanges ();

                    return RedirectToAction ("Login");
                }
            } else {
                return View ("Index");
            }

        }

        [HttpGet ("Login")]
        public IActionResult Login () {
            return View ("Login");
        }

        [HttpPost ("SignIn")]
        public IActionResult SignIn (LoginUser log) {
            if (ModelState.IsValid) {
                User check = dbContext.Users.FirstOrDefault (u => u.Email == log.LoginEmail);
                if (check == null) {
                    ModelState.AddModelError ("LoginEmail", "Invalid email or password.");
                    return View ("Login");
                } else {
                    PasswordHasher<LoginUser> compare = new PasswordHasher<LoginUser> ();
                    var result = compare.VerifyHashedPassword (log, check.Password, log.LoginPassword);
                    if (result == 0) {
                        ModelState.AddModelError ("LoginEmail", "Invalid email or password.");
                        return View ("Login");
                    } else {
                        HttpContext.Session.SetInt32 ("UserId", check.UserId);
                        return RedirectToAction("Index", "Wedding");
                    }
                }
            } else {
                return View ("Login");
            }
        }

        [HttpGet ("Logout")]
        public IActionResult Logout () {
            HttpContext.Session.Clear ();
            return RedirectToAction ("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
