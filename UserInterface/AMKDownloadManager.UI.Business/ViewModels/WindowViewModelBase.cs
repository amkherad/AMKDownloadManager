using System.Windows.Input;
using AMKsGear.Core.Automation.Support;

namespace AMKDownloadManager.UI.Business.ViewModels
{
    public abstract class WindowViewModelBase : ViewModelBase
    {
        public virtual string Title { get; set; }

        public virtual bool CanClose { get; set; }
        public virtual ICommand Close { get; }
    
        protected WindowViewModelBase()
        {
            Close = new AutoCommand(OnCloseCommand, context => CanClose);
        }

        public void OnCloseCommand(object obj)
        {
            
        }
    }
}