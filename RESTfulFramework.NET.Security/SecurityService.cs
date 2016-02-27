using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Security
{
    public class SecurityService : ISecurity<RequestModel>
    {
        public bool SecurityCheck(RequestModel requestModel)
        {
            var dataCheck = new DataCheck();
            if (!dataCheck.SecurityCheck(requestModel)) return false;

            var userSecurity = new UserSecurity(); 
            if (!userSecurity.SecurityCheck(requestModel)) return false;

            var dataSecurity = new DataSecurity();
            if (!dataSecurity.SecurityCheck(requestModel)) return false;

            return true;
        }
    }
}
