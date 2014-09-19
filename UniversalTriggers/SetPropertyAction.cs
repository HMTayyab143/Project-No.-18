// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetPropertyAction.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the SetPropertyAction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;
    using System.Reflection;

    using Windows.UI.Xaml;

    /// <summary>
    /// The set property action.
    /// </summary>
    public class SetPropertyAction : TriggerAction
    {
        /// <summary>
        /// The property name property.
        /// </summary>
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            "PropertyName",
            typeof(string),
            typeof(SetPropertyAction),
            new PropertyMetadata(DependencyProperty.UnsetValue, TargetPropertyChanged));

        /// <summary>
        /// The target property.
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            "Target",
            typeof(object),
            typeof(SetPropertyAction),
            new PropertyMetadata(DependencyProperty.UnsetValue, TargetPropertyChanged));

        /// <summary>
        /// The value property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(SetPropertyAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The target object property info.
        /// </summary>
        private PropertyInfo targetObjectPropertyInfo;

        /// <summary>
        /// The associated element property info.
        /// </summary>
        private PropertyInfo associatedElementPropertyInfo;

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName
        {
            get
            {
                return (string)this.GetValue(PropertyNameProperty);
            }

            set
            {
                this.SetValue(PropertyNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        public object Target
        {
            get
            {
                return this.GetValue(TargetProperty);
            }

            set
            {
                this.SetValue(TargetProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value
        {
            get
            {
                return this.GetValue(ValueProperty);
            }

            set
            {
                this.SetValue(ValueProperty, value);
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
            if (this.targetObjectPropertyInfo == null && this.associatedElementPropertyInfo == null)
            {
                if (this.Target != null && !string.IsNullOrEmpty(this.PropertyName))
                {
                    this.targetObjectPropertyInfo = this.Target.GetType().GetRuntimeProperty(this.PropertyName);
                }
                else if (this.Target == null && this.AssociatedElement != null)
                {
                    this.associatedElementPropertyInfo =
                        this.AssociatedElement.GetType().GetRuntimeProperty(this.PropertyName);
                }
            }

            PropertyInfo propertyInfo = null;
            object obj = null;

            if (this.targetObjectPropertyInfo != null && this.targetObjectPropertyInfo.CanWrite)
            {
                propertyInfo = this.targetObjectPropertyInfo;
                obj = this.Target;
            }
            else if (this.associatedElementPropertyInfo != null && this.associatedElementPropertyInfo.CanWrite)
            {
                propertyInfo = this.associatedElementPropertyInfo;
                obj = this.AssociatedElement;
            }

            if (propertyInfo != null && obj != null)
            {
                if (this.Value == null)
                {
                    propertyInfo.SetValue(obj, null);
                }
                else if (propertyInfo.PropertyType.GetTypeInfo().IsAssignableFrom(this.Value.GetType().GetTypeInfo()))
                {
                    var value = this.Value;
                    propertyInfo.SetValue(obj, value);
                }
                else
                {
                    {
                        try
                        {
                            object value = Convert.ChangeType(this.Value, propertyInfo.PropertyType);
                            propertyInfo.SetValue(obj, value);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to set property: " + ex.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles when the target is changed.
        /// </summary>
        /// <param name="obj">
        /// The set property action.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private static void TargetPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var action = obj as SetPropertyAction;
            if (action != null)
            {
                action.targetObjectPropertyInfo = null;
                action.associatedElementPropertyInfo = null;
            }
        }
    }
}