using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapStoneCraftxProject.Models
{
    public class EmailViewModel
    {
              
            [Required, Display(Name = "Name")]
            public string FromName { get; set; }
            [Required, Display(Name = "Email"), EmailAddress]
            public string FromEmail { get; set; }
            [Required, Display(Name = "Message")]
            public string Message { get; set; }
        
    }
}