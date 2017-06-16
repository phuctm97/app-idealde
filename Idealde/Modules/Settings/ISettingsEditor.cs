namespace Idealde.Modules.Settings
{
    public interface ISettingsEditor
    {
        string SettingsPageName { get; }
        string SettingsPagePath { get; }

        void ApplyChanges();
    }
}
