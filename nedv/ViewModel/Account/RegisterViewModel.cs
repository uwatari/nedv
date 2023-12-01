using System.ComponentModel.DataAnnotations;

namespace nedv.ViewModel.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите E-mail")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]   // тип элемента управления на странице
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]   // тип элемента управления на странице
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите ввод пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]   // механизм, который сверяет текущее значение PasswordConfirm, с указанным Password
        [DataType(DataType.Password)]   // тип элемента управления на странице
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }


    }
}