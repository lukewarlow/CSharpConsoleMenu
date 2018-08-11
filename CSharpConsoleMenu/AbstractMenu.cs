using System;
using System.Collections.Generic;

namespace CSharpConsoleMenu
{
    public abstract class AbstractMenu
    {
        private string Title { get; }

        private readonly List<MenuItem> MenuItems;

        protected AbstractMenu(string title)
        {
            Title = title;
            MenuItems = new List<MenuItem>();
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
                for (var i = 0; i < MenuItems.Count; i++)
                {
                    if (MenuItems[i].IsVisible)
                        Console.WriteLine(i + ". " + MenuItems[i].Description);
                }

                Console.Write("Select Option: ");
                var input = Console.ReadLine();

                try
                {
                    var itemIndex = int.Parse(input);
                    var menuItem = MenuItems[itemIndex];
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
                    Console.WriteLine($"Invalid Operation. Option at {input} is hidden.");
                    repeat = true;
                }
            }
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            if (!MenuItems.Contains(menuItem)) MenuItems.Add(menuItem);
            else throw new InvalidOperationException();
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
                var index = MenuItems.IndexOf(menuItem);
                MenuItems[index].Show();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Error showing menu item: "  + e);
            }
        }
        
        public void HideMenuItem(long itemId)
        {
            try
            {
                var menuItem = new MenuItem(itemId);
                var index = MenuItems.IndexOf(menuItem);
                MenuItems[index].Hide();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Error showing menu item: "  + e);
            }
        }
    }
}