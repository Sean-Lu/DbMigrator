using Sean.Core.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;

namespace Example.Infrastructure.Extensions
{
    public static class DIExtensions
    {
        public static void AddInfrastructureDI(this IDIRegister container)
        {
            container.RegisterType(typeof(ILogger<>), typeof(SimpleLocalLogger<>), ServiceLifeStyle.Transient);
        }
    }
}
