using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace R4sHearts
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod, IAssetEditor
    {
        private static readonly Rectangle sourceAreaHearts = new Rectangle(0, 0, 44, 55);
        private static readonly Rectangle targetAreaHearts = new Rectangle(140, 532, 44, 55);
        private static readonly Rectangle sourceAreaMax = new Rectangle(33, 55, 11, 11);
        private static readonly Rectangle targetAreaMax = new Rectangle(269, 495, 11, 11);

        private const string originalSprites = "LooseSprites/Cursors";

        private Texture2D heartIcons;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Display.MenuChanged += OnMenuChanged;

            heartIcons = helper.Content.Load<Texture2D>("assets/hearts.png");
        }

        /// <summary>Get whether this instance can edit the given asset.</summary>
        /// <param name="asset">Basic metadata about the asset being loaded.</param>
        public bool CanEdit<T>(IAssetInfo asset)
        {
            return asset.AssetNameEquals(originalSprites);
        }

        /// <summary>Edit a matched asset.</summary>
        /// <param name="asset">A helper which encapsulates metadata about an asset and enables changes to it.</param>
        public void Edit<T>(IAssetData asset)
        {
            if (Game1.currentSpeaker?.datable)
            {
                var img = asset.AsImage();
                img.PatchImage(heartIcons, sourceAreaHearts, targetAreaHearts);
                img.PatchImage(heartIcons, sourceAreaMax, targetAreaMax);
            }
        }

        private void OnMenuChanged(object sender, MenuChangedEventArgs e)
        {
            if (e.NewMenu?.GetType() == typeof(StardewValley.Menus.DialogueBox))
            {
                Helper.Content.InvalidateCache(originalSprites);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected override void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                Helper.Events.Display.MenuChanged -= OnMenuChanged;
                heartIcons?.Dispose();
                base.Dispose(disposing);
            }

            heartIcons = null;

            disposedValue = true;
        }
        #endregion
    }
}
