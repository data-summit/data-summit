using System;
using DataSummitModels.DB;

namespace DataSummitService.Dto
{
    /// <summary>
    /// </summary>
    public sealed class CompanyDto
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; }
        public string Website { get; set; }
        public string VatNumber { get; set; }
        public DateTime? CreatedDate { get; set; }

        public CompanyDto()
        { ; }

        public CompanyDto(Company company)
        {
            CompanyId = company.CompanyId;
            CompanyName = company.Name;
            CompanyNumber = company.CompanyNumber;
            Website = company.Website;
            VatNumber = company.Vatnumber;
            CreatedDate = company.CreatedDate;
        }

        public Company ToCompany() => new Company()
        {
            CompanyId = CompanyId,
            Name = CompanyName,
            CompanyNumber = CompanyNumber,
            Website = Website,
            CreatedDate = CreatedDate
        };

    }
}
