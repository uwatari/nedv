using System.ComponentModel.DataAnnotations;

namespace nedv.Models.Data
{
    public class RoomType
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип помещения")]
        [Display(Name = "Тип помещения")]
        public string RoomTypeName { get; set; }
    }
}
