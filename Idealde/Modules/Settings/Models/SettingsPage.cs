#region Using Namespace

using Caliburn.Micro;

#endregion

namespace Idealde.Modules.Settings.Models
{
    public class SettingsPage : PropertyChangedBase
    {
        // Backing fields

        #region Backing fields

        private string _name;

        #endregion

        // Initializations

        #region Initializations

        public SettingsPage()
        {
            Children = new BindableCollection<SettingsPage>();

            Editors = new BindableCollection<ISettingsEditor>();
        }

        #endregion

        // Bind properties

        #region Bind properties

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        #endregion

        // Bind models

        #region Bind models

        public IObservableCollection<ISettingsEditor> Editors { get; }

        public IObservableCollection<SettingsPage> Children { get; }

        #endregion
    }
}