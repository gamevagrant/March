using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class UserProfileUpdatedEvent : SoomlaEvent
    {
        public readonly UserProfile UserProfile;

		public UserProfileUpdatedEvent(UserProfile userProfile) : this(userProfile, null)
        {

		}

		public UserProfileUpdatedEvent(UserProfile userProfile, Object sender) : base(sender)
		{
			this.UserProfile = userProfile;
		}
    }
}
