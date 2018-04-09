using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class LoginCancelledEvent: SoomlaEvent
	{
		public readonly Provider Provider;
		public readonly bool AutoLogin;

		public LoginCancelledEvent (Provider provider, bool autoLogin, string payload): this (provider, autoLogin, payload, null)
		{

		}

		public LoginCancelledEvent (Provider provider, bool autoLogin, string payload, Object sender) : base(sender, payload)
		{
			Provider = provider;
			AutoLogin = autoLogin;
		}
	}
}