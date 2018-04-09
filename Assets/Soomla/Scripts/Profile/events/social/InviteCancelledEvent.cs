using System;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class InviteCancelledEvent : BaseSocialActionEvent
    {
		public readonly String payLoad;
		
		public InviteCancelledEvent(Provider provider, String payLoad) : base(provider)
		{
			this.payLoad = payLoad;
		}
    }
}
