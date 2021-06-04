using System.ComponentModel.DataAnnotations;

namespace InstaAutoBot.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}