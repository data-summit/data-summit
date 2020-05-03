using System;
using DataSummitModels.DB;

namespace DataSummitHelper.Dto
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

        public CompanyDto(Companies company)
        {
            CompanyId = company.CompanyId;
            CompanyName = company.Name;
            CompanyNumber = company.CompanyNumber;
            Website = company.Website;
            VatNumber = company.Vatnumber;
            CreatedDate = company.CreatedDate;
        }

        public Companies ToCompany() => new Companies()
        {
            CompanyId = CompanyId,
            Name = CompanyName,
            CompanyNumber = CompanyNumber,
            Website = Website,
            CreatedDate = CreatedDate
        };

    }
}
