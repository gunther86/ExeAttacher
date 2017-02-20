using ExeAttacher.Core.Injection;
using ExeAttacher.Core.Services;

namespace ExeAttacher.Services.AttachService
{
    internal class AttachServiceCompositionRoot : IInjectionCompositionRoot
    {
        public void Register(IInjectionContainer container)
        {
            container.Register<IAttachService, AttachService>();
        }
    }
}