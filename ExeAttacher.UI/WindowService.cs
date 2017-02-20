using ExeAttacher.Core.Constants;
using ExeAttacher.Core.UI;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;
using win32 = Microsoft.Win32;

namespace ExeAttacher.UI
{
    public class WindowService : IWindowService
    {
        public async Task ShowMessageDialog(string title, string message)
        {
            await this.GetMainWindow().ShowMessageAsync(title, message);
        }

        public string ShowOpenFileDialog()
        {
            string fileName = string.Empty;
            var dialog = new win32.OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = FileConsts.ExeFileExtension,
                Filter = $"Exe Files (*{FileConsts.ExeFileExtension})|*{FileConsts.ExeFileExtension}"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                fileName = dialog.FileName;
            }

            return fileName;
        }

        private MetroWindow GetMainWindow()
        {
            return Application.Current.MainWindow as MetroWindow;
        }
    }
}