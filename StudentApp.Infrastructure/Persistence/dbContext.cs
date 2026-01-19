using Microsoft.EntityFrameworkCore;
using StudentApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentApp.Infrastructure.Persistence
{
    public partial class myDbContext : DbContext
    {
        public myDbContext() { }

        public myDbContext(DbContextOptions<myDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
    }
}
