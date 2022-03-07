using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;
using Microsoft.Xna.Framework;
using Xunit;

namespace PoCBHudMVP
{
    // Clone of UncoupledTab, but the view and presenter refer to each other via interfaces.
    // Allows for unit testing at least the presenter, and eventually swapping the view for another?
    interface IInterfacedTabView : IView
    {
        void AddExclamationMark();
        void DisplayTimes(IList<DateTime> times);
    }

    interface IInterfacedTabPresenter : IPresenter<IInterfacedTabView>
    {
        void ButtonPressed();
    }

    class InterfacedTabView : View<IInterfacedTabPresenter>, IInterfacedTabView
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

    class InterfacedTabPresenter : Presenter<IInterfacedTabView, IList<DateTime>>, IInterfacedTabPresenter
    {
        public InterfacedTabPresenter(IInterfacedTabView view, IList<DateTime> model) : base(view, model) { }

        public void ButtonPressed()
        {
            this.Model.Add(DateTime.Now);

            this.View.AddExclamationMark();
            this.View.DisplayTimes(this.Model);
        }
    }

    // Ideally, this should be in a dedicated test project.
    public class InterfacedTabPresenterTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            //// Test data
            var view = new MockedView();
            var model = new List<DateTime>
            {
                DateTime.MinValue
            };

            //// System under test
            var sut = new InterfacedTabPresenter(view, model);

            // Act
            sut.ButtonPressed();

            // Assert
            Assert.Equal(1, view.ExclamationMarksCount);
            Assert.Equal(2, model.Count);
            Assert.Equal(2, view.TimesCount);
        }

        // Mock
        private class MockedView : IInterfacedTabView
        {
            public event EventHandler<EventArgs> Loaded;
            public event EventHandler<EventArgs> Built;
            public event EventHandler<EventArgs> Unloaded;

            public int ExclamationMarksCount { get; private set; } = 0;
            public int TimesCount { get; private set; } = 0;

            public void AddExclamationMark()
            {
                this.ExclamationMarksCount++;
            }

            public void DisplayTimes(IList<DateTime> times)
            {
                this.TimesCount = times.Count;
            }

            public void DoBuild(Container buildPanel) { /* NOOP */ }
            public Task<bool> DoLoad(IProgress<string> progress) { return Task.FromResult(true); }
            public void DoUnload() { /* NOOP */ }
        }
    }
}