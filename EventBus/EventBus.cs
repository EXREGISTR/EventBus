using System;
using System.Collections.Generic;

namespace Events {
	public static class EventBus {
		public static Action<string> ErrorLogger { get; set; }
		public static Action<string> WarningLogger { get; set; }

		static EventBus() {
			ErrorLogger = Console.Error.WriteLine;
			WarningLogger = Console.WriteLine;
		}

		private static readonly Dictionary<Type, IEventHandlersList> eventsMap = new();

		/// <summary>
		/// Subscribes handler on the message of type T
		/// </summary>
		/// <typeparam name="T">Type of message. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		/// <exception cref="InvalidCastException">If doesn't successed to cast to message type T</exception>
		public static void Subscribe<T>(T subscriber) where T : IEventHandler {
			var subscriberType = typeof(T);
			if (TryGetHandlersList<T>(subscriberType, out var handlers)) {
				handlers.AddSubscriber(subscriber);
				return;
			}
			
			var eventHandlers = new EventHandlersList<T>();
			eventHandlers.AddSubscriber(subscriber);
			eventsMap[subscriberType] = eventHandlers;
		}
		
		/// <summary>
		/// Unsubscribes handler object of the message
		/// </summary>
		/// <typeparam name="T">Type of message. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		/// <exception cref="InvalidCastException">If doesn't successed to cast to message type T</exception>
		public static void Unsubscribe<T>(T subscriber) where T: IEventHandler {
			var subscriberType = typeof(T);
			if (!TryGetHandlersList<T>(subscriberType, out var handlers)) {
				WarningLogger("Doesn't contains");
				return;
			}
			
			handlers.RemoveSubscriber(subscriber);
		}

		/// <summary>
		/// Raises event of message type
		/// </summary>
		/// <typeparam name="T">Type of message. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		/// <exception cref="InvalidCastException">If doesn't successed to cast to message type T</exception>
		public static void RaiseEvent<T>(Action<T> callback) where T : IEventHandler {
			var subscriberType = typeof(T);
			if (!TryGetHandlersList<T>(subscriberType, out var handlers)) {
				WarningLogger("Doesn't contains");
				return;
			}
			
			foreach (var subscriber in handlers.GetAllSubscribers()) {
				try {
					callback.Invoke(subscriber);
				} catch (Exception e) {
					ErrorLogger(e.Message);
				}
			}
		}

		/// <summary>
		/// Clear subscribers for type T message handler
		/// </summary>
		/// <typeparam name="T">Type of message handler. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		public static void ClearSubscribers<T>() where T : IEventHandler {
			var subscriberType = typeof(T);
			
			if (!subscriberType.IsInterface) {
				throw new ArgumentException();
			}
			
			if (eventsMap.TryGetValue(subscriberType, out IEventHandlersList handlers)) {
				handlers.ClearSubscribers();
				return;
			}

			WarningLogger("Doesn't contains");
		}

		/// <summary>
		/// Clears subscribers on all events
		/// </summary>
		public static void Reset() {
			foreach (var handlers in eventsMap.Values) {
				handlers.ClearSubscribers();
			}
			
			eventsMap.Clear();
		}
		
		private static bool TryGetHandlersList<T>(Type subscriberType, out EventHandlersList<T> founded) where T : IEventHandler {
			if (!subscriberType.IsInterface) {
				throw new ArgumentException();
			}

			if (eventsMap.TryGetValue(subscriberType, out IEventHandlersList list)) {
				if (list is not EventHandlersList<T> handlers) {
					throw new InvalidCastException();
				}

				founded = handlers;
				return true;
			}

			founded = null;
			return false;
		}
	}
}