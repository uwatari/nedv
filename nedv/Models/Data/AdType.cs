using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nedv.Models.Data
{
    public class AdType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип объявления")]
        [Display(Name = "Тип объявления")]
        public string AdTypeName { get; set; }
    }
}
