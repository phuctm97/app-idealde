using System.Collections.Generic;

namespace Idealde.Modules.Settings.Models
{
    public class SettingsPage
    {
        public SettingsPage()
        {
            Children = new List<SettingsPage>();
            Editors = new List<ISettingsEditor>();
        }

        public string Name { get; set; }
        public List<ISettingsEditor> Editors { get; private set; }
        public List<SettingsPage> Children { get; private set; }
    }
}