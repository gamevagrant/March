using System;
using System.Collections;
using System.Collections.Generic;

namespace Grow
{
	public class GrowEvent
	{
		public readonly Object Sender;
		public readonly String Payload;

		public GrowEvent() { }

		public GrowEvent(Object sender) : this(sender, "")
		{

		}

		public GrowEvent(Object sender, String payload)
		{
			this.Sender = sender;
			this.Payload = payload;
		}
	}
}
