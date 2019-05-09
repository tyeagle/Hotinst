using System;
using HOTINST.COMMON.Serialization;

namespace HOTINST.COMMON.License
{
	/// <summary>
	/// 授权类型
	/// </summary>
    public enum AuthorizationType
    {
		/// <summary>
		/// 无效授权
		/// </summary>
		AuthorizationInvalidated,
		/// <summary>
		/// 评估版本      90天
		/// </summary>
		AuthorizationEvaluation,
		/// <summary>
		/// 按使用时间授权, 20年
		/// </summary>
        AuthorizationByTime,
		/// <summary>
		/// 永不过期
		/// </summary>
        AuthorizationNeverExpire
    }

	/// <summary>
	/// License信息
	/// </summary>
    public class LicenseInfo 
    {
		/// <summary>
		/// ctor
		/// </summary>
        public LicenseInfo()
        {
            Authorization = AuthorizationType.AuthorizationInvalidated;
            AuthorizationTime = DateTime.MinValue;
            ExpireTime = DateTime.MinValue;
        }

        static  private readonly string RootName = "License";
		/// <summary>
		/// 客户计算机信息
		/// </summary>
		public String ComputerIdentify { get; set; }
		/// <summary>
		/// 授权类型
		/// </summary>
		public AuthorizationType Authorization { get; set; }
		/// <summary>
		/// 授权产品名称
		/// </summary>
		public String Product { get; set; }
		/// <summary>
		/// 授权产品版本
		/// </summary>
		public String Version;                          
        /// <summary>
		/// 注册时间
		/// </summary>
        public DateTime AuthorizationTime { get; set; }
		/// <summary>
		/// 过期时间
		/// </summary>
        public DateTime ExpireTime { get; set; }

        #region ILicenseService 成员

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strLicenseInfo"></param>
        public void FromXmlString(string strLicenseInfo)
        {
            LicenseInfo rh = SerializationHelper.LoadFromXmlString<LicenseInfo>(strLicenseInfo, RootName);
            this.Authorization = rh.Authorization;
            this.AuthorizationTime = rh.AuthorizationTime;
            this.ComputerIdentify = rh.ComputerIdentify;
            this.ExpireTime = rh.ExpireTime;
            this.Product = rh.Product;
            this.Version = rh.Version;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public string ToXmlString()
        {
            return SerializationHelper.SaveToXmlString(this, RootName);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="computer"></param>
		/// <param name="product"></param>
		/// <param name="version"></param>
		/// <returns></returns>
        public bool IsAuthenticated(string computer, string product, string version)
        {
            if (ComputerIdentify != computer || product != this.Product || version != this.Version
                || AuthorizationTime >= DateTime.Now
                || Authorization == AuthorizationType.AuthorizationInvalidated)
            {
                throw new Exception("Invalidated authorization information.");
            }

            if (Authorization == AuthorizationType.AuthorizationEvaluation
                && ExpireTime <= DateTime.Now)
            {
                throw new Exception("Authorization expired.");
            }
            else if (Authorization == AuthorizationType.AuthorizationByTime
                && ExpireTime <= DateTime.Now)
            {
                throw new Exception("Authorization expired.");
            }

            return true;
        }

        #endregion
    }
}
