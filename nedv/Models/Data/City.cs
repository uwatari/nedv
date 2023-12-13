using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nedv.Models.Data
{
    public class City
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        public string CityName { get; set; }

        [Required]
        [Display(Name = "Область")]
        public short IdRegion { get; set; }

        // Навигационные свойства
        [ForeignKey("IdRegion")]
        [Display(Name = "Область")]
        public Region Region { get; set; }
    }
}
