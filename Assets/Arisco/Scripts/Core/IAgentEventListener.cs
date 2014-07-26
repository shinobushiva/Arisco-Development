using System.Collections.Generic;
using Arisco.Core;

namespace Arisco
{
	namespace Core
	{
		public interface IAgentEventListener
		{

			void Initialized (AAgent agent);

			void Began (AAgent agent);

			void Stepped (AAgent agent);

			void Committed (AAgent agent);

			void Ended (AAgent agent);

			void Disposed (AAgent agent);

		}
	}
}