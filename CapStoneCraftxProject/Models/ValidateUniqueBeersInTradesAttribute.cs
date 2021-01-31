using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapStoneCraftxProject.Models
{
    public class ValidateUniqueBeersInTradesAttribute : ValidationAttribute
    {
        private CapStoneProjectEntities5 db = new CapStoneProjectEntities5();

        protected override ValidationResult
               IsValid(object value, ValidationContext validationContext)
        {
            Trade trade =(Trade)value;
            var sendingresult = db.Trades
                .Where(t => t.SendingBeerId == trade.SendingBeerId)
                .Where(t => t.SendingMemberId == trade.SendingMemberId)
                .Where(t => t.IsApproved == null)
                .Where(t => t.Id != trade.Id);

            var receivingresult = db.Trades
                .Where(t => t.ReceivingMemberId == trade.ReceivingMemberId)
                .Where(t => t.ReceiverBeerId == trade.ReceiverBeerId)
                .Where(t => t.IsApproved == null)
                .Where(t => t.Id != trade.Id);
            if (sendingresult.Any())
            {
                return new ValidationResult
                    ("This beer is in your pending trades,please chooose another.");
            }
            if (receivingresult.Any())
            {
                return new ValidationResult
                    ("This beer is already in a pending trade,please choose another.");
            }
            return ValidationResult.Success;
        }
    }
}
