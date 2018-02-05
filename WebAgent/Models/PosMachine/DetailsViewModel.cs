using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgent.Models.PosMachine
{
    public class DetailsViewModel:BaseViewModel
    {
        private Lumos.Entity.PosMachine _posMachine = new Lumos.Entity.PosMachine();


        public DetailsViewModel()
        {

        }

        public DetailsViewModel(int id)
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