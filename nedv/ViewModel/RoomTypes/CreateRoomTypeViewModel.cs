using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.RoomTypes
{
    public class CreateRoomTypeViewModel
    {
        [Required(ErrorMessage = "Введите тип помещения")]
        [Display(Name = "Тип помещения")]
        public string RoomTypeName { get; set; }
    }
}
