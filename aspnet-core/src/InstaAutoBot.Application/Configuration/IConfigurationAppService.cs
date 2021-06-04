using System.Threading.Tasks;
using InstaAutoBot.Configuration.Dto;

namespace InstaAutoBot.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
