using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapStoneCraftxProject.Models
{
    [ValidateUniqueBeersInTrades]
    public partial class Trade
    {[Display(Name = "Trade Status")]
        public string  tradesstatus{
            get
            {
                if (IsApproved.HasValue && IsApproved.Value)
                {
                    return "Accepted";
                }
                else if (IsApproved.HasValue)
                {
                    return "Denied";
                }
                else return "Trade Pending";

            }
        
        
        }
    }
}