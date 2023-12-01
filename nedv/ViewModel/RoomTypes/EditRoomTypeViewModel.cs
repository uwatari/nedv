using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.RoomTypes
{
    public class EditRoomTypeViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите тип помещения")]
        [Display(Name = "Тип помещения")]
        public string RoomTypeName { get; set; }
    }
}
