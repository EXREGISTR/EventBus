using Events;

namespace Test.Subscribers {
	public interface IMeowHandler : IEventHandler {
		public void Meow();
	}
}