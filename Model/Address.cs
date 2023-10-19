using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace trying.Model
{
    public class Address
    {
        [Key]
        public int AddressID { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
    }

}