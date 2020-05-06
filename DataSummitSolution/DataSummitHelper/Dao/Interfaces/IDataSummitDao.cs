using DataSummitHelper.Classes;
using DataSummitModels.DB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitHelper.Dao.Interfaces
{
    public interface IDataSummitDao
    {
        #region Companies
        Task<List<Companies>> GetAllCompanies();
        Task<Companies> GetCompanyById(int id);
        Task CreateCompany(Companies company);
        Task UpdateCompany(Companies company);
        Task DeleteCompany(int id);
        #endregion

        #region Projects
        Task<List<Projects>> GetAllCompanyProjects(int companyId);
        Task CreateProject(Projects company);
        Task UpdateProjectName(Projects company);
        Task DeleteProject(int id);
        #endregion

        Task<List<Drawings>> GetProjectDrawings(int companyId);

        Task<List<ProfileVersions>> GetCompanyProfileVersions(int companyId);

        Task<List<ProfileAttributes>> GetTemplateAttribtesForTemplateId(int profileId);

        Task DeleteProfileAttribute(long profileAttributeId);
        Task<List<Drawings>> GetAllProjectDrawings(int projectId);
        Task<List<DrawingProperty>> GetDrawingPropertiesByDrawingId(int drawingId);
        Task UpdateDrawingPropertyValue(Guid drawingPropertyId, string drawingPropertyValue);
        Task<List<ProfileAttributes>> GetProfileAttributesForDrawingId(int drawingId);
    }
}
