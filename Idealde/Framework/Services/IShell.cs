﻿#region Using Namespace

using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Modules.MainMenu;
using Idealde.Modules.StatusBar;
using Idealde.Modules.ToolBarTray;

#endregion

namespace Idealde.Framework.Services
{
    public interface IShell : IGuardClose, IDeactivate
    {
        // Bind models
        IMenu MainMenu { get; }

        IToolBarTray ToolBarTray { get; }

        IStatusBar StatusBar { get; }

        ILayoutItem ActiveLayoutItem { get; set; }

        IDocument ActiveItem { get; }

        IObservableCollection<IDocument> Documents { get; }

        IObservableCollection<ITool> Tools { get; }

        // Item actions
        void OpenDocument(IDocument document);

        void CloseDocument(IDocument document);

        void ShowTool(ITool tool);

        void ShowTool<TTool>() where TTool : ITool;

        // Shell actions
        void Close();
    }
}