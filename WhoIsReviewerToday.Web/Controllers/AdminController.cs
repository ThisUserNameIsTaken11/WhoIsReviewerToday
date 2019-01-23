using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Infrastructure.Services;
using WhoIsReviewerToday.Web.ViewModels;

namespace WhoIsReviewerToday.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IReviewViewModelFactory _reviewViewModelFactory;
        private readonly IAppointDutyOnReviewService _appointDutyOnReviewService;

        public AdminController(IReviewViewModelFactory reviewViewModelFactory,
            IAppointDutyOnReviewService appointDutyOnReviewService)
        {
            _reviewViewModelFactory = reviewViewModelFactory;
            _appointDutyOnReviewService = appointDutyOnReviewService;
        }

        public IActionResult Index() => View(_reviewViewModelFactory.CreateList());

        [HttpPost]
        public async Task<IActionResult> AppointDuties(Team team)
        {
            var result = await _appointDutyOnReviewService.TryAppointDutiesAndSaveAsync(DateTime.Now, team);
            if (!result)
                return BadRequest();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Index(ReviewViewModel[] reviewViewModels) => RedirectToAction("Index", "Home");
    }
}