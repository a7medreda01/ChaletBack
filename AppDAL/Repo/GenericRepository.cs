using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDAL.Context;
using AppDAL.IRepo;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HotelDbContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(HotelDbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _db.ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _db.FindAsync(id);

        public async Task AddAsync(T entity)
            => await _db.AddAsync(entity);

        public void Update(T entity)
            => _db.Update(entity);

        public void Delete(T entity)
            => _db.Remove(entity);
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

