#region Using Namespace

using System;
using System.Collections.Generic;

#endregion

namespace Idealde.Framework.Themes
{
    public interface IThemeManager
    {
        event EventHandler CurrentThemeChanged;

        IEnumerable<ITheme> Themes { get; }

        ITheme CurrentTheme { get; }

        bool SetCurrentTheme(string name);
    }
}