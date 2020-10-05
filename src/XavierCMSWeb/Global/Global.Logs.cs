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
using System.Data;
using System.Data.SqlClient;
using System.Web;
using XavierCMSWeb.Errors;

namespace XavierCMSWeb
{
    public partial class Global : System.Web.HttpApplication
    {

        #region Logger
        public enum EnumLoggerToType : int
        {
            DB = 2
            ,
            FILE = 1
        }

        private enum EnumLoggerLevel : int
        {
            DEBUG
            ,
            ERROR
            ,
            FATAL
            ,
            INFO
            ,
            WARN
        }

        public enum EnumLogCategories : int
        {
             CONFIGURATION
            ,DATABASE
            ,DEBUG
            ,GENERAL
            ,SECURITY
            ,EMAIL
            ,CONTENT
            ,FILESYSTEM


        }

        public static void LogInfo(HttpContext context, EnumLogCategories logCategory, string text)
        {
            if (Configuration.Logger.GetIsEnabledLogger() || Configuration.Logger.GetIsEnabledDeveloperLogger())
            {
                string UsuId = "0";
                string UsuRolId = "0";
                string UsuLogin = string.Empty;
                string UsuRolNombre = string.Empty;
                string ip = string.Empty;
                string agent = string.Empty;
                string url = string.Empty;


                if (context != null)
                {
                    if (context.Request != null)
                    {
                        ip = context.Request.UserHostAddress;
                        agent = context.Request.UserAgent;
                        url = context.Request.CurrentExecutionFilePath;
                    }

                    if (context.Session != null)
                    {
                        if (context.Session["User.UserID"] != null)
                        {
                            UsuId = context.Session["User.UserID"].ToString();
                        }
                        if (context.Session["User.UserRolID"] != null)
                        {
                            UsuRolId = context.Session["User.UserRolID"].ToString();
                        }
                        if (context.Session["User.UserLogin"] != null)
                        {
                            UsuLogin = context.Session["User.UserLogin"].ToString();
                        }
                        if (context.Session["User.UserRolName"] != null)
                        {
                            UsuRolNombre = context.Session["User.UserRolName"].ToString();
                        }
                    }
                }

                /*
                if (eLoggerToType == EnumLoggerToType.FILE)
                {
                    loggerFileInfo.Info(
                    "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                    );
                }
                if (eLoggerToType == EnumLoggerToType.DB)
                {

                }*/

            }
        }

