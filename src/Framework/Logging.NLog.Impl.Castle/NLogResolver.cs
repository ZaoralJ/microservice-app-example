namespace Logging.NLog.Impl.Castle
{
    using global::Castle.Core;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.Context;
    using global::NLog;

    /// <summary>
    /// The log4net resolver.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal class NLogResolver : ISubDependencyResolver
    {
        /// <summary>
        /// Can resolve.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return dependency.TargetType == typeof(Logging.ILogger);
        }

        /// <summary>
        /// Resolve.
        /// </summary>
        /// <returns>The <see cref="object"/>.</returns>
        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            var log = LogManager.GetLogger(model.Implementation.FullName);

            var component = new NLogImpl(log);

            return component;
        }
    }
}