using System;
using System.Collections.Generic;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.ViewModels.Main.MainMenu
{
    public class MenuItemViewModel : NotifyPropertyChangedBase, IEquatable<MenuItemViewModel>
    {
        public string Title { get; set; }
        
        public bool Enabled { get; set; }
        
        public bool Checked { get; set; }
        
        public string Icon { get; set; }
        
        public string Description { get; set; }
        
        public string Command { get; set; }
        
        
        public IList<MenuItemViewModel> Children { get; set; }
        
        
        public MenuItemViewModel(string title)
        {
            Title = title;
        }

//        public override string ToString()
//        {
//            return Title;
//        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MenuItemViewModel) obj);
        }

        public override int GetHashCode() => Command.GetHashCode();

        public bool Equals(MenuItemViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Command, other.Command);
        }
    }
}