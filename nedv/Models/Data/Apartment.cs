using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace nedv.Models.Data
{
    public class Apartment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Количество комнат")]
        public short CountRoom { get; set; }

        [Display(Name = "Количество метров квадратных")]
        public short NumberOfSquareMeters { get; set; }

        [Display(Name = "Этаж")]
        public short Floor { get; set; }

        [Display(Name = "Изображение")]
        public string? ImgUrl { get; set; }

        [Required(ErrorMessage = "Поле 'Изображение' обязательно для заполнения")]
        [NotMapped]
        [Display(Name = "Загрузить изображение")]
        public IFormFile ImageFile { get; set; }


        [Required]
        [Display(Name = "Тип объявления")]
        public short IdAdType { get; set; }

        [Required]
        [Display(Name = "Тип постройки")]
        public short IdTypeOfConstruction { get; set; }

        [Required]
        [Display(Name = "Город")]
        public short IdCity { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public string IdUser { get; set; }

        //НАВИГАЦИОННЫЕ СВОЙСТВА

        //тип объявления
        [ForeignKey("IdAdType")]
        [Display(Name = "Тип объявления")]
        public AdType AdType { get; set; }

        //тип застройки
        [ForeignKey("IdTypeOfConstruction")]
        [Display(Name = "Тип застройки")]
        public TypeOfConstruction TypeOfConstruction { get; set; }

        //Город
        [ForeignKey("IdCity")]
        [Display(Name = "Город")]
        public City City { get; set; }

        //Пользователь
        [ForeignKey("IdUser")]
        [Display(Name = "Пользователь")]
        public User User { get; set; }
    }
}
