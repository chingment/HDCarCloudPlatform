using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ExtendedApp
{
    public class PrimaryAuditViewModel : BaseViewModel
    {
        private Lumos.Entity.ExtendedApp _extendedApp = new Lumos.Entity.ExtendedApp();
        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit= new Lumos.Entity.BizProcessesAudit();
        private Lumos.Entity.BizProcessesAuditDetails _bizProcessesAuditDetails = new Lumos.Entity.BizProcessesAuditDetails();
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


        public Lumos.Entity.BizProcessesAudit BizProcessesAudit
        {
            get
            {
                return _bizProcessesAudit;
            }
            set
            {
                _bizProcessesAudit = value;
            }
        }

        public Lumos.Entity.BizProcessesAuditDetails BizProcessesAuditDetails
        {
            get
            {
                return _bizProcessesAuditDetails;
            }
            set
            {
                _bizProcessesAuditDetails = value;
            }
        }

        public PrimaryAuditViewModel()
        {

        }

        public PrimaryAuditViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeExtendedAppAuditStatus(this.Operater, id, Enumeration.ExtendedAppAuditStatus.InAudit);
            if(bizProcessesAudit!=null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                var extendedApp = CurrentDb.ExtendedApp.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                if(extendedApp!=null)
                {
                    _extendedApp = extendedApp;
                }
            }


            var bizProcessesAuditDetails = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == id).ToList();
            if (bizProcessesAuditDetails != null)
            {
                var primaryAuditComments = bizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == id && m.AuditStep == (int)Enumeration.ExtendedAppAuditStep.PrimaryAudit).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                if (primaryAuditComments != null)
                {
                    _bizProcessesAuditDetails = primaryAuditComments;

                    var auditComments = bizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == bizProcessesAudit.Id && m.AuditStep == (int)Enumeration.ExtendedAppAuditStep.PrimaryAudit && m.AuditComments != null).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();
                    if (auditComments != null)
                    {
                        _bizProcessesAuditDetails.AuditComments = auditComments.AuditComments;
                    }
                }
            }


        }

    }
}