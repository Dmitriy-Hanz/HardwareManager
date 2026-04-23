using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class Role
    {
        public long Id { get; set; }
        public string RoleName { get; set; }

        public Role() { }
        public Role(long id, string roleName)
        {
            Id = id;
            RoleName = roleName;
        }
    }
}
