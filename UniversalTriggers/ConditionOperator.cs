// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionOperator.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   The various conditions that can be applied to an <see cref="Condition" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    /// <summary>
    /// The operators that can be applied to a <see cref="Condition"/>
    /// </summary>
    public enum ConditionOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }
}