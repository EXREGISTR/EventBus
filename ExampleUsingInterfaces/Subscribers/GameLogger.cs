using System;
using Events;

namespace Test.Subscribers {
	public class GameLogger : IPlayerDeadHandler, IMeowHandler {
		public GameLogger() {
			EventBus.Subscribe<IPlayerDeadHandler>(this);
			EventBus.Subscribe<IMeowHandler>(this);
			Console.WriteLine("Game logger has subscribed on events");
		}

		~GameLogger() {
			EventBus.Unsubscribe<IPlayerDeadHandler>(this);
			EventBus.Unsubscribe<IMeowHandler>(this);
			Console.WriteLine("Game logger has unsubscribed of events");
		}
		
		public void HandleDeath(string playerName, string enemyName) {
			Console.WriteLine($"Player {playerName} was killed by {enemyName}");
		}

		public void Meow() {
			Console.WriteLine("Meow )))000))");
		}
	}
}