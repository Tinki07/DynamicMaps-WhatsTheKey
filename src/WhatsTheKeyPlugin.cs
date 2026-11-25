using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using WhatsTheKey.Patches;

namespace WhatsTheKey
{
    [BepInPlugin("com.tinki07.dynamicmaps.whatsthekey", "DynamicMaps - What's The Key", "1.0.0")]
    [BepInDependency("com.mpstark.dynamicmaps")]
    public class WhatsTheKeyPlugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }
        internal static ConfigEntry<bool> Enabled { get; private set; }

        private void Awake()
        {
            Log = Logger;

            Enabled = Config.Bind(
                "General",
                "Enabled",
                true,
                "Right-click a locked door on the map, out of raid, to search its key on the flea market");

            new MapMarkerCreatePatch().Enable();
            new InventoryScreenShowPatch().Enable();
        }
    }
}
