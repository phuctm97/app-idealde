using System.IO;
using Caliburn.Micro;
using Idealde.Properties;
using System.Linq;
using System.Windows.Forms;

namespace Idealde.Modules.Settings.Options.Compiler.ViewModels
{
    public class CompilerSettingsViewModel: PropertyChangedBase, ISettingsEditor
    {
        private string _outputPath;
        private WarningLevel _warningLevel;

        public CompilerSettingsViewModel()
        {
            WarningLevels = new BindableCollection<WarningLevel>
            {
                new WarningLevel("Level 1", "/W1"),
                new WarningLevel("Level 2", "/W2"),
                new WarningLevel("Level 3", "/W3"),
                new WarningLevel("Level 4", "/W4"),
            };

            WarningLevel = WarningLevels.First(x => x.Value == Properties.Settings.Default.CompileWarningLevel);
            OutputPath = Properties.Settings.Default.CompileOutputPath;
        }

        public void AddWaringLevel(string displayName, string value)
        {
            WarningLevels.Add(new WarningLevel(displayName, value));
        }

        public string SettingsPageName => Resources.CompilerSettingsName;
        public string SettingsPagePath => Resources.CompilerSettingsPath;

        public WarningLevel WarningLevel
        {
            get { return _warningLevel; }
            set
            {
                _warningLevel = value;
            }
        }

        public IObservableCollection<WarningLevel> WarningLevels { get; set; }

        public string OutputPath
        {
            get { return _outputPath; }
            set
            {
                if (string.Equals(_outputPath, value)) return;
                _outputPath = value;
                NotifyOfPropertyChange(() => OutputPath);
            }
        }

        public bool ApplyChanges()
        {
            if (Equals(OutputPath, Properties.Settings.Default.CompileOutputPath) &&
                Equals(WarningLevel.Value, Properties.Settings.Default.CompileWarningLevel)) return true;

            if (!Directory.Exists(OutputPath))
            {
                MessageBox.Show("Can't find output location");
                return false;
            }

            Properties.Settings.Default.CompileWarningLevel = WarningLevel.Value;
            Properties.Settings.Default.CompileOutputPath = OutputPath;

            Properties.Settings.Default.Save();
            return true;
        }
    }
}
