namespace Arisco
{
	namespace Core
	{
		public interface IWorldEventListener
		{

			void Initialized (World world);

			void Began (World world);

			void Stepped (World world);

			void Committed (World world);

			void Ended (World world);

			void Disposed (World world);

			void AgentAdded (World world, AAgent agent);

			void AgentRemoved (World world, AAgent agent);

		}

	}
}