using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;
using WebBack;

namespace System.Web
{
    public static class HmlExtension
    {
        public static string Encrypt(string plaintext)
        {
            string cl1 = plaintext;
            string pwd = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public static HtmlString GenerateUniqueSubmitIdentifier(this HtmlHelper htmlhelper)
        {
            string sessionname = Guid.NewGuid().ToString();
            HttpContext.Current.Session[sessionname] = null;

            TagBuilder builder = new TagBuilder("input");
            builder.Attributes["type"] = "hidden";
            builder.Attributes["name"] = "_UniqueSubmitIdentifier";
            builder.Attributes["value"] = Guid.NewGuid().ToString();



            TagBuilder builderSessionName = new TagBuilder("input");
            builderSessionName.Attributes["type"] = "hidden";
            builderSessionName.Attributes["name"] = "_UniqueSubmitIdentifierSessionName";
            builderSessionName.Attributes["value"] = sessionname;

            StringBuilder sb = new StringBuilder();
            sb.Append(builder.ToString(TagRenderMode.SelfClosing));
            sb.Append(builderSessionName.ToString(TagRenderMode.SelfClosing));

            return new HtmlString(sb.ToString());
        }

        public static IHtmlString initEnumeration<T>(this HtmlHelper helper, Enumeration.InputType inputType, string name, object htmlAttributes = null)
        {
            IDictionary<string, object> HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            StringBuilder sb = new StringBuilder();

            var hidValue = new T[] { };
            if (HtmlAttributes["hidevalue"] != null)
            {
                hidValue = (T[])HtmlAttributes["hidevalue"];
            }

            var classname = "";
            if (HtmlAttributes["class"] != null)
            {
                classname = "class=\"" + HtmlAttributes["class"].ToString() + "\"";
            }

            var selectValue = new T[] { };
            if (HtmlAttributes["selectedvalue"] != null)
            {
                selectValue = (T[])HtmlAttributes["selectedvalue"];
            }

            string defaulttext = "";
            if (HtmlAttributes["defaulttext"] != null)
            {
                defaulttext = HtmlAttributes["defaulttext"].ToString();
            }

            if (inputType == Enumeration.InputType.Select)
            {
                int i = 0;
                string id = name.Replace(".", "_");
                sb.Append("<select name=\"" + name + "\" id=\"" + id + "\"  " + classname + ">");
                if (defaulttext != "")
                {
                    sb.Append("<option value=\"\"  >" + defaulttext + "</option>");
                }
                foreach (T t in Enum.GetValues(typeof(T)))
                {
                    string strKey = Convert.ToInt32(t).ToString();
                    Enum en = (Enum)Enum.Parse(t.GetType(), strKey);
                    string strValue = en.GetCnName();
                    string checkeds = "";
                    if (selectValue != null)
                    {
                        if (selectValue.Length > 0)
                        {
                            if (selectValue.Contains(t))
                            {
                                checkeds = "selected";
                            }
                        }
                    }

                    bool isHide = false;
                    if (hidValue != null)
                    {
                        if (hidValue.Length > 0)
                        {
                            if (hidValue.Contains(t))
                            {
                                isHide = true;
                            }
                        }
                    }
                    if (!isHide)
                    {
                        sb.Append(" <option value=\"" + strKey + "\" " + checkeds + ">" + strValue + "</option>");
                    }
                    i++;

                }
                sb.Append("/<select>");
            }
            else if (inputType == Enumeration.InputType.CheckBox)
            {

                int i = 0;
                foreach (T t in Enum.GetValues(typeof(T)))
                {
                    string strKey = Convert.ToInt32(t).ToString();
                    string strValue = Enum.GetName(typeof(T), t);
                    string id = name + i;

                    string checkeds = "";
                    if (selectValue != null)
                    {
                        for (int j = 0; j < selectValue.Length; j++)
                        {
                            string key1 = strValue;
                            string key2 = selectValue[j].ToString();
                            if (key1 == key2)
                            {
                                checkeds = "checked";
                                break;
                            }
                        }
                    }

                    sb.Append(" <input type=\"checkbox\" name=\"" + name + "\" id=\"" + id + "\"  value=\"" + strKey + "\" " + checkeds + " /><label for=\"" + id + "\">" + strValue + "</label>");



                    i++;

                }
            }
            else if (inputType == Enumeration.InputType.Radio)
            {
                int i = 0;
                foreach (T t in Enum.GetValues(typeof(T)))
                {
                    string strKey = Convert.ToInt32(t).ToString();
                    string strValue = Enum.GetName(typeof(T), t);
                    string id = name + i;
                    string checkeds = "";
                    if (selectValue != null)
                    {
                        if (selectValue.Length > 0)
                        {
                            string key1 = strValue;
                            string key2 = selectValue[0].ToString();
                            if (key1 == key2)
                            {
                                checkeds = "checked";
                            }
                        }
                    }
                    sb.Append(" <input type=\"radio\" name=\"" + name + "\" id=\"" + name + "\" value=\"" + strKey + "\" " + checkeds + "  /><label for=\"" + id + "\">" + strValue + "</label>");
                    i++;
                }
            }


            return new MvcHtmlString(sb.ToString());
        }

        private static string GetDept(string id)
        {
            string length = id.ToString();
            string dept = "";
            for (var i = 0; i < length.Length; i++)
            {
                dept += "&nbsp;&nbsp;&nbsp;";
            }

            return dept;
        }

        public static IHtmlString initGoodsType(this HtmlHelper helper, Enumeration.InputType inputType, string name, object htmlAttributes = null)
        {
            IDictionary<string, object> HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            StringBuilder sb = new StringBuilder();



            var classname = "";
            if (HtmlAttributes["class"] != null)
            {
                classname = "class=\"" + HtmlAttributes["class"].ToString() + "\"";
            }

            var selectValue = "";
            if (HtmlAttributes["selectedvalue"] != null)
            {
                selectValue = HtmlAttributes["selectedvalue"].ToString();
            }

            string defaulttext = "";
            if (HtmlAttributes["defaulttext"] != null)
            {
                defaulttext = HtmlAttributes["defaulttext"].ToString();
            }

            if (inputType == Enumeration.InputType.Select)
            {
                string id = name.Replace(".", "_");
                sb.Append("<select name=\"" + name + "\" id=\"" + id + "\"  " + classname + ">");

                if (defaulttext != "")
                {
                    sb.Append("<option value=\"\"  >" + defaulttext + "</option>");
                }

                List<KeyValuePair<int, string>> goodsTypes = Lumos.BLL.BizFactory.Product.GetGoodsType();

                foreach (var goodsType in goodsTypes)
                {
                    var checkeds = "";
                    if (selectValue != null)
                    {
                        if (selectValue == goodsType.Key.ToString())
                        {
                            checkeds = "selected";
                        }
                    }

                    string dept = GetDept(goodsType.Key.ToString());
                    sb.Append(" <option value=\"" + goodsType.Key + "\" " + checkeds + " >" + dept + goodsType.Value + "</option>");
                }

                sb.Append("/<select>");

            }


            return new MvcHtmlString(sb.ToString());
        }


        public static IHtmlString IsInPermission(this HtmlHelper helper, object value, params string[] permissions)
        {
            if (permissions == null)
                return helper.Raw(value);

            if (permissions.Length == 0)
                return helper.Raw(value);

            bool isHas = OwnRequest.IsInPermission(permissions);
            if (isHas)
            {
                return helper.Raw(value);
            }
            else
            {
                return helper.Raw("");
            }
        }
    }
}