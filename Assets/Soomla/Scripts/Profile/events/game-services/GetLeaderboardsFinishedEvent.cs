using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class GetLeaderboardsFinishedEvent : BaseSocialActionEvent
	{
		public readonly SocialPageData<Leaderboard> Leaderboards;
		public GetLeaderboardsFinishedEvent(Provider provider, SocialPageData<Leaderboard> leaderboards, string payload)
			: base(provider, payload) {
			this.Leaderboards = leaderboards;
		}
	}
}