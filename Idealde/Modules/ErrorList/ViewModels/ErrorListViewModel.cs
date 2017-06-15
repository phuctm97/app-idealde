#region Using Namespace

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Action = System.Action;

#endregion

namespace Idealde.Modules.ErrorList.ViewModels
{
    public sealed class ErrorListViewModel : Tool, IErrorList
    {
        // Backing fields

        #region Backing fields

        private bool _isErrorsVisible;
        private bool _isWarningsVisible;
        private bool _isMessagesVisible;

        #endregion

        // Bind properties

        #region Bind properties

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        public bool IsErrorsVisible
        {
            get { return _isErrorsVisible; }
            set
            {
                if (value == _isErrorsVisible) return;
                _isErrorsVisible = value;
                NotifyOfPropertyChange(() => IsErrorsVisible);
                NotifyOfPropertyChange(() => FilteredItems);
            }
        }

        public bool IsWarningsVisible
        {
            get { return _isWarningsVisible; }
            set
            {
                if (value == _isWarningsVisible) return;
                _isWarningsVisible = value;
                NotifyOfPropertyChange(() => IsWarningsVisible);
                NotifyOfPropertyChange(() => FilteredItems);
            }
        }

        public bool IsMessagesVisible
        {
            get { return _isMessagesVisible; }
            set
            {
                if (value == _isMessagesVisible) return;
                _isMessagesVisible = value;
                NotifyOfPropertyChange(() => IsMessagesVisible);
                NotifyOfPropertyChange(() => FilteredItems);
            }
        }

        public int NumberOfErrorItems
        {
            get { return Items.Count(i => i.Type == ErrorListItemType.Error); }
        }

        public int NumberOfWarningItems
        {
            get { return Items.Count(i => i.Type == ErrorListItemType.Warning); }
        }

        public int NumberOfMessageItems
        {
            get { return Items.Count(i => i.Type == ErrorListItemType.Message); }
        }


        #endregion

        // Bind models

        #region Bind models

        public IEnumerable<ErrorListItem> FilteredItems
        {
            get
            {
                var items = Items.AsEnumerable();
                if (!IsErrorsVisible)
                {
                    items = items.Where(i => i.Type != ErrorListItemType.Error);
                }
                if (!IsWarningsVisible)
                {
                    items = items.Where(i => i.Type != ErrorListItemType.Warning);
                }
                if (!IsMessagesVisible)
                {
                    items = items.Where(i => i.Type != ErrorListItemType.Message);
                }
                return items;
            }
        }

        public IObservableCollection<ErrorListItem> Items { get; }

        #endregion

        // Initializations
        public ErrorListViewModel()
        {
            DisplayName = "Error List";
            IsErrorsVisible = true;
            IsWarningsVisible = true;
            IsMessagesVisible = true;

            Items = new BindableCollection<ErrorListItem>();
            Items.CollectionChanged += OnItemsCollectionChanged; ;
        }

        // Error list behaviors

        #region Error list behaviors

        public void AddItem(ErrorListItemType type, string code, string description, string path = null, int? line = null,
            int? column = null, Action onClick = null)
        {
            Items.Add(new ErrorListItem(type, code, description, path, line, column)
            {
                OnClick = onClick
            });
        }

        public void Clear()
        {
            Items.Clear();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => FilteredItems);
            NotifyOfPropertyChange(() => NumberOfErrorItems);
            NotifyOfPropertyChange(() => NumberOfWarningItems);
            NotifyOfPropertyChange(() => NumberOfMessageItems);
        }

        
        #endregion
    }
}