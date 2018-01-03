using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Warlord.Model
{
    public class Order
    {
        #region Constructors and Destructors

        public Order()
        {
            Vehicles = new Collection<Vehicle>();
        }

        #endregion

        #region Public Properties

        [Required]
        public bool Completed { get; set; }

        public Customer Customer { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        #endregion
    }
}