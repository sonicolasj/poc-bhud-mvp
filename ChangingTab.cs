using System.Collections.Generic;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    // View having two possible inner views, displaying the same model differently.
    class ChangingTabView : View
    {
        private const int SPACING = 5;

        #region Controls

        private Checkbox CompactCheckbox;
        private Label Title;
        private Menu Menu;
        private ViewContainer ViewContainer;

        #endregion Controls

        private IList<string> ButtonTitles = new List<string>
        {
            "Button 1",
            "Button 2",
            "Button 3",
        };

        protected override void Build(Container buildPanel)
        {
            this.CompactCheckbox = new Checkbox
            {
                Parent = buildPanel,
                Location = buildPanel.ContentRegion.Location,
                Text = "Compact?",
                Checked = false,
            };

            this.CompactCheckbox.CheckedChanged += (o, e) => this.ToggleCompact();

            this.ViewContainer = new ViewContainer
            {
                Parent = buildPanel,
                Location = new Point(buildPanel.ContentRegion.Left, this.CompactCheckbox.Bottom + SPACING),
                Width = 100,
                Height = 200,
            };

            this.ViewContainer.Show(new LargeView(this.ButtonTitles));
        }

        private void ToggleCompact()
        {
            if(this.CompactCheckbox.Checked)
            {
                this.ViewContainer.Show(new CompactView(this.ButtonTitles));
            }
            else
            {
                this.ViewContainer.Show(new LargeView(this.ButtonTitles));
            }
        }

        private interface IInnerView : IView
        {
            void AddButton(string title);
        }

        private class InnerPresenter : Presenter<IInnerView, IList<string>>
        {
            public InnerPresenter(IInnerView view, IList<string> model) : base(view, model) {}

            // Method used after the view has finished rendering.
            protected override void UpdateView()
            {
                foreach (var buttonTitle in this.Model)
                {
                    this.View.AddButton(buttonTitle);
                }
            }
        }

        private class LargeView : View<InnerPresenter>, IInnerView
        {
            private FlowPanel FlowPanel;

            public LargeView(IList<string> buttonTitles) : base()
            {
                this.Presenter = new InnerPresenter(this, buttonTitles);
            }

            protected override void Build(Container buildPanel)
            {
                this.FlowPanel = new FlowPanel
                {
                    Parent = buildPanel,
                    Location = buildPanel.ContentRegion.Location,
                    Size = buildPanel.ContentRegion.Size,
                };
            }

            public void AddButton(string title)
            {
                new StandardButton
                {
                    Text = title,
                    Height = 50,
                    Width = 75,
                    Parent = this.FlowPanel,
                };
            }
        }

        private class CompactView : View<InnerPresenter>, IInnerView
        {
            private FlowPanel FlowPanel;

            public CompactView(IList<string> buttonTitles) : base()
            {
                this.Presenter = new InnerPresenter(this, buttonTitles);
            }

            protected override void Build(Container buildPanel)
            {
                this.FlowPanel = new FlowPanel
                {
                    Parent = buildPanel,
                    Location = buildPanel.ContentRegion.Location,
                    Size = buildPanel.ContentRegion.Size,
                };
            }

            public void AddButton(string title)
            {
                new StandardButton
                {
                    Text = title,
                    Width = 75,
                    Parent = this.FlowPanel,
                };
            }
        }
    }
}