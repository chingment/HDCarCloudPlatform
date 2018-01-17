using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ExtendedApp
{
    public class ApplyOnViewModel:BaseViewModel
    {
        private Lumos.Entity.ExtendedApp _extendedApp = new Lumos.Entity.ExtendedApp();
        private string _remarks = "";


        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                _remarks = value;
            }
        }


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

        public ApplyOnViewModel()
        {

        }

    }
}