#region Using Namespace

using System.Collections.Generic;
using Caliburn.Micro;
using Idealde.Framework.Panes;

#endregion

namespace Idealde.Modules.ErrorList
{
    public interface IErrorList : ITool
    {
        // Bind properties
        bool IsErrorsVisible { get; set; }

        bool IsWarningsVisible { get; set; }

        bool IsMesagesVisible { get; set; }

        int NumberOfErrorItems { get; }

        int NumberOfWarningItems { get; }

        int NumberOfMessageItems { get; }


        // Bind models
        IEnumerable<ErrorListItem> FilteredItems { get; }

        // Error list behaviors
        void AddItem(ErrorListItemType type, string code, string description,
            string path = null, int? line = null, int? column = null,
            System.Action onClick = null);

        void Clear();
    }
}