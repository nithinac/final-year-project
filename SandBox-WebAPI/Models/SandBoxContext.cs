using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SandBox_WebAPI.Models
{
    public class SandBoxContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SandBoxContext() : base("name=SandBoxContext")
        {
        }

        public System.Data.Entity.DbSet<SandBox_WebAPI.Models.SafetyInstruction> SafetyInstructions { get; set; }

        public System.Data.Entity.DbSet<SandBox_WebAPI.Models.SafetyQuestion> SafetyQuestions { get; set; }

        public System.Data.Entity.DbSet<SandBox_WebAPI.Models.ContentType> ContentTypes { get; set; }

        public System.Data.Entity.DbSet<SandBox_WebAPI.Models.MaintenanceTask> MaintenanceTasks { get; set; }

        public System.Data.Entity.DbSet<SandBox_WebAPI.Models.DoneOnDueOn> DoneOnDueOns { get; set; }

        public System.Data.Entity.DbSet<SandBox_WebAPI.Models.Attachment> Attachments { get; set; }
    }
}
