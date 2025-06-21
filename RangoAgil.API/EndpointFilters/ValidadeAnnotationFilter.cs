
using MiniValidation;
using RangoAgil.API.Models;

namespace RangoAgil.API.EndpointFilters;

public class ValidadeAnnotationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var rangoparaCriaDTO = context.GetArgument<RangoCriaDTO>(2);

        if(!MiniValidator.TryValidate(rangoparaCriaDTO, out var validationErrors))
        {
            return TypedResults.ValidationProblem(validationErrors);
        }

        return await next(context);
    }
}
