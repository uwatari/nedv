using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.TypeOfConstructions
{
    public class CreateTypeOfConstructionViewModel
    {
        [Required(ErrorMessage = "Введите тип постройки")]
        [Display(Name = "Тип постройки")]
        public string TypeOfConstructionName { get; set; }
    }
}
