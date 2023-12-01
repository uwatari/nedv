using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.Regions
{
    public class CreateRegionViewModel
    {
        [Required(ErrorMessage = "Введите область")]
        [Display(Name = "Область")]
        public string RegionName { get; set; }
    }
}