using System;
using Events;

namespace Test.Subscribers {
	public class GameController : IPlayerDeadHandler {
		private readonly GameLogger logger;
		
		public GameController() {
			// incorrect variant: 
			// EventBus.Subscribe(this);
			// This will not show errors at the compilation stage, but an exception will be thrown in runtime,
			// because the type of event (that is, the subscriber responding to the event) can only be an interface,
			// and not a specific class.
			
			// correct
			EventBus.Subscribe<IPlayerDeadHandler>(this);
			logger = new GameLogger();
		} 
		
		~GameController() => EventBus.Unsubscribe<IPlayerDeadHandler>(this);
		
		public void HandleDeath(string playerName, string enemyName) {
			Console.WriteLine($"GameController has received message about player {playerName} death");
		}
	}
}