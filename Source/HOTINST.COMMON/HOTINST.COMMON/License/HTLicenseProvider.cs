/***************************************************************
*	Copyright (C) 2013-2018 Hotinst CO.,Ltd
*	All rights reserved
*	文 件 名:	HTLicenseProvider
*	CLR 版本:	4.0.30319.42000
*
*	作    者:	tanyu
*	创建时间:	2018/7/12 9:59:46
*	文件描述:			   	
*
***************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HOTINST.COMMON.License
{
    class HTLicenseProvider : LicenseProvider
    {
        public override System.ComponentModel.License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            if(context.UsageMode == LicenseUsageMode.Runtime)
            {
                string savedLicenseKey = context.GetSavedLicenseKey(type, null);
                if(savedLicenseKey == "")
                {
                    return new HTLicense(this, "");                                                                                                                                                                                                                                                                                
                }
            }
            return null;
            //string path = this.GetAss
        }
    }
}
