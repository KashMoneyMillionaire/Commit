using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using Commit.Web.Models;
using Infrastructure.Data;

namespace Commit.Web.Controllers.Api
{
    public class StaarRequestController : ApiController
    {
        private readonly OperationalDataContext _ctx = new OperationalDataContext();


        public List<DropDownViewModel> GetDemographics()
        {
            return
                _ctx.Demographics.Select(c => new DropDownViewModel { Text = c.Name, Value = c.Id.ToString() }).ToList();
        }

        public List<DropDownViewModel> GetDemographicDetails(GridFilterModel filter)
        {
            var id = int.Parse(filter.Filters[0].Field);
            return
                _ctx.DemographicDetails.Where(d => d.Demographic.Id == id)
                    .Select(c => new DropDownViewModel {Text = c.Description, Value = c.Id.ToString()}).ToList();
        }

        [HttpPost]
        public List<DemoGrid> ReadTests(RequestModel model)
        {
            return _ctx.StaarTests.Where(s => model.CategoryDetailIds.Contains(s.CategoryDetail_Id)
                                       && model.DemographicDetailIds.Contains(s.DemographicDetail_Id)
                                       && model.CampusIds.Contains(s.Campus_Id)
                                       && model.SubjectIds.Contains(s.Subject_Id))
                .Select(s => new DemoGrid
                {
                    Value = s.Value,
                    Subject = s.Subject.Description,
                    Demographic = s.DemographicDetail.Description,
                    Category = s.CategoryDetail.Description,
                    CampusName = s.Campus.Name
                }).ToList();
        }

        public List<DropDownViewModel> ReadCampuses(GridPageModel model)
        {
            var sort = model.Filter.Filters[0].Value;

            return _ctx.Campuses.Where(c => c.Name.Contains(sort))
                .Select(c => new DropDownViewModel{Value = c.Id.ToString(), Text = c.Name})
                .Take(model.Take)
                .ToList();

        } 

        public class DemoGrid
        {
            public string CampusName { get; set; }
            public string Subject { get; set; }
            public string Demographic { get; set; }
            public string Category { get; set; }
            public decimal Value { get; set; }
        }

        public class RequestModel
        {
            public List<long> CategoryDetailIds { get; set; }
            public List<long> DemographicDetailIds { get; set; }
            public List<long> CampusIds { get; set; }
            public List<long> SubjectIds { get; set; }
        }
    }
}