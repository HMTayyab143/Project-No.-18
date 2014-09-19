// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundTrigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the BoundTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The bindable trigger.
    /// </summary>
    public abstract class BoundTrigger : Trigger
    {
        /// <summary>
        /// The binding property.
        /// </summary>
        public static readonly DependencyProperty BindingProperty = DependencyProperty.Register(
            "Binding",
            typeof(object),
            typeof(BoundTrigger),
            new PropertyMetadata(DependencyProperty.UnsetValue, BindingValueChanged));

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        public object Binding
        {
            get
            {
                return this.GetValue(BindingProperty);
            }

            set
            {
                this.SetValue(BindingProperty, value);
            }
        }

        /// <summary>
        /// The on binding value changed event.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected abstract void OnBindingValueChanged(object oldValue, object newValue);

        /// <summary>
        /// Handles the event raised when the value of the property changes.
        /// </summary>
        /// <param name="obj">
        /// The bound trigger.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private static void BindingValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var trigger = obj as BoundTrigger;
            if (trigger != null)
            {
                trigger.OnBindingValueChanged(args.OldValue, args.NewValue);
            }
        }
    }
}