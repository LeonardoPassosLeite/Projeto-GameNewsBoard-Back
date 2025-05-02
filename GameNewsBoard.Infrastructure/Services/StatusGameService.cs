using AutoMapper;
using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Domain.Commons;
using GameNewsBoard.Domain.Entities;
using GameNewsBoard.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace GameNewsBoard.Infrastructure.Services
{
    public class StatusGameService : IStatusGameService
    {
        private readonly IStatusGameRepository _statusGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StatusGameService> _logger;

        public StatusGameService(
            IStatusGameRepository statusGameRepository,
            IGameRepository gameRepository,
            IMapper mapper,
            ILogger<StatusGameService> logger)
        {
            _statusGameRepository = statusGameRepository;
            _gameRepository = gameRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> SetStatusAsync(Guid userId, int gameId, Status status)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
                return Result.Failure("Jogo não encontrado.");

            var existing = await _statusGameRepository.GetByUserAndGameAsync(userId, gameId);

            if (existing is null)
            {
                var statusGame = StatusGame.Create(userId, gameId, status);
                await _statusGameRepository.AddAsync(statusGame);
            }
            else
            {
                existing.UpdateStatus(status);
            }

            await _statusGameRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IEnumerable<StatusGameResponse>>> GetUserGameStatusesAsync(Guid userId)
        {
            var list = await _statusGameRepository.GetByUserAsync(userId);
            var result = _mapper.Map<IEnumerable<StatusGameResponse>>(list);
            return Result<IEnumerable<StatusGameResponse>>.Success(result);
        }

        public async Task<Result> RemoveStatusAsync(Guid userId, int gameId)
        {
            var statusGame = await _statusGameRepository.GetByUserAndGameAsync(userId, gameId);
            if (statusGame == null)
                return Result.Failure("Status não encontrado.");

            _statusGameRepository.Remove(statusGame);
            await _statusGameRepository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
