namespace Idealde.Modules.Settings.Options.Compiler.ViewModels
{
    public class WarningLevel
    {
        public string LevelDisplayName { get; set; }
        public string Value { get; }

        public WarningLevel(string displayName, string value)
        {
            LevelDisplayName = displayName;
            Value = value;
        }
    }
}
