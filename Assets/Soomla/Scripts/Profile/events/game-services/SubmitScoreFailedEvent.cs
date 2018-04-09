using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class SubmitScoreFailedEvent : BaseSocialActionEvent
	{
		public readonly Leaderboard Destination;
		public readonly string ErrorDescription;

		public SubmitScoreFailedEvent(Provider provider, Leaderboard destination, string errorDescripion, string payload) : base(provider, payload)
		{
			this.Destination = destination;
			this.ErrorDescription = errorDescripion;
		}
	}
}