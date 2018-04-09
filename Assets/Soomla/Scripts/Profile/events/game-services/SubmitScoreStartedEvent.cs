using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class SubmitScoreStartedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard Destination;

		public SubmitScoreStartedEvent(Provider provider, Leaderboard destination, string payload) : base(provider, payload)
		{
			this.Destination = destination;
		}
	}
}