using DotNet.Testcontainers.Containers.Modules.Abstractions;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class ModuleFixture<T> where T : HostedServiceContainer
    {
        public ModuleFixture(T container)
        {
            Container = container;
        }

        public T Container { get; }
    }
}