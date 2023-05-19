using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DBApp
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DbConnection") 
        { }

        public DbSet<Subscriber> Subscribers { get; set; }
    }
}
