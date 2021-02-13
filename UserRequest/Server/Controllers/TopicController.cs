using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserRequest.Server.Data;
using UserRequest.Server.Models;

namespace UserRequest.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ILogger<TopicController> logger;

        private readonly ApplicationDbContext applicationDbContext;

        private readonly UserManager<ApplicationUser> _userManager;

        public TopicController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext, ILogger<TopicController> logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<IActionResult> GetAsync()
        {
            System.Collections.Generic.List<Shared.Topic> model = await applicationDbContext.Topics.Select(t => new UserRequest.Shared.Topic
            {
                Id = t.Id,
                Text = t.Text,
                Title = t.Title,
                Votes = t.Votes.Count()
            }).ToListAsync();

            return Ok(model);
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async System.Threading.Tasks.Task<IActionResult> GetDetails(int topicId)
        {
            Shared.Topic model = await GetTopic(topicId);

            return Ok(model);
        }

        private async Task<Shared.Topic> GetTopic(int topicId)
        {
            return await applicationDbContext.Topics
                            .Where(t => t.Id == topicId)
                            .Include(t => t.TopicComments)
                            .Select(t => new Shared.Topic
                            {
                                Id = t.Id,
                                Text = t.Text,
                                Title = t.Title,
                                Votes = t.Votes.Count(),
                                Comments = t.TopicComments.Select(c => new Shared.TopicComment
                                {
                                    Comment = c.Comment,
                                    Author = c.Author.Email,
                                    Id = c.Id
                                })
                            }).FirstOrDefaultAsync();
        }

        [Authorize]
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> Create(UserRequest.Shared.Topic model)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(currentUserName);

            //ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            var topic = applicationDbContext.Topics.Add(new Topic
            {
                Text = model.Text,
                Title = model.Title,
                Created = DateTime.UtcNow,
                Author = user

            });

            applicationDbContext.TopicVotes.Add(new TopicVote
            {
                TopicId = topic.Entity.Id,
                User = user
            });

            applicationDbContext.SaveChanges();

            return Ok(model);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> Vote(int topicId)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(currentUserName);

            TopicVote existingVote = applicationDbContext.TopicVotes.FirstOrDefault(v => v.User == user && v.TopicId == topicId);
            if (existingVote == null)
            {
                applicationDbContext.TopicVotes.Add(new TopicVote
                {
                    User = user,
                    TopicId = topicId
                });
                applicationDbContext.SaveChanges();
            }

            return Ok(applicationDbContext.TopicVotes.Count(t => t.TopicId == topicId));
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> CommentTopic(int topicId, string newComment)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(currentUserName);

            applicationDbContext.TopicComments.Add(new TopicComment
            {
                Author = user,
                TopicId = topicId,
                Comment = newComment
            });
            applicationDbContext.SaveChanges();

            Shared.Topic model = await GetTopic(topicId);
            return Ok(model);
        }
    }
}
