using System.Windows.Forms;

namespace Idealde.Modules.Settings.Test.ViewModels
{
    public class Test2ViewModel: ISettingsEditor
    {
        public string SettingsPageName { get { return "Compiler"; } }
        public string SettingsPagePath { get { return "Test\\Something\\System"; } }
        public bool ApplyChanges()
        {
            MessageBox.Show("Compiler");
            return true;
        }
    }
}
