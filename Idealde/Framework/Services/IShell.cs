using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Idealde.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
    {
        ILayoutItem ActiveItem { get; set; }

        ILayoutItem SelectedDocument { get; set; }

        IObservableCollection<ILayoutItem> Documents { get; }

        IObservableCollection<ILayoutItem> Tools { get; }
    }
}
