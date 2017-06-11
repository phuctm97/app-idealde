using System;

namespace Idealde.Modules.MainMenu.Models
{
    public interface IMenuItem
    {
        string Text { get; }
        Uri IconSource { get; }
    }
}
