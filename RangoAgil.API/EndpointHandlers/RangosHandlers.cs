﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DbContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;

namespace RangoAgil.API.EndpointHandlers;

public static class RangosHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> GetRangosAsync
        (RangoDbContext rangoDbContext,
        IMapper mapper,
        ILogger<RangoDTO> logger,
        [FromQuery(Name = "name")] string? rangoNome)
    {

        var rangosEntity = await rangoDbContext.Rangos
                                   .Where(x => rangoNome == null || x.Nome.ToLower().Contains(rangoNome.ToLower()))
                                   .ToListAsync();
        if (rangosEntity.Count <= 0 || rangosEntity == null)
        {
            logger.LogInformation($"Rango não encontrado. Parâmetro: {rangoNome}");
            return TypedResults.NoContent();
        }
           
        else
        {
            logger.LogInformation("Retornando o Rango encontrado");
            return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));
        }
            

    }

    public static async Task<Results<NotFound, Ok<RangoDTO>>> GetRangoById(
        RangoDbContext rangoDbContext,
        IMapper mapper,
        int rangoId)
    {
        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);
        if (rangosEntity == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(mapper.Map<RangoDTO>(
            await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId))
        );
    }

    public static async Task<CreatedAtRoute<RangoDTO>> CreateRangoAsync(
        RangoDbContext rangoDbContext,
        IMapper mapper,
        [FromBody] RangoCriaDTO rangoCriaDTO)
    {
        var rangoEntity = mapper.Map<Rango>(rangoCriaDTO);
        rangoDbContext.Add(rangoEntity);
        await rangoDbContext.SaveChangesAsync();

        var rangoToReturn = mapper.Map<RangoDTO>(rangoEntity);

        return TypedResults.CreatedAtRoute(
            rangoToReturn,
            "GetRangos",
            new { rangoId = rangoToReturn.Id });

    }

    public static async Task<Results<NotFound, Ok>> UpdateRangoAsync(
        RangoDbContext rangoDbContext,
        IMapper mapper,
        int rangoId,
        [FromBody] RangoAtualizaDTO rangoAtualizaDTO)
    {
        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
            return TypedResults.NotFound();

        mapper.Map(rangoAtualizaDTO, rangosEntity);
        await rangoDbContext.SaveChangesAsync();

        return TypedResults.Ok();

    }

    public static async Task<Results<NotFound, NoContent>> DeleteRangoAsync(
        RangoDbContext rangoDbContext,
        int rangoId)
    {
        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
            return TypedResults.NotFound();

        rangoDbContext.Rangos.Remove(rangosEntity);

        await rangoDbContext.SaveChangesAsync();

        return TypedResults.NoContent();
    }
}