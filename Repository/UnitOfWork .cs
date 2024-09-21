﻿using FurnitureStore.Data;
using FurnitureStore.IRepository;
using FurnitureStore.Models;

namespace FurnitureStore.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IRepository<Product> _productRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _productRepository = new Repository<Product>(_context);
            UserRepository = new Repository<User>(_context);
            UserRoleRepo= new Repository<UserRole>(_context);
        }

        public IRepository<Product> ProductRepository { get; }

        public IRepository<User> UserRepository { get; }

        public IRepository<UserRole> UserRoleRepo { get; }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
    