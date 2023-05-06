using System.Reflection;
using Example.Domain.Extensions;
using Sean.Core.DependencyInjection;

namespace Example.Application.Extensions
{
    public static class DIExtensions
    {
        public static void AddApplicationDI(this IDIRegister container)
        {
            container.AddDomainDI();

            container.RegisterAssemblyByInterfaceSuffix(Assembly.GetExecutingAssembly(), "Service", ServiceLifeStyle.Transient);
        }
    }
}