        public static void LogError(HttpContext context, EnumLogCategories logCategory, string text)
        {
            if (Configuration.Logger.GetIsEnabledLogger() || Configuration.Logger.GetIsEnabledDeveloperLogger())
            {
                string UsuId = "0";
                string UsuRolId = "0";
                string UsuLogin = string.Empty;
                string UsuRolNombre = string.Empty;
                string ip = string.Empty;
                string agent = string.Empty;
                string url = string.Empty;


                if (context != null)
                {
                    if (context.Request != null)
                    {
                        ip = context.Request.UserHostAddress;
                        agent = context.Request.UserAgent;
                        url = context.Request.CurrentExecutionFilePath;
                    }

                    if (context.Session != null)
                    {
                        if (context.Session["User.UserID"] != null)
                        {
                            UsuId = context.Session["User.UserID"].ToString();
                        }
                        if (context.Session["User.UserRolID"] != null)
                        {
                            UsuRolId = context.Session["User.UserRolID"].ToString();
                        }
                        if (context.Session["User.UserLogin"] != null)
                        {
                            UsuLogin = context.Session["User.UserLogin"].ToString();
                        }
                        if (context.Session["User.UserRolName"] != null)
                        {
                            UsuRolNombre = context.Session["User.UserRolName"].ToString();
                        }
                    }
                }

                /*
                if (eLoggerToType == EnumLoggerToType.FILE)
                {
                    loggerFileError.Error(
                    "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                    );
                }
                if (eLoggerToType == EnumLoggerToType.DB)
                {
                }
                */

                string error = (
                    "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                    );


                if (LogErrorsToTXTEnabled())
                {

                    Log_File_Write_Error(error);


                }


            }
        }

        public static void LogError(HttpContext context, EnumLogCategories logCategory, string text, Exception Exception)
        {
            if (context == null || text == null || Exception == null)
            {
                return;
            }

            if (Configuration.Logger.GetIsEnabledLogger() || Configuration.Logger.GetIsEnabledDeveloperLogger())
            {
                string UsuId = "0";
                string UsuRolId = "0";
                string UsuLogin = string.Empty;
                string UsuRolNombre = string.Empty;
                string ip = string.Empty;
                string agent = string.Empty;
                string url = string.Empty;


                if (context != null)
                {
                    if (context.Request != null)
                    {
                        ip = context.Request.UserHostAddress;
                        agent = context.Request.UserAgent;
                        url = context.Request.CurrentExecutionFilePath;
                    }

                    if (context.Session != null)
                    {
                        if (context.Session["User.UserID"] != null)
                        {
                            UsuId = context.Session["User.UserID"].ToString();
                        }
                        if (context.Session["User.UserRolID"] != null)
                        {
                            UsuRolId = context.Session["User.UserRolID"].ToString();
                        }
                        if (context.Session["User.UserLogin"] != null)
                        {
                            UsuLogin = context.Session["User.UserLogin"].ToString();
                        }
                        if (context.Session["User.UserRolName"] != null)
                        {
                            UsuRolNombre = context.Session["User.UserRolName"].ToString();
                        }
                    }
                }

                /*
                if (eLoggerToType == EnumLoggerToType.FILE)
                {
                    if (Exception != null)
                    {
                        loggerFileError.Error(
                        "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                        + Environment.NewLine + Exception.Message + Environment.NewLine + Exception.StackTrace);
                    }
                    else
                    {
                        loggerFileError.Error(
                        "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                        );
                    }
                }

                if (eLoggerToType == EnumLoggerToType.DB)
                {

                }
                */


                ExceptionUtility.LogException(Exception, context.Request.RawUrl);

            }
        }

        public static void LogFatal(HttpContext context, EnumLogCategories logCategory, string text)
        {
            if (Configuration.Logger.GetIsEnabledLogger() || Configuration.Logger.GetIsEnabledDeveloperLogger())
            {
                string UsuId = "0";
                string UsuRolId = "0";
                string UsuLogin = string.Empty;
                string UsuRolNombre = string.Empty;
                string ip = string.Empty;
                string agent = string.Empty;
                string url = string.Empty;


                if (context != null)
                {
                    if (context.Request != null)
                    {
                        ip = context.Request.UserHostAddress;
                        agent = context.Request.UserAgent;
                        url = context.Request.CurrentExecutionFilePath;
                    }

                    if (context.Session != null)
                    {
                        if (context.Session["User.UserID"] != null)
                        {
                            UsuId = context.Session["User.UserID"].ToString();
                        }
                        if (context.Session["User.UserRolID"] != null)
                        {
                            UsuRolId = context.Session["User.UserRolID"].ToString();
                        }
                        if (context.Session["User.UserLogin"] != null)
                        {
                            UsuLogin = context.Session["User.UserLogin"].ToString();
                        }
                        if (context.Session["User.UserRolName"] != null)
                        {
                            UsuRolNombre = context.Session["User.UserRolName"].ToString();
                        }
                    }
                }

                /*
                if (eLoggerToType == EnumLoggerToType.FILE)
                {
                    loggerFileError.Fatal(
                    "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                    );
                }
                if (eLoggerToType == EnumLoggerToType.DB)
                {
                }
                */


            }
        }

        public static void LogWarn(HttpContext context, EnumLogCategories logCategory, string text)
        {
            if (Configuration.Logger.GetIsEnabledLogger() || Configuration.Logger.GetIsEnabledDeveloperLogger())
            {
                string UsuId = "0";
                string UsuRolId = "0";
                string UsuLogin = string.Empty;
                string UsuRolNombre = string.Empty;
                string ip = string.Empty;
                string agent = string.Empty;
                string url = string.Empty;


                if (context != null)
                {
                    if (context.Request != null)
                    {
                        ip = context.Request.UserHostAddress;
                        agent = context.Request.UserAgent;
                        url = context.Request.CurrentExecutionFilePath;
                    }

                    if (context.Session != null)
                    {
                        if (context.Session["User.UserID"] != null)
                        {
                            UsuId = context.Session["User.UserID"].ToString();
                        }
                        if (context.Session["User.UserRolID"] != null)
                        {
                            UsuRolId = context.Session["User.UserRolID"].ToString();
                        }
                        if (context.Session["User.UserLogin"] != null)
                        {
                            UsuLogin = context.Session["User.UserLogin"].ToString();
                        }
                        if (context.Session["User.UserRolName"] != null)
                        {
                            UsuRolNombre = context.Session["User.UserRolName"].ToString();
                        }
                    }
                }

                /*
                if (eLoggerToType == EnumLoggerToType.FILE)
                {
                    loggerFileInfo.Warn(
                    "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                    );
                }
                if (eLoggerToType == EnumLoggerToType.DB)
                { }
                */

            }
        }

        public static void LogDebug(HttpContext context, EnumLogCategories logCategory, string text)
        {

            if (Configuration.Logger.GetIsEnabledLogger() || Configuration.Logger.GetIsEnabledDeveloperLogger())
            {
                string UsuId = "0";
                string UsuRolId = "0";
                string UsuLogin = string.Empty;
                string UsuRolNombre = string.Empty;
                string ip = string.Empty;
                string agent = string.Empty;
                string url = string.Empty;


                if (context != null)
                {
                    if (context.Request != null)
                    {
                        ip = context.Request.UserHostAddress;
                        agent = context.Request.UserAgent;
                        url = context.Request.CurrentExecutionFilePath;
                    }

                    if (context.Session != null)
                    {
                        if (context.Session["User.UserID"] != null)
                        {
                            UsuId = context.Session["User.UserID"].ToString();
                        }
                        if (context.Session["User.UserRolID"] != null)
                        {
                            UsuRolId = context.Session["User.UserRolID"].ToString();
                        }
                        if (context.Session["User.UserLogin"] != null)
                        {
                            UsuLogin = context.Session["User.UserLogin"].ToString();
                        }
                        if (context.Session["User.UserRolName"] != null)
                        {
                            UsuRolNombre = context.Session["User.UserRolName"].ToString();
                        }
                    }
                }

                /*
                if (eLoggerToType == EnumLoggerToType.FILE)
                {
                    loggerFileInfo.Debug(
                    "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                    );
                }
                if (eLoggerToType == EnumLoggerToType.DB)
                {
                }
                */

            }
        }
        #endregion Logger

        #region User Log

        // http://stackoverflow.com/questions/14903887/warning-this-call-is-not-awaited-execution-of-the-current-method-continues
        // A method marked as async can return void, Task or Task<T>. What are the differences between them?
        // A Task<T> returning async method can be awaited, and when the task completes it will proffer up a T.
        // A Task returning async method can be awaited, and when the task completes, the continuation of the task is scheduled to run.
        // A void returning async method cannot be awaited; it is a "fire and forget" method. It does work asynchronously, and you have no way of telling when it is done. This is more than a little bit weird; as SLaks says, normally you would only do that when making an asynchronous event handler. The event fires, the handler executes; no one is going to "await" the task returned by the event handler because event handlers do not return tasks, and even if they did, what code would use the Task for something? It's usually not user code that transfers control to the handler in the first place.

        /// <summary>
        /// Log user action in DB or File according var eUserLoggerToType (EnumLoggerToType)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logLevel"></param>
        /// <param name="logCategory"></param>
        /// <param name="logMessage"></param>
        /// <param name="logException"></param>
        /// <returns></returns>
        public static async void LogUser(HttpContext context, string logLevel, string logCategory, string logMessage, string logException)
        {
            if (!Configuration.Logger.GetIsEnabledUserLogger())
            {
                return;
            }

            string UsuId = "0";
            string UsuRolId = "0";
            string UsuLogin = string.Empty;
            string UsuRolNombre = string.Empty;
            string Ip = string.Empty;
            string UserAgent = string.Empty;
            string Url = string.Empty;


            if (context != null)
            {
                if (context.Request != null)
                {
                    Ip = context.Request.UserHostAddress;
                    UserAgent = context.Request.UserAgent;
                    Url = context.Request.CurrentExecutionFilePath;
                }

                if (context.Session != null)
                {
                    if (context.Session["User.UserID"] != null)
                    {
                        UsuId = context.Session["User.UserID"].ToString();
                    }
                    if (context.Session["User.UserRolID"] != null)
                    {
                        UsuRolId = context.Session["User.UserRolID"].ToString();
                    }
                    if (context.Session["User.UserLogin"] != null)
                    {
                        UsuLogin = context.Session["User.UserLogin"].ToString();
                    }
                    if (context.Session["User.UserRolName"] != null)
                    {
                        UsuRolNombre = context.Session["User.UserRolName"].ToString();
                    }
                }
            }

            /*
            if (eUserLoggerToType == EnumLoggerToType.FILE)
            {
                
                loggerUser.Info(
                "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
                );
                
            }
            */
            /*
            if (eUserLoggerToType == EnumLoggerToType.DB)
            {
            */
            using (SqlConnection connection = new SqlConnection() { ConnectionString = Configuration.DB.GetConnectionStringDBUserLogServer() })
            {

                await connection.OpenAsync();
                SqlCommand command = connection.CreateCommand();
                command.CommandTimeout = 60 * 5;

                /*
                command.CommandText = string.Format(@"INSERT INTO [UserLog]
   ([Date]
   ,[Thread]
   ,[Level]
   ,[Logger]
   ,[Message]
   ,[Exception]
   ,[Category]
   ,[UserId]
   ,[RolId]
   ,[Ip]
   ,[Url]
   ,[UserAgent])
VALUES
   (
     GETDATE() --@log_date
    ,NULL @thread
    ,{0} --@log_level
    ,NULL --@logger
    ,{1} --@message
    ,{2} --@Exception
    ,{3} --@Category
    ,{4} --@UserId
    ,{5} --@RolId
    ,{6} --@Ip
    ,{7} --@Url
    ,{8} --@UserAgent
   )"

    ,logLevel //{0} --@log_level
    ,logMessage //{1} --@message
    ,logException//{2} --@Exception
    ,logCategory //{3} --@Category
    ,UsuId //{4} --@UserId
    ,UsuRolId //{5} --@RolId
    ,ip //{6} --@Ip
    ,url //{7} --@Url
    ,agent //{8} --@UserAgent



      );
    */

                command.CommandText = @"INSERT INTO [User_Log]
           ([Date]
           ,[Thread]
           ,[Level]
           ,[Logger]
           ,[Message]
           ,[Exception]
           ,[Category]
           ,[User_Id]
           ,[Rol_Id]
           ,[Ip]
           ,[Url]
           ,[UserAgent])
     VALUES
           (
             GETDATE() --@log_date
            ,NULL --@thread
            ,@logLevel
            ,'DB' --@logger
            ,@logMessage
            ,@logException
            ,@logCategory
            ,@UserId
            ,@RolId
            ,@Ip
            ,@Url
            ,@UserAgent
           )";

                SqlParameter[] parameters = new SqlParameter[] {
              new SqlParameter{ ParameterName="logLevel", DbType= DbType.AnsiString, Size=60, Value= logLevel}
            , new SqlParameter{ ParameterName="logMessage", DbType= DbType.AnsiString, Size=8000, Value= logMessage }
            , new SqlParameter{ ParameterName="logException", DbType= DbType.AnsiString, Size=8000, Value= logException}
            , new SqlParameter{ ParameterName="logCategory", DbType= DbType.AnsiString, Size=60, Value= logCategory}
            , new SqlParameter{ ParameterName="UserId", DbType= DbType.Int32, Size=255, Value= UsuId}
            , new SqlParameter{ ParameterName="RolId", DbType= DbType.Int32, Size=255, Value= UsuRolId}
            , new SqlParameter{ ParameterName="Ip", DbType= DbType.AnsiString, Size=255, Value= Ip}
            , new SqlParameter{ ParameterName="Url", DbType= DbType.AnsiString, Size=255, Value= Url}
            , new SqlParameter{ ParameterName="UserAgent", DbType= DbType.AnsiString, Size=255, Value= UserAgent}
            };

                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }


                int result = await command.ExecuteNonQueryAsync();

            }



            // }

        }


