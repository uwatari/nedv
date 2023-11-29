using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.AdTypes
{
    public class EditAdTypeViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип объявления")]
        [Display(Name = "Тип объявления")]
        public string AdTypeName { get; set; }
    }
}
