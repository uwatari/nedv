using System.ComponentModel.DataAnnotations;

namespace nedv.Models.Data
{
    public class AdType
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип объявления")]
        [Display(Name = "Тип объявления")]
        public string AdTypeName { get; set; }
    }
}
