// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomVisualStateManager.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the CustomVisualStateManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;
    using System.Linq;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    /// <summary>
    /// The custom visual state manager.
    /// </summary>
    internal class CustomVisualStateManager : VisualStateManager
    {
        /// <summary>
        /// The static instance of this.
        /// </summary>
        private static readonly CustomVisualStateManager Instance = new CustomVisualStateManager();

        /// <summary>
        /// Changes the element to a new state.
        /// </summary>
        /// <param name="frameworkElement">
        /// The framework element.
        /// </param>
        /// <param name="stateName">
        /// The state name to change to.
        /// </param>
        public static void GoToState(FrameworkElement frameworkElement, string stateName)
        {
            var visualStateInfo = FindVisualState(frameworkElement, stateName);

            if (visualStateInfo != null)
            {
                Instance.GoToStateCore(
                    visualStateInfo.Control,
                    visualStateInfo.FrameworkElement,
                    stateName,
                    visualStateInfo.VisualStateGroup,
                    visualStateInfo.VisualState,
                    true);
            }
        }

        /// <summary>
        /// Finds the visual state with a given name for an element.
        /// </summary>
        /// <param name="frameworkElement">
        /// The framework element to find the state from.
        /// </param>
        /// <param name="stateName">
        /// The state name.
        /// </param>
        /// <returns>
        /// The <see cref="VisualStateInfo"/>.
        /// </returns>
        private static VisualStateInfo FindVisualState(FrameworkElement frameworkElement, string stateName)
        {
            DependencyObject obj = frameworkElement;
            VisualStateInfo result = null;

            do
            {
                var element = obj as FrameworkElement;
                if (element != null)
                {
                    result = (from stateGroup in GetVisualStateGroups(element)
                              let visualState =
                                  stateGroup.States.FirstOrDefault(
                                      state => state.Name.Equals(stateName, StringComparison.OrdinalIgnoreCase))
                              where visualState != null
                              select new VisualStateInfo(element, stateGroup, visualState)).FirstOrDefault();
                }

                obj = VisualTreeHelper.GetParent(obj);
            }
            while (result == null && obj != null);

            if (result != null)
            {
                do
                {
                    var control = obj as Control;
                    if (control != null)
                    {
                        result.Control = control;
                        return result;
                    }

                    obj = VisualTreeHelper.GetParent(obj);
                }
                while (obj != null);
            }

            return null;
        }

        /// <summary>
        /// The visual state info.
        /// </summary>
        private class VisualStateInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="VisualStateInfo"/> class.
            /// </summary>
            /// <param name="frameworkElement">
            /// The framework element.
            /// </param>
            /// <param name="group">
            /// The group.
            /// </param>
            /// <param name="visualState">
            /// The visual state.
            /// </param>
            public VisualStateInfo(FrameworkElement frameworkElement, VisualStateGroup group, VisualState visualState)
            {
                this.FrameworkElement = frameworkElement;
                this.VisualStateGroup = group;
                this.VisualState = visualState;
            }

            /// <summary>
            /// Gets or sets the control.
            /// </summary>
            public Control Control { get; set; }

            /// <summary>
            /// Gets or sets the framework element.
            /// </summary>
            public FrameworkElement FrameworkElement { get; set; }

            /// <summary>
            /// Gets the visual state.
            /// </summary>
            public VisualState VisualState { get; private set; }

            /// <summary>
            /// Gets the visual state group.
            /// </summary>
            public VisualStateGroup VisualStateGroup { get; private set; }
        }
    }
}