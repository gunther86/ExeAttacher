using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExeAttacher.Core.Injection
{
    /// <summary>
    /// Service locator static class.
    /// </summary>
    public static class ServiceLocator
    {
        private static IInjectionContainer container;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <returns>The instance</returns>
        public static T GetInstance<T>() => container.GetInstance<T>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>The instance</returns>
        public static T GetInstance<T>(string serviceName) => container.GetInstance<T>(serviceName);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>The object representing the instance.</returns>
        public static object GetInstance(Type service) => container.GetInstance(service);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>The object representing the instance.</returns>
        public static object GetInstance(Type service, string serviceName) => container.GetInstance(service, serviceName);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <returns>All the instances from the container.</returns>
        public static IEnumerable<T> GetAllInstances<T>() => container.GetAllInstances<T>();

        /// <summary>
        /// Initializes the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Initialize(IInjectionContainer container)
        {
            ServiceLocator.container = container;
        }
    }
}
