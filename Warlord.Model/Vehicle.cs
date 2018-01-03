using System;
using System.ComponentModel.DataAnnotations;

namespace Warlord.Model
{
    public class Vehicle
    {
        #region Public Properties

        [Required]
        [StringLength(30)]
        public string Color { get; set; }

        public string Condition { get; set; }

        [Required]
        public DateTime DateOfManufacture { get; set; }

        public string Filename { get; set; }

        [Key]
        public int Id { get; set; }

        public Order Order { get; set; }

        [Required]
        public float Price { get; set; }

        public VehicleModel VehicleModel { get; set; }

        [Required]
        public int VehicleModelId { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
        #endregion
    }
}