#region Using Namespace

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;
using Idealde.Modules.Shell.Commands;
using Idealde.Properties;
using Microsoft.Win32;

#endregion

namespace Idealde.Framework.Panes
{
    public abstract class PersistedDocument : Document, IPersistedDocument
    {
        // Backing fields
        private bool _isDirty;

        // Logic properties
        public bool IsNew { get; private set; }

        public string FileName { get; private set; }

        public string FilePath { get; private set; }

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (value == _isDirty) return;
                _isDirty = value;

                UpdateDisplayName();
                NotifyOfPropertyChange(() => IsDirty);
            }
        }

        // Initializations

        protected PersistedDocument()
        {
            _isDirty = false;
            IsNew = true;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        // Behaviors
        public async Task New(string fileName)
        {
            FileName = fileName;
            await DoNew();

            IsNew = true;
            IsDirty = false;
            UpdateDisplayName();
        }

        protected abstract Task DoNew();

        public async Task Load(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            await DoLoad();

            IsNew = false;
            IsDirty = false;
            UpdateDisplayName();
        }

        protected abstract Task DoLoad();

        public async Task Save(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileName(filePath);
            await DoSave();

            IsNew = false;
            IsDirty = false;
            UpdateDisplayName();
        }

        protected abstract Task DoSave();

        private void UpdateDisplayName()
        {
            DisplayName = IsDirty ? FileName + "*" : FileName;
        }

        void ICommandHandler<SaveFileCommandDefinition>.Update(Command command)
        {
            command.IsEnabled = IsNew || IsDirty;
            command.Tooltip = string.Format(Resources.FileSaveCommandTooltip, FileName);
        }

        async Task ICommandHandler<SaveFileCommandDefinition>.Run(Command command)
        {
            if (IsNew)
            {
                await DoSaveAs();
            }
            else
            {
                await Save(FilePath);
            }
        }

        void ICommandHandler<SaveFileAsCommandDefinition>.Update(Command command)
        {
            command.IsEnabled = !IsNew;
            command.Tooltip = string.Format(Resources.FileSaveAsCommandTooltip, FileName);
        }

        async Task ICommandHandler<SaveFileAsCommandDefinition>.Run(Command command)
        {
            await DoSaveAs();
        }

        private async Task DoSaveAs()
        {
            var dialog = new SaveFileDialog {FileName = FileName};
            var filter = string.Empty;

            var fileExtension = Path.GetExtension(FileName);
            var fileType = IoC.GetAll<IEditorProvider>()
                .SelectMany(x => x.FileTypes)
                .SingleOrDefault(x => x.Extension == fileExtension);
            if (fileType != null)
                filter = fileType.Name + "|*" + fileType.Extension + "|";
            filter += "All Files|*.*";
            dialog.Filter = filter;

            if (dialog.ShowDialog() != true)
                return;

            // Save file.
            await Save(dialog.FileName);
        }
    }
}