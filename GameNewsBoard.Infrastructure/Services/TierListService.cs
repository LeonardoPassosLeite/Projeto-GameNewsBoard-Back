using AutoMapper;
using GameNewsBoard.Application.DTOs.Requests;
using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace GameNewsBoard.Infrastructure.Services;

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

    public async Task<Result> CreateTierListAsync(TierListRequest request)
    {
        try
        {
            var tierList = new TierList(request.UserId, request.Title, request.ImageUrl);

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

    public async Task<Result> AddGameToTierAsync(Guid tierListId, int gameId, TierLevel tier)
    {
        var tierList = await _tierListRepository.GetByIdAsync(tierListId);
        if (tierList == null)
            return Result.Failure("Tier não encontrado.");

        var game = await _gameRepository.GetByIdAsync(gameId);
        if (game == null)
            return Result.Failure("Jogo não encontrado.");

        if (tierList.Entries.Any(e => e.GameId == gameId))
            return Result.Failure("Esse jogo já está no ranking.");

        try
        {
            // Cria nova entry corretamente
            var newEntry = TierListEntry.Create(gameId, tier, tierList.Id);

            // Salva diretamente no contexto
            await _tierListRepository.AddEntryAsync(newEntry);

            // Salva no banco
            await _tierListRepository.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> UpdateTierListAsync(Guid tierListId, string? newTitle = null, string? newImage = null)
    {
        var tierList = await _tierListRepository.GetByIdAsync(tierListId);
        if (tierList == null)
            return Result.Failure("Tier não encontrado.");

        try
        {
            tierList.UpdateInfo(newTitle, newImage);
            await _tierListRepository.SaveChangesAsync();
            return Result.Success();
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> UpdateGameTierAsync(Guid tierListId, int gameId, TierLevel newTier)
    {
        var tierList = await _tierListRepository.GetByIdAsync(tierListId);
        if (tierList == null)
            return Result.Failure("Tier não encontrado.");

        try
        {
            tierList.UpdateGameTier(gameId, newTier);
            await _tierListRepository.SaveChangesAsync();
            return Result.Success();
        }
        catch (InvalidOperationException ex)
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
}