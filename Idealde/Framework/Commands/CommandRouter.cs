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

        #region Dependencies

        private readonly IShell _shell;

        #endregion

        // Backing fields

        #region Backing fields

        private readonly Dictionary<Type, HashSet<Type>> _commandHandlerTypeToCommandDefinitionTypesLookup;
        private readonly Dictionary<Type, ICommandHandler> _commandDefinitionTypeToCommandHandlerLookup;

        #endregion

        // Initializations

        #region Initializations

        public CommandRouter(IShell shell)
        {
            _shell = shell;

            _commandHandlerTypeToCommandDefinitionTypesLookup = new Dictionary<Type, HashSet<Type>>();

            _commandDefinitionTypeToCommandHandlerLookup = new Dictionary<Type, ICommandHandler>();

            BuildCommandDefinitionTypeToCommandHandlerLookup();
        }

        private void BuildCommandDefinitionTypeToCommandHandlerLookup()
        {
            // get registered handler in container
            var commandHandlersList = IoC.GetAll<ICommandHandler>().ToList();

            foreach (var commandHandler in commandHandlersList)
            {
                // extract handled command definition type and hash to table

                var commandHandlerType = commandHandler.GetType();
                EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(commandHandlerType);
                var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType];

                foreach (var commandDefinitionType in commandDefinitionTypes)
                {
                    _commandDefinitionTypeToCommandHandlerLookup.Add(commandDefinitionType, commandHandler);
                }
            }
        }

        #endregion

        public CommandHandlerWrapper GetHandler(CommandDefinition commandDefinition)
        {
            ICommandHandler handler = null;

            // trav handler in visual tree start from shell active layout item
            if (_shell.ActiveLayoutItem != null)
            {
                handler = GetCommandHandlerForLayoutItem(commandDefinition, _shell.ActiveLayoutItem);
                if (handler != null) return CreateHandlerWrapper(commandDefinition.GetType(), handler);
            }

            // trav handler in visual tree start from shell active document
            if (_shell.ActiveItem != null)
            {
                handler = GetCommandHandlerForLayoutItem(commandDefinition, _shell.ActiveItem);
                if (handler != null) return CreateHandlerWrapper(commandDefinition.GetType(), handler);
            }

            // last case, find in global hash table
            _commandDefinitionTypeToCommandHandlerLookup.TryGetValue(commandDefinition.GetType(), out handler);
            return handler == null ? null : CreateHandlerWrapper(commandDefinition.GetType(), handler);
        }

        private ICommandHandler GetCommandHandlerForLayoutItem(CommandDefinition commandDefinition,
            ILayoutItem layoutItem)
        {
            // get view to trav visual tree
            var view = ViewLocator.LocateForModel(layoutItem, null, null);
            // check for current working view
            var window = Window.GetWindow(view);
            if (window == null) return null;

            // trav
            var initialElement = FocusManager.GetFocusedElement(view) ?? view;
            return FindCommandHandlerInVisualTree(commandDefinition, initialElement);
        }

        private ICommandHandler FindCommandHandlerInVisualTree(CommandDefinition commandDefinition,
            IInputElement initialElement)
        {
            // trav object
            var visualObject = initialElement as DependencyObject;
            if (visualObject == null)
                return null;

            // to pass same data context element
            object previousDataContext = null;
            do
            {
                var frameworkElement = visualObject as FrameworkElement;
                var dataContext = frameworkElement?.DataContext;

                if (dataContext != null && !ReferenceEquals(dataContext, previousDataContext))
                {
                    // check correct handle command definition
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
            // extract handled command definitions in handler type to find out current command definition
            var handlerType = handler.GetType();
            EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(handlerType);
            var commandDefinitionTypes = _commandHandlerTypeToCommandDefinitionTypesLookup[handlerType];
            return commandDefinitionTypes.Contains(commandDefinitionType);
        }

        private void EnsureCommandHandlerTypeToCommandDefinitionTypesPopulated(Type commandHandlerType)
        {
            if (_commandHandlerTypeToCommandDefinitionTypesLookup.ContainsKey(commandHandlerType)) return;

            // extract handled command definitions in handler type and hash to table
            var commandDefinitionTypes =
                _commandHandlerTypeToCommandDefinitionTypesLookup[commandHandlerType] = new HashSet<Type>();

            foreach (
                var handledCommandDefinitionType in
                GetAllHandledCommandedDefinitionTypes(commandHandlerType))
                commandDefinitionTypes.Add(handledCommandDefinitionType);
        }

        private IEnumerable<Type> GetAllHandledCommandedDefinitionTypes(
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

        private CommandHandlerWrapper CreateHandlerWrapper(Type commandDefinitionType, ICommandHandler handler)
        {
            return
                CommandHandlerWrapper.FromCommandHandler(
                    typeof(ICommandHandler<>).MakeGenericType(commandDefinitionType), handler);
        }
    }
}