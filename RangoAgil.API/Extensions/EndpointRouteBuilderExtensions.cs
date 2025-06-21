using RangoAgil.API.EndpointFilters;
using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
        var rangosEndpointsId = rangosEndpoints.MapGroup("/{rangoId:int}");

        var rangoscomIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
            .AddEndpointFilter(new RangoIsLockedFilter(6))
            .AddEndpointFilter(new RangoIsLockedFilter(4));

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);

        rangosEndpointsId.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");

        rangosEndpoints.MapPost("", RangosHandlers.CreateRangoAsync)
            .AddEndpointFilter<ValidadeAnnotationFilter>();

        rangoscomIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);

        rangoscomIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();
        
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndpoint = endpointRouteBuilder.MapGroup("rangos/{rangoId:int}/ingredientes");
        ingredientesEndpoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);
        ingredientesEndpoint.MapPost("", () =>
        {
            throw new NotImplementedException();
        });
    }
}

