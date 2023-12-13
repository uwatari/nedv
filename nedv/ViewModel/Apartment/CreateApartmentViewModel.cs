using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.Apartment
{
    public class CreateApartmentViewModel
    {
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Количество комнат")]
        public short CountRoom { get; set; }

        [Display(Name = "Количество метров квадратных")]
        public short NumberOfSquareMeters { get; set; }

        [Display(Name = "Этаж")]
        public short Floor { get; set; }



        [Required]
        [Display(Name = "Тип объявления")]
        public short IdAdType { get; set; }

        [Required]
        [Display(Name = "Тип объявления")]
        public short IdTypeOfConstruction { get; set; }

        [Required]
        [Display(Name = "Город")]
        public short IdCity { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }
    }
}
