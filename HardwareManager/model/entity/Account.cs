using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model
{
    public class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Role Role { get; set; }
        
        public Account() { }
        public Account(long id, string username, string password, Role role)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
        }
    }
}
