using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Commit.Web.Models
{
    public class DropDownViewModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class GridPageModel
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public List<GridSortModel> Sort { get; set; }
        public GridFilterModel Filter { get; set; }
    }

    public class GridFilterModel
    {
        public string Logic { get; set; }

        public string Field { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }

        public List<GridFilterModel> Filters { get; set; }
    }

    public class GridSortModel
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
}