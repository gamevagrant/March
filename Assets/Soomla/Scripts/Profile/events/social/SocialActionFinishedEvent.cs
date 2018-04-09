using System;

namespace Soomla.Profile
{
    public class SocialActionFinishedEvent : BaseSocialActionEvent
    {
		public readonly SocialActionType SocialType;
		public readonly String payload;
		
		public SocialActionFinishedEvent(Provider provider, SocialActionType socialType, String payload) : base(provider)
		{
			this.SocialType = socialType;
			this.payload = payload;
		}
    }
}
