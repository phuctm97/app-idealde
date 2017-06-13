using System;
using System.Threading.Tasks;
using Idealde.Framework.Panes;

namespace Idealde.Modules.CodeEditor.ViewModels
{
    public class CodeEditorViewModel : PersistedDocument, ICodeEditor
    {
        public CodeEditorViewModel()
        {
            
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
        }

        protected override Task DoNew()
        {
            throw new NotImplementedException();
        }

        protected override Task DoLoad()
        {
            throw new NotImplementedException();
        }

        protected override Task DoSave()
        {
            throw new NotImplementedException();
        }

        public void ChangeColor()
        {
            throw new NotImplementedException();
        }

        public string GetContent()
        {
            throw new NotImplementedException();
        }

        public void Goto(long row, long column)
        {
            throw new NotImplementedException();
        }
    }
}
