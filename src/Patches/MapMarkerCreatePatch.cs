using System.Reflection;
using DynamicMaps.Data;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace WhatsTheKey.Patches
{
    using MapMarker = DynamicMaps.UI.Components.MapMarker;

    // markers loaded from the map files (locked doors among them) are all created through this overload
    internal class MapMarkerCreatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(MapMarker), nameof(MapMarker.Create),
                [typeof(GameObject), typeof(MapMarkerDef), typeof(Vector2), typeof(float), typeof(float)]);
        }

        [PatchPostfix]
        public static void PatchPostfix(MapMarker __result)
        {
            if (__result == null || string.IsNullOrWhiteSpace(__result.AssociatedItemId))
                return;

            __result.gameObject.AddComponent<KeyMarkerClickHandler>().Init(__result);
        }
    }
}
