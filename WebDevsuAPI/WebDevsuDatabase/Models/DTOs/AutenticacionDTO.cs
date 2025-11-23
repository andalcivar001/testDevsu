using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDvpDatabase.Models.DTOs

{
    public class AutenticacionDTO
    {
    }

    public class LoginReq
    {
        public string Username { get; set; } 
        public string Password { get; set; }
    }

    public class RegisterReq
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
