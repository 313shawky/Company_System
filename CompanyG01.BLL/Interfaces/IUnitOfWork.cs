using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyG01.BLL.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        // Signature for properties for every and each repository
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        public Task<int> Complete();
    }
}
