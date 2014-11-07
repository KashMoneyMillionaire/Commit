using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Commit.Web.Models.ApiViewModels;
using Infrastructure.Data;

namespace Commit.Web.Controllers.Api
{
    public class DemographicsController : ApiController
    {
        private readonly AzureDataContext _ctx = new AzureDataContext();

        [HttpGet]
        public IEnumerable<DemographicViewModel> Read()
        {
            return _ctx.DemographicDetails.Select(d => new DemographicViewModel
            {
                DemographicDetailId = d.Id,
                Description = d.Description,
                Detail = d.Detail,
                YearStarted = d.YearStarted
            });
        }

        [HttpPost]
        public void Update(List<DemographicViewModel> models)
        {
        }

        [HttpPost]
        public void Create(int id, string value)
        {
        }

        [HttpPost]
        public void Delete(int id)
        {
        }
    }
}
