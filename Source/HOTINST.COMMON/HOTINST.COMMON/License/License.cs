using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HOTINST.COMMON.License
{
    /// <summary>
    /// 供用户端代码调用，生产用户信息，检查授权信息是否正确
    /// </summary>
    public class License
    {
        private static readonly string publicKey = "<RSAKeyValue><Modulus>nJ636YJCa5YSljqimaXfMH+0YIeshlc6y2pyDi+Pjk0ZczWPsYuwFbo+QkjnpryK/Bo6MK5fEbyHHUJnH1oHTSwsvCVTLMz172SUfHW156XKXr7PftOVG2rP5Dsg9BrZYhaqh1OgZrcI5a3EKcXBC4xs8aHHsV65IYlRl35yiWM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private static readonly string privateKey = "<RSAKeyValue><Modulus>nJ636YJCa5YSljqimaXfMH+0YIeshlc6y2pyDi+Pjk0ZczWPsYuwFbo+QkjnpryK/Bo6MK5fEbyHHUJnH1oHTSwsvCVTLMz172SUfHW156XKXr7PftOVG2rP5Dsg9BrZYhaqh1OgZrcI5a3EKcXBC4xs8aHHsV65IYlRl35yiWM=</Modulus><Exponent>AQAB</Exponent><P>y+EKQ3MG8RGUR/IIYsQWhg1vNY4FcLDmJR07quf4vzCkZLJtwNMWWR+lqZKQPeLugPQ1cy90rs5dsXwXTCsAVQ==</P><Q>xKjJXroiYz3wjD6tgSDftBo3OXmehDowUPW33OhdOYEHBKkXog6ooqvwEEacvdSFydwRSbWIabt/23rM4To61w==</Q><DP>c3k3dfJtiRZ61LD6HO6RD0YGqd+Rpz0abQT8qZUPZ0Jmqf4BechVDQ+Gpd+0QMkKaxFmQKItRWDu4jq1e1eTrQ==</DP><DQ>TXkY62J0jZgnHXjLrWUf+7mgK9pHoluyERLb/gDkSPUVqLZcgxE3Se5mQmMu+HGyyxUREnKbbNvawMId2FSyPQ==</DQ><InverseQ>pwHEmtwDW7UmG/SA8drZd6oFIOkZ9O+IfWD2QD9WI6NTKhiCV5C0ate548/Pb+M+KpyNZal4Ub+0o5SZU4+wqg==</InverseQ><D>FgHzg9tq6+U9nWCF4qM9NnprZTkLVCFDwLunZTjnqi5JSjgXhfJD/vmZsATAkFxkB0LENHz8HOjp74GaLfyfk8892AA9wjUc5VJ+wrNQsevXBKEpSycIqwOc3RHFXUMdY3z84h/rGnSkC+0Rcw4MxPVSR208Meq+z4XRVOspvHk=</D></RSAKeyValue>";

		/// <summary>
		/// 获取计算机标识
		/// </summary>
		/// <returns></returns>
		public static string ComputerIdentify()
        {
            return ComputerInfo.GetComputerIdentify();
        }
		/// <summary>
		/// 注册
		/// </summary>
		/// <param name="computer">计算机标识</param>
		/// <param name="product">产品名称</param>
		/// <param name="version">产品版本</param>
		/// <param name="type">授权类型</param>
		/// <returns>注册信息</returns>
		public static string CreateLicenseXml(string computer, string product, string version, AuthorizationType type)
        {
            LicenseInfo license = CreateAuthorization(computer, product, version, type);

            return RSAHelper.RSAEncrypt(publicKey, license.ToXmlString());
        }
		/// <summary>
		/// 校验
		/// </summary>
		/// <param name="licenseXml">注册信息</param>
		/// <param name="product">产品名称</param>
		/// <param name="version">产品版本</param>
		/// <returns>true: 成功；false: 失败</returns>
		public static bool ValidateLicense(string licenseXml, string product, string version)
        {
            LicenseInfo license = new LicenseInfo();
            license.FromXmlString(RSAHelper.RSADecrypt(privateKey, licenseXml));

            if (license.ComputerIdentify != ComputerInfo.GetComputerIdentify())
            {
                return false;
            }

            return license.IsAuthenticated(ComputerInfo.GetComputerIdentify(), product, version);
        }
        private static LicenseInfo CreateAuthorization(string computer, string product, string ver, AuthorizationType type)
        {
            LicenseInfo license = new LicenseInfo();
            license.ComputerIdentify = computer;
            license.Authorization = type;
            license.AuthorizationTime = DateTime.Now;
            if (license.Authorization == AuthorizationType.AuthorizationEvaluation)
            {
                license.ExpireTime = license.AuthorizationTime.AddDays(90);
            }
            else if (license.Authorization == AuthorizationType.AuthorizationByTime)
            {
                license.ExpireTime = license.AuthorizationTime.AddYears(20);
            }
            else if (license.Authorization == AuthorizationType.AuthorizationNeverExpire)
            {
                license.ExpireTime = license.AuthorizationTime.AddYears(100);
            }

            license.Product = product;
            license.Version = ver;

            return license;
        }
    }
}
