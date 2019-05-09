using System.Collections.Generic;
using System.Linq;

namespace HOTINST.COMMON.DynamicExpresso
{
	/// <summary>
	/// 
	/// </summary>
	public class IdentifiersInfo
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="unknownIdentifiers"></param>
		/// <param name="identifiers"></param>
		/// <param name="types"></param>
		public IdentifiersInfo(
			IEnumerable<string> unknownIdentifiers,
			IEnumerable<Identifier> identifiers,
			IEnumerable<ReferenceType> types)
		{
			UnknownIdentifiers = unknownIdentifiers.ToList();
			Identifiers = identifiers.ToList();
			Types = types.ToList();
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<string> UnknownIdentifiers { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<Identifier> Identifiers { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<ReferenceType> Types { get; private set; }
	}
}
