using System;
using System.Collections.Generic;

namespace Idealde.Framework.Themes
{
    public interface ITheme
    {
        string Name { get; }

        IEnumerable<Uri> ApplicationResources { get; }

        IEnumerable<Uri> MainWindowResources { get; }
    }
}