using EventBusByMessages;

namespace ExampleUsingMessages.Messages {
	public class PlayerDeadMessage : IMessage {
		public string PlayerName { get; }
		public string EnemyName { get; }

		public PlayerDeadMessage(string playerName, string enemyName) {
			PlayerName = playerName;
			EnemyName = enemyName;
		}
	}
}