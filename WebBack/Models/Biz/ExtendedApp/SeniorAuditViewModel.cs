using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBack.Models.Biz.ExtendedApp
{
    public class SeniorAuditViewModel : BaseViewModel
    {
        private Lumos.Entity.ExtendedApp _extendedApp = new Lumos.Entity.ExtendedApp();
        private Lumos.Entity.BizProcessesAudit _bizProcessesAudit = new Lumos.Entity.BizProcessesAudit();
        private Lumos.Entity.BizProcessesAuditDetails _auditCommentsCurrent = new Lumos.Entity.BizProcessesAuditDetails();
        private List<Lumos.Entity.BizProcessesAuditDetails> _auditCommentsHistory = new List<Lumos.Entity.BizProcessesAuditDetails>();

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

        public Lumos.Entity.BizProcessesAuditDetails AuditCommentsCurrent
        {
            get
            {
                return _auditCommentsCurrent;
            }
            set
            {
                _auditCommentsCurrent = value;
            }
        }

        public List<Lumos.Entity.BizProcessesAuditDetails> AuditCommentsHistory
        {
            get
            {
                return _auditCommentsHistory;
            }
            set
            {
                _auditCommentsHistory = value;
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

        public SeniorAuditViewModel()
        {

        }

        public SeniorAuditViewModel(int id)
        {
            var bizProcessesAudit = BizFactory.BizProcessesAudit.ChangeExtendedAppAuditStatus(this.Operater, id, Enumeration.ExtendedAppAuditStatus.InReview);
            if (bizProcessesAudit != null)
            {
                _bizProcessesAudit = bizProcessesAudit;

                var extendedApp = CurrentDb.ExtendedApp.Where(m => m.Id == bizProcessesAudit.AduitReferenceId).FirstOrDefault();
                if (extendedApp != null)
                {
                    _extendedApp = extendedApp;
                }
            }


            var auditCommentsHistory = CurrentDb.BizProcessesAuditDetails.Where(m => m.BizProcessesAuditId == id).ToList();
            if (auditCommentsHistory != null)
            {
                _auditCommentsHistory = auditCommentsHistory.Where(m => m.AuditTime != null && m.Auditor != null).ToList();

                var auditCommentsCurrent = auditCommentsHistory.Where(m => m.BizProcessesAuditId == id && m.AuditStep == (int)Enumeration.ExtendedAppAuditStep.SeniorAudit).OrderByDescending(m => m.CreateTime).Take(1).FirstOrDefault();

                if (auditCommentsCurrent != null)
                {
                    _auditCommentsCurrent = auditCommentsCurrent;

                }
            }


        }
    }
}