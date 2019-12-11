using System.Reflection;
using Harmony;
using UnityEngine;
using CMS.Mods;

namespace NoMoreAsking {
	public class Main : Mod {
		private HarmonyInstance harmonyInstance;
		
		public override void Activate() {
			harmonyInstance = HarmonyInstance.Create("com.sauler.cms.nomoreasking");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
		}

		public override void Deactivate() {
			harmonyInstance.UnpatchAll();
		}

		public override ModInfo GetInfo() {
			return new ModInfo {
				Name = "No more asking",
				Description = "Disables confirmation windows if you hold Left Shift when clicking.",
				Author = "Rafał Babiarz",
				Version = "1.0.0"
			};
		}
	}
	
	[HarmonyPatch(typeof(UIManager))]
	[HarmonyPatch("ShowAskWindow")]
	[HarmonyPatch(new[] { typeof(NewHash) })]
	class NoMoreAskingPatch {
		[HarmonyPrefix]
		public static bool Prefix(NewHash hash) {
			if (!Input.GetKey(KeyCode.LeftShift)) 
				return true;

			var type = hash.GetFromKey("WindowType") as string;
			if (type == "SellPerCondition")
				return true;
			
			GameManager.Get().ButtonAccept(hash);
			return false;
		}
	}
}