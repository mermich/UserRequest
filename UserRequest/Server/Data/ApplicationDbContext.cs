using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserRequest.Server.Models;

namespace UserRequest.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {

        }
        public DbSet<Topic> Topics { get; set; }

        public DbSet<TopicComment> TopicComments { get; set; }

        public DbSet<TopicVote> TopicVotes { get; set; }
    }
}