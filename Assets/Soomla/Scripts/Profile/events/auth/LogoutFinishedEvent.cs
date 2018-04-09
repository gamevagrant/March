using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class LogoutFinishedEvent : SoomlaEvent
    {
        public readonly Provider Provider;

        public LogoutFinishedEvent(Provider provider) : this (provider, null)
        {

		}

		public LogoutFinishedEvent(Provider provider, Object sender): base(sender)
		{
			Provider = provider;
		}
    }
}