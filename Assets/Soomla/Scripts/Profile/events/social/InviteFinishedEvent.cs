using System;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class InviteFinishedEvent : BaseSocialActionEvent
    {
        public readonly string RequestId;

        public readonly List<String> InvitedIds;

		public readonly String payLoad;
		
        public InviteFinishedEvent(Provider provider, String requestId, List<String> invitedIds, String payLoad) : base(provider)
        {
            RequestId = requestId;
            InvitedIds = invitedIds;
			this.payLoad = payLoad;
        }
    }
}
