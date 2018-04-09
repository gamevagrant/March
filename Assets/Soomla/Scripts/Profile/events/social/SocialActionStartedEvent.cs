using System;

namespace Soomla.Profile
{
    public class SocialActionStartedEvent : BaseSocialActionEvent
    {
		public readonly SocialActionType SocialType;
		public readonly String payload;

		public SocialActionStartedEvent(Provider provider, SocialActionType socialType, String payload) : base(provider)
        {
			this.SocialType = socialType;
			this.payload = payload;
        }
    }
}
