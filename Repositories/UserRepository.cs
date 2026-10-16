using Harjoitustyo.Models;
using Microsoft.EntityFrameworkCore;

namespace Harjoitustyo.Repositories
{
    public class UserRepository : IUserRepository
    {
        MessageServiceContext _repository;
        public UserRepository(MessageServiceContext repository)
        {
            _repository = repository;
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            _repository.Users.Add(user);
            await _repository.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(long id)
        {
            User? user = await _repository.Messages.FindAsync(id);
            if (user == null)
            {

                return false;
            }
            _repository.Users.Remove(user);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserAsync(long id)
        {
            User? user = await _repository.Messages.FindAsync(id);
            return user;
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await _repository.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await  _repository.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            User? dbUser = await _repository.Users.FindAsync(user.Id);
            if (dbUser == null)
            {
                return false;
            }
            dbUser.Username = user.Username;
            dbUser.Password = user.Password;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.JoinedAt = user.JoinedAt;
            dbUser.LastLogin = user.LastLogin;
            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO! mitä tehdään virhetapauksessa
                return false;
            }
            return true;
        }
    }
}