        /*
public static void LogUser(HttpContext context, string logCategory, string text)
{
    if (bEnableUserLogger)
    {
        var UsuId = "0";
        var UsuRolId = "0";
        var UsuLogin = string.Empty;
        var UsuRolNombre = string.Empty;
        var ip = string.Empty;
        var agent = string.Empty;
        var url = string.Empty;


        if (context != null)
        {
            if (context.Request != null)
            {
                ip = context.Request.UserHostAddress;
                agent = context.Request.UserAgent;
                url = context.Request.CurrentExecutionFilePath;
            }

            if (context.Session != null)
            {
                if (context.Session["User.UserID"] != null)
                {
                    UsuId = context.Session["User.UserID"].ToString();
                }
                if (context.Session["User.UserRolID"] != null)
                {
                    UsuRolId = context.Session["User.UserRolID"].ToString();
                }
                if (context.Session["User.UserLogin"] != null)
                {
                    UsuLogin = context.Session["User.UserLogin"].ToString();
                }
                if (context.Session["User.UserRolName"] != null)
                {
                    UsuRolNombre = context.Session["User.UserRolName"].ToString();
                }
            }
        }


        if (eUserLoggerToType == EnumLoggerToType.FILE)
        {
            loggerUser.Info(
            "[" + logCategory + "][" + UsuId + " " + UsuLogin + "][" + UsuRolId + " " + UsuRolNombre + "][" + ip + "][" + agent + "][" + url + "] " + Environment.NewLine + text
            );
        }

        if (eUserLoggerToType == EnumLoggerToType.DB)
        {



#if LOG4NETUSE_MDC
        MDC.Set("Category", logCategory.ToString());
        MDC.Set("UserId", UsuId);
        MDC.Set("RolId", UsuRolId);
        MDC.Set("Ip", ip);
        MDC.Set("Url", url);
        MDC.Set("UserAgent", agent);
#else
            log4net.ThreadContext.Properties["Category"] = logCategory.ToString();
            log4net.ThreadContext.Properties["UserId"] = UsuId;
            log4net.ThreadContext.Properties["RolId"] = UsuRolId;
            log4net.ThreadContext.Properties["Ip"] = ip;
            log4net.ThreadContext.Properties["Url"] = url;
            log4net.ThreadContext.Properties["UserAgent"] = agent;
#endif
 
            loggerUser.Info(text);


#if LOG4NETUSE_MDC
        // limpiar
        MDC.Set("Exception", "");
        MDC.Set("Category", "");
        MDC.Set("UserId", "0");
        MDC.Set("RolId", "0");
        MDC.Set("Ip", "");
        MDC.Set("Url", "");
        MDC.Set("UserAgent", "");
#else
            log4net.ThreadContext.Properties["Exception"] = string.Empty;
            log4net.ThreadContext.Properties["Category"] = string.Empty;
            log4net.ThreadContext.Properties["UserId"] = "0";
            log4net.ThreadContext.Properties["RolId"] = "0";
            log4net.ThreadContext.Properties["Ip"] = string.Empty;
            log4net.ThreadContext.Properties["Url"] = string.Empty;
            log4net.ThreadContext.Properties["UserAgent"] = string.Empty;
#endif

        }
    }
}
*/

        #endregion User Log



    }
}
