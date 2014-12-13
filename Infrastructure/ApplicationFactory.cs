using System;
using Infrastructure.Data;
using Infrastructure.Services;

namespace Infrastructure
{
    public static class ApplicationFactory
    {
        private static AzureDataContext _ctx;
        private static BackgroundService _backgroundSvc;
        private static UnpivotorService _unpivotorSvc;

        public static AzureDataContext RetrieveContext()
        {
            return _ctx ?? (_ctx = new AzureDataContext());
        }

        public static IService RetrieveService<T>() where T : IService
        {
            if (typeof(T) == typeof(BackgroundService))
            {
                return _backgroundSvc ?? (_backgroundSvc = new BackgroundService());
            }

            if (typeof(T) == typeof(UnpivotorService))
            {
                return _unpivotorSvc ?? (_unpivotorSvc = new UnpivotorService());
            }

            throw new ArgumentNullException("No service found");
        }
    }
}
