using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.Data.Conventions;

namespace Lombiq.Hosting.Azure.Lucene.Models
{
    public class AppDataFileRecord
    {
        public virtual int Id { get; set; }
        public virtual string Path { get; set; }
        [StringLengthMax]
        public virtual string Content { get; set; }
    }
}