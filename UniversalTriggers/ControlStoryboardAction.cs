// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlStoryboardAction.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the ControlStoryboardAction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The control storyboard action.
    /// </summary>
    public class ControlStoryboardAction : TriggerAction
    {
        /// <summary>
        /// The action property.
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register(
            "Action",
            typeof(StoryboardAction),
            typeof(ControlStoryboardAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// The storyboard property.
        /// </summary>
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(
            "Storyboard",
            typeof(Storyboard),
            typeof(ControlStoryboardAction),
            new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        public StoryboardAction Action
        {
            get
            {
                return (StoryboardAction)this.GetValue(ActionProperty);
            }

            set
            {
                this.SetValue(ActionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the storyboard.
        /// </summary>
        public Storyboard Storyboard
        {
            get
            {
                return (Storyboard)this.GetValue(StoryboardProperty);
            }

            set
            {
                this.SetValue(StoryboardProperty, value);
            }
        }

        /// <summary>
        /// Invokes the Storyboard based on the given action.
        /// </summary>
        /// <param name="eventData">
        /// The event data.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the given action is unknown.
        /// </exception>
        protected override void DoInvoke(object eventData)
        {
            if (this.Storyboard != null && this.AssociatedElement != null)
            {
                switch (this.Action)
                {
                    case StoryboardAction.Stop:
                        this.Storyboard.Stop();
                        break;
                    case StoryboardAction.Start:
                        this.Storyboard.Begin();
                        break;
                    case StoryboardAction.Pause:
                        this.Storyboard.Pause();
                        break;
                    default:
                        throw new InvalidOperationException("Unknown action " + this.Action);
                }
            }
        }
    }
}