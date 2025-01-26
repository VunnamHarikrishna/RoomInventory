using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext): base(repositoryContext)
        {
        }
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges) =>
                                     FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                                     .OrderBy(e => e.Name).ToList();

        public Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) =>
                         FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
                         .FirstOrDefaultAsync();

        public async Task CreateEmployeeForCompanyAsync(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            await CreateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(Employee employee) => Delete(employee);

    }
}
