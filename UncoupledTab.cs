using System;
using System.Collections.Generic;
using System.Text;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    // Clone of FullTab without instanciating the Presenter in the view.
    // Right now, it feels a bit useless to uncouple them, but wait for more…
    class UncoupledTabView : View<UncoupledTabPresenter>
    {
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

    class UncoupledTabPresenter : Presenter<UncoupledTabView, IList<DateTime>>
    {
        public UncoupledTabPresenter(UncoupledTabView view, IList<DateTime> model) : base(view, model) { }

        public void ButtonPressed()
        {
            ScreenNotification.ShowNotification("You clicked");

            this.Model.Add(DateTime.Now);

            this.View.AddExclamationMark();
            this.View.DisplayTimes(this.Model);
        }
    }
}