using System;
using System.ComponentModel.DataAnnotations;

namespace Warlord.Model
{
    public class Vehicle
    {
        #region Public Properties

        [Required]
        public string Color { get; set; }

        public string Condition { get; set; }

        [Required]
        public DateTime DateOfManufacture { get; set; }

        public string Imagepath { get; set; }

        [Key]
        public int Id { get; set; }

        public Order Order { get; set; }

        public int? OrderId { get; set; }

        [Required]
        public int Price { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public VehicleModel VehicleModel { get; set; }

        [Required]
        public int VehicleModelId { get; set; }

        #endregion
    }
}