namespace Logging.NLog.Impl.Castle
{
    using global::Castle.MicroKernel.Registration;

    /// <summary>
    /// The log installer.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LogInstaller : IWindsorInstaller
    {
        /// <summary>
        /// The is initialized.
        /// </summary>
        private static bool _isInitialized;

        /// <summary>
        /// The install method.
        /// </summary>
        public void Install(global::Castle.Windsor.IWindsorContainer container, global::Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;

                container.Kernel.Resolver.AddSubResolver(new NLogResolver());
                container.Register(Component.For<ILogger>().LifestyleTransient());
            }
        }
    }
}