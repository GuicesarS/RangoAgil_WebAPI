using Microsoft.AspNetCore.Identity;
using RangoAgil.API.EndpointFilters;
using RangoAgil.API.EndpointHandlers;


namespace RangoAgil.API.Extensions;
public static class EndpointRouteBuilderExtensions
{
    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapGroup("/identity/").MapIdentityApi<IdentityUser>();

        endpointRouteBuilder.MapGet("/prato/{pratoId:int}", (int pratoId) => $"Prato {pratoId} é Maravilhoso!")
            .WithOpenApi(operation =>
            {
                operation.Deprecated = true;
                return operation;
            })
            .WithSummary("Este Endpoint está 'deprecated' e será descontinuado na versão 2 desta API")
            .WithDescription("Favor utilizar a rota /rangos/{rangoId} para evitar transtornos.");
            

        var rangosEndpoints = endpointRouteBuilder.MapGroup("/rangos")
             .RequireAuthorization();
        var rangosEndpointsId = rangosEndpoints.MapGroup("/{rangoId:int}");

        var rangoscomIdAndLockFilterEndpoints = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
            .RequireAuthorization("RequireAdminFromBrazil")
            .RequireAuthorization()
            .AddEndpointFilter(new RangoIsLockedFilter(6))
            .AddEndpointFilter(new RangoIsLockedFilter(4));

        rangosEndpoints.MapGet("", RangosHandlers.GetRangosAsync)
            .WithOpenApi()
            .WithSummary("Esta rota retornará todos os rangos cadastrados");

        rangosEndpointsId.MapGet("", RangosHandlers.GetRangoById).WithName("GetRangos")
            .AllowAnonymous(); // Não solicita autorização
           

        rangosEndpoints.MapPost("", RangosHandlers.CreateRangoAsync)
            .AddEndpointFilter<ValidadeAnnotationFilter>();

        rangoscomIdAndLockFilterEndpoints.MapPut("", RangosHandlers.UpdateRangoAsync);

        rangoscomIdAndLockFilterEndpoints.MapDelete("", RangosHandlers.DeleteRangoAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();
        
    }

    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var ingredientesEndpoint = endpointRouteBuilder.MapGroup("rangos/{rangoId:int}/ingredientes").RequireAuthorization();
        ingredientesEndpoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);
        ingredientesEndpoint.MapPost("", () =>
        {
            throw new NotImplementedException();
        });
    }
}

