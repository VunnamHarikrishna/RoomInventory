using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Shared.DataTransferObjects;
using System.Diagnostics;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        ///  test by hari
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var companies = _repository.Company.GetAllCompaniesAsync(trackChanges);//5
            var company2 = _repository.Company.GetAllCompaniesAsync( trackChanges);//5
            var company3 = _repository.Company.GetAllCompaniesAsync(trackChanges);//5
            stopwatch.Stop();
            _logger.LogInfo("Time: "+stopwatch.ElapsedMilliseconds.ToString());

            await companies;

            var companies2 = _repository.Company.GetAllCompanies();

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return companiesDto;
        }

        public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges)
        {
            var company =await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            //Check if the company is null
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }
        public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities =await _repository.Company.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;
        }
        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync
(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn =
            _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);
        }
        public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            await _repository.Company.DeleteCompanyAsync(company);
            await _repository.SaveAsync();
        }
        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);
            _mapper.Map(companyForUpdate, company);
            await _repository.SaveAsync();
        }

        private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(id);
            return company;
        }
    }
}