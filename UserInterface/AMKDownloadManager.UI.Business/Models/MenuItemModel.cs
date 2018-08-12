using System;
using System.Collections.Generic;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.Models
{
    public class MenuItemModel : NotifyPropertyChangedBase, IEquatable<MenuItemModel>
    {
        public string Title { get; set; }
        
        public bool Enabled { get; set; }
        
        public bool Checked { get; set; }
        
        public string Icon { get; set; }
        
        public string Description { get; set; }
        
        public string Command { get; set; }
        
        
        public IList<MenuItemModel> Children { get; set; }
        
        
        public MenuItemModel(string title)
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
            return Equals((MenuItemModel) obj);
        }

        public override int GetHashCode() => Command.GetHashCode();

        public bool Equals(MenuItemModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Command, other.Command);
        }
    }
}