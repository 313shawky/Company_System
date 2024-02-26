using CompanyG01.BLL.Interfaces;
using CompanyG01.DAL.Data;
using CompanyG01.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyG01.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _dbContext;
        public EmployeeRepository(AppDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<Employee> GetEmployeesByAddress(string address)
            => _dbContext.Employees.Where(E => E.Address == address);

        public IQueryable<Employee> SearchEmployeesByName(string SearchValue)
            => _dbContext.Employees.Where(E => E.Name.ToLower().Contains(SearchValue.ToLower()));

    }
}
