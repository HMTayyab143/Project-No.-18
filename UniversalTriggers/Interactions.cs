// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interactions.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the Interactions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;

    using Windows.UI.Xaml;

    /// <summary>
    /// The interactions.
    /// </summary>
    public static class Interactions
    {
        /// <summary>
        /// The triggers property.
        /// </summary>
        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached(
            "Triggers",
            typeof(TriggerCollection),
            typeof(Interactions),
            new PropertyMetadata(DependencyProperty.UnsetValue, OnCommandsChanged));

        /// <summary>
        /// Gets the attached triggers.
        /// </summary>
        /// <param name="obj">
        /// The object to get the triggers from.
        /// </param>
        /// <returns>
        /// The <see cref="TriggerCollection"/>.
        /// </returns>
        public static TriggerCollection GetTriggers(DependencyObject obj)
        {
            if (obj != null)
            {
                var triggers = (TriggerCollection)obj.GetValue(TriggersProperty);

                if (triggers == null)
                {
                    triggers = new TriggerCollection();
                    obj.SetValue(TriggersProperty, triggers);
                }

                return triggers;
            }

            return null;
        }

        /// <summary>
        /// Sets the attached triggers.
        /// </summary>
        /// <param name="obj">
        /// The dependency object.
        /// </param>
        /// <param name="triggers">
        /// The triggers.
        /// </param>
        public static void SetTriggers(DependencyObject obj, TriggerCollection triggers)
        {
            obj.SetValue(TriggersProperty, triggers);
        }

        /// <summary>
        /// Handles when commands are changed.
        /// </summary>
        /// <param name="obj">
        /// The element.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private static void OnCommandsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var element = obj as FrameworkElement;
            if (element == null)
            {
                throw new InvalidOperationException(
                    "Triggers can only be attached to types that derive from FrameworkElement");
            }

            var oldValue = args.OldValue as TriggerCollection;
            var newValue = args.NewValue as TriggerCollection;

            if (oldValue != newValue)
            {
                if (oldValue != null)
                {
                    oldValue.Detach();
                }

                if (newValue != null)
                {
                    newValue.Attach(element);
                }
            }
        }
    }
}