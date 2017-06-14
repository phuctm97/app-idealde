#region Using Namespace

using System.Collections.Generic;
using System.Threading.Tasks;
using Idealde.Framework.Panes;

#endregion

namespace Idealde.Framework.Services
{
    public class EditorFileType
    {
        public EditorFileType(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }

        public string Name { get; }

        public string Extension { get; }
    }

    public interface IEditorProvider
    {
        string Name { get; }

        IEnumerable<EditorFileType> FileTypes { get; }

        bool CanRead(string path);

        IDocument Create();

        Task New(IDocument document, string fileName);

        Task Open(IDocument document, string filePath);
    }
}