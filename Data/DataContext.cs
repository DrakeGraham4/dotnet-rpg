using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        
        //Able to query and save RpgCharacters. The name of the DbSet will be the name of the corresponding database table (characters). Whenever you want to see a representation of your model in the database you have to add a DbSet of that model. That's how Entity knows what tables it should use
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
    }
}