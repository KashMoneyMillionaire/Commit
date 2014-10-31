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
        public List<DemoGrid> GetTests(long categoryDetailId, long demographicDetailId, long campusId, long subjectId)
        {
            return _ctx.StaarTests.Where(s => s.CategoryDetail_Id == categoryDetailId
                                       && s.DemographicDetail_Id == demographicDetailId
                                       && s.Campus_Id == campusId
                                       && s.Subject_Id == subjectId)
                .Select(s => new DemoGrid
                {
                    Value = s.Value,
                    Subject = s.Subject.Name,
                    Demographic = s.DemographicDetail.Description,
                    Category = s.CategoryDetail.Description,
                    CampusName = s.Campus.Name
                }).ToList();
        }


        public class DemoGrid
        {
            public string CampusName { get; set; }
            public string Subject { get; set; }
            public string Demographic { get; set; }
            public string Category { get; set; }
            public decimal Value { get; set; }
        }
    }
}