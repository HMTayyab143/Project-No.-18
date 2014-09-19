// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTrigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the EventTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The event trigger.
    /// </summary>
    public class EventTrigger : EventHookingTrigger
    {
        /// <summary>
        /// The event name property.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(
            "EventName",
            typeof(string),
            typeof(EventTrigger),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The source object property.
        /// </summary>
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register(
            "TargetObject",
            typeof(object),
            typeof(EventTrigger),
            new PropertyMetadata(DependencyProperty.UnsetValue, TargetObjectPropertyChanged));

        /// <summary>
        /// Gets or sets the event name.
        /// </summary>
        public string EventName
        {
            get
            {
                return (string)this.GetValue(EventNameProperty);
            }

            set
            {
                this.SetValue(EventNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        public object TargetObject
        {
            get
            {
                return this.GetValue(TargetObjectProperty);
            }

            set
            {
                this.SetValue(TargetObjectProperty, value);
            }
        }

        /// <summary>
        /// Attaches an element to this.
        /// </summary>
        /// <param name="element">
        /// The element to attach.
        /// </param>
        public override void Attach(FrameworkElement element)
        {
            base.Attach(element);

            this.HookEvent(this.EventName, this.TargetObject);
        }

        /// <summary>
        /// Detaches the associated element.
        /// </summary>
        public override void Detach()
        {
            base.Detach();

            this.UnhookEvent(this.TargetObject);
        }

        /// <summary>
        /// Called when the target object changes.
        /// </summary>
        /// <param name="obj">
        /// The event trigger.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private static void TargetObjectPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var trigger = obj as EventTrigger;
            if (trigger != null)
            {
                trigger.UnhookEvent(args.OldValue);
                trigger.HookEvent(trigger.EventName, args.NewValue);
            }
        }
    }
}