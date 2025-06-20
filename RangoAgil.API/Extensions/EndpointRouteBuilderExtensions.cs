using RangoAgil.API.EndpointHandlers;

namespace RangoAgil.API.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos");
        var rangosEndpointsId = rangosEndpoints.MapGroup("/{rangoId:int}");

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync);
        rangosEndpointsId.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos");
        rangosEndpoints.MapPost("", RangosHandlers.CreateRangoAsync);
        rangosEndpointsId.MapPut("", RangosHandlers.UpdateRangoAsync);
        rangosEndpointsId.MapDelete("", RangosHandlers.DeleteRangoAsync);

    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndpoint = endpointRouteBuilder.MapGroup("rangos/{rangoId:int}/ingredientes");
        ingredientesEndpoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);
    }
}

