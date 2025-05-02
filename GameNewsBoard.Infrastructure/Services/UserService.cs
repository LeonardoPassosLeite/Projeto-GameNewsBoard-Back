using AutoMapper;
using GameNewsBoard.Application.IServices;
using GameNewsBoard.Application.IRepository;
using GameNewsBoard.Application.Responses.DTOs.Responses;
using GameNewsBoard.Application.Settings;
using Microsoft.Extensions.Options;

namespace GameNewsBoard.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITierListRepository _tierListRepository;
        private readonly IMapper _mapper;
        private readonly BackendSettings _backendSettings;

        public UserService(
            IUserRepository userRepository,
            ITierListRepository tierListRepository,
            IMapper mapper,
            IOptions<BackendSettings> backendSettings)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _tierListRepository = tierListRepository ?? throw new ArgumentNullException(nameof(tierListRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _backendSettings = backendSettings?.Value ?? throw new ArgumentNullException(nameof(backendSettings));
        }

        public async Task<UserProfileResponse> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ApplicationException("Usuário não encontrado.");

            var tierLists = await _tierListRepository.GetByUserAsync(userId);
            var mappedTiers = _mapper.Map<List<TierListResponse>>(tierLists);

            foreach (var tier in mappedTiers)
            {
                tier.ImageUrl = $"{_backendSettings.Url}{tier.ImageUrl}";
            }

            // Em vez de _mapper.Map<UserProfileResponse>(user)
            // cria manualmente o profile:
            var profile = new UserProfileResponse
            {
                UserId = user.Id,
                Username = user.Username,
                Tiers = mappedTiers
            };

            return profile;
        }
    }
}
