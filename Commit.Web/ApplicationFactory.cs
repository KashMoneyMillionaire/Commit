using System.Web;
using Infrastructure.Data;

namespace Commit.Web
{
    public static class ApplicationFactory
    {
        private static AzureDataContext _ctx;
        private static SessionFacade _sessionFacade;

        public static AzureDataContext RetrieveContext()
        {
            return _ctx ?? (_ctx = new AzureDataContext());
        }

        public static SessionFacade SessionFacace()
        {
            return _sessionFacade ?? (_sessionFacade = new SessionFacade());
        }

    }
}
