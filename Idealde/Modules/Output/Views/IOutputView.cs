namespace Idealde.Modules.Output.Views
{
    public interface IOutputView
    {
        void SetText(string text);

        void ScrollToEnd();

        void Clear();
    }
}