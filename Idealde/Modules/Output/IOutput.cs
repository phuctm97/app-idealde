#region Using Namespace

using Idealde.Framework.Panes;

#endregion

namespace Idealde.Modules.Output
{
    public interface IOutput : ITool
    {
        // Output behaviors
        void Append(string text);

        void AppendLine(string text);

        void BreakLine();

        void Clear();
    }
}