using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    // Tab with a View and a Presenter without Model
    class SplitTabView : View<SplitTabPresenter>
    {
        public SplitTabView()
        {
            this.Presenter = new SplitTabPresenter(this);
        }

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
                AutoSizeWidth = true,
                AutoSizeHeight = true,
                Text = "Hello world!",
            };

            this.Button = new StandardButton
            {
                Parent = buildPanel,
                Location = new Point(this.Label.Right + SPACING, this.Label.Top),
                Text = "Click me!",
            };

            this.Button.Click += (o, e) => this.Presenter.ButtonPressed();
        }

        public void AddExclamationMark()
        {
            this.Label.Text += "!";
            this.Button.Left = this.Label.Right + SPACING;
        }
    }

    class SplitTabPresenter : Presenter<SplitTabView, int> // int because we *have* to provide a model type, even if not using it.
    {
        public SplitTabPresenter(SplitTabView view) : base(view, 0) {}

        public void ButtonPressed()
        {
            ScreenNotification.ShowNotification("You clicked");
            this.View.AddExclamationMark();
        }
    }
}