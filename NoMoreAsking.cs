using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;

namespace NoMoreAsking {
	[BepInPlugin("com.sauler.cms.nomoreasking", "No more asking", "2.0.0.0")]
	[BepInProcess("cms2018.exe")]
	public class NoMoreAsking : BaseUnityPlugin {
		#region Unity events
		private void Awake() {
			LoadConfig();
			Patch();
		}

		private void OnDestroy() {
			Unpatch();
		}
		#endregion

		#region Patching
		private Harmony harmonyInstance;
		private bool patched;
		
		private void Patch() {
			if (patched)
				return;
			
			harmonyInstance = new Harmony("com.sauler.cms.nomoreasking");
			harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
			patched = true;
			Logger.LogInfo("Patching done!");
		}
		
		private void Unpatch() {
			if (!patched)
				return;
			
			harmonyInstance.UnpatchAll();
			patched = false;
		}
		#endregion
		
		#region Config
		private void LoadConfig() {
			var defaultShortcut = new KeyboardShortcut(KeyCode.LeftShift);
			
			Settings.keyboardShortcut = Config.Bind("General", 
				"Shortcut",
				defaultShortcut,
				"Keyboard shortcut that need to be pressed to skip AskWindow");
		}
		#endregion
	}
}