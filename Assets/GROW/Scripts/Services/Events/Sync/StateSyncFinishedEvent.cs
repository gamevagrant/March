using Grow.Highway;
using Grow;
using System;
using System.Collections.Generic;

namespace Grow.Sync {
	public class StateSyncFinishedEvent : GrowEvent {

		public readonly IList<string> ChangedComponents;
		public readonly IList<string> FailedComponents;

		public StateSyncFinishedEvent(IList<string> changedComponents, IList<string> failedComponents) {
			this.ChangedComponents = changedComponents;
			this.FailedComponents = failedComponents;
		}
	}
}