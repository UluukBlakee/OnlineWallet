using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OnlineWallet.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле 'Email' обязательно для заполнения")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать минимум {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Пароль должен содержать как минимум одну заглавную букву, одну строчную букву и одну цифру.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Поле 'Подтвердить пароль' обязательно для заполнения")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string? PasswordConfirm { get; set; }
    }
}
