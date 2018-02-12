using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{

    public class LumosDbContext : AuthorizeRelayDbContext
    {
        public IDbSet<ExtendedApp> ExtendedApp { get; set; }

        public IDbSet<PosMachine> PosMachine { get; set; }

        public IDbSet<Merchant> Merchant { get; set; }

        public IDbSet<MerchantEstimateCompany> MerchantEstimateCompany { get; set; }

        public IDbSet<MerchantPosMachine> MerchantPosMachine { get; set; }

        public IDbSet<MerchantPosMachineChangeHistory> MerchantPosMachineChangeHistory { get; set; }

        public IDbSet<Bank> Bank { get; set; }

        public IDbSet<BankCard> BankCard { get; set; }

        public IDbSet<CarInsurePlan> CarInsurePlan { get; set; }

        public IDbSet<CarInsurePlanKind> CarInsurePlanKind { get; set; }

        public IDbSet<CarKind> CarKind { get; set; }

        public IDbSet<InsuranceCompany> InsuranceCompany { get; set; }

        public IDbSet<CarInsuranceCompany> CarInsuranceCompany { get; set; }

        public IDbSet<Product> Product { get; set; }

        public IDbSet<Order> Order { get; set; }

        public IDbSet<OrderPayResultNotifyLog> OrderPayResultNotifyLog { get; set; }

        public IDbSet<OrderToCarInsure> OrderToCarInsure { get; set; }

        public IDbSet<OrderToCarInsureOfferCompany> OrderToCarInsureOfferCompany { get; set; }

        public IDbSet<OrderToCarInsureOfferKind> OrderToCarInsureOfferKind { get; set; }

        public IDbSet<OrderToCarClaim> OrderToCarClaim { get; set; }

        public IDbSet<OrderToTalentDemand> OrderToTalentDemand { get; set; }

        public IDbSet<OrderToApplyLossAssess> OrderToApplyLossAssess { get; set; }

        public IDbSet<OrderToServiceFee> OrderToServiceFee { get; set; }

        public IDbSet<BizProcessesAudit> BizProcessesAudit { get; set; }

        public IDbSet<BizProcessesAuditDetails> BizProcessesAuditDetails { get; set; }

        public IDbSet<Fund> Fund { get; set; }

        public IDbSet<Transactions> Transactions { get; set; }

        public IDbSet<SalesmanApplyPosRecord> SalesmanApplyPosRecord { get; set; }


        public IDbSet<OrderPayResultNotifyByMinShunLog> OrderPayResultNotifyByMinShunLog { get; set; }

        public IDbSet<CarInsuranceExpireDate> CarInsuranceExpireDate { get; set; }


        public IDbSet<CarInsuranceExpireDateSearchHis> CarInsuranceExpireDateSearchHis { get; set; }

        public IDbSet<OrderPayResultNotifyByStaffLog> OrderPayResultNotifyByStaffLog { get; set; }


        public IDbSet<SysItemCacheUpdateTime>  SysItemCacheUpdateTime { get; set; }

        //public FxDbContext()
        //    : base("DefaultConnection")
        //{
        //   // this.Configuration.ProxyCreationEnabled = false;
        //}


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }


    public class FxContextDatabaseInitializerForCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<LumosDbContext>
    {
        protected override void Seed(LumosDbContext context)
        {
            int userIdAutoNum = 100;

            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT ('SysUser', RESEED," + userIdAutoNum + " )");

            //初始用户
            context.SysStaffUser.Add(new SysStaffUser() { Id = userIdAutoNum, UserName = "admin", SecurityStamp = "61c7b4a2-4197-4d32-b9a5-629425fc2000", FullName = "李珊", PasswordHash = "AD5hJcUUIJ4NxikOI2O1ChwVgoGYwPXDxGHp+nSIX8GHEeQw5h0C9mECSFyXeo/kCw==", IsDelete = false, Status = Enumeration.UserStatus.Normal, Creator = 0, CreateTime = DateTime.Now, RegisterTime = DateTime.Now });

            //初始角色
            //context.Roles.Add(new SysRole() { Id = 1, Name = "角色", PId = 0, Description = "" });
            //context.Roles.Add(new SysRole() { Id = 2, Name = "角色1", PId = 1, Description = "" });
            //context.Roles.Add(new SysRole() { Id = 3, Name = "角色2", PId = 1, Description = "" });
            //context.Roles.Add(new SysRole() { Id = 4, Name = "角色3", PId = 2, Description = "" });
            //context.Roles.Add(new SysRole() { Id = 5, Name = "角色4", PId = 2, Description = "" });
            //context.Roles.Add(new SysRole() { Id = 6, Name = "角色5", PId = 6, Description = "" });
            //context.Roles.Add(new SysRole() { Id = 7, Name = "角色6", PId = 1, Description = "" });
            //初始角色用户
            //context.SysUserRole.Add(new SysUserRole() { UserId = 1, RoleId = userIdAutoNum });

            List<SysMenu> sysMenus = new List<SysMenu>();
            //初始菜单
            sysMenus.Add(new SysMenu() { Id = 1, Name = "后台管理菜单", PId = 0, Url = "#", Description = "", Priority = 0 });
            sysMenus.Add(new SysMenu() { Id = 2, Name = "系统管理", PId = 1, Url = "#", Description = "", Priority = 0 });
            sysMenus.Add(new SysMenu() { Id = 3, Name = "菜单设置", PId = 2, Url = "/Manager/Sys/Menu/Index", Description = "", Priority = 6 });
            sysMenus.Add(new SysMenu() { Id = 4, Name = "角色设置", PId = 2, Url = "/Manager/Sys/Role/Index", Description = "", Priority = 5 });
            sysMenus.Add(new SysMenu() { Id = 5, Name = "后台用户", PId = 2, Url = "/Manager/Sys/StaffUser/Index", Description = "", Priority = 4 });
            sysMenus.Add(new SysMenu() { Id = 6, Name = "所有用户", PId = 2, Url = "/Manager/Sys/User/Index", Description = "", Priority = 4 });
            sysMenus.Add(new SysMenu() { Id = 7, Name = "日志查看", PId = 2, Url = "/Manager/Sys/LogView/Index", Description = "", Priority = 3 });
            //模块 可去掉
            sysMenus.Add(new SysMenu() { Id = 8, Name = "Banner", PId = 2, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            sysMenus.Add(new SysMenu() { Id = 9, Name = "测试评语模板", PId = 2, Url = "/Manager/Mod/CommentTemplate/Index", Description = "", Priority = 3 });



            //业务 可去掉
            //sysMenus.Add(new SysMenu() { Id = 9, Name = "机构管理", PId = 1, Url = "#", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 10, Name = "机构列表", PId = 9, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 11, Name = "机构审核", PId = 9, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 12, Name = "教师管理", PId = 1, Url = "#", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 13, Name = "教师列表", PId = 12, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 14, Name = "家长管理", PId = 1, Url = "#", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 15, Name = "家长列表", PId = 14, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 16, Name = "学生管理", PId = 1, Url = "#", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 17, Name = "学生列表", PId = 16, Url = "/Manager/Biz/Student/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 18, Name = "出勤管理", PId = 16, Url = "/Manager/Biz/StudentAttendance/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 19, Name = "作业管理", PId = 16, Url = "/Manager/Biz/StudentHomework/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 20, Name = "测验管理", PId = 16, Url = "/Manager/Biz/StudentPhysiclalStatus/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 21, Name = "生活管理", PId = 16, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 22, Name = "动态分享管理", PId = 1, Url = "#", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 23, Name = "分享管理", PId = 22, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 24, Name = "家长分享审核", PId = 22, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 25, Name = "老师分享审核", PId = 22, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 26, Name = "家长分享审核", PId = 22, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });
            //sysMenus.Add(new SysMenu() { Id = 27, Name = "校长分享审核", PId = 22, Url = "/Manager/Mod/Banner/Index", Description = "", Priority = 3 });



            foreach (var m in sysMenus)
            {
                context.SysMenu.Add(m);
            }

            //初始角色菜单
            foreach (var m in sysMenus)
            {
                context.SysRoleMenu.Add(new SysRoleMenu() { MenuId = m.Id, RoleId = 1 });
            }

            //初始权限
            List<SysPermission> sysPermissions = GetPermissionList(new PermissionCode());
            foreach (var m in sysPermissions)
            {
                context.SysPermission.Add(new SysPermission() { Id = m.Id, PId = m.PId, Name = m.Name });
            }


            context.SysMenuPermission.Add(new SysMenuPermission() { MenuId = 3, PermissionId = PermissionCode.菜单管理 });
            context.SysMenuPermission.Add(new SysMenuPermission() { MenuId = 4, PermissionId = PermissionCode.角色管理 });
            context.SysMenuPermission.Add(new SysMenuPermission() { MenuId = 5, PermissionId = PermissionCode.后台用户管理 });
            context.SysMenuPermission.Add(new SysMenuPermission() { MenuId = 6, PermissionId = PermissionCode.所有用户管理 });
 


            //context.Student.Add(new Student() { Name = "邱庆文", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "宋润强", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "练锦华", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "张学友", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "黄家强", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "钟彭宇", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "余世超", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.Student.Add(new Student() { Name = "洪山", UserId = 1, Sex = Enumeration.Sex.Male, AdmissionDate = DateTime.Now, GraduateDate = DateTime.Now, IsGraduate = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });

            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 1, UserId = 1, Status = Enumeration.AttendanceStatus.Sign, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 2, UserId = 1, Status = Enumeration.AttendanceStatus.Late, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 3, UserId = 1, Status = Enumeration.AttendanceStatus.Late, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 4, UserId = 1, Status = Enumeration.AttendanceStatus.Sign, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 5, UserId = 1, Status = Enumeration.AttendanceStatus.Late, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 6, UserId = 1, Status = Enumeration.AttendanceStatus.Sign, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 7, UserId = 1, Status = Enumeration.AttendanceStatus.Late, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentAttendance.Add(new StudentAttendance() { StudentId = 8, UserId = 1, Status = Enumeration.AttendanceStatus.Late, AttendanceDate = DateTime.Now, Description = "", CreateTime = DateTime.Now, Creator = 0 });

            //context.StudentHomework.Add(new StudentHomework() { StudentId = 1, UserId = 1, CompleteDate = DateTime.Now, Title = "写123", Content = "dasddadda", IsComplete = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentHomework.Add(new StudentHomework() { StudentId = 2, UserId = 1, CompleteDate = DateTime.Now, Title = "听力", Content = "dasddadda", IsComplete = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentHomework.Add(new StudentHomework() { StudentId = 3, UserId = 1, CompleteDate = DateTime.Now, Title = "练字体", Content = "dasddadda", IsComplete = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });
            //context.StudentHomework.Add(new StudentHomework() { StudentId = 3, UserId = 1, CompleteDate = DateTime.Now, Title = "书法", Content = "dasddadda", IsComplete = Enumeration.YesOrNo.Yes, CreateTime = DateTime.Now, Creator = 0 });


            //context.StudentPhysiclalStatus.Add(new StudentPhysiclalStatus() { StudentId = 1, UserId = 1, CheckeTime = DateTime.Now, Description = "今日发烧", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentPhysiclalStatus.Add(new StudentPhysiclalStatus() { StudentId = 2, UserId = 1, CheckeTime = DateTime.Now, Description = "感冒", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentPhysiclalStatus.Add(new StudentPhysiclalStatus() { StudentId = 3, UserId = 1, CheckeTime = DateTime.Now, Description = "打咳嗽", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentPhysiclalStatus.Add(new StudentPhysiclalStatus() { StudentId = 4, UserId = 1, CheckeTime = DateTime.Now, Description = "肚子疼", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentPhysiclalStatus.Add(new StudentPhysiclalStatus() { StudentId = 5, UserId = 1, CheckeTime = DateTime.Now, Description = "拉斯", Creator = 0, CreateTime = DateTime.Now });


            //context.StudentTests.Add(new StudentTests() { StudentId = 1, UserId = 1, Subject = "语文", Score = 90, Comment = "不错", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentTests.Add(new StudentTests() { StudentId = 2, UserId = 1, Subject = "数学", Score = 80, Comment = "不错", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentTests.Add(new StudentTests() { StudentId = 3, UserId = 1, Subject = "英语", Score = 88, Comment = "不错", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentTests.Add(new StudentTests() { StudentId = 6, UserId = 1, Subject = "活动", Score = 66, Comment = "不错", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentTests.Add(new StudentTests() { StudentId = 7, UserId = 1, Subject = "听力", Score = 88, Comment = "不错", Creator = 0, CreateTime = DateTime.Now });
            //context.StudentTests.Add(new StudentTests() { StudentId = 4, UserId = 1, Subject = "练字", Score = 33, Comment = "不错", Creator = 0, CreateTime = DateTime.Now });


            base.Seed(context);
        }

        public List<SysPermission> GetPermissionList(PermissionCode permission)
        {
            Type t = permission.GetType();
            List<SysPermission> list = new List<SysPermission>();
            list = GetPermissionList(t, list);
            return list;
        }


        private List<SysPermission> GetPermissionList(Type t, List<SysPermission> list)
        {
            if (t.Name != "Object")
            {
                System.Reflection.FieldInfo[] properties = t.GetFields();
                foreach (System.Reflection.FieldInfo property in properties)
                {
                    string pId = "0";
                    object[] typeAttributes = property.GetCustomAttributes(false);
                    foreach (PermissionCodeAttribute attribute in typeAttributes)
                    {
                        pId = attribute.PId;
                    }
                    object id = property.GetValue(null);
                    string name = property.Name;
                    SysPermission model = new SysPermission();
                    model.Id = id.ToString();
                    model.Name = name;
                    model.PId = pId;
                    list.Add(model);
                }
                list = GetPermissionList(t.BaseType, list);
            }
            return list;
        }

    }

}
