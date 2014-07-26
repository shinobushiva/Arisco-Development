namespace Arisco
{
	namespace Core
	{
		public abstract class WorldEventAdapter : IWorldEventListener
		{

	
			public virtual void AgentAdded (World world, AAgent agent)
			{

			}
	
			public virtual void AgentRemoved (World world, AAgent agent)
			{

			}
	
			public virtual  void Committed (World world)
			{

			}
	
			public virtual void Disposed (World world)
			{

			}
	
			public void Ended (World world)
			{

			}
	
			public virtual void Initialized (World world)
			{

			}
	
			public virtual void Began (World world)
			{

			}
	
			public virtual void Stepped (World world)
			{

			}

		}

	}
}
