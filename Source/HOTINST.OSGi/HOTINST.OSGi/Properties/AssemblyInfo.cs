using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("HOTINST.OSGI")]
[assembly: AssemblyDescription("插件平台基础库")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("www.hotinst.com")]
[assembly: AssemblyProduct("HOTINST.OSGI")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyTrademark("Hotinst")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyKeyFile("Properties/hotinst.snk")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1F11136D-F455-4CEB-987D-160283264C01")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.1.2")]
[assembly: AssemblyFileVersion("1.0.1.1228")]

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]