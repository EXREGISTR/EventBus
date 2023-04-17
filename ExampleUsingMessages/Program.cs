using System.Threading;
using EventBusByMessages;
using ExampleUsingMessages.Messages;

namespace ExampleUsingMessages {
	internal class Program {
		public static void Main(string[] args) {
			var logger = new GameLogger();
			
			Thread.Sleep(4000);
			
			EventBus.RaiseEvent(new PlayerDeadMessage("EXREGISTR", "NOT_EXREGISTR"));
		}
	}
}