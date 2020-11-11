using HarmonyLib;

namespace NoMoreAsking.Patches {
	[HarmonyPatch(typeof(UIManager))]
	[HarmonyPatch("ShowAskWindow")]
	[HarmonyPatch(new[] { typeof(NewHash) })]
	internal class SkipAskWindow {
		public static bool Prefix(NewHash hash) {
			if (!Settings.keyboardShortcut.Value.IsPressed()) 
				return true;

			var type = hash.GetFromKey("WindowType") as string;
			if (type == "SellPerCondition")
				return true;
			
			GameManager.Get().ButtonAccept(hash);
			return false;
		}
	}
}