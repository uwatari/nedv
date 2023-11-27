using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.Cities
{
    public class CreateCityViewModel
    {
        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        public string CityName { get; set; }
    }
}