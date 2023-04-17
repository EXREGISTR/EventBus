using System;
using System.Collections.Generic;

namespace Events {
	public static class EventBus {
		public static Action<string> WarningLogger { get; set; }
		public static Action<string> ErrorLogger { get; set; }

		static EventBus() {
			ErrorLogger = Console.Error.WriteLine;
			WarningLogger = Console.WriteLine;
		}

		private static readonly Dictionary<Type, IEventHandlersList> eventsMap = new(8);

		/// <summary>
		/// Subscribes handler on the event of type T
		/// </summary>
		/// <typeparam name="T">Type of event handler. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		/// <exception cref="InvalidCastException">If doesn't successed to cast to handlers list by type T</exception>
		public static void Subscribe<T>(T subscriber) where T : IEventHandler {
			var subscriberType = typeof(T);
			if (TryGetHandlersList<T>(subscriberType, out var handlers)) {
				handlers.AddSubscriber(subscriber);
				return;
			}
			
			handlers = new EventHandlersList<T>();
			handlers.AddSubscriber(subscriber);
			eventsMap[subscriberType] = handlers;
		}
		
		/// <summary>
		/// Unsubscribes handler object of the event of type T
		/// </summary>
		/// <typeparam name="T">Type of event handler. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		/// <exception cref="InvalidCastException">If doesn't successed to cast to handlers list by type T</exception>
		public static void Unsubscribe<T>(T subscriber) where T: IEventHandler {
			var subscriberType = typeof(T);
			if (TryGetHandlersList<T>(subscriberType, out var handlers)) {
				handlers.RemoveSubscriber(subscriber);
			}
		}

		/// <summary>
		/// Raises event of the event of type T
		/// </summary>
		/// <typeparam name="T">Type of event handler. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		/// <exception cref="InvalidCastException">If doesn't successed to cast to handlers list by type T</exception>
		public static void RaiseEvent<T>(Action<T> callback) where T : IEventHandler {
			var subscriberType = typeof(T);
			if (!TryGetHandlersList<T>(subscriberType, out var handlers)) {
				WarningLogger($"No list of handlers found for subscriber type {subscriberType}");
				return;
			}
			
			foreach (var subscriber in handlers) {
				try {
					callback.Invoke(subscriber);
				} catch (Exception e) {
					ErrorLogger(e.Message);
				}
			}
		}

		/// <summary>
		/// Clear subscribers for event of type T
		/// </summary>
		/// <typeparam name="T">Type of event handler. Should be interface</typeparam>
		/// <exception cref="ArgumentException">If type parameter T is not interface</exception>
		public static void ClearSubscribers<T>() where T : IEventHandler {
			var subscriberType = typeof(T);
			
			if (!subscriberType.IsInterface) {
				throw new ArgumentException(GetIncorrectTypeMessage(subscriberType));
			}
			
			if (eventsMap.TryGetValue(subscriberType, out IEventHandlersList handlers)) {
				handlers.Clear();
			}
		}
		
		/// <summary>
		/// Clears subscribers on all events
		/// </summary>
		public static void Reset() {
			foreach (var handlers in eventsMap.Values) {
				handlers.Clear();
			}
			
			eventsMap.Clear();
		}

		private static bool TryGetHandlersList<T>(Type subscriberType, out EventHandlersList<T> founded)
			where T : IEventHandler {
			if (!subscriberType.IsInterface) {
				throw new ArgumentException(GetIncorrectTypeMessage(subscriberType));
			}

			if (eventsMap.TryGetValue(subscriberType, out IEventHandlersList list)) {
				if (list is not EventHandlersList<T> handlers) {
					throw new InvalidCastException(
						$"Failed to convert type {list.GetType()} to type {typeof(EventHandlersList<T>)}");
				}

				founded = handlers;
				return true;
			}

			founded = null;
			return false;
		}

		private static string GetIncorrectTypeMessage(Type subscriberType) {
			return $"It is not possible to register a handler for a handler type {subscriberType} that is not an interface";
		}
	}
}