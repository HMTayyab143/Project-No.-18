// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventHookingTrigger.cs" company="James Croft">
//    Copyright (C) James Croft 2014. All rights reserved.
// </copyright>
// <summary>
//   Defines the EventHookingTrigger type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UniversalTriggers
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices.WindowsRuntime;

    using Windows.UI.Xaml;

    /// <summary>
    /// A trigger that hooks into an event on an element.
    /// </summary>
    public abstract class EventHookingTrigger : Trigger
    {
        /// <summary>
        /// The hooked event.
        /// </summary>
        private WeakReference<EventHooker> hookedEvent;

        /// <summary>
        /// Gets the event target object.
        /// </summary>
        /// <param name="target">
        /// The target object, if known.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        protected object GetEventTarget(object target)
        {
            return target == DependencyProperty.UnsetValue || target == null ? this.AssociatedElement : target;
        }

        /// <summary>
        /// Hooks an event on the target object.
        /// </summary>
        /// <param name="eventName">
        /// The event name.
        /// </param>
        /// <param name="targetObj">
        /// The source object.
        /// </param>
        protected void HookEvent(string eventName, object targetObj)
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                var target = this.GetEventTarget(targetObj);

                if (target != null)
                {
                    var eventHooker = new EventHooker(this);
                    eventHooker.HookEvent(target, eventName);

                    this.hookedEvent = new WeakReference<EventHooker>(eventHooker);
                }
            }
        }

        /// <summary>
        /// Unhooks an event on the target object.
        /// </summary>
        /// <param name="targetObj">
        /// The target object.
        /// </param>
        protected void UnhookEvent(object targetObj)
        {
            EventHooker currentEventHooker;

            var eventTarget = this.GetEventTarget(targetObj);
            if (eventTarget != null && this.hookedEvent != null && this.hookedEvent.TryGetTarget(out currentEventHooker))
            {
                currentEventHooker.UnhookEvent(eventTarget);
            }

            this.hookedEvent = null;
        }

        /// <summary>
        /// The event hooker.
        /// </summary>
        private sealed class EventHooker
        {
            /// <summary>
            /// The event trigger.
            /// </summary>
            private readonly EventHookingTrigger binding;

            /// <summary>
            /// The event info.
            /// </summary>
            private EventInfo eventInfo;

            /// <summary>
            /// The event registration token.
            /// </summary>
            private EventRegistrationToken? eventRegistrationToken;

            /// <summary>
            /// The event handler.
            /// </summary>
            private Delegate handler;

            /// <summary>
            /// Initializes a new instance of the <see cref="EventHooker"/> class.
            /// </summary>
            /// <param name="binding">
            /// The event trigger binding.
            /// </param>
            public EventHooker(EventHookingTrigger binding)
            {
                this.binding = binding;
            }

            /// <summary>
            /// Hooks up the element with the given named event.
            /// </summary>
            /// <param name="obj">
            /// The object to hook the event to.
            /// </param>
            /// <param name="eventName">
            /// The name of the event to hook up.
            /// </param>
            public void HookEvent(object obj, string eventName)
            {
                this.eventInfo = GetEventInfo(obj.GetType(), eventName);

                if (this.eventInfo != null)
                {
                    this.handler = this.GetEventHandler(this.eventInfo);

                    if (this.eventInfo.AddMethod.ReturnType == typeof(EventRegistrationToken))
                    {
                        WindowsRuntimeMarshal.AddEventHandler(
                            e =>
                                {
                                    this.eventRegistrationToken =
                                        (EventRegistrationToken)this.eventInfo.AddMethod.Invoke(obj, new object[] { e });
                                    return this.eventRegistrationToken.Value;
                                },
                            e => this.eventInfo.RemoveMethod.Invoke(obj, new object[] { e }),
                            this.handler);
                    }
                    else
                    {
                        this.eventInfo.AddMethod.Invoke(obj, new object[] { this.handler });
                    }
                }
            }

            /// <summary>
            /// Unhooks the element with the given named event.
            /// </summary>
            /// <param name="obj">
            /// The object to unhook the event from.
            /// </param>
            public void UnhookEvent(object obj)
            {
                if (this.eventInfo != null && this.handler != null)
                {
                    if (this.eventInfo.AddMethod.ReturnType == typeof(EventRegistrationToken))
                    {
                        if (this.eventRegistrationToken != null)
                        {
                            this.eventInfo.RemoveMethod.Invoke(obj, new object[] { this.eventRegistrationToken });
                            this.eventRegistrationToken = null;
                        }
                    }
                    else
                    {
                        this.eventInfo.RemoveMethod.Invoke(obj, new object[] { this.handler });
                    }

                    this.handler = null;
                    this.eventInfo = null;
                }
            }

            /// <summary>
            /// Gets the event information from the specified event.
            /// </summary>
            /// <param name="type">
            /// The type where the event is.
            /// </param>
            /// <param name="eventName">
            /// The name of the event.
            /// </param>
            /// <returns>
            /// The <see cref="EventInfo"/>.
            /// </returns>
            private static EventInfo GetEventInfo(Type type, string eventName)
            {
                EventInfo eventInfo;

                do
                {
                    var typeInfo = type.GetTypeInfo();
                    eventInfo = typeInfo.GetDeclaredEvent(eventName);
                    type = typeInfo.BaseType;
                }
                while (eventInfo == null && type != null);

                return eventInfo;
            }

            /// <summary>
            /// Gets the event handler for handling the event.
            /// </summary>
            /// <param name="info">
            /// The information about the event.
            /// </param>
            /// <returns>
            /// The <see cref="Delegate"/>.
            /// </returns>
            private Delegate GetEventHandler(EventInfo info)
            {
                if (info == null)
                {
                    throw new ArgumentNullException("info");
                }

                if (info.EventHandlerType == null)
                {
                    throw new ArgumentException("info.EventHandlerType must be set");
                }

                var eventRaisedMethod = this.GetType().GetTypeInfo().GetDeclaredMethod("OnEventRaised");
                return eventRaisedMethod.CreateDelegate(info.EventHandlerType, this);
            }

            /// <summary>
            /// This event is fired via reflection.
            /// </summary>
            /// <param name="sender">
            /// The sender.
            /// </param>
            /// <param name="eventData">
            /// The event data.
            /// </param>
            private void OnEventRaised(object sender, object eventData)
            {
                this.binding.OnTriggered(eventData);
            }
        }
    }
}