namespace Idealde.Modules.Output
{
    public interface IOutputView
    {
        // Output view behaviors
        void SetText(string text);

        void ScrollToEnd();

        void Clear();
    }
}