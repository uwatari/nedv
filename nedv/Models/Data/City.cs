using System.ComponentModel.DataAnnotations;

namespace nedv.Models.Data
{
    public class City
    {
       
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        public string CityName { get; set; }
    }
}
