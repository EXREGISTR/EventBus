using System;
using EventsByMessages;
using ExampleUsingMessages.Messages;

namespace ExampleUsingMessages {
	public class GameLogger {
		public GameLogger() {
			EventBus.Subscribe<PlayerDeadMessage>(OnPlayerDead);
			Console.WriteLine("Subscribed");
		}

		~GameLogger() {
			Console.WriteLine("Unsubscribed");
			EventBus.Unsubscribe<PlayerDeadMessage>(OnPlayerDead);
		}

		private void OnPlayerDead(PlayerDeadMessage message) {
			Console.WriteLine($"Player {message.PlayerName} was killed by {message.EnemyName}");
		}
	}
}