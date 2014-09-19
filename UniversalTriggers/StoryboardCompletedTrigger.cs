// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoryboardCompletedTrigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the StoryboardCompletedTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Animation;

    /// <summary>
    /// The storyboard completed trigger.
    /// </summary>
    public class StoryboardCompletedTrigger : EventHookingTrigger
    {
        /// <summary>
        /// The storyboard property.
        /// </summary>
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(
            "Storyboard",
            typeof(Storyboard),
            typeof(StoryboardCompletedTrigger),
            new PropertyMetadata(DependencyProperty.UnsetValue, StoryboardPropertyChanged));

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
        /// Handles when the storyboard has changed.
        /// </summary>
        /// <param name="obj">
        /// The storyboard completed trigger.
        /// </param>
        /// <param name="args">
        /// The event arguments.
        /// </param>
        private static void StoryboardPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var trigger = obj as StoryboardCompletedTrigger;
            if (trigger == null)
            {
                return;
            }

            if (args.OldValue != null)
            {
                trigger.UnhookEvent(args.OldValue);
            }

            if (args.NewValue != null)
            {
                trigger.HookEvent("Completed", args.NewValue);
            }
        }
    }
}