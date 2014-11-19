using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Commit.Web.Models.ApiViewModels;
using Commit.Web.Models.BasicViewModels;
using Commit.Web.Models.WebViewModels;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Domain;

namespace Commit.Web.Controllers.Api
{
    public class BackgroundServiceController : ApiController
    {
        private readonly AzureDataContext _ctx = new AzureDataContext();


        #region Demographic Details

        [System.Web.Http.HttpGet]
        public IEnumerable<DemographicDetailViewModel> ReadDemographicDetails()
        {
            return _ctx.DemographicDetails.Select(d => new DemographicDetailViewModel
            {
                Demographic = new DropDownViewModel
                {
                    Value = d.Demographic.Id,
                    Text = d.Demographic.Name
                },
                DemographicDetailId = d.Id,
                Description = d.Description,
                Detail = d.Detail,
                YearStarted = d.YearStarted,
            });
        }

        [System.Web.Http.HttpPost]
        public List<DemographicDetailViewModel> UpdateDemographicDetails(List<DemographicDetailViewModel> models)
        {
            var modelIds = models.Select(m => m.DemographicDetailId).ToList();
            var details = _ctx.DemographicDetails.Where(d => modelIds.Contains(d.Id)).ToList();
            var dems = _ctx.Demographics.ToList();

            foreach (var model in models)
            {
                var detail = details.Single(d => d.Id == model.DemographicDetailId);
                detail.Description = model.Description;
                detail.Detail = model.Detail;
                detail.YearStarted = model.YearStarted;
                detail.Demographic = dems.Single(d => d.Id == model.Demographic.Value);
            }

            _ctx.SaveChanges();

            return models;
        }

        [System.Web.Http.HttpPost]
        public void CreateDemographicDetails(List<DemographicDetailViewModel> models)
        {
            foreach (var entity in models.Select(ddvm => new DemographicDetail
            {
                Description = ddvm.Description,
                Detail = ddvm.Detail,
                YearStarted = ddvm.YearStarted,
                Demographic = _ctx.Demographics.Find(ddvm.Demographic.Value)
            }))
            {
                _ctx.DemographicDetails.Add(entity);
            }

            _ctx.SaveChanges();
        }

        #endregion

        #region Category Details

        [System.Web.Http.HttpGet]
        public IEnumerable<CategoryDetailViewModel> ReadCategoryDetails()
        {
            return _ctx.CategoryDetails.Select(d => new CategoryDetailViewModel
            {
                Category = new DropDownViewModel
                {
                    Value = d.Category.Id,
                    Text = d.Category.Name
                },
                CategoryType = new DropDownViewModel
                {
                    Value = (long)d.CategoryType,
                    Text = d.CategoryType.ToString()
                },
                CategoryDetailId = d.Id,
                Description = d.Description,
                Detail = d.Detail,
                YearStarted = d.YearStarted,
            });
        }

        [System.Web.Http.HttpPost]
        public List<CategoryDetailViewModel> UpdateCategoryDetails(List<CategoryDetailViewModel> models)
        {
            var modelIds = models.Select(m => m.CategoryDetailId).ToList();
            var details = _ctx.CategoryDetails.Where(d => modelIds.Contains(d.Id)).ToList();
            var cats = _ctx.Categories.ToList();

            foreach (var model in models)
            {
                var detail = details.Single(d => d.Id == model.CategoryDetailId);
                detail.Description = model.Description;
                detail.Detail = model.Detail;
                detail.YearStarted = model.YearStarted;
                detail.Category = cats.Single(d => d.Id == model.Category.Value);
                detail.CategoryType = (CategoryType)model.CategoryType.Value;
            }

            _ctx.SaveChanges();

            return models;
        }

        [System.Web.Http.HttpPost]
        public List<CategoryDetailViewModel> CreateCategoryDetails(List<CategoryDetailViewModel> models)
        {
            foreach (var entity in models.Select(ddvm => new CategoryDetail()
            {
                Description = ddvm.Description,
                Detail = ddvm.Detail,
                YearStarted = ddvm.YearStarted,
                Category = _ctx.Categories.Find(ddvm.Category.Value),
                CategoryType = (CategoryType)ddvm.CategoryType.Value
            }))
            {
                _ctx.CategoryDetails.Add(entity);
            }

            _ctx.SaveChanges();

            return models;
        }

        #endregion

        #region Subjects

        [System.Web.Http.HttpGet]
        public IEnumerable<SubjectViewModel> ReadSubjects()
        {
            return _ctx.Subjects.Select(d => new SubjectViewModel
            {
                Id = d.Id,
                Description = d.Description,
                Name = d.Name,
                YearStarted = d.YearStarted,
            });
        }

        [System.Web.Http.HttpPost]
        public List<SubjectViewModel> UpdateSubjects(List<SubjectViewModel> models)
        {
            var modelIds = models.Select(m => m.Id).ToList();
            var subjects = _ctx.Subjects.Where(d => modelIds.Contains(d.Id)).ToList();

            foreach (var model in models)
            {
                var entity = subjects.Single(d => d.Id == model.Id);
                entity.Description = model.Description;
                entity.Name = model.Name;
                entity.YearStarted = model.YearStarted;
            }

            _ctx.SaveChanges();

            return models;
        }

        [System.Web.Http.HttpPost]
        public List<SubjectViewModel> CreateSubjects(List<SubjectViewModel> models)
        {
            foreach (var entity in models.Select(ddvm => new Subject
            {
                Description = ddvm.Description,
                Name = ddvm.Name,
                YearStarted = ddvm.YearStarted,
            }))
            {
                _ctx.Subjects.Add(entity);
            }

            _ctx.SaveChanges();

            return models;
        }

        #endregion

        #region Languages

        [System.Web.Http.HttpGet]
        public IEnumerable<LanguageViewModel> ReadLanguages()
        {
            return _ctx.Subjects.Select(d => new LanguageViewModel
            {
                Id = d.Id,
                Name = d.Name,
                YearStarted = d.YearStarted,
            });
        }

        [System.Web.Http.HttpPost]
        public List<LanguageViewModel> UpdateLanguages(List<LanguageViewModel> models)
        {
            var modelIds = models.Select(m => m.Id).ToList();
            var subjects = _ctx.Languages.Where(d => modelIds.Contains(d.Id)).ToList();

            foreach (var model in models)
            {
                var entity = subjects.Single(d => d.Id == model.Id);
                entity.Name = model.Name;
                entity.YearStarted = model.YearStarted;
            }

            _ctx.SaveChanges();

            return models;
        }

        [System.Web.Http.HttpPost]
        public List<LanguageViewModel> CreateLanguages(List<LanguageViewModel> models)
        {
            foreach (var entity in models.Select(ddvm => new Language
            {
                Name = ddvm.Name,
                YearStarted = ddvm.YearStarted,
            }))
            {
                _ctx.Languages.Add(entity);
            }

            _ctx.SaveChanges();

            return models;
        }

        #endregion
    }
}
