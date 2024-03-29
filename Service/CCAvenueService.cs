using Microsoft.Extensions.Configuration;
using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Service
{
    public class CCAvenueService : BaseService<CCAvenue>
    {
        private readonly IService<PaymentConfiguration> _paymentCongurationService;
        private readonly IConfiguration _configuration;
        public CCAvenueService(IService<PaymentConfiguration> paymentCongurationService,
            IConfiguration configuration)
        {
            _paymentCongurationService = paymentCongurationService;
            _configuration = configuration;
        }
        public override async Task<CCAvenue> Post(CCAvenue cCAvenue)
        {
            var paymentReturnUrl = _configuration.GetValue<string>("PaymentReturnUrl");
            PaymentConfiguration paymentConfiguration = await _paymentCongurationService.Get(true);
            //TODO: Change after Configuration fixed
            paymentConfiguration.PaymentReturnUrl = paymentReturnUrl;
            var response = await KYCUtility.CCAEncrypt(cCAvenue, paymentConfiguration);    
            return response;
        }
    }
}
