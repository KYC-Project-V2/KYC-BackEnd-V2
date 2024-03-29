using Model;

using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PendingRequestService : BaseService<PendingRequest>
    {
        private readonly IService<RequestOrigin> _requestOriginService;
        private readonly IService<PersonalInfo> _personalInfoService;
        private readonly IService<BusinessInfo> _bussinessinfoService;
        private readonly IService<OTPVerification> _otpVerificationService;
        public PendingRequestService(IRepository<PendingRequest> repository, IService<RequestOrigin> requestOriginService, IService<BusinessInfo> bussinessinfoService, IService<PersonalInfo> personalInfoService, IService<OTPVerification> otpVerificationService)
        {
            Repository = repository;
            _requestOriginService = requestOriginService;
            _personalInfoService = personalInfoService;
            _bussinessinfoService = bussinessinfoService;
            _otpVerificationService = otpVerificationService;
        }
        public override async Task<PendingRequest> Get(string code)
        {
            var response = await Repository.Get(code);
            return response;
        }
        public override async Task<PendingRequest> Post(PendingRequest pendingRequest)
        {
            RequestOrigin requestOrigin = await _requestOriginService.Get(pendingRequest.RequestNo);
            if (requestOrigin != null)
            {
                pendingRequest.RequestTypeId = requestOrigin.RequestTypeId;
                if (requestOrigin.RequestTypeId == 1)
                {
                    PersonalInfo personalInfo = await _personalInfoService.Get(pendingRequest.RequestNo);
                    if (personalInfo != null && personalInfo.FirstName == pendingRequest.FirstName && personalInfo.LastName == pendingRequest.LastName && personalInfo.Email == pendingRequest.Email)
                    {
                        OTPVerification oTPVerification = new OTPVerification();
                        oTPVerification.RequestNumber = pendingRequest.RequestNo;
                        oTPVerification.Email = pendingRequest.Email;
                        oTPVerification.Type = requestOrigin.RequestTypeId.ToString();
                        oTPVerification.Name = pendingRequest.FirstName;
                        var emailres = _otpVerificationService.Put(oTPVerification);
                        pendingRequest.StatusCode = true;
                    }
                    else
                    {
                        pendingRequest.StatusCode = false;
                    }
                    return pendingRequest;
                }
                if (requestOrigin.RequestTypeId == 2)
                {
                    BusinessInfo businessInfo = await _bussinessinfoService.Get(pendingRequest.RequestNo);
                    if (businessInfo != null && businessInfo.ContactFirstName == pendingRequest.FirstName && businessInfo.ContactLastName == pendingRequest.LastName && businessInfo.ContactEmail == pendingRequest.Email)
                    {
                        OTPVerification oTPVerification = new OTPVerification();
                        oTPVerification.RequestNumber = pendingRequest.RequestNo;
                        oTPVerification.Email = pendingRequest.Email;
                        oTPVerification.Type = "1";
                        oTPVerification.Name = pendingRequest.FirstName;
                        var emailres = _otpVerificationService.Put(oTPVerification);
                        pendingRequest.StatusCode = true;
                    }
                    else
                    {
                        pendingRequest.StatusCode = false;
                    }
                    return pendingRequest;
                }
            }
            else
            {
                pendingRequest.StatusCode = false;
            }
            return pendingRequest;
        }


    }
}

