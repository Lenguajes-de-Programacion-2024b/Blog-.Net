using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blog_.Net.Models
{
    public class InfoUser
    {
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Passcode { get; set; }


        public string ConfirmPasscode { get; set; }
    }
}