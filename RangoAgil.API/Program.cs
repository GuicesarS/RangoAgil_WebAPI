using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConStr"])
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

var rangosEndpoints = app.MapGroup("/rangos");
var rangosEndpointsId = rangosEndpoints.MapGroup("/{rangoId:int}");
var ingredientesEndpoint = rangosEndpointsId.MapGroup("/ingredientes");

app.MapGet("/", () => "Hello World!");


rangosEndpoints.MapGet("", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>>
    (RangoDbContext rangoDbContext,
    IMapper mapper,
    [FromQuery(Name = "name")] string? rangoNome) =>
{

    var rangosEntity = await rangoDbContext
       .Rangos
       .Where(x => rangoNome == null || x.Nome.ToLower().Contains(rangoNome.ToLower()))
       .ToListAsync();

    if (rangosEntity.Count <= 0 || rangosEntity == null)
        return TypedResults.NoContent();
    else
        return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));

});

ingredientesEndpoint.MapGet("", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int rangoId) =>
{
    return mapper.Map<IEnumerable<IngredienteDTO>>((await rangoDbContext
        .Rangos
        .Include(rango => rango.Ingredientes)
        .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);
});

rangosEndpointsId.MapGet("", async (
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int rangoId) =>
{

    return mapper.Map<RangoDTO>(await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId));

}).WithName("GetRangos");



rangosEndpoints.MapPost("", async Task<CreatedAtRoute<RangoDTO>> (RangoDbContext rangoDbContext,
    IMapper mapper,
    [FromBody] RangoCriaDTO rangoCriaDTO
    //LinkGenerator linkGenerator,
    //HttpContext httpContext
    ) =>
{
    var rangoEntity = mapper.Map<Rango>(rangoCriaDTO);
    rangoDbContext.Add(rangoEntity);
    await rangoDbContext.SaveChangesAsync();

    var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);

    return TypedResults.CreatedAtRoute(
        rangoToReturn,
        "GetRangos",
        new { rangoId = rangoToReturn.Id });

    // Explicação para Consultas Futuras
    /*
             app.MapPost("/rango", async Task<CreatedAtRoute<PratoDTO>>(
            RangoDbContext bancoDeDados,
            IMapper conversorDeObjetos,
            [FromBody] CriarPratoDTO dadosRecebidos
            ) =>
        {
            // 1. Converter os dados recebidos do cliente (DTO) para uma entidade do banco
            var pratoEntidade = conversorDeObjetos.Map<Prato>(dadosRecebidos);

            // 2. Adicionar o novo prato no banco
            bancoDeDados.Add(pratoEntidade);
            await bancoDeDados.SaveChangesAsync();

            // 3. Converter a entidade salva para um DTO de resposta
            var pratoParaRetorno = conversorDeObjetos.Map<PratoDTO>(pratoEntidade);

            // 4. Retornar resposta 201 Created, informando o local onde o recurso pode ser acessado
            return TypedResults.CreatedAtRoute(
                pratoParaRetorno,
                "GetPrato", // Nome da rota que recupera um prato pelo Id
                new { id = pratoParaRetorno.Id });
        });

     */


    // Referência para consultas 
    //var linkToReturn = linkGenerator.GetUriByName(
    //    httpContext,
    //    "GetRango",
    //    new { id = rangoToReturn.Id });

    //return TypedResults.Created(linkToReturn, rangoToReturn);
});

rangosEndpointsId.MapPut("", async Task<Results<NotFound,Ok>>(
    RangoDbContext rangoDbContext,
    IMapper mapper,
    int rangoId,
    [FromBody] RangoAtualizaDTO rangoAtualizaDTO) =>
{
    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    mapper.Map(rangoAtualizaDTO, rangosEntity);
    await rangoDbContext.SaveChangesAsync();

    return TypedResults.Ok();

    //Explicação para Consulta

     /*   app.MapPut("/rango/{id:int}", async (
        RangoDbContext bancoDeDados,
        IMapper conversorDeObjetos,
        int idDoPrato,
        [FromBody] AtualizarPratoDTO novosDadosRecebidos) =>
        {
            // 1. Buscar o prato existente no banco, pelo Id enviado na URL
            var pratoExistente = await bancoDeDados.Rangos.FirstOrDefaultAsync(prato => prato.Id == idDoPrato);

            // 2. Se o prato não existir, retorna 404 Not Found
            if (pratoExistente == null)
                return TypedResults.NotFound();

            // 3. Atualizar os campos do prato com os novos dados recebidos
            conversorDeObjetos.Map(novosDadosRecebidos, pratoExistente);

            // 4. Salvar as mudanças no banco
            await bancoDeDados.SaveChangesAsync();
        });
     */

});

rangosEndpointsId.MapDelete("", async Task<Results<NotFound, NoContent>> (
    RangoDbContext rangoDbContext,
    int rangoId) =>
{
var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

if (rangosEntity == null)
    return TypedResults.NotFound();

    rangoDbContext.Rangos.Remove(rangosEntity);

await rangoDbContext.SaveChangesAsync();

return TypedResults.NoContent();

    //Explicação para Consultas
    /* app.MapDelete("/rango/{id:int}", async Task<Results<NotFound, NoContent>> (
     RangoDbContext bancoDeDados,
     int idDoPrato) =>
     {
         1. Buscar o prato que queremos excluir, usando o Id informado na URL
        var pratoExistente = await bancoDeDados.Rangos.FirstOrDefaultAsync(prato => prato.Id == idDoPrato);

         2. Se o prato não for encontrado, retornar 404 Not Found
        if (pratoExistente == null)
            return TypedResults.NotFound();

        3. Remover o prato do banco de dados
        bancoDeDados.Rangos.Remove(pratoExistente);

         4. Salvar as alterações no banco
        await bancoDeDados.SaveChangesAsync();

         5. Retornar status 204 No Content (remoção bem-sucedida, sem conteúdo na resposta)
        return TypedResults.NoContent();
      });

    */


});



app.Run();