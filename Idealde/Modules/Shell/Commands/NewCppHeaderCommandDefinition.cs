﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idealde.Framework.Commands;
using Idealde.Properties;

namespace Idealde.Modules.Shell.Commands
{
    public class NewCppHeaderCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.New.CppHeader";

        public override string Name => CommandName;

        public override string Tooltip => Resources.FileNewCppHeaderCommandTooltip;

        public override string Text => Resources.FileNewCppHeaderCommandText;

        public override Uri IconSource => new Uri("pack://application:,,,/Idealde;component/Resources/Images/CppFile.png");
    }
}
