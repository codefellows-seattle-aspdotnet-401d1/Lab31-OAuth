using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDay2.Models
{
    public class CMS
    {
        public int ID { get; set; }
        public Channel Channel { get; set; }
        public string Content { get; set; }
        public bool IsAuthorized { get; set; }
    }
}
