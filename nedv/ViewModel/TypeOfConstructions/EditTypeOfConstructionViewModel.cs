using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.TypeOfConstructions
{
    public class EditTypeOfConstructionViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип постройки")]
        [Display(Name = "Тип постройки")]
        public string TypeOfConstructionName { get; set; }
    }
}
