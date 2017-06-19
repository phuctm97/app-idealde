using System;
using System.IO;
using System.Windows;
using Caliburn.Micro;
using Idealde.Properties;

namespace Idealde.Modules.ProjectExplorer.ViewModels
{
    public class NewItemViewModel : Screen
    {
        private bool _closing;
        private string _name;

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

        public NewItemViewModel()
        {
            Name = string.Empty;

            _closing = false;
        }

        protected override void OnInitialize()
        {
            DisplayName = Resources.NewItemWindowTitle;

            base.OnInitialize();
        }



        public override void TryClose(bool? dialogResult = default(bool?))
        {
            if (dialogResult != null)
            {
                _closing = dialogResult.Value;
            }

            base.TryClose(dialogResult);
        }

        public override void CanClose(Action<bool> callback)
        {
            var canClose = true;

            if (_closing)
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    canClose = false;
                    MessageBox.Show(Resources.PleaseEnterItemNameText);
                }
            }

            callback(canClose);
        }
    }
}