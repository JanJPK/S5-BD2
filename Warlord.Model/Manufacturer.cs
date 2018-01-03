using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Warlord.Model
{
    public class Manufacturer
    {
        #region Constructors and Destructors

        public Manufacturer()
        {
            VehicleModels = new Collection<VehicleModel>();
        }

        #endregion

        #region Public Properties

        [Required]
        [StringLength(30)]
        public string Country { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Key]
        public int Id { get; set; }

        public ICollection<VehicleModel> VehicleModels { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [StringLength(20)]
        public string ShortName { get; set; }

        #endregion
    }
}