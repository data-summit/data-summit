using System;

namespace DataSummitModels.DB
{
    public partial class Employees
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }
        public string JobTitle { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string Notes { get; set; }
        public int? ReportsTo { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
        public byte? GenderId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Genders Gender { get; set; }
    }
}
