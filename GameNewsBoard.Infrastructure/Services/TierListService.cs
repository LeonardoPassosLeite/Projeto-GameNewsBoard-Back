using AutoMapper;
using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GameNewsBoard.Infrastructure.Services
{
    public class TierListService : ITierListService
    {
        private readonly ITierListRepository _tierListRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TierListService> _logger;

        public TierListService(
            ITierListRepository tierListRepository,
            IGameRepository gameRepository,
            IMapper mapper,
            ILogger<TierListService> logger)
        {
            _tierListRepository = tierListRepository ?? throw new ArgumentNullException(nameof(tierListRepository));
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Tier Methods
        public async Task<Result> CreateTierListAsync(Guid userId, TierListRequest request)
        {
            try
            {
                var tierList = new TierList(userId, request.Title, request.ImageUrl);

                await _tierListRepository.AddAsync(tierList);
                await _tierListRepository.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a lista de tiers.");
                return Result.Failure("Ocorreu um erro ao criar a lista de tiers. Tente novamente mais tarde.");
            }
        }

        public async Task<Result> UpdateTierListAsync(Guid tierListId, UpdateTierListRequest request)
        {
            var tierList = await _tierListRepository.GetByIdAsync(tierListId);
            if (tierList == null)
                return Result.Failure("Tier não encontrado.");

            try
            {
                tierList.UpdateInfo(request.NewTitle, request.NewImageUrl);
                await _tierListRepository.SaveChangesAsync();
                return Result.Success();
            }
            catch (ArgumentException ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> DeleteTierListAsync(Guid tierListId)
        {
            var tierList = await _tierListRepository.GetByIdAsync(tierListId);
            if (tierList == null)
                return Result.Failure("Tier não encontrado.");

            _tierListRepository.Remove(tierList);
            await _tierListRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<TierListResponse>>> GetTierListsByUserAsync(Guid userId)
        {
            var tiers = await _tierListRepository.GetByUserAsync(userId);
            var response = _mapper.Map<IEnumerable<TierListResponse>>(tiers);
            return Result<IEnumerable<TierListResponse>>.Success(response);
        }
        #endregion

        #region Game In Tier Methods
        public async Task<Result> SetGameTierAsync(Guid tierListId, TierListEntryRequest request)
        {
            var tierList = await _tierListRepository.GetByIdAsync(tierListId);
            if (tierList == null)
                return Result.Failure("Tier não encontrado.");

            var game = await _gameRepository.GetByIdAsync(request.GameId);
            if (game == null)
                return Result.Failure("Jogo não encontrado.");

            var existingEntry = tierList.Entries.FirstOrDefault(e => e.GameId == request.GameId);

            if (existingEntry is null)
            {
                var newEntry = TierListEntry.Create(request.GameId, request.Tier, tierList.Id);
                await _tierListRepository.AddEntryAsync(newEntry);
            }
            else
            {
                existingEntry.UpdateTier(request.Tier);
            }

            await _tierListRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> RemoveGameFromTierAsync(Guid tierListId, int gameId)
        {
            var tierList = await _tierListRepository.GetByIdAsync(tierListId);
            if (tierList == null)
                return Result.Failure("Tier não encontrado.");

            tierList.RemoveGameFromTier(gameId);
            await _tierListRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<TierListResponse>> GetTierListByIdAsync(Guid tierListId)
        {
            var tierList = await _tierListRepository.GetByIdAsync(tierListId);

            if (tierList == null)
                return Result<TierListResponse>.Failure("Tier não encontrado.");

            var response = _mapper.Map<TierListResponse>(tierList);
            return Result<TierListResponse>.Success(response);
        }

        #endregion
    }
}