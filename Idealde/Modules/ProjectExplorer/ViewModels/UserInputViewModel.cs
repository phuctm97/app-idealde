using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using Idealde.Framework.Commands;
using Screen = Caliburn.Micro.Screen;

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class UserInputViewModel : Screen
    {
        private string _fileName;

        public UserInputViewModel(string title, string filePath, string fileFileExtension)
        {
            FileName = "";
            OkCommand = new RelayCommand(p => OkPressed());
            CancelCommand = new RelayCommand(p => CancelPressed());
            Title = title;
            FilePath = filePath;
            FileExtension = fileFileExtension;
        }

        public string FileExtension { get; }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public string Title { get; }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (Equals(value, _fileName)) return;
                _fileName = value;
                NotifyOfPropertyChange(() => FileName);
            }
        }

        public string FilePath { get; }

        private void OkPressed()
        {
            if (File.Exists(FilePath + "\\" + FileName + FileExtension))
            {
                MessageBox.Show("File name " + FileName + FileExtension + " already exists");
                return;
            }
            TryClose(true);
        }

        private void CancelPressed()
        {
            FileName = "";
            TryClose(true);
        }
    }
}