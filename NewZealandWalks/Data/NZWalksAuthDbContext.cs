using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewZealandWalks.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var readerRoleId = "9dd69df3-8bbe-4fc1-add5-27c4b798661b";
            var writerRoleId = "57f6141a-bf5a-4c05-bdf7-e0c41229c285";

            var roles = new List<IdentityRole>
                {
                  new IdentityRole
                  {
                      Id = readerRoleId,
                      ConcurrencyStamp = readerRoleId,
                      Name = "Reader",
                      NormalizedName = "Reader".ToUpper()
                  },
                  new IdentityRole 
                  {
                      Id = writerRoleId,
                      ConcurrencyStamp = writerRoleId,
                      Name = "Writer",
                      NormalizedName = "Writer".ToUpper()
                  }
                };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
