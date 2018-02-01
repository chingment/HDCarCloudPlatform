using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.DAL.AuthorizeRelay;

namespace System
{

    /// <summary>
    /// 权限代码
    /// </summary>
    public class PermissionCode
    {
        public const string 所有用户管理 = "Sys1000";
        public const string 后台用户管理 = "Sys2000";
        public const string 角色管理 = "Sys4000";
        public const string 菜单管理 = "Sys5000";

        public const string 报表之商户账号未激活 = "Biz1001";
        public const string 报表之商户提现 = "Biz1002";
        public const string 报表之商户信息登记 = "Biz1003";
        public const string 报表之商户车险订单 = "Biz1004";

        public const string 订单查询 = "Biz2001";

        public const string 佣金调整申请 = "Biz3001";
        public const string 佣金调整初审 = "Biz3002";
        public const string 佣金调整复审 = "Biz3003";

        public const string POS机登记信息 = "Biz4003";
        public const string POS机更换 = "Biz4004";

        public const string 商户资料维护 = "Biz5001";
        public const string 商户资料初审 = "Biz5002";
        public const string 商户资料复审 = "Biz5003";

        public const string 车险订单报价 = "Biz6001";
        public const string 订单支付确认 = "Biz6002";

        public const string 理赔需求核实 = "Biz7001";
        public const string 理赔金额核实 = "Biz7002";

        public const string 提现截单 = "Biz8001";
        public const string 提现处理 = "Biz8002";

        public const string 扩展应用申请 = "Biz9001";
        public const string 扩展应用初审 = "Biz9002";
        public const string 扩展应用复审 = "Biz9003";

        public const string 人才需求核实 = "Biz10001";
        public const string 定损点申请 = "Biz20001";

        public const string 保险公司设置 = "BizA001";
        public const string 车险保险公司设置 = "BizA002";

        public const string 业务人员设置 = "BizB001";
        public const string 业务人员申领POS机登记 = "BizB002";

        public const string 商品设置 = "BizC001";

        public const string 广告设置 = "BizD001";
    }

}
