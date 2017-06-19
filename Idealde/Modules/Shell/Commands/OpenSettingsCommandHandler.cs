using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Modules.Settings.ViewModels;

namespace Idealde.Modules.Shell.Commands
{
    public class OpenSettingsCommandHandler :
        ICommandHandler<OpenSettingsCommandDefinition>
    {
        public void Update(Command command)
        {
        }

        public Task Run(Command command)
        {
            var settingsDialog = IoC.Get<SettingsViewModel>();
            var windowManager = IoC.Get<IWindowManager>();
            windowManager.ShowDialog(settingsDialog);

            return Task.FromResult(true);
        }
    }
}