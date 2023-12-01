using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.Cities
{
    public class EditCityViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        public string CityName { get; set; }

        [Required]
        [Display(Name = "Область")]
        public short IdRegion { get; set; }
    }
}
