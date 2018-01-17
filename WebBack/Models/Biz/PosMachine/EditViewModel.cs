using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.PosMachine
{
    public class EditViewModel:BaseViewModel
    {
        private Lumos.Entity.PosMachine _posMachine = new Lumos.Entity.PosMachine();


        public EditViewModel()
        {

        }

        public EditViewModel(int id)
        {
            var posMachine = CurrentDb.PosMachine.Where(m => m.Id == id).FirstOrDefault();
            if(posMachine!=null)
            {
                _posMachine = posMachine;
            }
        }

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