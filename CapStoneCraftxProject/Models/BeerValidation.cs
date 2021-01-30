using System.ComponentModel.DataAnnotations;

namespace CapStoneCraftxProject.Models
{
    [MetadataType(typeof(BeerValidation))]
    public partial class Beer
    { }

    public class BeerValidation
    {
        [Required]
        public string BeerName { get; set; }
    }
}
