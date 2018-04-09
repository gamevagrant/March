using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class GetScoresFinishedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard From;
		public readonly SocialPageData<Score> Scores;

		public GetScoresFinishedEvent(Provider provider, Leaderboard from, SocialPageData<Score> scores, string payload)
			: base(provider, payload)
		{
			this.From = from;
			this.Scores = scores;
		}
	}
}