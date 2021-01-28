using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CapStoneCraftxProject.Models
{
    public enum TradeListType
    {
        [Display(Name ="Pending Offers Received")]
        PendingOffersRecieved,
        [Display(Name ="Pending Offers Made")]
        PendingOffersMade,
        [Display(Name ="All Offers Made")]
        AllOffersMade,
        [Display(Name ="All Offers Recieved")]
        AllOffersRecieved,
        [Display(Name = "Accepted Offers")]
        AcceptedOffers,
        [Display(Name = "Denied Offers")]
        DeniedOffers,
    }
}