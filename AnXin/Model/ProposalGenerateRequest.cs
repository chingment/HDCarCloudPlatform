using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnXinSdk
{
    public class ProposalGenerateRequest
    {

        public RequestHead RequestHead { set; get; }
        public ProposalGenerateRequestMain ProposalGenerateRequestMain { set; get; }

        public ProposalGenerateRequest()
        {
            DateTime today = DateTime.Now;
            RequestHead = new RequestHead();
            ProposalGenerateRequestMain = new ProposalGenerateRequestMain();

            RequestHead.tradeTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            RequestHead.request_Type = "01";

            ProposalGenerateRequestMain.Channel.channelCode = "AEC16110001";
            ProposalGenerateRequestMain.Channel.channelTradeCode = "000003";
            ProposalGenerateRequestMain.Channel.channelRelationNo = RequestHead.tradeTime;
            ProposalGenerateRequestMain.Channel.channelTradeDate = RequestHead.tradeTime.Substring(0, 8);

            ProposalGenerateRequestMain.ApplicantVO.cCertfCls = "120001";//证件类型
            ProposalGenerateRequestMain.ApplicantVO.cZipCde = "000000";//邮编
            ProposalGenerateRequestMain.ApplicantVO.cWorkDpt = "6";//工作单位,单位性质
            ProposalGenerateRequestMain.ApplicantVO.cCustRiskRank = "925105";//反洗钱客户风险等级 
            ProposalGenerateRequestMain.ApplicantVO.cAppCate = "1";//投保人类型 

            ProposalGenerateRequestMain.InsuredVO.cCertfCls = "120001";
            ProposalGenerateRequestMain.InsuredVO.cZipCde = "000000";//邮编
            ProposalGenerateRequestMain.InsuredVO.cWorkDpt = "6";//工作单位,单位性质
            ProposalGenerateRequestMain.InsuredVO.cCustRiskRank = "925105";//反洗钱客户风险等级 
            ProposalGenerateRequestMain.InsuredVO.cResvTxt20 = "1";//被保人类型 

            ProposalGenerateRequestMain.VhlownerVO.cCertfCls = "120001";
            ProposalGenerateRequestMain.VhlownerVO.cZipCde = "000000";//邮编
            ProposalGenerateRequestMain.VhlownerVO.cOwnerCls = "1";//车主性质 

            ProposalGenerateRequestMain.PackageSYVO.cProdNo = "030001";//产品代码
            ProposalGenerateRequestMain.PackageSYVO.cPlyTyp = "0";//保单形式
        }

    }
}
