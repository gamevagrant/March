using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class SubmitScoreFinishedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard Destination;
		public readonly Score Score;

		public SubmitScoreFinishedEvent(Provider provider, Leaderboard destination, Score newScore, string payload) : base(provider, payload)
		{
			this.Destination = destination;
			this.Score = newScore;
		}
	}
}