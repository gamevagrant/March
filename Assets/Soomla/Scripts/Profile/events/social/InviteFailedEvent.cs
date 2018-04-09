using System;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class InviteFailedEvent : BaseSocialActionEvent
    {
        public readonly String ErrorDescription;

        public readonly String payLoad;
		
        public InviteFailedEvent(Provider provider, String errorDescription, String payLoad) : base(provider)
        {
			this.payLoad = payLoad;
        }
    }
}
