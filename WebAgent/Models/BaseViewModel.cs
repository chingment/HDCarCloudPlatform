using Lumos.DAL;
using Lumos.Entity;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models
{
    public class BaseViewModel
    {
        private LumosDbContext _currentDb;

        [JsonIgnore]
        public LumosDbContext CurrentDb
        {
            get
            {
                return _currentDb;
            }
        }

        public BaseViewModel()
        {
            _currentDb = new LumosDbContext();
        }

        public Enumeration.OperateType Operate { get; set; }

        public int Operater
        {
            get
            {
                return HttpContext.Current.User.Identity.GetUserId<int>();
            }
        }

        public bool IsHasOperater { get; set; }

        public string OperaterName { get; set; }
    }
}