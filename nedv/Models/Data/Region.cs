using System.ComponentModel.DataAnnotations;

namespace nedv.Models.Data
{
    public class Region
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите область")]
        [Display(Name = "Область")]
        public string RegionName { get; set; }
    }
}
