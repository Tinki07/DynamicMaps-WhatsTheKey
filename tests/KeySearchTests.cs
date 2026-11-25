using WhatsTheKey;
using Xunit;

namespace WhatsTheKey.Tests
{
    public class KeySearchTests
    {
        private const string KeyId = "5937ee6486f77408994ba448";

        [Fact]
        public void CanSearch_WhenEverythingIsAvailable()
        {
            Assert.True(KeySearch.CanSearch(enabled: true, inRaid: false, fleaAvailable: true, KeyId));
        }

        [Theory]
        [InlineData(false, false, true, KeyId)]
        [InlineData(true, true, true, KeyId)]
        [InlineData(true, false, false, KeyId)]
        [InlineData(true, false, true, null)]
        [InlineData(true, false, true, "")]
        [InlineData(true, false, true, "  ")]
        public void CannotSearch_WhenSomethingIsMissing(bool enabled, bool inRaid, bool fleaAvailable, string keyId)
        {
            Assert.False(KeySearch.CanSearch(enabled, inRaid, fleaAvailable, keyId));
        }

        [Fact]
        public void LocaleKey_UsesTheShortName()
        {
            Assert.Equal($"{KeyId} ShortName", KeySearch.GetLocaleKey(KeyId));
        }

        [Fact]
        public void Label_ContainsTheKeyName()
        {
            Assert.Equal("Search Tank on Flea Market", KeySearch.GetLabel(KeyId, "Tank"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(KeyId + " ShortName")]
        public void Label_FallsBack_WhenTheNameIsUnknown(string localizedName)
        {
            Assert.Equal("Search on Flea Market", KeySearch.GetLabel(KeyId, localizedName));
        }

        [Theory]
        [InlineData(0f, KeySearch.MenuMinWidth)]
        [InlineData(100f, KeySearch.MenuMinWidth)]
        [InlineData(250f, 250f + KeySearch.MenuTextPadding)]
        [InlineData(1000f, KeySearch.MenuMaxWidth)]
        public void MenuWidth_StaysWithinBounds(float textWidth, float expected)
        {
            Assert.Equal(expected, KeySearch.GetMenuWidth(textWidth));
        }

        [Theory]
        [InlineData(100f, 900f, 0f, 1f)]   // top left: opens down and to the right
        [InlineData(1800f, 900f, 1f, 1f)]  // right edge: opens to the left
        [InlineData(100f, 100f, 0f, 0f)]   // bottom edge: opens upwards
        [InlineData(1800f, 100f, 1f, 0f)]  // bottom right corner
        public void MenuPivot_KeepsTheMenuOnScreen(float x, float y, float expectedX, float expectedY)
        {
            var (pivotX, pivotY) = KeySearch.GetMenuPivot(x, y, 1920f, 1080f);

            Assert.Equal(expectedX, pivotX);
            Assert.Equal(expectedY, pivotY);
        }
    }
}
