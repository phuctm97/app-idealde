﻿using System.Threading.Tasks;
using Caliburn.Micro;
using Idealde.Framework.Commands;
using Idealde.Framework.Services;

namespace Idealde.Modules.Shell.Commands
{
    public class ExitCommandHandler : ICommandHandler<ExitCommandDefinition>
    {
        public ExitCommandHandler()
        {
        }

        public void Update(Command command)
        {}

        public Task Run(Command command)
        {
            IoC.Get<IShell>().Close();
            return Task.FromResult(true);
        }
    }
}