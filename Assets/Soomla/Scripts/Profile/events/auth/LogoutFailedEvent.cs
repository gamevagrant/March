using System;
using System.Collections;

namespace Soomla.Profile
{
	public class LogoutFailedEvent: SoomlaEvent
	{
		public readonly Provider Provider;
		public readonly string ErrorDescription;

		public LogoutFailedEvent (Provider provider, string errorDescription): this(provider, errorDescription, null)
		{

		}

		public LogoutFailedEvent (Provider provider, string errorDescription, Object sender) : base(sender)
		{
			Provider = provider;
			ErrorDescription = errorDescription;
		}
	}
}
