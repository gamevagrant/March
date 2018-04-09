using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class LoginFailedEvent : SoomlaEvent
	{
		public readonly Provider Provider;
		public readonly string ErrorDescription;
		public readonly bool AutoLogin;

		public LoginFailedEvent (Provider provider, string errorDescription, bool autoLogin, string payload) : this(provider, errorDescription, autoLogin, payload, null)
		{

		}

		public LoginFailedEvent (Provider provider, string errorDescription, bool autoLogin, string payload, Object sender) : base(sender,payload)
		{
			Provider = provider;
			ErrorDescription = errorDescription;
			AutoLogin = autoLogin;
		}
	}
}
