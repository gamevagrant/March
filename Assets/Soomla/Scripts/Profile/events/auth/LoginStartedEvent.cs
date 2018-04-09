using System;
using System.Collections;

namespace Soomla.Profile
{
	public class LoginStartedEvent : SoomlaEvent
	{
		public readonly Provider Provider;
		public readonly bool AutoLogin;

		public LoginStartedEvent (Provider provider, bool autoLogin, String payload):this(provider, autoLogin, payload, null)
		{

		}

		public LoginStartedEvent (Provider provider, bool autoLogin, String payload, Object sender): base(sender, payload)
		{
			Provider = provider;
			AutoLogin = autoLogin;
		}
	}
}
