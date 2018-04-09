using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class UserRatingEvent : SoomlaEvent
	{
		public UserRatingEvent () : this(null)
		{

		}

		public UserRatingEvent (Object sender) : base(sender)
		{

		}
	}
}
