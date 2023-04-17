using Events;

namespace Test.Subscribers {
	public interface IPlayerDeadHandler : IEventHandler {
		public void HandleDeath(string playerName, string enemyName);
	}
}