﻿#region Using Namespace

using Idealde.Framework.Panes;

#endregion

namespace Idealde.Modules.Output
{
    public interface IOutput : ITool
    {
        void Append(string text);

        void BreakLine();

        void Clear();
    }
}