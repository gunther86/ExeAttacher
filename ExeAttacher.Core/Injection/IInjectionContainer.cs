using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExeAttacher.Core.Injection
{
    /// <summary>
    /// Injection Container Interface
    /// </summary>
    public interface IInjectionContainer
    {
        /// <summary>
        /// Registers the specified scope.
        /// </summary>
        /// <typeparam name="T">Interface to register</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="scope">The scope.</param>
        void Register<T, TImplementation>(Scope scope = Scope.Transient)
            where TImplementation : T;

        /// <summary>
        /// Registers the specified service name.
        /// </summary>
        /// <typeparam name="T">Interface to register</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="scope">The scope.</param>
        void Register<T, TImplementation>(string serviceName, Scope scope = Scope.Transient)
            where TImplementation : T;

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceName">Name of the service.</param>
        void RegisterInstance<T>(Func<IInjectionContainer, T> instance, string serviceName);

        /// <summary>
        /// Registers the specified scope.
        /// </summary>
        /// <typeparam name="T">Interface to register</typeparam>
        /// <param name="scope">The scope.</param>
        void Register<T>(Scope scope = Scope.Transient);

        /// <summary>
        /// Registers the T service with the factory that describe the dependency of the service.
        /// </summary>
        /// <typeparam name="T">Service to register</typeparam>
        /// <param name="factory">Lambda expression that describe the dependency of the service</param>
        /// <param name="scope">The scope of the service</param>
        /// <param name="serviceName">Name of the service.</param>
        void Register<T>(Func<IInjectionContainer, T> factory, Injection.Scope scope = Scope.Transient, string serviceName = "");

        /// <summary>
        /// Registers the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        void Register(Type service, Type implementation);

        /// <summary>
        /// Registers the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="serviceName">Name of the service.</param>
        void Register(Type service, Type implementation, string serviceName);

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="scope">The scope.</param>
        void Register(Type serviceType, Type implementation, Injection.Scope scope);

        /// <summary>
        /// Registers the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="scope">The scope.</param>
        void Register(Type service, Type implementation, string serviceName, Injection.Scope scope);

        /// <summary>
        /// Registers the specified module.
        /// </summary>
        /// <param name="module">The module.</param>
        void Register(IInjectionCompositionRoot module);

        /// <summary>
        /// Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        void Register(Assembly assembly);

        /// <summary>
        /// Registers the specified search assembly name.
        /// </summary>
        /// <param name="searchAssemblyName">Name of the search assembly.</param>
        void Register(string searchAssemblyName);

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterInstance<T>(T instance);

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceName">Name of the service.</param>
        void RegisterInstance<T>(T instance, string serviceName);

        /// <summary>
        /// Determines whether this instance [can get instance] the specified service name.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Returns if the instance can be get or not</returns>
        bool CanGetInstance<T>(string serviceName);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type of the service to get</typeparam>
        /// <returns>Instance of the service</returns>
        T GetInstance<T>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="type">The type of the service to get.</param>
        /// <returns>
        /// Instance of the service
        /// </returns>
        object GetInstance(Type type);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="type">The type of the service to get</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        /// Instance of the service
        /// </returns>
        object GetInstance(Type type, string serviceName);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type of the service to get</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Instance of the service</returns>
        T GetInstance<T>(string serviceName);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="T">Type of the service to get</typeparam>
        /// <returns>Collection of instance services of the type T</returns>
        IEnumerable<T> GetAllInstances<T>();
    }
}