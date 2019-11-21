using System;
using System.Collections.Generic;

namespace CSharpConsoleMenu
{
    public abstract class AbstractMenu
    {
        private string Title { get; }

        private readonly List<MenuItem> _menuItems;

        protected AbstractMenu(string title)
        {
            Title = title;
            _menuItems = new List<MenuItem>();
            Init();
        }

        protected abstract void Init();
        
        protected virtual void UpdateMenuItems() {}

        public void Display()
        {
            var repeat = true;
            while (repeat)
            {
                UpdateMenuItems();
                Console.WriteLine();
                Console.WriteLine(Title);
                for (var i = 0; i < _menuItems.Count; i++)
                {
                    if (_menuItems[i].IsVisible)
                        Console.WriteLine(i + ". " + _menuItems[i].Description);
                }

                Console.Write("Select Option: ");
                var input = Console.ReadLine();

                try
                {
                    var itemIndex = int.Parse(input);
                    var menuItem = _menuItems[itemIndex];
                    if (menuItem.IsVisible) repeat = menuItem.Run();
                    else throw new InvalidOperationException();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option, you need to enter a number.");
                    repeat = true;
                    
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine($"Invalid option. Option {input} doesn't exist.");
                    repeat = true;
                }
                
                catch (InvalidOperationException)
                {
                    Console.WriteLine($"Invalid option. Option at {input} is hidden.");
                    repeat = true;
                }
            }
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            if (!_menuItems.Contains(menuItem)) _menuItems.Add(menuItem);
            else throw new ArgumentException($"Menu item with id {menuItem.Id} already exists!");
        }
        
        public void AddHiddenMenuItem(MenuItem menuItem)
        {
            AddMenuItem(menuItem.Hide());
        }

        public void ShowMenuItem(long itemId)
        {
            try
            {
                var menuItem = new MenuItem(itemId);
                var index = _menuItems.IndexOf(menuItem);
                _menuItems[index].Show();
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentException($"Error showing menu item. Menu item with ID {itemId} hasn't been added to this menu.");
            }
        }
        
        public void HideMenuItem(long itemId)
        {
            try
            {
                var menuItem = new MenuItem(itemId);
                var index = _menuItems.IndexOf(menuItem);
                _menuItems[index].Hide();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException($"Error hiding menu item. Menu item with ID {itemId} hasn't been added to this menu.");
            }
        }
    }
}