using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nedv.Models.Data
{
    public class TypeOfConstruction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип постройки")]
        [Display(Name = "Тип постройки")]
        public string TypeOfConstructionName { get; set; }
    }
}
