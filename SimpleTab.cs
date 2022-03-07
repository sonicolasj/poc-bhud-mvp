using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    // Tab with only a View
    internal class SimpleTabView : View
    {
        private const int SPACING = 5;

        #region Controls

        private Label Label;
        private StandardButton Button;

        #endregion Controls

        protected override void Build(Container buildPanel)
        {
            this.Label = new Label
            {
                Parent = buildPanel,
                Location = buildPanel.ContentRegion.Location,
                AutoSizeWidth = true, AutoSizeHeight = true,
                Text = "Hello world!",
            };

            this.Button = new StandardButton
            {
                Parent = buildPanel,
                Location = new Point(this.Label.Right + SPACING, this.Label.Top),
                Text = "Click me!",
            };

            this.Button.Click += (o, e) => ScreenNotification.ShowNotification("You clicked");
        }
    }
}