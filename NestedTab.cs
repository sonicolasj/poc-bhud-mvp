using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    // View (and Presenter) having nested Views
    // Here the nested view are selected from the Presenter, but they could
    // come from anywhere, even from the view itself.
    class NestedTabView : View<NestedTabPresenter>
    {
        private const int SPACING = 5;

        #region Controls

        private Label Title;
        private Menu Menu;
        private ViewContainer ViewContainer;

        #endregion Controls

        public void SetTitle(string title)
        {
            this.Title.Text = title;
        }

        protected override void Build(Container buildPanel)
        {
            this.Title = new Label
            {
                Parent = buildPanel,
                Location = buildPanel.ContentRegion.Location,
                AutoSizeWidth = true,
                AutoSizeHeight = true,
                Text = "Select an item",
            };

            this.Menu = new Menu
            {
                Parent = buildPanel,
                Location = new Point(buildPanel.ContentRegion.Left, this.Title.Bottom + SPACING),
                Width = 200,
            };

            this.Menu.AddMenuItem("Inner view 1").ItemSelected += (o, e) => this.ShowView("Inner view 1");
            this.Menu.AddMenuItem("Inner view 2").ItemSelected += (o, e) => this.ShowView("Inner view 2");

            this.ViewContainer = new ViewContainer
            {
                Parent = buildPanel,
                Location = new Point(this.Menu.Right + SPACING, this.Title.Bottom + SPACING),
                Width = 300,
            };
        }

        private void ShowView(string viewName)
        {
            var view = this.Presenter.SelectView(viewName);
            this.ViewContainer.Show(view);
        }
    }

    class NestedTabPresenter : Presenter<NestedTabView, int>
    {
        public NestedTabPresenter(NestedTabView view) : base(view, 0) { }

        public View SelectView(string name)
        {
            this.View.SetTitle(name.ToUpper());

            switch (name)
            {
                case "Inner view 1":
                    return new InnerView1();

                default:
                case "Inner view 2":
                    return new InnerView2();
            }
        }

        private class InnerView1 : View
        {
            protected override void Build(Container buildPanel)
            {
                new Label
                {
                    Parent = buildPanel,
                    AutoSizeWidth = true,
                    Text = "This is the first view!",
                };
            }
        }

        private class InnerView2 : View
        {
            protected override void Build(Container buildPanel)
            {
                new StandardButton
                {
                    Parent = buildPanel,
                    Text = "2nd view",
                };
            }
        }
    }
}