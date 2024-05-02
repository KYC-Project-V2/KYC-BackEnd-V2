using Model;
using Repository;
using System.Text;

namespace Service
{
    public class DashboardService : BaseService<Dashboard>
    {
        public DashboardService(IRepository<Dashboard> repository)
        {
            Repository = repository;
        }
        public override async Task<Dashboard> GetDashboardData()
        {
            var response = await Repository.GetDashboardData();
            if (response == null)
            {
                var dashboard = new Dashboard();
                dashboard.ErrorMessage = "No Dashboard Data Found";
                return dashboard;
            }
            return response;
        }
    }
}
