using System;

namespace Soomla.Profile
{
    public class SocialActionFailedEvent : BaseSocialActionEvent
    {
        public readonly String ErrorDescription;
		public readonly SocialActionType SocialType;
		public readonly String payload;
		public readonly Provider provider;
		public SocialActionFailedEvent(Provider provider, SocialActionType socialType, String errorDescription, String payload) : base(provider)
		{
			this.provider = provider;
			this.SocialType = socialType;
			this.payload = payload;
			this.ErrorDescription = errorDescription;
		}
    }
}
