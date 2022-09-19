
using ContactManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Models.Data;

public class ContactDbContext : DbContext
{

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Job> Jobs { get; set; }
   
    public ContactDbContext(DbContextOptions<ContactDbContext> options)
        : base(options)
    {
            base.Database.EnsureCreated();
    }
}