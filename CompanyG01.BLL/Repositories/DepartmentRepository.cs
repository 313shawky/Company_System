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
    public class DepartmentRepository : GenericRepository<Department>,  IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext dbContext):base(dbContext)
        {
        }
    }
}
