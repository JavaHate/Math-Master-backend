using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JavaHateBE.Data
{
    public class MathMasterDbContextFactory : IDesignTimeDbContextFactory<MathMasterDBContext>
    {
        public MathMasterDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MathMasterDBContext>();
            optionsBuilder.UseSqlite("Data Source=localdatabase.db");

            return new MathMasterDBContext(optionsBuilder.Options);
        }
    }
}