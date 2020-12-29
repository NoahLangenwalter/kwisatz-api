using LiteDB;
using kwisatz.Data.Entities;

namespace kwisatz.Data
{
    public class LiteDbContext : ILiteDbContext
    {
        public LiteDatabase Database { get; }

        public LiteDbContext()
        {
            Database = new LiteDatabase("data.db");
        }
    }
    
    public interface ILiteDbContext
    {
        LiteDatabase Database { get; }
    }
}