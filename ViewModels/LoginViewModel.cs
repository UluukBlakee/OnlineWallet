using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OnlineWallet.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле 'Email или Личный счет' обязательно для заполнения")]
        [Display(Name = "Email или Личный счет")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
