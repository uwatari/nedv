using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace nedv.Models.Data
{
    public class RoomType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип помещения")]
        [Display(Name = "Тип помещения")]
        public string RoomTypeName { get; set; }
    }
}
