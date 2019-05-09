/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	HTLicense
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/7/12 9:57:32
*	文件描述:			   	
*
***************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HOTINST.COMMON.License
{
    class HTLicense : System.ComponentModel.License
    {
        private string licenseKey = string.Empty;
        private HTLicenseProvider licenseProvider = null;

        public HTLicense(HTLicenseProvider provider, string key)
        {
            this.licenseProvider = provider;
            this.licenseKey = key;
        }
        public override string LicenseKey
        {
            get { return this.licenseKey; }
        }

        public override void Dispose()
        {
            this.licenseProvider = null;
            this.licenseKey = string.Empty;
        }
    }
}
