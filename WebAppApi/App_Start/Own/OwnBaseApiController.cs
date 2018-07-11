using log4net;
using Lumos.Common;
using Lumos.DAL;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAppApi.Models;
using Lumos.Entity.AppApi;
using Lumos.Entity;
using Lumos;

namespace WebAppApi
{

    public class OwnBaseApiController : BaseApiController
    {

        private APIResult _result = new APIResult();
        private LumosDbContext _currentDb;

        public APIResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public LumosDbContext CurrentDb
        {
            get
            {
                return _currentDb;
            }
        }

        protected void SetTrackID()
        {
            if (LogicalThreadContext.Properties["trackid"] == null)
                LogicalThreadContext.Properties["trackid"] = Guid.NewGuid().ToString("N");
        }

        public OwnBaseApiController()
        {
            SetTrackID();
            _currentDb = new LumosDbContext();
            _result = new APIResult { Result = ResultType.Unknown, Code = ResultCode.Unknown, Message = "未知" };
        }

        public APIResponse ResponseResult(APIResult result)
        {
            return new APIResponse(result);
        }

        public APIResponse ResponseResult(ResultType resultType, ResultCode resultCode, string message = null, object data = null)
        {
            _result.Result = resultType;
            _result.Code = resultCode;
            _result.Message = message;
            _result.Data = data;
            return new APIResponse(_result);
        }

        public string GetUploadImageUrl(ImageModel imagemodel, string savepath)
        {

            if (imagemodel == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(imagemodel.Data))
            {
                return null;
            }

            string fileExt = imagemodel.Type;
            string basedata = imagemodel.Data;


            string imageUrl = "";
            try
            {

                string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:UploadServerUrl"];


                byte[] bytes = Convert.FromBase64String(basedata);

                UploadFileEntity entity = new UploadFileEntity();
                entity.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;//自定义文件名称，这里以当前时间为例
                entity.FileData = bytes;
                entity.UploadFolder = savepath;
                CustomJsonResult rm = HttpClientOperate.Post<CustomJsonResult>(savepath, strUrl, entity);//封装的POST提交方

                if (rm.Result == ResultType.Success)
                {
                    ImageUpload imageUpload = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageUpload>(rm.Data.ToString());

                    imageUrl = imageUpload.OriginalPath;
                }
                else
                {
                    rm.Message = "上传图片发生异常";
                    LogUtil.Error("调用API上传图片发生异常");
                }


            }
            catch (Exception ex)
            {
                LogUtil.Error("", ex);

            }
            return imageUrl;
        }

        public string GetRemarks(string pRemarks, int len)
        {
            string remarks = pRemarks;

            if (pRemarks != null)
            {
                if (pRemarks.Length >= len)
                {
                    remarks = pRemarks.Substring(0, len) + "...";
                }
            }



            return remarks;
        }

        public bool IsSaleman(int userId)
        {
            var user = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();

            if (user == null)
                return true;

            if (user.UserName.Length < 2)
                return false;

            string pfrix = user.UserName.Substring(0, 2);

            if (pfrix.ToLower() == "ag")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetAppVersion()
        {
            int version = 0;

            try
            {
                string app_version = HttpContext.Current.Request.Headers["version"];
                if (app_version != null)
                {
                    string[] s = app_version.Split('.');

                    foreach (var item in s)
                    {
                        version = version + int.Parse(item);
                    }

                }


            }
            catch (Exception ex)
            {

            }

            return version;
        }



    }
}