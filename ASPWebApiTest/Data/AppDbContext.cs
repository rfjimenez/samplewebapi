using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ASPWebApiTest.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Hero> Heroes { get; set; }

        public void AddTestData()
        {
            using (StreamReader reader = new StreamReader("./Data/hero_stats.csv"))
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    var parts = s.Split(',');
                    var hero = new Hero
                    {
                        Name = parts[0],
                        Type = parts[1],
                        BaseAgi = double.Parse(parts[2]),
                        BaseInt = double.Parse(parts[3]),
                        BaseStr = double.Parse(parts[4])
                    };
                    Heroes.Add(hero);
                    SaveChanges();
                }
            }
        }

    }
}
