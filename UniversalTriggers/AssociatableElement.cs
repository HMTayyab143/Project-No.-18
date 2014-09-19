// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssociatableElement.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the AssociatableElement type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;

    using UniversalTriggers.Interfaces;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// The associatable element.
    /// </summary>
    public abstract class AssociatableElement : FrameworkElement, IAssociatableElement
    {
        /// <summary>
        /// Gets the associated object.
        /// </summary>
        /// <value>
        /// The associated object.
        /// </value>
        public FrameworkElement AssociatedElement { get; private set; }

        /// <summary>
        /// Attaches an element to this.
        /// </summary>
        /// <param name="element">
        /// The element to attach.
        /// </param>
        public virtual void Attach(FrameworkElement element)
        {
            if (this.AssociatedElement != element)
            {
                if (this.AssociatedElement != null)
                {
                    throw new InvalidOperationException("Element is attached to another object.");
                }

                this.AssociatedElement = element;

                if (element != null)
                {
                    element.Loaded += this.OnAssociatedElementLoaded;
                }
            }
        }

        /// <summary>
        /// Detaches the associated element.
        /// </summary>
        public virtual void Detach()
        {
            if (this.AssociatedElement == null)
            {
                return;
            }

            if (this.AssociatedElement != null)
            {
                this.AssociatedElement.Loaded -= this.OnAssociatedElementLoaded;
            }

            this.ClearValue(FrameworkElement.DataContextProperty);

            this.AssociatedElement = null;
        }

        /// <summary>
        /// Handles the property changed event.
        /// </summary>
        /// <param name="obj">
        /// The object the property changed on.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        internal static void AssociatedPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                ((IAssociatableElement)args.OldValue).Detach();
            }

            var trigger = obj as IAssociatableElement;
            if (trigger != null && args.NewValue != null && trigger.AssociatedElement != null)
            {
                ((IAssociatableElement)args.NewValue).Attach(trigger.AssociatedElement);
            }
        }

        /// <summary>
        /// Called when the associated element has loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private void OnAssociatedElementLoaded(object sender, RoutedEventArgs args)
        {
            var element = (FrameworkElement)sender;
            element.Loaded -= this.OnAssociatedElementLoaded;

            var binding = new Binding
                              {
                                  Source = element,
                                  Path = new PropertyPath("DataContext"),
                                  Mode = BindingMode.OneWay
                              };

            this.SetBinding(FrameworkElement.DataContextProperty, binding);
        }
    }
}