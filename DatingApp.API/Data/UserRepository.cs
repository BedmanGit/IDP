using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoAsync(int userId)
        {
            return await _context.Photos.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhotoAsync(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Photo>> GetPhotosAsync(int userId)
        {
            return await _context.Photos.Where(p => p.UserId == userId).ToListAsync();
        }
        public async Task<IEnumerable<Photo>> GetPhotosAsync()
        {
            return await _context.Photos.ToListAsync();
        }
        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.Include(u => u.Claims).Include(p => p.Photos)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _context.Users.Include(u => u.Claims).ToListAsync();
            users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;//await users;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}