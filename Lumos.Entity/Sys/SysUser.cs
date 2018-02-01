using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    //用户信息表AspNetUsers
    //通过一个类的继承来扩展IdentityUser的属性对应的表是AspNetUsers表
    //在这里测试 添加了UserType,Age属性
    [Table("SysUser")]
    public class SysUser : IdentityUser<int, SysUserLoginProvider, SysUserRole, SysUserClaim>
    {
        public SysUser() { }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SysUser, int> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
        public SysUser(string name) : this() { UserName = name; }

        /// <summary>
        /// 用户帐号
        /// </summary>
        [MaxLength(128)]
        public override string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(128)]
        public string FullName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [MaxLength(68)]
        [Required]
        public override string PasswordHash { get; set; }

        /// <summary>
        ///  安全钥匙
        /// </summary>
        [MaxLength(36)]
        [Required]
        public override string SecurityStamp { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(20)]
        public override string PhoneNumber { get; set; }


        /// <summary>
        /// 是否修改过密码
        /// </summary>
        public bool IsModifyDefaultPwd { get; set; }

        /// <summary>
        /// 用户头像图片
        /// </summary>
        [MaxLength(256)]
        public string HeadImg { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [MaxLength(50)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        
        public Enumeration.UserStatus Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int? Mender { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

    }
}
