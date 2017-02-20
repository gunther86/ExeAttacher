using System.Threading.Tasks;

namespace ExeAttacher.Core.Services
{
    public interface IAttachService
    {
        Task AttachExe(string filePath);

        Task RevertExe(string filePath);
    }
}