namespace ExeAttacher.Core.Injection
{
    /// <summary>
    /// Interface of the Injection Module
    /// </summary>
    public interface IInjectionCompositionRoot
    {
        /// <summary>
        /// Registers the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        void Register(IInjectionContainer container);
    }
}