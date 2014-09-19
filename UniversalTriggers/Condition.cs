// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   The condition.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The condition.
    /// </summary>
    public class Condition : AssociatableElement
    {
        /// <summary>
        /// The operator property.
        /// </summary>
        public static readonly DependencyProperty OperatorProperty = DependencyProperty.Register(
            "Operator",
            typeof(ConditionOperator),
            typeof(Condition),
            new PropertyMetadata(ConditionOperator.Equals));

        /// <summary>
        /// The left operand property.
        /// </summary>
        public static readonly DependencyProperty LeftOperandProperty = DependencyProperty.Register(
            "LeftOperand",
            typeof(object),
            typeof(Condition),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The right operand property.
        /// </summary>
        public static readonly DependencyProperty RightOperandProperty = DependencyProperty.Register(
            "RightOperand",
            typeof(object),
            typeof(Condition),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        public ConditionOperator Operator
        {
            get
            {
                return (ConditionOperator)this.GetValue(OperatorProperty);
            }

            set
            {
                this.SetValue(OperatorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the left operand.
        /// </summary>
        public object LeftOperand
        {
            get
            {
                return this.GetValue(LeftOperandProperty);
            }

            set
            {
                this.SetValue(LeftOperandProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the right operand.
        /// </summary>
        public object RightOperand
        {
            get
            {
                return this.GetValue(RightOperandProperty);
            }

            set
            {
                this.SetValue(RightOperandProperty, value);
            }
        }

        /// <summary>
        /// Compares the values of the left and right operand using the given operator.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/> result.
        /// </returns>
        public bool CompareValue()
        {
            return ValueComparer.Compare(this.LeftOperand, this.Operator, this.RightOperand);
        }
    }
}