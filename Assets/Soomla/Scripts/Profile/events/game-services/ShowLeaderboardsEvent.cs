using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class ShowLeaderboardsEvent : BaseSocialActionEvent
	{
		public ShowLeaderboardsEvent(Provider provider, string payload) : base(provider, payload) { }
	}
}