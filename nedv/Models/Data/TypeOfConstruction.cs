using System.ComponentModel.DataAnnotations;

namespace nedv.Models.Data
{
    public class TypeOfConstruction
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип постройки")]
        [Display(Name = "Тип постройки")]
        public string TypeOfConstructionName { get; set; }
    }
}
