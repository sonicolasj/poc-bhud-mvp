using System;
using System.Collections.Generic;
using System.Text;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    // Tab with a View and a Presenter with Model
    class FullTabView : View<FullTabPresenter>
    {
        public FullTabView()
        {
            this.Presenter = new FullTabPresenter(this, new List<DateTime>());
        }

        private const int SPACING = 5;

        #region Controls

        private Label Label;
        private StandardButton Button;
        private Label TimesLabel;

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

            this.TimesLabel = new Label
            {
                Parent = buildPanel,
                Location = new Point(this.Label.Left, this.Button.Bottom + SPACING),
                AutoSizeWidth = true,
                AutoSizeHeight = true,
            };
        }

        public void AddExclamationMark()
        {
            this.Label.Text += "!";
            this.Button.Left = this.Label.Right + SPACING;
        }

        public void DisplayTimes(IList<DateTime> times)
        {
            var sb = new StringBuilder();

            foreach (var time in times)
            {
                sb.AppendLine(time.ToString("O"));
            }

            this.TimesLabel.Text = sb.ToString().Trim();
        }
    }

    class FullTabPresenter : Presenter<FullTabView, IList<DateTime>>
    {
        public FullTabPresenter(FullTabView view, IList<DateTime> model) : base(view, model) {}

        public void ButtonPressed()
        {
            ScreenNotification.ShowNotification("You clicked");

            this.Model.Add(DateTime.Now);

            this.View.AddExclamationMark();
            this.View.DisplayTimes(this.Model);
        }
    }
}