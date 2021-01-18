using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using UserRequest.Server.Data;
using UserRequest.Server.Models;

namespace UserRequest.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TopicsController : ControllerBase
    {
        private readonly ILogger<TopicsController> logger;

        private readonly ApplicationDbContext applicationDbContext;

        private readonly UserManager<ApplicationUser> _userManager;

        public TopicsController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext,ILogger<TopicsController> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> GetAsync()
        {
            System.Collections.Generic.List<Shared.Topic> model = applicationDbContext.Topics.Select(t => new UserRequest.Shared.Topic
            {
                Comments = t.TopicComments.Count(),
                Id = t.Id,
                Text = t.Text,
                Title = t.Title,
                Votes = t.Votes.Count()
            }).ToList();

            return Ok(model);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create(UserRequest.Shared.Topic model)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(currentUserName);

            //ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            applicationDbContext.Add(new Topic
            {
                Text = model.Text,
                Title = model.Title,
                Created = DateTime.UtcNow,
                Author = user

            });
            applicationDbContext.SaveChanges();

            return Ok(model);
        }
    }
}
