using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class GetContactsStartedEvent : BaseSocialActionEvent
    {
		public readonly bool FromStart;
		public readonly String payload;

		public GetContactsStartedEvent(Provider provider, bool fromStart, String payload) : base(provider)
		{
			this.FromStart = fromStart;
			this.payload = payload;
		}
    }
}
