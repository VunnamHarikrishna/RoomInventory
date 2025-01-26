using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        protected RepositoryContext RepositoryContext;

        public CompanyRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        { 
            var companies = await RepositoryContext.Companies.ToListAsync();
            return companies;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges) =>
                                    await FindAll(trackChanges)
                                     .OrderBy(c => c.Name)
                                     .ToListAsync();

        public IEnumerable<Company> GetAllCompanies()
        {
            var test = RepositoryContext.Companies.Select(x => x.Name).ToQueryString();
            return FindAll(true);
        }

        public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
                          await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
                          .SingleOrDefaultAsync();

        public void CreateCompany(Company company) => CreateAsync(company);

        public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
                                    await FindByCondition(x => ids.Contains(x.Id), trackChanges)
                                     .ToListAsync();

        public async Task DeleteCompanyAsync(Company company) => Delete(company);

    }
}