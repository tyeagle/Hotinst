/**
 * ==============================================================================
 *
 * ClassName: HsvColor
 * Description: 
 *
 * Version: 1.0
 * Created: 2017/3/30 10:45:05
 * Compiler: Visual Studio 2015
 * CLR Version: 4.0.30319.42000
 *
 * Author: caixs
 * Company: hotinst
 *
 * ==============================================================================
 */

namespace HOTINST.COMMON.Controls.Core
{
	/// <summary>
	/// 
	/// </summary>
	internal struct HsvColor
	{
		#region props

		public double H;
		public double S;
		public double V;

		#endregion

		#region .ctor

		public HsvColor(double h, double s, double v)
		{
			H = h;
			S = s;
			V = v;
		}

		#endregion
	}
}