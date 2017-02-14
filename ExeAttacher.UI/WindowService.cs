using ExeAttacher.Core.UI;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace ExeAttacher.UI
{
    public class WindowService : IWindowService
    {
        public async Task ShowMessageDialog(string title, string message)
        {
            await this.GetMainWindow().ShowMessageAsync(title, message);
        }

        private MetroWindow GetMainWindow()
        {
            return Application.Current.MainWindow as MetroWindow;
        }
    }
}