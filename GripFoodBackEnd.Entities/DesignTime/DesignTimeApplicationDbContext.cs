using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripFoodBackEnd.Entities.DesignTime
{
    public class DesignTimeApplicationDbContext: IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("Data Source=local.db");
            optionsBuilder.UseOpenIddict();

            var db = new ApplicationDbContext(optionsBuilder.Options);
            return db;
        }
    }
}
