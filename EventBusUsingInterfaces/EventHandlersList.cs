using System.Collections.Generic;

namespace Events {
	internal sealed class EventHandlersList<T> : IEventHandlersList where T: IEventHandler {
		private readonly List<T> subscribers = new();

		internal void AddSubscriber(T handler) {
			if (subscribers.Contains(handler)) {
				EventBus.WarningLogger($"Handler {handler.GetType()} already registered for event {typeof(T)}!");
				return;
			}
			
			subscribers.Add(handler);
		}

		internal void RemoveSubscriber(T handler) => subscribers.Remove(handler);
		
		void IEventHandlersList.ClearSubscribers() => subscribers.Clear();
		internal IEnumerable<T> GetAllSubscribers() => subscribers;
	}
}