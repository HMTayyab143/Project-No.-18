// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAssociatableElement.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the IAssociatableElement type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers.Interfaces
{
    using Windows.UI.Xaml;

    /// <summary>
    /// The AssociatableElement interface.
    /// </summary>
    public interface IAssociatableElement
    {
        /// <summary>
        /// Gets the associated element attached to this.
        /// </summary>
        /// <value>
        /// The associated object.
        /// </value>
        FrameworkElement AssociatedElement { get; }

        /// <summary>
        /// Attaches an element to this.
        /// </summary>
        /// <param name="element">
        /// The element to attach.
        /// </param>
        void Attach(FrameworkElement element);

        /// <summary>
        /// Detaches the associated element.
        /// </summary>
        void Detach();
    }
}