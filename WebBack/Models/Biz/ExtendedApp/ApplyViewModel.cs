using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ExtendedApp
{
    public class ApplyViewModel:BaseViewModel
    {
        private Lumos.Entity.ExtendedApp _extendedApp = new Lumos.Entity.ExtendedApp();

        public Lumos.Entity.ExtendedApp ExtendedApp
        {
            get
            {
                return _extendedApp;
            }
            set
            {
                _extendedApp = value;
            }
        }

    }
}