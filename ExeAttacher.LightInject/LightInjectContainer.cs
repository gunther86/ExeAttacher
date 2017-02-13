using ExeAttacher.Core.Injection;
using LightInject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ExeAttacher.Core.LightInject
{
    public class LightInjectContainer : IInjectionContainer
    {
        private const string BasePath = ".";
        private const string DllExtension = ".dll";
        private IServiceContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightInjectContainer"/> class.
        /// </summary>
        public LightInjectContainer()
            : this(new ServiceContainer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LightInjectContainer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public LightInjectContainer(IServiceContainer container)
        {
            this.container = container;
            ServiceLocator.Initialize(this);
        }

        /// <summary>
        /// Determines whether this instance [can get instance] the specified service name.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        /// Returns if the instance can be get or not
        /// </returns>
        public bool CanGetInstance<T>(string serviceName) => ((IServiceContainer)this.container).CanGetInstance(typeof(T), serviceName);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="T">Type of the service to get</typeparam>
        /// <returns>
        /// Collection of instance services of the type T
        /// </returns>
        public IEnumerable<T> GetAllInstances<T>() => ((IServiceContainer)this.container).GetAllInstances<T>();

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        /// Collection of instance services
        /// </returns>
        public IEnumerable<object> GetAllInstances(Type serviceType) => ((IServiceContainer)this.container).GetAllInstances(serviceType);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type of the service to get</typeparam>
        /// <returns>
        /// Instance of the service
        /// </returns>
        public T GetInstance<T>() => ((IServiceContainer)this.container).GetInstance<T>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="T">Type of the service to get</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        /// Instance of the service
        /// </returns>
        public T GetInstance<T>(string serviceName) => ((IServiceContainer)this.container).GetInstance<T>(serviceName);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        /// Instance of the service
        /// </returns>
        public object GetInstance(Type serviceType) => ((IServiceContainer)this.container).GetInstance(serviceType);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>
        /// Instance of the service
        /// </returns>
        public object GetInstance(Type serviceType, string serviceName) => ((IServiceContainer)this.container).GetInstance(serviceType, serviceName);

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void Register<T, TImplementation>()
            where TImplementation : T
        {
            this.Register<T, TImplementation>(Injection.Scope.Transient);
        }

        /// <summary>
        /// Registers the specified scope.
        /// </summary>
        /// <typeparam name="T">Type of the service to register</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="scope">The scope.</param>
        public void Register<T, TImplementation>(Injection.Scope scope)
            where TImplementation : T
        {
            if (scope == Injection.Scope.Singleton)
            {
                this.container.Register<T, TImplementation>(new PerContainerLifetime());
            }
            else
            {
                this.container.Register<T, TImplementation>();
            }
        }

        /// <summary>
        /// Registers the specified service name.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        public void Register<T, TImplementation>(string serviceName)
            where TImplementation : T
        {
            this.Register<T, TImplementation>(serviceName, Injection.Scope.Transient);
        }

        /// <summary>
        /// Registers the specified service name.
        /// </summary>
        /// <typeparam name="T">Type of the service to register</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="scope">The scope.</param>
        public void Register<T, TImplementation>(string serviceName, Injection.Scope scope)
            where TImplementation : T
        {
            if (scope == Injection.Scope.Singleton)
            {
                this.container.Register<T, TImplementation>(serviceName, new PerContainerLifetime());
            }
            else
            {
                this.container.Register<T, TImplementation>(serviceName);
            }
        }

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        public void Register<T>()
        {
            this.Register<T>(Injection.Scope.Transient);
        }

        /// <summary>
        /// Registers the specified scope.
        /// </summary>
        /// <typeparam name="T">Interface to register</typeparam>
        /// <param name="scope">The scope.</param>
        public void Register<T>(Injection.Scope scope)
        {
            if (scope == Injection.Scope.Singleton)
            {
                this.container.Register<T>(new PerContainerLifetime());
            }
            else
            {
                this.container.Register<T>();
            }
        }

        /// <summary>
        /// Registers the specified module.
        /// </summary>
        /// <param name="compositionRoot">The module.</param>
        public void Register(IInjectionCompositionRoot compositionRoot)
        {
            if (compositionRoot != null)
            {
                compositionRoot.Register(this);
            }
        }

        /// <summary>
        /// Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Register(System.Reflection.Assembly assembly)
        {
            if (assembly != null)
            {
                Type[] types = assembly.GetTypes();
                foreach (var t in types.Where(t => t.GetInterface(typeof(IInjectionCompositionRoot).Name) == typeof(IInjectionCompositionRoot)))
                {
                    IInjectionCompositionRoot compositionRoot = assembly.CreateInstance(t.FullName) as IInjectionCompositionRoot;
                    if (compositionRoot != null)
                    {
                        compositionRoot.Register(this);
                    }
                }
            }
        }

        /// <summary>
        /// Registers the specified search assembly name.
        /// </summary>
        /// <param name="searchAssemblyName">Name of the search assembly.</param>
        public void Register(string searchAssemblyName)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName() + DllExtension == searchAssemblyName);
            if (assembly != null)
            {
                try
                {
                    this.Register(assembly);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                foreach (var f in Directory.GetFiles(BasePath, searchAssemblyName, SearchOption.AllDirectories))
                {
                    try
                    {
                        assembly = Assembly.LoadFile(Path.GetFullPath(f));
                        if (assembly != null)
                        {
                            this.Register(assembly);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation.</param>
        public void Register(Type serviceType, Type implementation)
        {
            this.Register(serviceType, implementation, Injection.Scope.Transient);
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="serviceName">Name of the service.</param>
        public void Register(Type serviceType, Type implementation, string serviceName)
        {
            this.Register(serviceType, implementation, serviceName, Injection.Scope.Transient);
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation of the service.</param>
        /// <param name="scope">The scope.</param>
        public void Register(Type serviceType, Type implementation, Injection.Scope scope)
        {
            if (scope == Injection.Scope.Singleton)
            {
                this.container.Register(serviceType, implementation, new PerContainerLifetime());
            }
            else
            {
                this.container.Register(serviceType, implementation);
            }
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementation">The implementation.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="scope">The scope.</param>
        public void Register(Type serviceType, Type implementation, string serviceName, Injection.Scope scope)
        {
            if (scope == Injection.Scope.Singleton)
            {
                this.container.Register(serviceType, implementation, serviceName, new PerContainerLifetime());
            }
            else
            {
                this.container.Register(serviceType, implementation, serviceName);
            }
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<T>(T instance)
        {
            this.container.RegisterInstance<T>(instance);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceName">Name of the service.</param>
        public void RegisterInstance<T>(T instance, string serviceName)
        {
            this.container.RegisterInstance<T>(instance, serviceName);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="T">Type of the instance</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="serviceName">Name of the service.</param>
        public void RegisterInstance<T>(Func<IInjectionContainer, T> instance, string serviceName)
        {
            this.container.Register<T>(factory => instance(this), serviceName);
        }

        /// <summary>
        /// Registers the specified instance.
        /// </summary>
        /// <typeparam name="T">The of the instance to register.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="serviceName">Name of the service.</param>
        public void Register<T>(Func<IInjectionContainer, T> instance, Injection.Scope scope = Injection.Scope.Transient, string serviceName = "")
        {
            if (scope == Injection.Scope.Singleton)
            {
                this.container.Register<T>(factory => instance(this), serviceName, new PerContainerLifetime());
            }
            else
            {
                this.container.Register(factory => instance(this), serviceName);
            }
        }
    }
}