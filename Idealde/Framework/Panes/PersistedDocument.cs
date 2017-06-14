using System.IO;
using System.Threading.Tasks;

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
    }
}