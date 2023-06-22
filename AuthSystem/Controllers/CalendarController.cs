﻿using AuthSystem.Areas.Identity.Data;
using AuthSystem.Data;
using AuthSystem.Migrations;
using AuthSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Controllers
{
    public class CalendarController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _test;
        public CalendarController(AuthDbContext test , UserManager<ApplicationUser> userManager)
        {

            _test = test;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new Test
            {
                TestList = _test.Tests.OrderByDescending(q => q.Id).ToList(),
                Subjects = _test.Subjects.Include(td => td.Subjects).ToList(),
                TestDetails = _test.TestsDetail.Include(td => td.Test).ToList(),
                TestCalenders = _test.TestCalenders.Include(td => td.Test).Include(td => td.TestCenter).ToList(),
            };

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                viewModel.UserCalendars = _test.UserCalendars
                    .Where(uc => uc.UserId == user.Id)
                    .ToList();
            }

            return View(viewModel);
        }


        public async Task<IActionResult> SelectTest(int testId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userId = user.Id;

            var existingUserCalendar = _test.UserCalendars.FirstOrDefault(uc => uc.UserId == userId && uc.TestId == testId);

            if (existingUserCalendar == null)
            {
                _test.UserCalendars.Add(new UserCalendars
                {
                    UserId = userId,
                    TestId = testId,

                    SelectionTime = DateTime.Now,

                });

                _test.SaveChanges();
                return Conflict("Calendar Updated!");
            }



            

            return Ok("Applied Successfully!");
        }



        public async Task<IActionResult> UserCalendars()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Content("User not found");
            }

            var userId = user.Id;

            var userCalendars = _test.UserCalendars
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Test)
                .Include(uc => uc.Calendar).Include(uc => uc.Calendar.TestCenter)
                .ToList();
            if (userCalendars != null) {

                return View(userCalendars);

            }
            return NoContent();

        }
        public IActionResult PrintVoucher(int testId, string testName , string applicantName)
        {

            try
            {

                var test = _test.Tests.FirstOrDefault(q => q.Id == testId);
                if (test != null) {

                    var feeVoucher = new Models.FeeVoucher
                    {

                        TestName = test.TestName,
                        Amount = 5000,
                        ApplicantName = applicantName,
                        isPaid = true


                    };
                     _test.FeeVoucher.Add(feeVoucher);
                    _test.SaveChanges();
                    return View("PrintVoucher", feeVoucher);
                }
                return NotFound();
            }
            catch (Exception e)
            {

                return Json(new { Error = e.Message });


            }


        }
        public async Task<IActionResult> SelectCalendar()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Content("User not found");
            }

            var userId = user.Id;
            var appliedTests = _test.TestCalenders
                .Where(tc => _test.UserCalendars.Any(uc => uc.UserId == userId && uc.TestId == tc.TestId))
                .Include(tc => tc.Test)
                .Include(tc => tc.TestCenter)
                .ToList();

            if (appliedTests.Count > 0)
            {
                ViewBag.UserID = userId;
                return View("SelectCalendar", appliedTests);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SelectCalendarUser(int testId, int calendarId, string calendarToken)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Content("User not found");
            }

            var userId = user.Id;
            var appliedTest = _test.UserCalendars.FirstOrDefault(q => q.UserId == userId && q.TestId == testId);

            if (appliedTest != null)
            {
                if (appliedTest.CalendarId != calendarId && appliedTest.CalenderToken != calendarToken)
                {
                    appliedTest.CalenderToken = calendarToken;
                    appliedTest.CalendarId = calendarId;
                    _test.SaveChanges();
                }
                else
                {
                    appliedTest.CalenderToken = calendarToken;
                    _test.SaveChanges();
                }
            }

            return Ok();
        }




    }
}
