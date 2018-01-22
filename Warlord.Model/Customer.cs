using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Warlord.Model
{
    public class Customer
    {
        #region Constructors and Destructors

        public Customer()
        {
            Orders = new Collection<Order>();
        }

        #endregion

        #region Public Properties

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Order> Orders { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string PostalCode { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        #endregion
    }
}