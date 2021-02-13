using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserRequest.Server.Data;
using UserRequest.Server.Models;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAsync()
        {
            List<Shared.Topic> model = await applicationDbContext.Topics
                .OrderByDescending(c => c.Created)
                .Select(t => new UserRequest.Shared.Topic
                {
                    Id = t.Id,
                    Text = t.Text,
                    Title = t.Title,
                    Votes = t.Votes.Count(),
                    CommentsCount = t.TopicComments.Count(),
                    Created = t.Created,
                    Author = t.Author.Email
                }).ToListAsync();

            return Ok(model);
        }


        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetails(int topicId)
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
                    Created = t.Created,
                    Author = t.Author.Email,
                    CommentsCount = t.TopicComments.Count(),
                    Comments = t.TopicComments.OrderByDescending(c => c.Created).Select(c => new Shared.TopicComment
                    {
                        Comment = c.Comment,
                        Author = c.Author.Email,
                        Created = c.Created,
                        Id = c.Id
                    })
                }).FirstOrDefaultAsync();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(UserRequest.Shared.Topic model)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(currentUserName);


            Topic topicToCreate = new Topic
            {
                Text = model.Text,
                Title = model.Title,
                Created = DateTime.UtcNow,
                Author = user
            };
            topicToCreate.Votes = new List<TopicVote> { new TopicVote
            {
                Topic = topicToCreate,
                User = user
            } };

            applicationDbContext.Topics.Add(topicToCreate);
            applicationDbContext.SaveChanges();

            Shared.Topic res = await GetTopic(topicToCreate.Id);
            return Ok(res);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> Vote(int topicId)
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
        public async Task<IActionResult> CommentTopic(int topicId, string newComment)
        {
            ClaimsPrincipal currentUser = User;
            string currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = await _userManager.FindByIdAsync(currentUserName);

            applicationDbContext.TopicComments.Add(new TopicComment
            {
                Author = user,
                TopicId = topicId,
                Comment = newComment,
                Created = DateTime.UtcNow
            });
            applicationDbContext.SaveChanges();

            Shared.Topic model = await GetTopic(topicId);
            return Ok(model);
        }
    }
}
