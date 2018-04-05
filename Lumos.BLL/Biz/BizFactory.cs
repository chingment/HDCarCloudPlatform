using Lumos.BLL.Biz.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class BizFactory: BaseFactory
    {
        public static PosMachineProvider PosMachine
        {
            get
            {
                return new PosMachineProvider();
            }
        }

        public static MerchantProvider Merchant
        {
            get
            {
                return new MerchantProvider();
            }
        }

        public static BizProcessesAuditProvider BizProcessesAudit
        {
            get
            {
                return new BizProcessesAuditProvider();
            }
        }

        public static OrderProvider Order
        {
            get
            {
                return new OrderProvider();
            }
        }

        public static OrderToTalentDemandProvider OrderToTalentDemand
        {
            get
            {
                return new OrderToTalentDemandProvider();
            }
        }

        public static OrderToApplyLossAssessProvider OrderToApplyLossAssess
        {
            get
            {
                return new OrderToApplyLossAssessProvider();
            }
        }

        public static ExtendedAppProvder ExtendedApp
        {
            get
            {
                return new ExtendedAppProvder();
            }
        }

        public static ProductProvider Product
        {
            get
            {
                return new ProductProvider();
            }
        }

        public static InsuranceCompanyProvider InsuranceCompany
        {
            get
            {
                return new InsuranceCompanyProvider();
            }
        }
        public static CarInsuranceCompanyProvider CarInsuranceCompany
        {
            get
            {
                return new CarInsuranceCompanyProvider();
            }
        }


        public static PayProvider Pay
        {
            get
            {
                return new PayProvider();
            }
        }

        public static Launcher Launcher
        {
            get
            {
                return new Launcher();
            }
        }

        public static ApplyPosProvider ApplyPos
        {
            get
            {
                return new ApplyPosProvider();
            }
        }


        public static CarInsurePlanProvider CarInsurePlan
        {
            get
            {
                return new CarInsurePlanProvider();
            }
        }

        public static OrderToCreditProvider OrderToCredit
        {
            get
            {
                return new OrderToCreditProvider();
            }
        }

        public static OrderToInsuranceProvider OrderToInsurance
        {
            get
            {
                return new OrderToInsuranceProvider();
            }
        }

        public static OrderToLllegalDealtProvider OrderToLllegalDealt
        {
            get
            {
                return new OrderToLllegalDealtProvider();
            }
        }

    }
}
