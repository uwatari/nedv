using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.Regions
{
    public class EditRegionViewModel
    {
        public short Id { get; set; }

        [Required(ErrorMessage = "Введите область")]
        [Display(Name = "Область")]
        public string RegionName { get; set; }
    }
}
