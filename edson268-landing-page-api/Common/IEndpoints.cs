using Asp.Versioning.Builder;

namespace edson268_landing_page_api.Common
{
    public interface IEndpoints
    {
        static abstract IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder, ApiVersionSet versions);
    }

    public static class EndpointsRegister
    {
        public static IEndpointRouteBuilder Map<T>(this IEndpointRouteBuilder builder, ApiVersionSet versions) where T : IEndpoints
        {
            T.MapEndpoints(builder, versions);
            return builder;
        }
    }
}
