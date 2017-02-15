using System.Threading.Tasks;

namespace ExeAttacher.Core.UI
{
    public interface IWindowService
    {
        Task ShowMessageDialog(string title, string message);

        string ShowOpenFileDialog();
    }
}