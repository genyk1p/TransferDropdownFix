using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TransferDropdownFix
{
    /// <summary>
    /// Konstruction's "Transfer resources" window shows no vessel name in its two closed drop-downs: the open
    /// list has the names, the closed drop-down stays blank, even the default "Select a target..." (seen
    /// 2026-10-13, Konstruction v112.0.1). The caption is wired and holds the right text, but its rect is 17 px
    /// high for Arial 16, whose line is ~18-19 px; with Unity's default vertical overflow (Truncate) a line that
    /// does not fit is not drawn at all. A postfix on ResourceTransferWindow.ShowWindow sets the caption's
    /// vertical overflow to Overflow.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class Loader : MonoBehaviour
    {
        private const string HarmonyId = "TransferDropdownFix";

        private void Start()
        {
            if (AssemblyLoader.loadedAssemblies.All(a => a.name != "KonstructionUI"))
            {
                Log.Warning("Konstruction (KonstructionUI.dll) is not installed. Nothing patched.");
                return;
            }
            try
            {
                new Harmony(HarmonyId).Patch(
                    AccessTools.Method(typeof(KonstructionUI.ResourceTransferWindow), nameof(KonstructionUI.ResourceTransferWindow.ShowWindow)),
                    postfix: new HarmonyMethod(typeof(Loader), nameof(ShowWindowPostfix)));
                Log.Info("patched KonstructionUI.ResourceTransferWindow.ShowWindow (drop-down captions)");
            }
            catch (Exception e)
            {
                Log.Error($"patching failed, Konstruction left as is: {e}");
            }
        }

        private static void ShowWindowPostfix(KonstructionUI.ResourceTransferWindow __instance)
        {
            try
            {
                foreach (string field in new[] { "TargetADropdown", "TargetBDropdown" })
                    FixCaption(Traverse.Create(__instance).Field(field).GetValue<Dropdown>());
            }
            catch (Exception e)
            {
                Log.Error($"drop-down fix failed: {e}");
            }
        }

        private static void FixCaption(Dropdown dropdown)
        {
            Text caption = dropdown != null ? dropdown.captionText : null;
            if (caption == null || caption.verticalOverflow == VerticalWrapMode.Overflow)
                return;
            caption.verticalOverflow = VerticalWrapMode.Overflow;
            dropdown.RefreshShownValue();
        }
    }
}
