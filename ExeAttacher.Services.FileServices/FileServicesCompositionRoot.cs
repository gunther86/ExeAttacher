using ExeAttacher.Core.Injection;
using ExeAttacher.Core.Services;

namespace ExeAttacher.Services.FileServices
{
    public class FileServicesCompositionRoot : IInjectionCompositionRoot
    {
        public void Register(IInjectionContainer container)
        {
            container.Register<IFileHandlingService, FileHandlingService>();
        }
    }
}