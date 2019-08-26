﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Policy;

using Mono.Cecil;
using log4net;

using HOTINST.OSGI.Event;
using HOTINST.OSGI.Extension;
using HOTINST.OSGI.Provider;
using HOTINST.OSGI.Utils;
using HOTINST.OSGI.Core.Root;

namespace HOTINST.OSGI.Core
{

    /// <summary>
    /// Bundle表示由内核创建的一个插件
    /// </summary>
    class Bundle : IBundle
    {
        #region Static Property

        /// <summary>
        /// 日志
        /// </summary>
        private static ILog log = LogManager.GetLogger(typeof(Bundle));

        #endregion

        #region Property & Field

        /// <summary>
        /// framwork
        /// </summary>
        private IFramework framework;

        /// <summary>
        /// bundle上下文
        /// </summary>
        private IBundleContext bundleContext;

        /// <summary>
        /// Bundle路径
        /// </summary>
        private string bundleDirectoryPath;

        /// <summary>
        /// Bundle文件名称
        /// </summary>
        private string bundleAssemblyFileName;

        /// <summary>
        /// Bundle 配置信息
        /// </summary>
        private XmlNode manifestData;

        /// <summary>
        /// bundle程序集全名
        /// </summary>
        private string bundleAssemblyFullName;

        /// <summary>
        /// bundle符号名称
        /// </summary>
        private string bundleSymbolicName = "<未加载>";

        /// <summary>
        /// bundle版本
        /// </summary>
        private Version bundleVersion;
        /// <summary>
        /// bundle描述信息
        /// </summary>
        private String description;

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        private long lastModified;

        /// <summary>
        /// bundle元数据字典
        /// </summary>
        private IDictionary<string, string> metaDataDictionary;

        /// <summary>
        /// 当前bundle引用程序集
        /// </summary>
        private IDictionary<string, Assembly> bundleRefAssemblyDict;

        /// <summary>
        /// 当前bundle引用程序集定义
        /// </summary>
        private IDictionary<string, AssemblyDefinition> bundleRefDefinitionDict;

        /// <summary>
        /// 扩展点
        /// </summary>
        private IList<ExtensionPoint> extensionPoints;

        /// <summary>
        /// 扩展数据
        /// </summary>
        private IList<ExtensionData> extensionDatas;

        /// <summary>
        /// bundle程序集
        /// </summary>
        private Assembly bundleAssembly;

        /// <summary>
        /// bundle启动器类型
        /// </summary>
        private Type activatorClass;

        /// <summary>
        /// bundle启动器实例
        /// </summary>
        private IBundleActivator bundleActivator;

        /// <summary>
        /// bundle依赖bundles集合
        /// </summary>
        private IList<Bundle> requiredBundleList;

        /// <summary>
        /// 状态
        /// </summary>
        private int state = BundleStateConst.INSTALLED;


        #endregion

        #region Constructor
        /// <summary>
        /// Bundle构造
        /// </summary>
        /// <param name="framework">框架实例</param>
        /// <param name="bundleDirectoryPath">Bundle路径</param>
        /// <param name="bundleConfigData">Bundle配置节点</param>
        public Bundle(IFramework framework, string bundleDirectoryPath, XmlNode bundleConfigData)
        {

            this.framework = framework;
            this.bundleDirectoryPath = bundleDirectoryPath;
            this.manifestData = bundleConfigData;

            this.bundleRefDefinitionDict = new Dictionary<string, AssemblyDefinition>();
            this.bundleRefAssemblyDict = new Dictionary<string, Assembly>();
            this.extensionDatas = new List<ExtensionData>();
            this.extensionPoints = new List<ExtensionPoint>();
            this.bundleContext = new BundleContext(framework, this);

            Init();
        }

        #endregion

        #region Method

