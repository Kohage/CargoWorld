using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repo
{
        public class Repo<T> : IRepo<T> where T : class
        {
            AppDbContext _context;
            DbSet<T> _dbSet;
            public Repo(AppDbContext context)
            {
                _context = context;
                _dbSet = context.Set<T>();
            }
            public async Task<T> SaveAsync(T entity)
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }

            public IEnumerable<T> GetAll()
            {
                return _dbSet.AsEnumerable();
            }

            public T GetById(int id)
            {
                return _dbSet.Find(id);
            }
        }
}
