using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nedv.Models.Data
{
    public class Region
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите область")]
        [Display(Name = "Область")]
        public string RegionName { get; set; }
    }
}
