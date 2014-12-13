using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Commit.Web.Models.BasicViewModels;
using Infrastructure;
using Infrastructure.Data;

namespace Commit.Web.Razor.Controllers.Api
{
    public class StaarServiceController : ApiController
    {
        private readonly AzureDataContext _ctx = ApplicationFactory.RetrieveContext();


        public List<DropDownViewModel> GetDemographics()
        {
            return
                _ctx.Demographics.Select(c => new DropDownViewModel { Text = c.Name, Value = c.Id }).ToList();
        }

        public List<DropDownViewModel> GetDemographicDetails(GridFilterModel filter)
        {
            var id = int.Parse(filter.Filters[0].Field);
            return
                _ctx.DemographicDetails.Where(d => d.Demographic.Id == id)
                    .Select(c => new DropDownViewModel { Text = c.Description, Value = c.Id }).ToList();
        }

        [HttpPost]
        public List<DemoGrid> ReadTests(RequestModel model)
        {
            if (model == null) return new List<DemoGrid>();


            //must have at least one from each

            if (model.CampusIds == null || model.SubjectIds == null
                || model.CategoryDetailIds == null || model.DemographicDetailIds == null)
            {
                return new List<DemoGrid>();
            }

            try
            {

                var x = _ctx.StaarTests.Where(s =>
                    model.CampusIds.Contains(s.Campus_Id)
                    && model.SubjectIds.Contains(s.Subject_Id)
                    && (model.CategoryDetailIds.Contains(s.CategoryDetail_Id) || model.CategoryDetailIds.Contains(s.CategoryDetail.PartnerDetail.Id))
                    && model.DemographicDetailIds.Contains(s.DemographicDetail_Id))
                    .Select(s => new
                    {
                        s.Value,
                        SubjectDescription = s.Subject.Description,
                        DemographicName = s.DemographicDetail.Demographic.Name,
                        CategoryName = s.CategoryDetail.Category.Name,
                        DemoDetailDescription = s.DemographicDetail.Description,
                        CatDetailDescription = s.CategoryDetail.Description,
                        CampusName = s.Campus.Name,
                        CountId = s.CategoryDetail.PartnerDetail == null ? 0 : s.CategoryDetail.PartnerDetail.Id,
                        CategoryDetailId = s.CategoryDetail_Id,
                        s.CategoryDetail.CategoryType
                    })
                    .Take(500) //take at most 500 at a time
                    .ToList(); ;

                var list = new List<DemoGrid>(x.Count);
                list.AddRange(from s in x
                              where s.CategoryType != CategoryType.Count
                              let count = x.FirstOrDefault(d => d.CategoryDetailId == s.CountId)
                              let value = count == null ? 0 : count.Value
                              select new DemoGrid
                              {
                                  Count = value,
                                  Percent = s.Value, //this gets the percent value
                                  Subject = s.SubjectDescription,
                                  Demographic = s.DemographicName + ": " + s.DemoDetailDescription,
                                  Category = s.CategoryName + ": " + s.CatDetailDescription,
                                  CampusName = s.CampusName
                              });

                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        public IEnumerable<DropDownViewModel> ReadCampuses(GridPageModel model)
        {
            if (model == null) return new List<DropDownViewModel>();

            var sort = model.Filter.Filters[0].Value;

            var campuses = _ctx.Campuses.Where(c => c.Name.Contains(sort))
                .Take(25)
                .ToDropDown(c => c.Name);
            return campuses;
        }

        public class DemoGrid
        {
            public string CampusName { get; set; }
            public string Subject { get; set; }
            public string Demographic { get; set; }
            public string Category { get; set; }
            public decimal Count { get; set; }
            public decimal Percent { get; set; }
        }

        public class RequestModel : GridPageModel
        {
            public List<long> CategoryDetailIds { get; set; }
            public List<long> DemographicDetailIds { get; set; }
            public List<long> CampusIds { get; set; }
            public List<long> SubjectIds { get; set; }
        }
    }
}