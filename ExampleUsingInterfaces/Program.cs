using System.Threading;
using Events;
using Test.Subscribers;

namespace Test {
	internal class Program {
		public static void Main(string[] args) {
			var gameController = new GameController();
			Thread.Sleep(5000);
			
			EventBus.RaiseEvent<IPlayerDeadHandler>(playerDeadHandler => {
				playerDeadHandler.HandleDeath("EXREGISTR", "NOT_EXREGISTR");
			});
		}
	}
}