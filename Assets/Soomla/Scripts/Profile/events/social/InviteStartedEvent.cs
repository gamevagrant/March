using System;

namespace Soomla.Profile
{
    public class InviteStartedEvent : BaseSocialActionEvent
    {
		public readonly String payLoad;

        public InviteStartedEvent(Provider provider, String payLoad) : base(provider)
        {
			this.payLoad = payLoad;
        }
    }
}
