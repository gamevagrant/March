using Grow.Highway;
using Grow;
using System;
using System.Collections.Generic;

namespace Grow.Sync {
	public class ModelSyncFinishedEvent : GrowEvent {

		public readonly IList<string> ChangedComponents;

		public ModelSyncFinishedEvent(IList<string> changedComponents) {
			this.ChangedComponents = changedComponents;
		}
	}
}