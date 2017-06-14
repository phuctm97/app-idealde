#region Using Namespace

using System.Threading.Tasks;
using Idealde.Framework.Commands;
using Idealde.Modules.Shell.Commands;

#endregion

namespace Idealde.Framework.Panes
{
    public interface IPersistedDocument : IDocument,
        ICommandHandler<SaveFileCommandDefinition>

    {
        bool IsNew { get; }

        string FileName { get; }

        string FilePath { get; }

        Task New(string fileName);

        Task Load(string filePath);

        Task Save(string filePath);
    }
}