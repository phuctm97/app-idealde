using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Idealde.Modules.Settings.Test.ViewModels
{
    public class TestViewModel: ISettingsEditor
    {
        public string SettingsPageName { get { return "Evironment"; } }
        public string SettingsPagePath { get { return "Test\\Settings\\Something\\About"; } } 
        public bool ApplyChanges()
        {
            MessageBox.Show("Applied");
            return true;
        }

    }
}
