using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class GetScoresFailedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard From;
		public readonly bool FromStart;
		public readonly string ErrorDescription;

		public GetScoresFailedEvent(Provider provider, Leaderboard from, bool fromStart, string errorDescription, string payload)
			: base(provider, payload)
		{
			this.From = from;
			this.FromStart = fromStart;
			this.ErrorDescription = errorDescription;
		}
	}
}