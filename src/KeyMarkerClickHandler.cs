using System;
using DynamicMaps.Utils;
using EFT.UI;
using EFT.UI.Ragfair;
using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;
using WhatsTheKey.Patches;

namespace WhatsTheKey
{
    using MapMarker = DynamicMaps.UI.Components.MapMarker;

    internal class KeyMarkerClickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        // Text isn't public on MapMarker
        private static readonly Func<MapMarker, string> GetMarkerText = CreateTextGetter();

        private MapMarker _marker;

        public void Init(MapMarker marker)
        {
            _marker = marker;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // needed so that this marker, and not a parent, receives the click
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right || _marker == null)
                return;

            var keyId = _marker.AssociatedItemId;
            var fleaAvailable = ItemUiContext.Instance?.Session?.RagFair != null;
            if (!KeySearch.CanSearch(WhatsTheKeyPlugin.Enabled.Value, GameUtils.IsInRaid(), fleaAvailable, keyId))
                return;

            var label = KeySearch.GetLabel(keyId, KeySearch.GetLocaleKey(keyId).BsgLocalized());
            KeySearchMenu.Open(_marker, GetMarkerText(_marker), label, () => SearchOnFlea(keyId), eventData.position);
        }

        private static void SearchOnFlea(string keyId)
        {
            var map = DynamicMaps.Plugin.Instance?.Map;
            if (map != null && map.IsShowingMapScreen)
            {
                InventoryScreenShowPatch.ReturnToMapOnNextShow();
            }

            // same as the "Filter by item" context menu entry on items
            ItemUiContext.Instance.ExternalRagfairSearch(new(EFilterType.FilterSearch, keyId, true));
        }

        private static Func<MapMarker, string> CreateTextGetter()
        {
            var getter = AccessTools.PropertyGetter(typeof(MapMarker), "Text");
            if (getter == null)
                return _ => null;

            return marker => getter.Invoke(marker, null) as string;
        }
    }
}
