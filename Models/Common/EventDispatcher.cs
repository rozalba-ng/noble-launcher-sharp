using System;
using System.Collections.Generic;
using NoblegardenLauncherSharp.Structures;

namespace NoblegardenLauncherSharp.Models
{
    class EventDispatcher
    {
        private static readonly Dictionary<EventDispatcherEvent, List<Action>> ActionHandlers = new Dictionary<EventDispatcherEvent, List<Action>>();

        public static void CreateSubscription(EventDispatcherEvent subscribeTo, Action action) {
            if (!ActionHandlers.ContainsKey(subscribeTo)) {
                ActionHandlers[subscribeTo] = new List<Action>();
            }
            ActionHandlers[subscribeTo].Add(action);
        }

        public static void RemoveSubscription(EventDispatcherEvent unsubscribeFrom, Action action) {
            var indexOfAction = ActionHandlers[unsubscribeFrom].FindIndex(handler => handler.Equals(action));
            if (indexOfAction == -1)
                return;

            ActionHandlers[unsubscribeFrom].RemoveAt(indexOfAction);
        }

        public static void Dispatch(EventDispatcherEvent dispatchedEvent) {
            if (!ActionHandlers.ContainsKey(dispatchedEvent)) {
                Console.WriteLine("Dispatcher key not found: " + dispatchedEvent);
                return;
            }
            if (ActionHandlers[dispatchedEvent].Count == 0) {
                Console.WriteLine("Dispatcher handler not found: " + dispatchedEvent);
                return;
            }
            ActionHandlers[dispatchedEvent].ForEach(action => action());
        }

        public static void RemoveAllSubscriptionFromEvent(EventDispatcherEvent unsubscribeFrom) {
            ActionHandlers[unsubscribeFrom].Clear();
        }

        public static void RemoveAllSubscriptions() {
            ActionHandlers.Clear();
        }
    }
}
