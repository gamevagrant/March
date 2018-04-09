using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class GetScoresStartedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard From;
		public readonly bool FromStart;

		public GetScoresStartedEvent(Provider provider, Leaderboard from, bool fromStart, string payload) : base(provider, payload)
		{
			this.From = from;
			this.FromStart = fromStart;
		}
	}
}