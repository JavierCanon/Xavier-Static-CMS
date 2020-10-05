namespace Web4CRM.BLL.Modules.Web4CMS
{

	/// <summary>
	/// Descripción breve de version
	/// </summary>
	public static class VersionAspNet
	{
		public static string GetAspNetVersion()
		{
			return "0.1.0";
		}

		public static string GetAppName()
		{
			return "Web4CMS";
		}
		public static string GetCoreVersion()
		{
			return Versions.GetVersion();
		}
	}
}
