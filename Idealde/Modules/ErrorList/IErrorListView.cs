using System;

namespace Idealde.Modules.ErrorList
{
    public interface IErrorListView
    {
        event EventHandler<ErrorListItem> OnErrorItemQuery;
    }
}