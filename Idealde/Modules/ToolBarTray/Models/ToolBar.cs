#region Using Namespace

using Caliburn.Micro;

#endregion

namespace Idealde.Modules.ToolBarTray.Models
{
    public class ToolBar : PropertyChangedBase
    {
        public string Name { get; }
        public IObservableCollection<ToolBarItemBase> Items { get; }

        public ToolBar(string name)
        {
            Items = new BindableCollection<ToolBarItemBase>();
            Name = name;
        }
    }
}