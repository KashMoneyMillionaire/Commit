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

            var x = _ctx.StaarTests.Where(s =>
                model.CampusIds.Contains(s.Campus_Id)
                && model.SubjectIds.Contains(s.Subject_Id)
                && model.CategoryDetailIds.Contains(s.CategoryDetail_Id)
                && model.DemographicDetailIds.Contains(s.DemographicDetail_Id));

            return x.Select(s => new DemoGrid
            {
                Value = s.Value,
                Subject = s.Subject.Description,
                Demographic = s.DemographicDetail.Demographic.Name + ": " + s.DemographicDetail.Description,
                Category = s.CategoryDetail.Category.Name + ": " + s.CategoryDetail.Description,
                CampusName = s.Campus.Name
            })
                .Take(500) //take at most 500 at a time
                .ToList();

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
            public decimal Value { get; set; }
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