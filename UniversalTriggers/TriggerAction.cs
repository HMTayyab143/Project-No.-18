// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggerAction.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the TriggerAction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Markup;

    /// <summary>
    /// The trigger action.
    /// </summary>
    [ContentProperty(Name = "Conditions")]
    public abstract class TriggerAction : AssociatableElement
    {
        /// <summary>
        /// The conditions property.
        /// </summary>
        public static readonly DependencyProperty ConditionsProperty = DependencyProperty.Register(
            "Conditions",
            typeof(AttachableCollection<Condition>),
            typeof(TriggerAction),
            new PropertyMetadata(DependencyProperty.UnsetValue, AssociatableElement.AssociatedPropertyChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAction"/> class.
        /// </summary>
        protected TriggerAction()
        {
            this.Conditions = new AttachableCollection<Condition>();
        }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        public AttachableCollection<Condition> Conditions
        {
            get
            {
                return (AttachableCollection<Condition>)this.GetValue(ConditionsProperty);
            }

            set
            {
                this.SetValue(ConditionsProperty, value);
            }
        }

        /// <summary>
        /// Invokes the event if it meets the conditions of the trigger.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        public void Invoke(object eventData)
        {
            var conditions = this.Conditions;
            if (conditions == null || conditions.All(c => c.CompareValue()))
            {
                this.DoInvoke(eventData);
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

            this.Conditions.Attach(element);
        }

        /// <summary>
        /// Detaches the associated element.
        /// </summary>
        public override void Detach()
        {
            base.Detach();

            this.Conditions.Detach();
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        protected abstract void DoInvoke(object eventData);
    }
}