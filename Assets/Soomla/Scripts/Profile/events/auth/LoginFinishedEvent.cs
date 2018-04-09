using System;
using System.Collections;

namespace Soomla.Profile
{
	public class LoginFinishedEvent : SoomlaEvent
	{
		public readonly UserProfile UserProfile;
		public readonly bool AutoLogin;

		public LoginFinishedEvent (UserProfile userProfile, bool autoLogin, string payload):this(userProfile, autoLogin, payload,null)
		{

		}

		public LoginFinishedEvent (UserProfile userProfile, bool autoLogin, string payload, Object sender):base(sender, payload)
		{
			UserProfile = userProfile;
			AutoLogin = autoLogin;
		}

		public Provider getProvider ()
		{
			return UserProfile.Provider;
		}
	}
}
