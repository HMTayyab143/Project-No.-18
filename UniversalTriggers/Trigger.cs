// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Trigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the Trigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Markup;

    /// <summary>
    /// The trigger.
    /// </summary>
    [ContentProperty(Name = "TriggerActions")]
    public abstract class Trigger : AssociatableElement
    {
        /// <summary>
        /// The trigger actions property.
        /// </summary>
        public static readonly DependencyProperty TriggerActionsProperty = DependencyProperty.Register(
            "TriggerActions",
            typeof(AttachableCollection<TriggerAction>),
            typeof(Trigger),
            new PropertyMetadata(DependencyProperty.UnsetValue, AssociatedPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="Trigger"/> class.
        /// </summary>
        protected Trigger()
        {
            this.TriggerActions = new AttachableCollection<TriggerAction>();
        }

        /// <summary>
        /// Gets or sets the trigger actions.
        /// </summary>
        public AttachableCollection<TriggerAction> TriggerActions
        {
            get
            {
                return (AttachableCollection<TriggerAction>)this.GetValue(TriggerActionsProperty);
            }

            set
            {
                this.SetValue(TriggerActionsProperty, value);
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

            this.TriggerActions.Attach(element);
        }

        /// <summary>
        /// Detaches the associated element.
        /// </summary>
        public override void Detach()
        {
            base.Detach();

            this.TriggerActions.Detach();
        }

        /// <summary>
        /// Called when the trigger is fired.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        protected virtual void OnTriggered(object eventData = null)
        {
            var actions = this.TriggerActions;
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action.Invoke(eventData);
                }
            }
        }
    }
}