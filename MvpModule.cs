using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Modules.Managers;
using Microsoft.Xna.Framework;

namespace PoCBHudMVP
{
    [Export(typeof(Module))]
    public class MvpModule : Module
    {
        internal ContentsManager ContentsManager => this.ModuleParameters.ContentsManager;

        #region Controls

        private TabbedWindow2 MainWindow;

        private Tab SimpleTab;
        private Tab SplitTab;
        private Tab FullTab;
        private Tab UncoupledTab;
        private Tab InterfacedTab;
        private Tab NestedTab;
        private Tab ChangingTab;

        #endregion Controls

        [ImportingConstructor]
        public MvpModule([Import("ModuleParameters")] ModuleParameters moduleParameters) : base(moduleParameters) { }

        protected override void OnModuleLoaded(EventArgs e)
        {
            ScreenNotification.ShowNotification("Hello from Blish HUD!");

            this.BuildUI();

            this.MainWindow.Show();

            base.OnModuleLoaded(e);
        }

        private void BuildUI()
        {
            this.MainWindow = new TabbedWindow2(ContentsManager.GetTexture("155985.png"),
                new Rectangle(40, 26, 913, 691),
                new Rectangle(70, 71, 839, 605)
            )
            {
                Parent = GameService.Graphics.SpriteScreen,
            };

            // Simple tab (View only)
            this.SimpleTab = new Tab(
                icon: ContentsManager.GetTexture("aegis.png"),
                view: () => new SimpleTabView(),
                name: "Simple tab"
            );

            this.MainWindow.Tabs.Add(this.SimpleTab);

            // Split tab (View with Presenter)
            this.SplitTab = new Tab(
                icon: ContentsManager.GetTexture("alacrity.png"),
                view: () => new SplitTabView(),
                name: "Split tab"
            );

            this.MainWindow.Tabs.Add(this.SplitTab);

            // Full tab (View with Presenter with Model)
            this.FullTab = new Tab(
                icon: ContentsManager.GetTexture("fury.png"),
                view: () => new FullTabView(),
                name: "Full tab"
            );

            this.MainWindow.Tabs.Add(this.FullTab);

            // Uncoupled tab (View using independant Presenter and Model)
            this.UncoupledTab = new Tab(
                icon: ContentsManager.GetTexture("might.png"),
                view: () => {
                    var view = new UncoupledTabView();
                    var presenter = new UncoupledTabPresenter(view, new List<DateTime>());
                    
                    return view.WithPresenter(presenter);
                },
                name: "Uncoupled tab"
            );

            this.MainWindow.Tabs.Add(this.UncoupledTab);

            // Interfaced tab (View and Presenter talk through interfaces)
            this.InterfacedTab = new Tab(
                icon: ContentsManager.GetTexture("protection.png"),
                view: () => {
                    var view = new InterfacedTabView();
                    var presenter = new InterfacedTabPresenter(view, new List<DateTime>());

                    return view.WithPresenter(presenter);
                },
                name: "Interfaced tab"
            );

            this.MainWindow.Tabs.Add(this.InterfacedTab);

            // Nested tab (the View contains another View)
            this.NestedTab = new Tab(
                icon: ContentsManager.GetTexture("quickness.png"),
                view: () => {
                    var view = new NestedTabView();
                    var presenter = new NestedTabPresenter(view);

                    return view.WithPresenter(presenter);
                },
                name: "Nested tab"
            );

            this.MainWindow.Tabs.Add(this.NestedTab);

            // Changing tab (same model is displayed differently by 2 views)
            this.ChangingTab = new Tab(
                icon: ContentsManager.GetTexture("regeneration.png"),
                view: () => new ChangingTabView(),
                name: "Changing tab"
            );

            this.MainWindow.Tabs.Add(this.ChangingTab);
        }
    }
}