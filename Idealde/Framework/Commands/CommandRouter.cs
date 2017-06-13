#region Using Namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;
using Idealde.Framework.Panes;
using Idealde.Framework.Services;

#endregion

namespace Idealde.Framework.Commands
{
    public class CommandRouter : ICommandRouter
    {
        // Dependencies
        private readonly IShell _shell;

        // Backing fields
        private readonly Dictionary<Type, HashSet<Type>> _commandHandlerTypeToCommandDefinitionTypesLookup;

        public CommandRouter(IShell shell)
        {
            _shell = shell;

            _commandHandlerTypeToCommandDefinitionTypesLookup = new Dictionary<Type, HashSet<Type>>();
        }

        public ICommandHandler GetHandler(CommandDefinition commandDefinition)
        {
            ICommandHandler handler = null;

            if (_shell.ActiveLayoutItem != null)
            {
                handler = GetCommandHandlerForLayoutItem(commandDefinition, _shell.ActiveLayoutItem);
                if (handler != null) return handler;
            }

            if (_shell.ActiveItem != null)
            {
                handler = GetCommandHandlerForLayoutItem(commandDefinition, _shell.ActiveItem);
                if (handler != null) return handler;
            }

            try
            {
                handler = (ICommandHandler) IoC.GetInstance(typeof(CommandDefinition), string.Empty);
            }
            catch
            {
                return null;
            }

            return handler;
        }

        private ICommandHandler GetCommandHandlerForLayoutItem(CommandDefinition commandDefinition,
            ILayoutItem layoutItem)
        {
            var view = ViewLocator.LocateForModel(layoutItem, null, null);
            var window = Window.GetWindow(view);
            if (window == null) return null;

            var initialElement = FocusManager.GetFocusedElement(view) ?? view;
            return FindCommandHandlerInVisualTree(commandDefinition, initialElement);
        }

        private ICommandHandler FindCommandHandlerInVisualTree(CommandDefinition commandDefinition,
            IInputElement initialElement)
        {
            var visualObject = initialElement as DependencyObject;
            if (visualObject == null)
                return null;

            object previousDataContext = null;
            do
            {
                var frameworkElement = visualObject as FrameworkElement;
                var dataContext = frameworkElement?.DataContext;

                if (dataContext != null && !ReferenceEquals(dataContext, previousDataContext))
                {
                    if (IsCommandHandlerForCommandDefinitionType(dataContext, commandDefinition.GetType()))
                        return (ICommandHandler) dataContext;

                    previousDataContext = dataContext;
                }

                visualObject = VisualTreeHelper.GetParent(visualObject);
            } while (visualObject != null);

            return null;
        }

        private bool IsCommandHandlerForCommandDefinitionType(object handler, Type commandDefinitionType)
        {
            var handlerType = handler.GetType();
            EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(handlerType);
            var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[handlerType];
            return commandDefinitionTypes.Contains(commandDefinitionType);
        }

        private void EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(Type commandHandlerType)
        {
            if (_commandHandlerTypeToCommandDefinitionTypesLookup.ContainsKey(commandHandlerType)) return;

            var commandDefinitionTypes =
                _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType] = new HashSet<Type>();

            foreach (
                var handledCommandDefinitionType in
                GetAllHandledCommandedDefinitionTypes(commandHandlerType))
                commandDefinitionTypes.Add(handledCommandDefinitionType);
        }

        private static IEnumerable<Type> GetAllHandledCommandedDefinitionTypes(
            Type type)
        {
            var result = new List<Type>();

            var genericInterfaceType = typeof(ICommandHandler<>);

            while (type != null)
            {
                result.AddRange(type.GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType)
                    .Select(x => x.GetGenericArguments().First()));

                type = type.BaseType;
            }

            return result;
        }
    }
}