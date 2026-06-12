using ShooterPlatform.Api.Application.Common.CustomExceptions;
using ShooterPlatform.Api.Application.Features.Players.DTOs;
using ShooterPlatform.Api.Application.Features.Players.Interfaces;
using ShooterPlatform.Api.Application.Features.Players.Mappers;
using ShooterPlatform.Api.Application.Features.Players.Models;
using ShooterPlatform.Api.Infrastructure;

namespace ShooterPlatform.Api.Application.Features.Players.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ShooterPlatformDbContext _dbContext;

        public PlayerService(ShooterPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IEnumerable<PlayerResponse> GetAllPlayers()
        {
            return _dbContext.Players
                .Select(PlayerMapper.ToResponse)
                .ToList();
        }

        public PagedResult<PlayerResponse> GetPlayers(PlayerQueryRequest request)
        {
            IQueryable<Player> query = _dbContext.Players;

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                query = query.Where(x => x.Name.Contains(request.Keyword));
            }

            if (request.MinLevel.HasValue)
            {
                query = query.Where(x => x.Level >= request.MinLevel.Value);
            }

            if (request.MaxLevel.HasValue)
            {
                query = query.Where(x => x.Level <= request.MaxLevel.Value);
            }

            query = ApplySorting(query, request.SortBy, request.Order);

            var totalCount = query.Count();

            var page = Math.Max(1, request.Page);
            var pageSize = Math.Clamp(request.PageSize, 1, 100);

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(PlayerMapper.ToResponse)
                .ToList();

            return new PagedResult<PlayerResponse>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }

        private IQueryable<Player> ApplySorting(
            IQueryable<Player> query,
            PlayerSortBy sortBy,
            SortOrder order)
        {
            var isDesc = order == SortOrder.Desc;

            return sortBy switch
            {
                PlayerSortBy.Name => isDesc
                    ? query.OrderByDescending(x => x.Name)
                    : query.OrderBy(x => x.Name),

                PlayerSortBy.Level => isDesc
                    ? query.OrderByDescending(x => x.Level)
                    : query.OrderBy(x => x.Level),

                _ => isDesc
                    ? query.OrderByDescending(x => x.Id)
                    : query.OrderBy(x => x.Id)
            };
        }

        public PlayerResponse GetPlayer(int id)
        {
            var player = _dbContext.Players.FirstOrDefault(x => x.Id == id)
                ?? throw new NotFoundException("Player not found");

            return PlayerMapper.ToResponse(player);
        }

        public void CreatePlayer(PlayerCreateRequest request)
        {
            var player = PlayerMapper.ToEntity(request);

            _dbContext.Players.Add(player);
            _dbContext.SaveChanges();
        }

        public PlayerResponse UpdatePlayer(int id, PlayerUpdateRequest request)
        {
            var player = _dbContext.Players
                .FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException($"Player {id} not found");

            PlayerMapper.UpdateEntity(player, request);

            _dbContext.SaveChanges();

            return PlayerMapper.ToResponse(player);
        }

        public void DeletePlayer(int id)
        {
            var player = _dbContext.Players
                .FirstOrDefault(x => x.Id == id) ?? throw new NotFoundException($"Player {id} not found");

            _dbContext.Players.Remove(player);
            _dbContext.SaveChanges();
        }
    }
}

