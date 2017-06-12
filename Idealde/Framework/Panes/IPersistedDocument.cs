﻿using System.Threading.Tasks;

namespace Idealde.Framework.Panes
{
    public interface IPersistedDocument : IDocument
    {
        bool IsNew { get; }

        string FileName { get; }

        string FilePath { get; }

        Task New(string fileName);

        Task Load(string filePath);

        Task Save(string filePath);
    }
}