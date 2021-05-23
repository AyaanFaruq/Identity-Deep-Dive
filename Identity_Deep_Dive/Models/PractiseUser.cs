using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_Deep_Dive.Models
{
    public class PractiseUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
