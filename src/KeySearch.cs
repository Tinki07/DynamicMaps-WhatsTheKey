using System;

namespace WhatsTheKey
{
    internal static class KeySearch
    {
        public const float MenuMinWidth = 200f;
        public const float MenuMaxWidth = 360f;
        public const float MenuTextPadding = 24f;

        public static bool CanSearch(bool enabled, bool inRaid, bool fleaAvailable, string keyId)
        {
            return enabled && !inRaid && fleaAvailable && !string.IsNullOrWhiteSpace(keyId);
        }

        public static string GetLocaleKey(string keyId)
        {
            return $"{keyId} ShortName";
        }

        // the game returns the locale key itself when it has no translation for it
        public static string GetLabel(string keyId, string localizedName)
        {
            if (string.IsNullOrWhiteSpace(localizedName) || localizedName == GetLocaleKey(keyId))
            {
                return "Search on Flea Market";
            }

            return $"Search {localizedName} on Flea Market";
        }

        public static float GetMenuWidth(float textWidth)
        {
            return Math.Min(MenuMaxWidth, Math.Max(MenuMinWidth, textWidth + MenuTextPadding));
        }

        // opens the menu towards the inside of the screen so it never goes off screen
        public static (float x, float y) GetMenuPivot(float cursorX, float cursorY, float screenWidth, float screenHeight)
        {
            var x = cursorX > screenWidth * 0.75f ? 1f : 0f;
            var y = cursorY < screenHeight * 0.25f ? 0f : 1f;
            return (x, y);
        }
    }
}
