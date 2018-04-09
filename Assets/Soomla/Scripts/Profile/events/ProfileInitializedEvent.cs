using System;
using System.Collections;

namespace Soomla.Profile
{
	public class ProfileInitializedEvent : SoomlaEvent
    {
		public ProfileInitializedEvent() : this(null)
        {

        }

		public ProfileInitializedEvent(Object sender) : base(sender)
		{

		}
    }
}