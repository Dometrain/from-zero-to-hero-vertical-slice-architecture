using System.Reflection;

namespace TravelInspiration.API.Shared.Slices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSlices(this IServiceCollection services)
    {
        // get current assembly
        var currentAssembly = Assembly.GetExecutingAssembly();

        // get slices 
        var slices = currentAssembly.GetTypes().Where(t =>
            typeof(ISlice).IsAssignableFrom(t) && 
            t != typeof(ISlice) &&
            t.IsPublic && 
            !t.IsAbstract);

        // register them as singletons
        foreach (var slice in slices)
        {
            services.AddSingleton(typeof(ISlice), slice);
        }
        
        return services; 
    }
}
