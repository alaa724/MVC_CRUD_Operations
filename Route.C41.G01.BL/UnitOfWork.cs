using Route.C41.G01.BL.Interfaces;
using Route.C41.G01.BL.Repositories;
using Route.C41.G01.DAL.Data;
using Route.C41.G01.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G01.BL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        //private Dictionary<string , IGenericRepository<BaseModel>> _repository;
        private Hashtable _repository;

        public IGenericRepository<T> Repository<T>()/*Repository<Employee>*/ where T : BaseModel
        {
            var key = typeof(T).Name; // Employee

            if (!_repository.ContainsKey(key))
            {
                if(key == nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repository.Add(key, repository);

                }
                else
                {
                    var repository = new GenericRepository<T>(_dbContext);
                    _repository.Add(key, repository);
                }
            }

            return _repository[key] as IGenericRepository<T>;
        }

        public UnitOfWork(ApplicationDbContext dbContext) // Ask CLR for creating object from 'DbContext'
        {
            _dbContext = dbContext;

            _repository = new Hashtable();
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose(); // Close Connetion
        }

    }
}
