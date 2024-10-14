using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAndDataProtectionExample.Context
{
    public class IdentityAppDbContext : IdentityDbContext<IdentityUser>
    {
        public IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options): base(options)
        {
            
        }


    }
}
