// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyChangedTrigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the PropertyChangedTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    /// <summary>
    /// The property changed trigger.
    /// </summary>
    public class PropertyChangedTrigger : BoundTrigger
    {
        /// <summary>
        /// The on binding value changed event.
        /// </summary>
        /// <param name="oldValue">
        /// The old value.
        /// </param>
        /// <param name="newValue">
        /// The new value.
        /// </param>
        protected override void OnBindingValueChanged(object oldValue, object newValue)
        {
            this.OnTriggered();
        }
    }
}