using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Soomla.Profile
{
	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	public class ProfilePostBuildTool : ISoomlaPostBuildTool {

		#if UNITY_EDITOR

		static ProfilePostBuildTool instance = new ProfilePostBuildTool();
		static ProfilePostBuildTool()
		{
			SoomlaPostBuildTools.AddTool("Profile", instance);
		}

		#region ISoomlaPostBuildTool implementation

		public string GetToolMetaData (BuildTarget target)
		{
			if (target ==
			    #if UNITY_5
			    BuildTarget.iOS
			    #else
			    BuildTarget.iOS
			    #endif
			    )
			{
				return GetProfileMetaIOS();
			}

			return null;
		}

		#endregion

		private string GetProfileMetaIOS ()
		{
			string result = "";
			Dictionary<string, bool?> state = ProfileSettings.IntegrationState;

			result += ProfileSettings.iTunesAppId + "~";

			foreach (var entry in state) {
				Provider targetProvider = Provider.fromString(entry.Key);
				if (entry.Value.HasValue && entry.Value.Value) {
					if (targetProvider == Provider.GOOGLE) {
						result += entry.Key + "^" + PlayerSettings.applicationIdentifier + "^" + ProfileSettings.GPEnableGS + "^" + ProfileSettings.GPClientId + ";";
					}
					else if (targetProvider == Provider.TWITTER) {
						result += entry.Key + "^" + ProfileSettings.TwitterConsumerKey + ";";
					}
					else if (targetProvider == Provider.GAME_CENTER) {
						result += entry.Key;
					}
				}
			}

			return result.ToString();
		}

		#endif
	}
}