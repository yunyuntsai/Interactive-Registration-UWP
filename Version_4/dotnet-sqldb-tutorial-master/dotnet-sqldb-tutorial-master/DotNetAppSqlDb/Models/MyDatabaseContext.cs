using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DotNetAppSqlDb.Models
{
    public partial class WebApplication2201802_dbEntities : DbContext
    {
        public WebApplication2201802_dbEntities()
            : base("name=WebApplication2201802_dbEntities")
        {
        }

 

        public virtual DbSet<Tag_Profile> Tag_Profile { get; set; }
        public virtual DbSet<Visitor_Photo> Visitor_Photo { get; set; }
        public virtual DbSet<Visitor_Profile> Visitor_Profile { get; set; }
    }
}
