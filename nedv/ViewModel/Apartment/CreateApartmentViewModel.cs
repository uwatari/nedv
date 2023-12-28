﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
