using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExeAttacher.Core.Injection
{
    /// <summary>
    /// Type of scope of the services.
    /// </summary>
    public enum Scope
    {
        /// <summary>
        /// Set a service as a singleton, returns always the same instance in the whole application.
        /// </summary>
        SingletonPerContainer,

        /// <summary>
        /// Set a service as singleton per request, returns the same instance during that web request.
        /// </summary>
        Singleton,

        /// <summary>
        /// Set a service as transient, return a new instance in every call.
        /// </summary>
        Transient
    }
}
