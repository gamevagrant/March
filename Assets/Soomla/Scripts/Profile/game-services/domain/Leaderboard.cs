/// Copyright (C) 2012-2015 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

namespace Soomla.Profile {

	/// <summary>
	/// This class holds information about the user for a specific <c>Provider</c>.
	/// </summary>
	public class Leaderboard {

		private const string TAG = "SOOMLA UserProfile";

		public readonly string ID;
		public readonly Provider Provider;
		public readonly string Name;
		public readonly string IconURL;

		public Leaderboard(JSONObject jsonLB) {
			this.ID = jsonLB[PJSONConsts.UP_IDENTIFIER].str;
			this.Provider = Provider.fromString(jsonLB[PJSONConsts.UP_PROVIDER].str);
			if (jsonLB[PJSONConsts.UP_NAME] != null && jsonLB[PJSONConsts.UP_NAME].type == JSONObject.Type.STRING) {
				this.Name = jsonLB[PJSONConsts.UP_NAME].str;
			} else {
				this.Name = "";
			}
			if (jsonLB[PJSONConsts.UP_ICON_URL] != null && jsonLB[PJSONConsts.UP_ICON_URL].type == JSONObject.Type.STRING) {
				this.IconURL = jsonLB[PJSONConsts.UP_ICON_URL].str;
			} else {
				this.IconURL = "";
			}
		}

		public JSONObject toJSONObject() {
			JSONObject obj = new JSONObject();
			obj.AddField(PJSONConsts.UP_IDENTIFIER, this.ID);
			obj.AddField(PJSONConsts.UP_PROVIDER, this.Provider.ToString());
			obj.AddField(PJSONConsts.UP_NAME, this.Name);
			obj.AddField(PJSONConsts.UP_ICON_URL, this.IconURL);
			return obj;
		}
	}
}

