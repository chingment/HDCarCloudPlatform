using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.PosMachine
{
    public class AddViewModel:BaseViewModel
    {
        private Lumos.Entity.PosMachine _posMachine=new Lumos.Entity.PosMachine();

        public Lumos.Entity.PosMachine PosMachine
        {
            get
            {
                return _posMachine;
            }
            set
            {
                _posMachine = value;
            }
        }
    }
}