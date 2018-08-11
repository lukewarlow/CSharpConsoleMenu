using System;
using CSharpConsoleMenu;

namespace LibraryTest
{
    public class TestMenu : AbstractMenu
    {
        private bool _showHiddenMenu;
        
        public TestMenu() : base("Welcome to the test menu.") {}

        protected override void Init()
        {
            AddMenuItem(new MenuItem(0, "Exit menu").SetAsExitOption());
            AddMenuItem(new MenuItem(1, "Test sub menu", new TestSubMenu()));
            
            AddMenuItem(new MenuItem(2, "Show hidden menu item", () =>
            {
                Console.WriteLine("Showing hidden menu item");
                _showHiddenMenu = true;
            }));
            
            AddHiddenMenuItem(new MenuItem(3, "Hidden menu item", () => Console.WriteLine("I was a hidden menu item")));
        }

        protected override void UpdateMenuItems()
        {
            if (_showHiddenMenu) ShowMenuItem(3);
        }
    }
    
    public class TestSubMenu : AbstractMenu
    {
        public TestSubMenu() : base("Welcome to the test sub menu.") {}

        protected override void Init()
        {
            AddMenuItem(new MenuItem(0, "Exit current menu").SetAsExitOption());
            AddMenuItem(new MenuItem(1, "Test submenu item", () => Console.WriteLine("Test sub menu item selected")));
        }
    }
}