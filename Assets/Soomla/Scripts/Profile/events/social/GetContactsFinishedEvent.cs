using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class GetContactsFinishedEvent : BaseSocialActionEvent
    {
		public readonly SocialPageData<UserProfile> Contacts;
        public readonly bool HasMore;
		public readonly String payLoad;
		public GetContactsFinishedEvent(Provider provider, SocialPageData<UserProfile> contacts, string payLoad) : base(provider)
        {
            this.Contacts = contacts;
			this.payLoad = payLoad;
        }
    }
}
