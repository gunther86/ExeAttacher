using Caliburn.Micro;
using ExeAttacher.Core.UI;
using ExeAttacher.UI.ViewModels.Interfaces;

namespace ExeAttacher.UI.ViewModels
{
    public class MainViewModel : PropertyChangedBase, IMainViewModel
    {
        private readonly IWindowService windowService;

        public MainViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }
    }
}