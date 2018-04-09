using System;

namespace Soomla.Profile
{
    public class SocialActionCancelledEvent : BaseSocialActionEvent
    {
		public readonly SocialActionType SocialType;
		public readonly String payload;
		
		public SocialActionCancelledEvent(Provider provider, SocialActionType socialType, String payload) : base(provider)
		{
			this.SocialType = socialType;
			this.payload = payload;
		}
    }
}
