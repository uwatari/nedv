using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.AdTypes
{
    public class CreateAdTypeViewModel
    {
        [Required(ErrorMessage = "Введите тип объявления")]
        [Display(Name = "Тип объявления")]
        public string AdTypeName { get; set; }
    }
}
