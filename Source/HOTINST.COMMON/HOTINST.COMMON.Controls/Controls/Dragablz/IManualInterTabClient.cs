namespace HOTINST.COMMON.Controls.Controls.Dragablz
{
	/// <summary>
	/// 
	/// </summary>
    public interface IManualInterTabClient : IInterTabClient
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
        void Add(object item);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
        void Remove(object item);
    }
}