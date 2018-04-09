using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class LogoutStartedEvent:SoomlaEvent
	{
		public readonly Provider Provider;

		public LogoutStartedEvent (Provider provider): this(provider,null)
		{

		}

		public LogoutStartedEvent (Provider provider, Object sender): base(sender)
		{
			Provider = provider;
		}
	}
}
