﻿using System;
using System.Resources;
using System.Windows.Markup;

namespace HOTINST.COMMON.Localization
{
    /// <summary>
    /// Creates a resource manager.
    /// </summary>
    [MarkupExtensionReturnType(typeof(ResourceManager))]
    public class ResourceManagerExtension : MarkupExtension
    {
        /// <summary>
        /// The type to use to initialize the <see cref="ResourceManager"/>.
        /// </summary>
        /// <remarks>
        /// Either <see cref="Type"/> or <see cref="AssemblyName"/> and <see cref="BaseName"/>
        /// must be specified. Depending on the specified properties the corresponding constructor
        /// of the <see cref="ResourceManager"/> class is called. If both kind of values are specified
        /// <see cref="Type"/> is used.
        /// </remarks>
        public Type Type { get; set; }

        /// <summary>
        /// The name of the assembly that contains the resources.
        /// </summary>
        /// <remarks>
        /// Either <see cref="Type"/> or <see cref="AssemblyName"/> and <see cref="BaseName"/>
        /// must be specified. Depending on the specified properties the corresponding constructor
        /// of the <see cref="ResourceManager"/> class is called. If both kind of values are specified
        /// <see cref="Type"/> is used.
        /// </remarks>
        public string AssemblyName { get; set; }

        /// <summary>
        /// The root name of the resources.
        /// </summary>
        /// <remarks>
        /// Either <see cref="Type"/> or <see cref="AssemblyName"/> and <see cref="BaseName"/>
        /// must be specified. Depending on the specified properties the corresponding constructor
        /// of the <see cref="ResourceManager"/> class is called. If both kind of values are specified
        /// <see cref="Type"/> is used.
        /// </remarks>
        public string BaseName { get; set; }

	    private ResourceManager _manager;

		/// <summary>
		/// 
		/// </summary>
        public ResourceManagerExtension() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
        public ResourceManagerExtension(Type type)
        {
            Type = type;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_manager == null)
            {
                if (Type != null)
                {
                    _manager = LocalizationManager.LoadResourceManager(Type);
                }
                else if (string.IsNullOrEmpty(AssemblyName) || string.IsNullOrEmpty(BaseName))
                {
                    return null;
                }
                else
                {
                    _manager = LocalizationManager.LoadResourceManager(AssemblyName, BaseName);
                }
            }

            return _manager;
        }
    }
}
