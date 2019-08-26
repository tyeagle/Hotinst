﻿using System;
using System.Collections.Generic;
using System.Text;
using HOTINST.OSGI.Core;
using HOTINST.OSGI.Core.Root;

namespace HOTINST.OSGI.ConsoleSample.Command
{
    class StopBundleCommand : ICommand
    {
        private IFramework framework;
        public StopBundleCommand(IFramework framework)
        {
            this.framework = framework;
        }

        public string GetCommandName()
        {
            return "stop";
        }

        public string GetHelpText()
        {
            return "停止插件";
        }

        public string GetDetailHelpText()
        {
            return "停止插件\r\n\r\n"
            + "stop [插件Index]  停止插件Index为[插件Index]的插件";
        }

        public string ExecuteCommand(string commandLine)
        {
            String bundleIdStr = commandLine.Substring(GetCommandName().Length).Trim();
            var bundleId = int.Parse(bundleIdStr);
            IBundle bundle = framework.GetBundleContext().GetBundle(bundleId);
            if (bundle == null) return String.Format("未找到ID为[{0}]的Bundle", bundleId);
            bundle.Stop();
            return String.Format("插件[{0} ({1})]已停止.当前状态为:{2}", bundle.GetSymbolicName(), bundle.GetVersion(), BundleUtils.GetBundleStateString(bundle.GetState()));
        }
    }
}
