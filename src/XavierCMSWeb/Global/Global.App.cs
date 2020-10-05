// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace XavierCMSWeb
{
    public partial class Global : System.Web.HttpApplication
    {


        #region Application Configuration

        void SyncSOAPPandDBSettings(string dbConnection)
        {

            System.Collections.Specialized.NameValueCollection config = ConfigurationManager.AppSettings;
            string tsql = "", setting, value;

            foreach (string key in config.AllKeys)
            {

                setting = key;
                value = config[key].ToString();


                SqlParameter[] paramsToSP = {
                 new SqlParameter(){ ParameterName= "@Name", DbType= DbType.AnsiString, Size= 60, Value= setting }
                ,new SqlParameter(){ ParameterName= "@Value", DbType= DbType.AnsiString, Size= 240, Value= value }
                 };

                tsql = string.Format("EXEC [Settings_Update_Value] '{0}', '{1}'", setting, value) + Environment.NewLine;

                if (Configuration.Development.GetIsEnabledDeveloperMode())
                {
                    LogDebug(null, EnumLogCategories.CONFIGURATION, "Sync Config >>> " + Environment.NewLine + tsql);
                }

                // need to use parameters becuase use or ilegal chars in tsql in config values.
                Dalayer.DAL.SqlApiSqlClient.ExecuteSqlString("EXEC [Settings_Update_Value] @Name, @Value ;", paramsToSP, dbConnection);


            }
        }

        #endregion Application Configuration

        #region Optimization
        /// <summary>
        /// use minification javascript file version
        /// </summary>
        /// <returns></returns>
        public static string UseMinJS()
        {
            
            if (IsDebugJs)
                return ".min";
            else
                return "";
                
        }
        /// <summary>
        /// use minification css styleshet file version
        /// </summary>
        /// <returns></returns>
        public static string UseMinCSS()
        {

            
            if (IsDebugCSS)
                return ".min";
            else
                return "";
                
        }
        #endregion Optimization



        #region Gets info



        public static string GetCredits()
        {
            if (!string.IsNullOrEmpty(_AppCredits))
            {
                return _AppCredits;
            }

            _AppCredits = "<a href=\"https://github.com/JavierCanon/quorum.net/\" target=\"_blank\">Quorum.net</a> Ver.[" + Global.Versions.GetAppVersion() + "] &copy; 2020 - " + System.DateTime.Now.Year.ToString() + " by <a href=\"https://www.javiercanon.com/\" target=\"_blank\">Javier Cañon</a>";

            return _AppCredits;
        }


        public static string GetImageLogo()
        {

            return Global.Utils.Webpath.GetRequestApplicationUrlPath() + "Content/Images/LogoSO.png";

        }


        #endregion  Gets info



    }
}
