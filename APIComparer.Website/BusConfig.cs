﻿namespace APIComparer.Website
{
    using Autofac;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Logging;

    public static class BusConfig
    {
        public static IBus Setup(ILifetimeScope container)
        {
            var configuration = new BusConfiguration();
            configuration.EndpointName("APIComparer.Website");
            configuration.UseContainer<AutofacBuilder>(x => x.ExistingLifetimeScope(container));
            configuration.AssembliesToScan(AllAssemblies.Except("Microsoft.Windows.Azure.Storage"));

            configuration.UseTransport<AzureStorageQueueTransport>();
            configuration.UsePersistence<AzureStoragePersistence>();
            configuration.DisableFeature<SecondLevelRetries>();
            configuration.DisableFeature<Sagas>();
            configuration.DisableFeature<TimeoutManager>();

            LogManager.Use<DefaultFactory>().Directory(@".\");

            var startableBus = Bus.Create(configuration);
            return startableBus.Start();
        }
    }
}