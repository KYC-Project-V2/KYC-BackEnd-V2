using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DocumentVerificationService : BaseService<DocumentVerification>
    {
        private readonly IService<AadharInfo> _AadharInfoService;
        private readonly IService<VoterInfo> _VoterInfoService;
        private readonly IService<PanCardInfo> _PanCardInfoService;
        private readonly IService<DriverLicenseInfo> _DriverLicenseInfoService;
        private readonly IService<BusinessPanCardInfo> _BusinessPanCardInfoService;
        private readonly IService<BusinessInCorpInfo> _BusinessInCorpInfoService;
        public DocumentVerificationService(
            IRepository<DocumentVerification> repository,
            IService<AadharInfo> AadharInfoService,
            IService<VoterInfo> VoterInfoService,
            IService<PanCardInfo> PanCardInfoService,
            IService<DriverLicenseInfo> DriverLicenseInfoService,
            IService<BusinessPanCardInfo> BusinessPanCardInfoService,
            IService<BusinessInCorpInfo> BusinessInCorpInfoService
            )
        {
            Repository = repository;
            _AadharInfoService = AadharInfoService;
            _VoterInfoService = VoterInfoService;
            _PanCardInfoService = PanCardInfoService;
            _DriverLicenseInfoService = DriverLicenseInfoService;
            _BusinessPanCardInfoService = BusinessPanCardInfoService;
            _BusinessInCorpInfoService = BusinessInCorpInfoService;
        }

        public override async Task<DocumentVerification> Post(DocumentVerification model)
        {
            if(model.RequestType == "1")
            {
                //TODO: verfication API call and logic
                //TODO: face reco API call and logic

                if (model.AadharInfo != null)
                {
                    model.AadharInfo.RequestNo = model.RequestNumber;
                    model.AadharInfo.CreatedDate = DateTime.Now;
                    var aadharResponse = await _AadharInfoService.Post(model.AadharInfo);
                }
                if (model.VoterInfo != null)
                {
                    model.VoterInfo.RequestNo = model.RequestNumber;
                    model.VoterInfo.CreatedDate = DateTime.Now;
                    var voterResponse = await _VoterInfoService.Post(model.VoterInfo);
                }
                if (model.PanCardInfo != null)
                {
                    model.PanCardInfo.RequestNo = model.RequestNumber;
                    model.PanCardInfo.CreatedDate = DateTime.Now;
                    var panResponse = await _PanCardInfoService.Post(model.PanCardInfo);
                }
                if (model.DriverLicenseInfo != null)
                {
                    model.DriverLicenseInfo.RequestNo = model.RequestNumber;
                    model.DriverLicenseInfo.CreatedDate = DateTime.Now;
                    var DriverLicenseInfoResponse = await _DriverLicenseInfoService.Post(model.DriverLicenseInfo);
                }
                if (model.BusinessInCorpInfo != null)
                {
                    model.BusinessInCorpInfo.RequestNo = model.RequestNumber;
                    model.BusinessInCorpInfo.CreatedDate = DateTime.Now;
                    model.BusinessInCorpInfo.CreatedBy = model.BusinessInCorpInfo.Name;
                    var BusinessInCorpInfoResponse = await _BusinessInCorpInfoService.Post(model.BusinessInCorpInfo);
                }
            }
            else if (model.RequestType == "2")
            {
                //TODO: verfication API call and logic
                //TODO: face reco API call and logic

                if (model.BusinessPanCardInfo != null)
                {
                    model.BusinessPanCardInfo.RequestNo = model.RequestNumber;
                    model.BusinessPanCardInfo.CreatedDate = DateTime.Now;
                    model.BusinessPanCardInfo.CreatedBy = model.BusinessPanCardInfo.Name;
                    var BusinessPanCardInfoResponse = await _BusinessPanCardInfoService.Post(model.BusinessPanCardInfo);
                }
                if (model.BusinessInCorpInfo != null)
                {
                    model.BusinessInCorpInfo.RequestNo = model.RequestNumber;
                    model.BusinessInCorpInfo.CreatedDate = DateTime.Now;
                    model.BusinessInCorpInfo.CreatedBy = model.BusinessInCorpInfo.Name;
                    var BusinessInCorpInfoResponse = await _BusinessInCorpInfoService.Post(model.BusinessInCorpInfo);
                }

                if (model.AadharInfo != null)
                {
                    model.AadharInfo.RequestNo = model.RequestNumber;
                    model.AadharInfo.CreatedDate = DateTime.Now;
                    var aadharResponse = await _AadharInfoService.Post(model.AadharInfo);
                }

                if (model.VoterInfo != null)
                {
                    model.VoterInfo.RequestNo = model.RequestNumber;
                    model.VoterInfo.CreatedDate = DateTime.Now;
                    var voterResponse = await _VoterInfoService.Post(model.VoterInfo);
                }

                if (model.DriverLicenseInfo != null)
                {
                    model.DriverLicenseInfo.RequestNo = model.RequestNumber;
                    model.DriverLicenseInfo.CreatedDate = DateTime.Now;
                    var DriverLicenseInfoResponse = await _DriverLicenseInfoService.Post(model.DriverLicenseInfo);
                }
            }
            
            return model;
        }
    }
}
