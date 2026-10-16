using Harjoitustyo.Middleware;
using Harjoitustyo.Models;
using Harjoitustyo.Repositories;

namespace Harjoitustyo.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IUserAuthenticationService _authenticationService;
        public UserService(IUserRepository repository, IUserAuthenticationService authenticationService)
        {
            _repository = repository;
            _authenticationService = authenticationService;
        }
        public async Task<UserDTO?> CreateUserAsync(User user)
        {
            User? oldUser = await _repository.GetUserAsync(user.Username);
            if(oldUser !=null)
            {
                 return null;
            }
            user.JoinedAt = DateTime.UtcNow;

            user = _authenticationService.CreateUserCredentials(user);

            return userToDto(await _repository.CreateUserAsync(user));
        }
        public async Task<bool> DeleteUserAsync(string username)
        {
            User? user = await _repository.GetUserAsync(username);
            if (user == null)
            {
                return false;
            }
            return await _repository.DeleteUserAsync(user.Id);
        }
        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            IEnumerable<User> users = await _repository.GetUsersAsync();
            List<UserDTO> userDtos = new List<UserDTO>();
            foreach (User usr in users)
            {            
                userDtos.Add(userToDto(usr));
            }
            return userDtos;
        }
        public async Task<UserDTO?> GetUserAsync(string username)
        {
            /*User? user = await _repository.GetUserAsync(id);
            if (user == null)
            {
                return null;
            }*/
            return userToDto(await _repository.GetUserAsync(username));
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user == null)
            {
                return false;
            }
            return await _repository.UpdateUserAsync(user);
        }
        private UserDTO userToDto(User user)
        {
            if(user == null)
            {
                return null;
            }
            UserDTO dto = new UserDTO();

            dto.UserName = user.Username;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            dto.JoinedAt = user.JoinedAt;
            dto.LastLogin = user.LastLogin;
            return dto;
        }

        public async Task<UserDTO?> GetUserAsync(long id)
        {
            return userToDto(await _repository.GetUserAsync(id));
        }
    }
}