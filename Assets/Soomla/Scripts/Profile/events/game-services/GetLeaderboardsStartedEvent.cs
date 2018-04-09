using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class GetLeaderboardsStartedEvent : BaseSocialActionEvent
	{
		public GetLeaderboardsStartedEvent(Provider provider, string payload) : base(provider, payload) {

		}
	}
}