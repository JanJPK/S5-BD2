using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Warlord.Model
{
    public class VehicleModel
    {
        #region Constructors and Destructors

        public VehicleModel()
        {
            Vehicles = new Collection<Vehicle>();
        }

        #endregion

        #region Public Properties

        [Required]
        public int Crew { get; set; }

        [Required]
        [StringLength(50)]
        public string Engine { get; set; }

        [Required]
        public int EnginePower { get; set; }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string MainArmament { get; set; }

        public Manufacturer Manufacturer { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [StringLength(50)]
        public string SecondaryArmament { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }

        [Required]
        public float Weight { get; set; }

        #endregion
    }
}