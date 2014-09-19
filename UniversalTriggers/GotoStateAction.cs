// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GotoStateAction.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   An action that changes the current state associated to the <see cref="VisualStateManager" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The goto state action.
    /// </summary>
    public class GotoStateAction : TriggerAction
    {
        /// <summary>
        /// The state name property.
        /// </summary>
        public static readonly DependencyProperty StateNameProperty = DependencyProperty.Register(
            "StateName",
            typeof(string),
            typeof(GotoStateAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        public string StateName
        {
            get
            {
                return (string)this.GetValue(StateNameProperty);
            }

            set
            {
                this.SetValue(StateNameProperty, value);
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
            if (this.AssociatedElement != null && !string.IsNullOrEmpty(this.StateName))
            {
                CustomVisualStateManager.GoToState(this.AssociatedElement, this.StateName);
            }
        }
    }
}