using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;

namespace ParkyApi.Data
{
    public class ParkyApiContext : DbContext
    {
        public ParkyApiContext (DbContextOptions<ParkyApiContext> options)
            : base(options)
        {
        }

        public DbSet<NationalPark> NationalPark { get; set; }
        public DbSet<Trail> Trail { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
