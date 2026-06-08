using GamePlatform.Api.Application.DTOs;
using GamePlatform.Api.Application.Interfaces;
using GamePlatform.Api.Domain.Entities;
using GamePlatform.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Api.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly GamePlatformDbContext _dbContext;

    public PlayerService(GamePlatformDbContext dbContext)
    {
        _dbContext = dbContext;
    }



    public IEnumerable<Player> GetPlayers()
    {
        return _dbContext.Players.ToList();
    }

    public Player? GetPlayer(int id)
    {
        return _dbContext.Players.FirstOrDefault(x => x.Id == id);
    }

    public void AddPlayer(PlayerDto dto)
    {
        var player = new Player
        {
            Name = dto.Name,
            Level = dto.Level
        };

        _dbContext.Players.Add(player);
        _dbContext.SaveChanges();
    }

    public Player? UpdatePlayer(int id, Player updatedPlayer)
    {
        var player = _dbContext.Players.FirstOrDefault(x => x.Id == id);

        if (player == null)
            return null;

        player.Name = updatedPlayer.Name;
        player.Level = updatedPlayer.Level;

        _dbContext.SaveChanges();

        return player;
    }

    public bool DeletePlayer(int id)
    {
        var player = _dbContext.Players.FirstOrDefault(x => x.Id == id);

        if (player == null)
            return false;

        _dbContext.Players.Remove(player);
        _dbContext.SaveChanges();

        return true;
    }
}