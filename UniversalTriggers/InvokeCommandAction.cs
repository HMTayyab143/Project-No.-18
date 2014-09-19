// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvokeCommandAction.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the InvokeCommandAction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System.Windows.Input;

    using Windows.UI.Xaml;

    /// <summary>
    /// The invoke command action.
    /// </summary>
    public sealed class InvokeCommandAction : TriggerAction
    {
        /// <summary>
        /// The command parameter property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                "CommandParameter",
                typeof(object),
                typeof(InvokeCommandAction),
                new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The command property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(InvokeCommandAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The pass event args to command property.
        /// </summary>
        public static readonly DependencyProperty PassEventArgsToCommandProperty =
            DependencyProperty.Register(
                "PassEventArgsToCommand",
                typeof(bool),
                typeof(InvokeCommandAction),
                new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }

            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to pass event args to command.
        /// </summary>
        public bool PassEventArgsToCommand
        {
            get
            {
                return (bool)this.GetValue(PassEventArgsToCommandProperty);
            }

            set
            {
                this.SetValue(PassEventArgsToCommandProperty, value);
            }
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        protected override void DoInvoke(object eventData)
        {
            var command = this.Command;
            if (command != null && command.CanExecute(this.CommandParameter))
            {
                command.Execute(
                    this.CommandParameter == null && this.PassEventArgsToCommand ? eventData : this.CommandParameter);
            }
        }
    }
}