        #region Init
        /// <summary>
        /// 初始化Bundle，读取相关信息
        /// </summary>
        private void Init()
        {

            //清除之前的所有程序集引用
            this.RemoveAllRefAssembly();

            var assemblyName = this.GetBundleAssemblyFileName();
            this.bundleAssemblyFileName = Path.Combine(bundleDirectoryPath, string.Format("{0}.dll", assemblyName));
            if (false == File.Exists(this.bundleAssemblyFileName))
            {
                this.bundleAssemblyFileName = Path.Combine(bundleDirectoryPath, string.Format("{0}.exe", assemblyName));
            }
            lastModified = File.GetLastWriteTime(bundleAssemblyFileName).Ticks;

            this.LoadMetaData(assemblyName);

            var frameworkFireEvent = (IFrameworkFireEvent)framework;
            
            frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.INSTALLED, this));
        }


        /// <summary>
        /// 读取元数据信息
        /// </summary>
        private void LoadMetaData(string assemblyName)
        {
            log.Debug(string.Format("模块读取元数据信息！[{0}]", assemblyName));

            //另开一个AppDomain来加载主程序集，得到名称和版本信息
            metaDataDictionary = new Dictionary<string, string>();

            AppDomainSetup domaininfo = new AppDomainSetup();
            domaininfo.ApplicationBase = System.Environment.CurrentDirectory;
            Evidence adevidence = AppDomain.CurrentDomain.Evidence;
            AppDomain tmpAppDomain = AppDomain.CreateDomain(string.Format("Bundle[{0}] AppDomain", assemblyName), adevidence, domaininfo);
            AssemblyResolver assemblyResolver = tmpAppDomain.CreateInstanceAndUnwrap(typeof(Bundle).Assembly.FullName, typeof(AssemblyResolver).FullName) as AssemblyResolver;
            if (assemblyResolver != null)
            {
                assemblyResolver.Init((File.ReadAllBytes(bundleAssemblyFileName)), GetBundleLibsDirectoryName(), BundleConfigProvider.GetShareLibsDirectory());

                this.bundleAssemblyFullName = assemblyResolver.GetAssemblyFullName();
                this.bundleSymbolicName = assemblyResolver.GetAssemblyName();
                metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_SYMBOLIC_NAME, bundleSymbolicName);
                this.bundleVersion = assemblyResolver.GetVersion();
                this.description = assemblyResolver.GetDescription();

                metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_VERSION, bundleVersion.ToString());
                metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_DESCRIPTION, description.ToString());
                metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_NAME, assemblyResolver.GetAssemblyTitle());
                metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_VENDOR, assemblyResolver.GetVendor());
                metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_REQUIRE_BUNDLE, assemblyResolver.GetAssemblyRequiredAssembly());
                assemblyResolver = null;
            }

            AppDomain.Unload(tmpAppDomain);
        }


        /// <summary>
        /// 清除当前Bundle引用程序集
        /// </summary>
        private void RemoveAllRefAssembly()
        {
            foreach (string assemblyName in bundleRefAssemblyDict.Keys)
            {
                BundleAssemblyProvider.RemoveAssembly(assemblyName);
            }
            bundleRefAssemblyDict.Clear();
            bundleRefDefinitionDict.Clear();
        }

        /// <summary>
        /// 获取当前Bundle目录
        /// </summary>
        /// <returns></returns>
        private string GetBundleFolderName()
        {
            return new DirectoryInfo(bundleDirectoryPath).Name;
        }

        #endregion

        #region Start
        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (GetBundleStartLevel() >= 255)
            {
                log.Warn($"模块{GetBundleAssemblyFileName()}启动级别大于255,不启动。");
            }
            log.Debug(string.Format("模块{0}启动开始！", this.GetBundleAssemblyFileName()));

            if (this.state == BundleStateConst.INSTALLED)
            {
                try
                {
                    Resolve();
                }
                catch (Exception ex)
                {
                    this.state = BundleStateConst.INSTALLED;
                    log.Error(string.Format("模块{0}解析失败！", this.GetBundleAssemblyFileName()));
                    throw ex;
                }
            }
            if (this.state == BundleStateConst.RESOLVED)
            {
                this.state = BundleStateConst.STARTING;
                var frameworkFireEvent = (IFrameworkFireEvent)framework;
                try
                {
                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STARTING, this));

                    if (this.activatorClass != null)
                    {
                        log.Debug("激活器启动！");
                        bundleActivator = Activator.CreateInstance(activatorClass) as IBundleActivator;
                        //启动激活器
                        bundleActivator.Start(bundleContext);

                        log.Debug("加载扩展点数据！");
                        //初始化扩展信息
                        LoadExtensions();
                    }
                    this.state = BundleStateConst.ACTIVE;

                    
                }
                catch (Exception ex)
                {
                    this.state = BundleStateConst.RESOLVED;
                    log.Error($"模块{GetBundleAssemblyFileName()}启动失败！{Environment.NewLine}{ex.Message}");
                    //throw;
                }
                finally
                {
                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STARTED, this));
                }
            }

            log.Debug(string.Format("模块{0}启动结束！", this.GetBundleAssemblyFileName()));

        }

        /// <summary>
        /// 初始化扩展信息
        /// </summary>
        private void LoadExtensions()
        {
            log.Debug("扩展点加载开始！");

            try
            {
                this.extensionPoints.Clear();
                this.extensionDatas.Clear();

                var extensionPointNames = BundleConfigProvider.GetBundleConfigExtensionPoints(this.manifestData);

                this.extensionPoints = extensionPointNames.Select(name => new ExtensionPoint()
                {
                    Name = name,
                    Owner = this
                }).ToList();

                var extensionDataDic = BundleConfigProvider.GetBundleConfigExtensionDatas(this.manifestData);

                this.extensionDatas = extensionDataDic.Select(item => new ExtensionData()
                {
                    Name = item.Key,
                    Owner = this,
                    ExtensionList = item.Value
                }).ToList();

                //扩展已安装的Bundle的扩展点信息
                foreach (IBundle bundle in this.framework.GetBundles())
                {
                    var bundlePoints = bundle.GetExtensionPoints();
                    foreach (ExtensionPoint extensionPoint in bundlePoints)
                    {
                        var extensionData = extensionDatas.FirstOrDefault(item => item.Name == extensionPoint.Name);
                        if (extensionData != null)
                        {

                            log.Debug(string.Format("扩展点{0}加载！", extensionPoint.Name));

                            var eventArgs = new ExtensionEventArgs(ExtensionEventArgs.LOAD, extensionPoint, extensionData);

                            var fireContext = (IContextFireEvent)bundle.GetBundleContext();
                            fireContext.FireExtensionChanged(this, eventArgs);


                            var frameworkFireEvent = (IFrameworkFireEvent)framework;
                            frameworkFireEvent.FireExtensionEvent(eventArgs);

                        }
                    }
                }

                //从已经安装的Bundle中发现扩展数据
                foreach (ExtensionPoint extensionPoint in extensionPoints)
                {
                    foreach (IBundle bundle in this.framework.GetBundles())
                    {
                        var bundleExtensionDatas = bundle.GetExtensionDatas();
                        var extensionData = bundleExtensionDatas.FirstOrDefault(item => item.Name == extensionPoint.Name);
                        if (extensionData != null)
                        {
                            log.Debug(string.Format("扩展点{0}加载！", extensionPoint.Name));

                            var eventArgs = new ExtensionEventArgs(ExtensionEventArgs.LOAD, extensionPoint, extensionData);

                            var fireContext = (IContextFireEvent)this.GetBundleContext();
                            fireContext.FireExtensionChanged(bundle, eventArgs);

                            var frameworkFireEvent = (IFrameworkFireEvent)framework;
                            frameworkFireEvent.FireExtensionEvent(eventArgs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("加载Bundle扩展点配置信息出错！", ex);
                throw ex;
            }

            log.Debug("扩展点加载结束！");

        }

        #endregion

        #region Resolve

        /// <summary>
        /// 装载Bundle
        /// </summary>
        public void Resolve()
        {
            log.Debug(string.Format("模块{0}加载开始！", this.GetBundleAssemblyFileName()));

            //加载插件Lib目录下的程序集
            LoadShareAssemblys();

            //加载依赖Bundle
            LoadRequiredBundle();

            //加载程序集
            LoadAssemblys();

            //搜索Activator
            LoadActivator();

            //解析Bundle完成
            this.state = BundleStateConst.RESOLVED;

            var frameworkFireEvent = (IFrameworkFireEvent)framework;
            frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.RESOLVED, this));

            log.Debug(string.Format("模块{0}加载结束！", this.GetBundleAssemblyFileName()));

        }

        /// <summary>
        /// 1.加载依赖Bundle
        /// </summary>
        private void LoadRequiredBundle()
        {
            log.Debug("加载相关依赖模块！");

            requiredBundleList = new List<Bundle>();
            var requireBundles = GetRequireBundleList();
            //加载Required-Bundle中的所有程序集
            foreach (string tmpStr in requireBundles)
            {
                string requireBundleString = tmpStr.Trim();
                if (string.IsNullOrEmpty(requireBundleString)) continue;

                string requireBundleName = null;
                string requireBundleVersionString = null;
                this.ParseRequireBundleVersion(requireBundleString, out requireBundleName, out requireBundleVersionString);

                Bundle matchBundle = null;
                List<Bundle> matchBundleList = new List<Bundle>();
                foreach (IBundle bundle in this.GetBundleContext().GetBundles())
                {
                    Bundle tmpBundle = bundle as Bundle;

                    if (this.Equals(bundle) || tmpBundle == null) continue;

                    if (requireBundleName.Equals(tmpBundle.GetSymbolicName()))
                    {
                        //如果没有版本信息则直接匹配上
                        if (string.IsNullOrEmpty(requireBundleVersionString))
                        {
                            matchBundle = tmpBundle;
                            break;
                        }
                        else
                        {
                            //否则根据添加至匹配Bundle
                            matchBundleList.Add(tmpBundle);
                        }
                    }
                }
                //未找到，并且有版本信息
                if (matchBundle == null && !string.IsNullOrEmpty(requireBundleVersionString))
                {
                    Version requireBundleVersion = new Version(requireBundleVersionString);

                    matchBundleList.Sort(new Comparison<Bundle>(delegate(Bundle x, Bundle y)
                    {
                        return x.GetVersion().CompareTo(y.GetVersion());
                    }));
                    //找到匹配的一个版本或比匹配版本高一个版本的Bundle
                    foreach (Bundle tmpBundle in matchBundleList)
                    {
                        if (tmpBundle.GetVersion().CompareTo(requireBundleVersion) >= 0)
                        {
                            matchBundle = tmpBundle;
                            break;
                        }
                    }
                }
                if (matchBundle == null)
                    continue;
                //如果此Bundle没有解析，则解析此Bundle
                if (matchBundle.GetState() != BundleStateConst.RESOLVED
                    && matchBundle.GetState() != BundleStateConst.ACTIVE)
                {
                    matchBundle.Resolve();
                }
                requiredBundleList.Add(matchBundle);
            }
        }


        /// <summary>
        /// 从共享程序集LIB目录和根目录中配置项加载程序集
        /// </summary>
        /// <returns></returns>
        private void LoadShareAssemblys()
        {
            log.Debug("框架插件共享程序集！");

            string sharedLibsDirectory = GetBundleLibsDirectoryName();
            if (Directory.Exists(GetBundleLibsDirectoryName()))
            {
                var files = Directory.GetFiles(sharedLibsDirectory, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.ToLower().EndsWith(".dll") || s.ToLower().EndsWith(".exe")).ToList();
                foreach (var file in files)
                {
                    LoadShareAssemblyByReflect(file);
                }
            }
        }

        /// <summary>
        /// 通过反射加载程序集
        /// </summary>
        /// <param name="assemblyFileName"></param>
        /// <returns></returns>
        private void LoadShareAssemblyByReflect(string assemblyFileName)
        {
            try
            {
                AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFileName);
                if (!BundleAssemblyProvider.CheckHasShareLib(assemblyDefinition.FullName))
                {
                    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().ToList()
                        .FirstOrDefault(item => item.FullName == assemblyDefinition.FullName);

                    if (assembly == null)
                    {
                        if (BundleConfigProvider.HOTINST_OSGI_IS_DEBUG_MODE)
                            assembly = Assembly.LoadFrom(assemblyFileName);
                        else
                            assembly = Assembly.Load(File.ReadAllBytes(assemblyFileName));
                    }

                    if (BundleConfigProvider.HOTINST_OSGI_ALLTYPES_LOAD)
                        assembly.GetTypes();

                    log.Debug(string.Format("框架加载共享程序集[{0}]！", assembly.GetName().Name));

                    BundleAssemblyProvider.AddShareAssembly(assembly.FullName, assembly);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("加载共享程序集[{0}]时出现异常！", assemblyFileName), ex);
            }
        }

        /// <summary>
        /// 2.加载所需的所有程序集
        /// </summary>
        private void LoadAssemblys()
        {
            log.Debug("模块加载自身程序集！");

            AssemblyDefinition bundleAssemblyDefinition = AssemblyDefinition.ReadAssembly(bundleAssemblyFileName);
            AssemblyNameDefinition bundleAssemblyNameDefinition = bundleAssemblyDefinition.Name;

            //去掉强签名
            BundleUtils.RemoveAssemblyStrongName(bundleAssemblyNameDefinition);

            foreach (ModuleDefinition moduleDefinition in bundleAssemblyDefinition.Modules)
            {
                foreach (AssemblyNameReference assemblyNameReference in moduleDefinition.AssemblyReferences)
                {
                    string assemblyName = assemblyNameReference.Name;

                    //是否属于FCL
                    if (BundleUtils.IsAssemblyBelongsFcl(assemblyName))
                    {
                        continue;
                    }

                    //依赖项是否是Bundle
                    Bundle bundle = GetBundleFromRequiredBundles(assemblyName);
                    if (bundle != null)
                    {
                        AssemblyName requiredBundleAssemblyName = bundle.bundleAssembly.GetName();

                        assemblyNameReference.Name = requiredBundleAssemblyName.Name;
                        assemblyNameReference.Version = requiredBundleAssemblyName.Version;
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);

                        log.Debug(string.Format("模块关联引用模块[{0}]！", assemblyName));

                        continue;
                    }

                    //1.首先尝试从共享程序集加载
                    if (BundleAssemblyProvider.CheckHasShareLib(assemblyNameReference.FullName))
                    {
                        //关联共享程序集
                        var shareAssembly = BundleAssemblyProvider.GetShareAssembly(assemblyNameReference.FullName);

                        assemblyNameReference.Name = shareAssembly.GetName().Name;
                        assemblyNameReference.Version = shareAssembly.GetName().Version;
                        //目前共项目为反射，不移除强命名
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);
                        log.Debug(string.Format("模块关联共享库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    //2.尝试从当前Bundle已加载的程序集关联
                    var bundleRefAssembly = GetBundleRefAssembly(assemblyNameReference.FullName);
                    if (bundleRefAssembly != null)
                    {
                        assemblyNameReference.Name = bundleRefAssembly.GetName().Name;
                        assemblyNameReference.Version = bundleRefAssembly.GetName().Version;
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    //3.尝试从lib目录重新加载
                    var newAssemblyDefinition = LoadAssemblyFromLibDir(assemblyName);
                    if (newAssemblyDefinition != null)
                    {
                        assemblyNameReference.Name = newAssemblyDefinition.Name.Name;
                        assemblyNameReference.Version = newAssemblyDefinition.Name.Version;
                        BundleUtils.RemoveAssemblyStrongName(assemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    //如果按如上步骤仍未发现程序集，抛出异常
                    throw new IOException(string.Format("{0}不能解析依赖的程序集[{1}]", this.ToString(), assemblyName));
                }
                moduleDefinition.Attributes &= ~ModuleAttributes.StrongNameSigned;
            }
            

            //是否Debug模式，如果debug，需要反射方式才能保证程序能进入Bundle断点
            if (BundleConfigProvider.HOTINST_OSGI_IS_DEBUG_MODE)
            {
                bundleAssembly = Assembly.LoadFrom(bundleAssemblyFileName);
            }                
            else
            {
                MemoryStream ms = new MemoryStream();
                bundleAssemblyDefinition.Write(ms);
                bundleAssembly = Assembly.Load(ms.ToArray());
            }
                
            //加载程序集中所有的类
            bundleAssembly.GetTypes();
            //将bundle程序集加入程序集提供程序
            AddRefAssembly(bundleAssemblyNameDefinition.FullName, bundleAssembly, bundleAssemblyDefinition);
        }

        /// <summary>
        /// 3.读取Activator
        /// </summary>
        private void LoadActivator()
        {
            log.Debug("模块搜索激活器！");

            //搜索Activator
            this.activatorClass = null;
            foreach (Type type in bundleAssembly.GetTypes())
            {
                if (typeof(IBundleActivator).IsAssignableFrom(type))
                {
                    this.activatorClass = type;
                    metaDataDictionary.Add(BundleConst.BUNDLE_MANIFEST_ACTIVATOR, activatorClass.FullName);
                    break;
                }
            }

            if (null == this.activatorClass)
            {
                log.Error($"插件{this.bundleAssemblyFullName}中没有搜索到可用的激活器.");
            }
        }


        /// <summary>
        /// 从LIB目录加载程序集并返回新的程序集名称
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>程序集定义</returns>
        private AssemblyDefinition LoadAssemblyFromLibDir(string assemblyName)
        {

            //如果是.NET运行库自带的程序集,或本框架程序集
            if (BundleUtils.IsAssemblyBelongsFcl(assemblyName))
                return null;

            string libsDirPath = GetBundleLibsDirectoryName();
            if (!Directory.Exists(libsDirPath))
                Directory.CreateDirectory(libsDirPath);

            //string assemblyFileName = Path.Combine(libsDirPath, assemblyName + ".dll");
            string[] files = Directory.GetFiles(libsDirPath, assemblyName + ".dll", SearchOption.AllDirectories);

            if (files.Length == 0)
                return null;

            var assemblyFileName = files[0];

            var readerParameter = new ReaderParameters()
            {
                AssemblyResolver = new CustomAssemblyResolver(
                    resolverAssemblyFullName =>
                    {
                        var resolverAssemblyName = new AssemblyName(resolverAssemblyFullName);
                        var resolverDefinition = this.GetBundleRefDefinition(resolverAssemblyFullName);
                        if (resolverDefinition == null)
                            return LoadAssemblyFromLibDir(resolverAssemblyName.Name);
                        return resolverDefinition;
                    })
            };

            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFileName, readerParameter);
            AssemblyNameDefinition assemblyNameDefinition = assemblyDefinition.Name;

            //去掉强签名
            BundleUtils.RemoveAssemblyStrongName(assemblyNameDefinition);

            //目前机制采用修订版本号方式控制程序集隔离
            MarkAssembleyVersion(assemblyNameDefinition);

            foreach (ModuleDefinition moduleDefinition in assemblyDefinition.Modules)
            {
                foreach (AssemblyNameReference refAssemblyNameReference in moduleDefinition.AssemblyReferences)
                {
                    string refAssemblyName = refAssemblyNameReference.Name;
                    //1.共享库优先
                    if (BundleAssemblyProvider.CheckHasShareLib(refAssemblyNameReference.FullName))
                    {
                        var shareAssembly = BundleAssemblyProvider.GetShareAssembly(refAssemblyNameReference.FullName);
                        refAssemblyNameReference.Name = shareAssembly.GetName().Name;
                        refAssemblyNameReference.Version = shareAssembly.GetName().Version;
                        //目前共项目为反射，不移除强命名

                        log.Debug(string.Format("模块关联共享库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    //2.尝试从当前Bundle已加载的程序集关联
                    var bundleRefAssembly = GetBundleRefAssembly(refAssemblyNameReference.FullName);
                    if (bundleRefAssembly != null)
                    {
                        refAssemblyNameReference.Name = bundleRefAssembly.GetName().Name;
                        refAssemblyNameReference.Version = bundleRefAssembly.GetName().Version;
                        BundleUtils.RemoveAssemblyStrongName(refAssemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    //3.Bundle Lib文件夹
                    var newRefAssemblyDefinition = LoadAssemblyFromLibDir(refAssemblyName);
                    if (newRefAssemblyDefinition != null)
                    {
                        refAssemblyNameReference.Name = newRefAssemblyDefinition.Name.Name;
                        refAssemblyNameReference.Version = newRefAssemblyDefinition.Name.Version;
                        BundleUtils.RemoveAssemblyStrongName(refAssemblyNameReference);

                        log.Debug(string.Format("模块关联自身库程序集[{0}]！", assemblyName));

                        continue;
                    }

                    //目前对于依赖程序集的引用程序集如无发现，不做处理
                }
                moduleDefinition.Attributes &= ~ModuleAttributes.StrongNameSigned;
            }
            

            Assembly assembly;
            if (BundleConfigProvider.HOTINST_OSGI_IS_DEBUG_MODE)
            {
                assembly = Assembly.LoadFrom(assemblyFileName);
            }
            else
            {
                MemoryStream ms = new MemoryStream();
                assemblyDefinition.Write(ms);
                assembly = Assembly.Load(ms.ToArray());
            }
                
            //将Lib程序集加载到程序集提供程序
            if (BundleConfigProvider.HOTINST_OSGI_ALLTYPES_LOAD)
            {
                assembly.GetTypes();
            }

            log.Debug(string.Format("模块加载依赖程序集[{0}]！", assemblyName));

            AddRefAssembly(assemblyNameDefinition.FullName, assembly, assemblyDefinition);

            return assemblyDefinition;
        }



        /// <summary>
        /// 重名程序集生成唯一版本号
        /// </summary>
        /// <param name="assemblyNameReference">程序集Name引用</param>
        /// <returns>新版本</returns>
        private Version MarkAssembleyVersion(AssemblyNameReference assemblyNameReference)
        {
            //如果其他Bundle管理程序集已经存在
            if (BundleAssemblyProvider.CheckHasBundleLib(assemblyNameReference.FullName))
            {
                var version = assemblyNameReference.Version;
                var fixVersion = new Random().Next(0, 100);
                var newVersion = new Version(version.Major, version.Minor, version.Build, fixVersion);
                assemblyNameReference.Version = newVersion;

                if (BundleAssemblyProvider.CheckHasBundleLib(assemblyNameReference.FullName))
                {
                    this.MarkAssembleyVersion(assemblyNameReference);
                }
            }
            return assemblyNameReference.Version;
        }


        /// <summary>
        /// 获取依赖Bundle信息
        /// </summary>
        /// <returns></returns>
        private List<string> GetRequireBundleList()
        {
            var requireBundleStr = metaDataDictionary[BundleConst.BUNDLE_MANIFEST_REQUIRE_BUNDLE];
            return requireBundleStr.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 获取依赖Bundle及版本
        /// </summary>
        /// <param name="requireBundleString"></param>
        /// <param name="requireBundleName"></param>
        /// <param name="requireBundleVersionString"></param>
        private void ParseRequireBundleVersion(string requireBundleString, out string requireBundleName, out string requireBundleVersionString)
        {
            string[] requireBundleStringArray = requireBundleString.Split(new Char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            requireBundleName = requireBundleStringArray[0].Trim();
            requireBundleVersionString = string.Empty;
            IDictionary<string, string> otherDict = new Dictionary<string, string>();
            for (int i = 1; i < requireBundleStringArray.Length; i++)
            {
                string requireBundleStringPart = requireBundleStringArray[i];
                string[] requireBundleStringPartStringArray = requireBundleStringPart.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                otherDict.Add(requireBundleStringPartStringArray[0].Trim(), requireBundleStringPartStringArray[1].Trim());
            }
            //得到需要的版本信息
            if (otherDict.ContainsKey(BundleConst.BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION))
            {
                requireBundleVersionString = otherDict[BundleConst.BUNDLE_MANIFEST_REQUIRED_BUNDLE_VERSION];
                requireBundleVersionString = requireBundleVersionString.Replace("\"", "");
            }
        }


        /// <summary>
        /// 根据符号名称从已装载的Bundle列表获取依赖Bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        private Bundle GetBundleFromRequiredBundles(string bundleName)
        {
            foreach (Bundle bundle in requiredBundleList)
            {
                if (bundle.GetSymbolicName().Equals(bundleName))
                    return bundle;
            }
            return null;
        }

        /// <summary>
        /// 得到Bundle依赖库的目录名称
        /// </summary>
        /// <returns></returns>
        private string GetBundleLibsDirectoryName()
        {
            return Path.Combine(bundleDirectoryPath, BundleConst.BUNDLE_LIBS_DIRECTORY_NAME);
        }


        /// <summary>
        /// 添加引用程序集
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <param name="assemblyDefinition"></param>
        /// <param name="assembly"></param>
        private void AddRefAssembly(string assemblyFullName, Assembly assembly, AssemblyDefinition assemblyDefinition)
        {
            if (!bundleRefAssemblyDict.ContainsKey(assemblyFullName))
            {
                bundleRefAssemblyDict.Add(assemblyFullName, assembly);
                bundleRefDefinitionDict.Add(assemblyFullName, assemblyDefinition);
            }
            BundleAssemblyProvider.AddAssembly(assemblyFullName, assembly);
        }

        /// <summary>
        /// 获取当前Bundle已经引用的程序集
        /// </summary>
        /// <param name="assemblyFullName"></param>
        private Assembly GetBundleRefAssembly(string assemblyFullName)
        {
            var refAssemblyName = new AssemblyName(assemblyFullName);
            var bundleRefAssembly = bundleRefAssemblyDict.FirstOrDefault(
                          keyValuePair =>
                          {
                              var bundleRefAssemblyName = new AssemblyName(keyValuePair.Key);
                              if (bundleRefAssemblyName.Name == refAssemblyName.Name
                                  && bundleRefAssemblyName.Version == refAssemblyName.Version)
                                  return true;
                              return false;
                          });
            return bundleRefAssembly.Value;
        }

        /// <summary>
        /// 获取当前Bundle已经引用的程序集定义
        /// </summary>
        /// <param name="assemblyFullName"></param>
        private AssemblyDefinition GetBundleRefDefinition(string assemblyFullName)
        {
            var refAssemblyName = new AssemblyName(assemblyFullName);
            var bundleRefDefinition = bundleRefDefinitionDict.FirstOrDefault(
                          keyValuePair =>
                          {
                              var bundleRefAssemblyName = new AssemblyName(keyValuePair.Key);
                              if (bundleRefAssemblyName.Name == refAssemblyName.Name
                                  && bundleRefAssemblyName.Version == refAssemblyName.Version)
                                  return true;
                              return false;
                          });
            return bundleRefDefinition.Value;
        }


        #endregion

        #region Stop

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            log.Debug(string.Format("模块{0}停止开始！", this.GetBundleAssemblyFileName()));

            if (this.state == BundleStateConst.ACTIVE)
            {
                this.state = BundleStateConst.STOPPING;
                if (bundleActivator != null)
                {
                    var frameworkFireEvent = (IFrameworkFireEvent)framework;
                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STOPPING, this));

                    log.Debug("卸载扩展点数据！");
                    UnLoadExtensions();

                    log.Debug("激活器停止");
                    bundleActivator.Stop(bundleContext);

                    ((BundleContext)bundleContext).Stop();

                    this.state = BundleStateConst.RESOLVED;

                    frameworkFireEvent.FireBundleEvent(new BundleEventArgs(BundleEventArgs.STOPPED, this));

                    bundleActivator = null;
                }
            }

            log.Debug(string.Format("模块{0}停止结束！", this.GetBundleAssemblyFileName()));

        }

        /// <summary>
        /// 卸载扩展点数据
        /// </summary>
        private void UnLoadExtensions()
        {
            log.Debug("扩展点卸载开始");

            //扩展已装载的扩展点
            foreach (IBundle bundle in this.framework.GetBundles())
            {
                var bundlePoints = bundle.GetExtensionPoints();
                foreach (ExtensionPoint extensionPoint in bundlePoints)
                {
                    var extensionData = extensionDatas.FirstOrDefault(item => item.Name == extensionPoint.Name);
                    if (extensionData != null)
                    {
                        log.Debug(string.Format("扩展点{0}卸载！", extensionPoint.Name));

                        var fireContext = (IContextFireEvent)extensionPoint.Owner.GetBundleContext();
                        fireContext.FireExtensionChanged(this, new ExtensionEventArgs(ExtensionEventArgs.UNLOAD, extensionPoint, extensionData));
                    }
                }
            }

            this.extensionDatas.Clear();
            this.extensionPoints.Clear();

            log.Debug("扩展点卸载停止");
        }


        #endregion

        #region Uninstall

        /// <summary>
        /// 卸载组件
        /// </summary>
        public void UnInstall()
        {
            log.Debug("模块卸载开始！");

            ((IFrameworkInstaller)framework).UnInstall(this);
            this.RemoveAllRefAssembly();
            //移除Bundle配置信息
            BundleConfigProvider.RemoveBundleConfig(this.GetBundleFolderName());

            log.Debug("模块卸载完成！");

        }

        #endregion

        #region Update

        /// <summary>
        /// 指定路径更新当前Bundle
        /// </summary>
        /// <param name="zipFile">更新的Bundle路径</param>
        public void Update(string zipFile)
        {
            log.Debug("模块更新开始！");

            int preState = state;
            if (File.Exists(zipFile))
            {
                string tmpBundleDirectoryPath = string.Format("{0}_{1}", this.GetBundleFolderName(), Guid.NewGuid().ToString());
                BundleUtils.ExtractBundleFile(tmpBundleDirectoryPath, zipFile);
                var xmlNode = BundleConfigProvider.ReadBundleConfig(tmpBundleDirectoryPath);
                try
                {
                    var tmpassemblyName = BundleConfigProvider.GetBundleConfigAssemblyName(xmlNode);
                    var assemblyName = this.GetBundleAssemblyFileName();
                    if (!assemblyName.Equals(tmpassemblyName))
                    {
                        throw new Exception(string.Format("要更新的插件[{0}]与输入流中的插件[{1}]不匹配！", assemblyName, tmpassemblyName));
                    }
                }
                catch (Exception ex)
                {
                    Directory.Delete(tmpBundleDirectoryPath, true);
                    throw ex;
                }
                //删除当前bundle
                ((IFrameworkInstaller)framework).Delete(this);
                //替换当前bundle
                Directory.Move(tmpBundleDirectoryPath, bundleDirectoryPath);

                //更新配置节点
                this.manifestData = xmlNode;

                //更新Bundle配置信息
                BundleConfigProvider.SyncBundleConfig(this.GetBundleFolderName(), xmlNode);
            }
            //目前插件持续状态存在三种
            if (preState == BundleStateConst.INSTALLED)
            {
                this.Init();
            }
            else if (preState == BundleStateConst.RESOLVED)
            {
                this.Init();
                this.Resolve();
            }
            else if (preState == BundleStateConst.ACTIVE)
            {
                this.Stop();
                this.Init();
                this.Resolve();
                this.Start();
            }
            log.Debug("模块更新完成！");
        }

        #endregion

        #region Public

        /// <summary>
        /// 获取当前Bundle上下文对象
        /// </summary>
        /// <returns>Bundle上下文对象</returns>
        public IBundleContext GetBundleContext()
        {
            return this.bundleContext;
        }


        /// <summary>
        /// 获取当前Bundle状态
        /// </summary>
        /// <returns>Bundle状态</returns>
        public int GetState()
        {
            return this.state;
        }

        /// <summary>
        /// 获取当前Bundle版本信息
        /// </summary>
        /// <returns>Bundle版本信息</returns>
        public Version GetVersion()
        {
            return this.bundleVersion;
        }

        /// <summary>
        /// 获取当前Bundle的描述信息
        /// </summary>
        /// <returns></returns>
        public String GetDescription()
        {
            return this.description;
        }


        /// <summary>
        /// 获取当前Bundle符号名称
        /// </summary>
        /// <returns>Bundle符号名称</returns>
        public string GetSymbolicName()
        {
            return this.bundleSymbolicName;
        }

        /// <summary>
        /// 获取当前Bundle程序集全名
        /// </summary>
        /// <returns>Bundle程序集全名</returns>
        public string GetBundleAssemblyFullName()
        {
            return bundleAssemblyFullName;
        }

        /// <summary>
        /// 获取当前Bundle程序集清单数据
        /// </summary>
        /// <returns>Bundle程序清单数据</returns>
        public IDictionary<string, string> GetManifest()
        {
            return new Dictionary<string, string>(metaDataDictionary);
        }

        /// <summary>
        /// 获取当前Bundle目录
        /// </summary>
        /// <returns>Bundle目录</returns>
        public string GetBundleDirectoryPath()
        {
            return bundleDirectoryPath;
        }

        /// <summary>
        /// 获取当前Bundle程序集文件名称
        /// </summary>
        /// <returns>Bundle程序集文件名称</returns>
        public string GetBundleAssemblyFileName()
        {
            return BundleConfigProvider.GetBundleConfigAssemblyName(this.manifestData);
        }

        /// <summary>
        /// 获取当前Bundle启动级别
        /// </summary>
        /// <returns>Bundle启动级别</returns>
        public int GetBundleStartLevel()
        {
            return BundleConfigProvider.GetBundleConfigStartLevel(this.manifestData);
        }

        /// <summary>
        /// 获取模块清单数据
        /// </summary>
        /// <returns>清单数据节点</returns>
        public XmlNode GetBundleManifestData()
        {
            return this.manifestData.Clone();
        }

        /// <summary>
        /// 获取当前Bundle扩展点
        /// </summary>
        /// <returns>扩展点列表</returns>
        public IList<ExtensionPoint> GetExtensionPoints()
        {
            var tmpExtensionPoints = new ExtensionPoint[extensionPoints.Count];
            extensionPoints.CopyTo(tmpExtensionPoints, 0);
            return tmpExtensionPoints.ToList();
        }

        /// <summary>
        /// 获取当前Bundle扩展的扩展数据
        /// </summary>
        /// <returns>扩展数据列表</returns>
        public IList<ExtensionData> GetExtensionDatas()
        {
            var tmpExtensionDatas = new ExtensionData[extensionDatas.Count];
            extensionDatas.CopyTo(tmpExtensionDatas, 0);
            return tmpExtensionDatas.ToList();
        }

        #endregion

        #endregion

        #region override Equals

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            var bundle = obj as Bundle;

            if (bundle == null)
            {
                return false;
            }

            return this.bundleAssemblyFullName == bundle.bundleAssemblyFullName
                && this.bundleVersion == bundle.bundleVersion;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.Format("{0}^{1}",
                this.bundleAssemblyFullName, this.bundleVersion.ToString()).GetHashCode();
        }

        #endregion

        #region override ToString
        public override string ToString()
        {
            return string.Format("Bundle[AssemblyName:{0},SymbolicName:{1},Version:{2}]", bundleAssemblyFullName, bundleSymbolicName, bundleVersion);
        }
        #endregion

    }
}
