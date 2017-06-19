using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Idealde.Modules.Settings;
using Idealde.Properties;

namespace Idealde.Modules.Compiler.ViewModels
{
    public class CompilerSettingsViewModel : PropertyChangedBase, ISettingsEditor
    {
        private string _vcVarSallPath;

        public string SettingsPageName => "Compiler";

        public string SettingsPagePath => "Environment";

        public string VcVarSallPath
        {
            get { return _vcVarSallPath; }
            set
            {
                if (value == _vcVarSallPath) return;
                _vcVarSallPath = value;
                NotifyOfPropertyChange(() => VcVarSallPath);
            }
        }

        public CompilerSettingsViewModel()
        {
            VcVarSallPath = Properties.Settings.Default.VCVarSallPath;
        }

        public bool ApplyChanges()
        {
            if (!File.Exists(VcVarSallPath))
            {
                MessageBox.Show(Resources.FileNotExistText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            Properties.Settings.Default.VCVarSallPath = VcVarSallPath;
            Properties.Settings.Default.Save();
            return true;
        }
    }
}
