using System;

namespace McStatBot.Utils
{
    public static class ServiceProviderExtensions
    {
        public static T Resolve<T>(this IServiceProvider serviceProvider)
        {
            var resolved = serviceProvider.GetService(typeof(T));
            return (T)resolved;
        }
    }
}
