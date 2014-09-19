// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertySetTrigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the PropertySetTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;
    using System.Globalization;

    using Windows.UI.Xaml;

    /// <summary>
    /// The property set trigger.
    /// </summary>
    public class PropertySetTrigger : BoundTrigger
    {
        /// <summary>
        /// The required value property.
        /// </summary>
        public static readonly DependencyProperty RequiredValueProperty = DependencyProperty.Register(
            "RequiredValue",
            typeof(object),
            typeof(PropertySetTrigger),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Gets or sets the required value.
        /// </summary>
        public object RequiredValue
        {
            get
            {
                return this.GetValue(RequiredValueProperty);
            }

            set
            {
                this.SetValue(RequiredValueProperty, value);
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
        protected override void OnBindingValueChanged(object oldValue, object newValue)
        {
            bool isEqual;

            if (newValue == null)
            {
                isEqual = this.RequiredValue == null
                          || (this.RequiredValue is string && string.IsNullOrEmpty((string)this.RequiredValue));
            }
            else
            {
                isEqual = newValue.Equals(this.RequiredValue);

                if (!isEqual && this.RequiredValue != null)
                {
                    if (newValue is float || newValue is double)
                    {
                        double value;
                        if (double.TryParse(
                            this.RequiredValue.ToString(),
                            NumberStyles.Any,
                            CultureInfo.InvariantCulture,
                            out value))
                        {
                            isEqual = Math.Abs(value - (double)newValue) < double.Epsilon;
                        }
                    }
                    else
                    {
                        isEqual = string.Equals(
                            newValue.ToString(),
                            this.RequiredValue.ToString(),
                            StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            if (isEqual)
            {
                this.OnTriggered();
            }
        }
    }
}