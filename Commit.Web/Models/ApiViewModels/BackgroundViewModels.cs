using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commit.Web.Models.ApiViewModels
{
    public class DemographicViewModel
    {
        public long DemographicDetailId { get; set; }
        public string Detail { get; set; }
        public string Description { get; set; }
        public int YearStarted { get; set; }
    }
}