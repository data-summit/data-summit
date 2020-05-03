using System;
using DataSummitHelper.Dto;
using DataSummitModels.DB;

namespace DataSummitWeb.Models
{
    /// <summary>
    /// </summary>
    public sealed class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; }
        public string Website { get; set; }
        public string VatNumber { get; set; }
        public DateTime? CreatedDate { get; set; }

        public static Company FromDto(CompanyDto dto) => new Company
        {
            CompanyId = dto.CompanyId,
            CompanyName = dto.CompanyName,
            CompanyNumber = dto.CompanyNumber,
            Website = dto.Website,
            VatNumber = dto.VatNumber,
            CreatedDate = dto.CreatedDate
        };

        public CompanyDto ToDto() => new CompanyDto()
        {
            CompanyId = CompanyId,
            CompanyName = CompanyName,
            CompanyNumber = CompanyNumber,
            Website = Website,
            VatNumber = VatNumber,
            CreatedDate = CreatedDate
        };
    }
}
