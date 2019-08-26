/**
 * ==============================================================================
 *
 * Filename: ICDWord
 * Description: 描述ICD文件结构
 *
 * Version: 1.0
 * Created: 2016/5/24 17:53:08
 * Compiler: Visual Studio 2013
 *
 * Author: cc
 * Company: hotinst
 *
 * ==============================================================================
 */

using System;
using HOTINST.COMMON.Bitwise;

#pragma warning disable 1591

namespace HOTINST.ICD
{
	/// <summary>
	/// ICD结构类。
	/// </summary>
	[Serializable]
	public class ICDWord
	{
		#region MyRegion

		private DataType _innerType;
		private DataType _type;
		private uint _offset;
		private uint _startBit;
		private uint _bitWidth;
		private Endian _endian;
		private string _name;
		private string _groupName;
		private string _description;
		private string _contract;
		private string _systemName;
		private bool _isCallFunction;
		private double _param1;
		private double _param2;
		private double _param3;
		private double _param4;
		private string _functionName;
		private double _min;
		private double _max;
		private string _unit;
		private string _valueTextList;
		private string _defaultValue;
		private bool _dataValid;
		private string _tag;
		private AccessMode _access;

		//[XmlIgnore]
		//public bool EnableSet { get; set; }

		//[XmlIgnore]
		//public ICDWords IcdFile { get; }

		/// <summary>
		/// 信号起始字节，指存放信号的这个底层数据在帧缓冲区中的字节偏移
		/// </summary>
		public uint Offset { get => _offset; set => _offset = value; }
		/// <summary>
		/// 起始位偏移，指信号在底层数据类型中的起始位偏移，如果InnerType=Int8,StartBit[0-7],InnerTYpe=UInt16,StartBit[0-15]
		/// </summary>
		public uint StartBit { get => _startBit; set => _startBit = value; }
		/// <summary>
		/// 位宽，指信号在底层数据类型中的位宽度,BitWidth+StartBit表示的bit数目不应该超过InnerType表示的类型所占用的最大位数
		/// </summary>
		public uint BitWidth { get => _bitWidth; set => _bitWidth = value; }
		/// <summary>
		/// 字节序，指InnerType表示的底层数据在缓冲区中存放时使用的字节序列，只有InnerType表示的底层类型超过1字节时有效
		/// </summary>
		public Endian Endian { get => _endian; set => _endian = value; }
		/// <summary>
		/// 底层数据类型，指该信号所存放的内存区域应该按该类型来读取
		/// </summary>
		public DataType InnerType
		{
			get => _innerType;
			set => _innerType = value;
		}
		/// <summary>
		/// 信号值类型，可与InnerType不一样，比如一个电压信号,Type=Double,但是InnerType=UInt16,即传输过程中使用16位整形表示
		/// 一个电压值，在计算或展示时，通过标定算法将这个UInt8通过标定算法转换为一个Double，电压=UInt16/65535*10;
		/// </summary>
		public DataType Type
		{
			get => _type;
			set => _type = value;
		}
		/// <summary>
		/// 信号名称
		/// </summary>
		public string Name { get => _name; set => _name = value; }
		/// <summary>
		/// 信号组名称，
		/// </summary>
		public string GroupName { get => _groupName; set => _groupName = value; }
		/// <summary>
		/// 信号描述信息
		/// </summary>
		public string Description { get => _description; set => _description = value; }
		/// <summary>
		/// 协议(使能信号配置)
		/// </summary>
		public string Contract { get => _contract; set => _contract = value; }
		/// <summary>
		/// 系统名称，该信号所属性的系统，
		/// </summary>
		public string SystemName { get => _systemName; set => _systemName = value; }
		/// <summary>
		/// 信号是否需要标定
		/// </summary>
		public bool IsCallFunction
		{
			get => _isCallFunction;
			set
			{
				_isCallFunction = value;
				//UpdateIcdWord(nameof(IsCallFunction), value);
			}

		}
		/// <summary>
		/// 信号标定参数
		/// </summary>
		public double Param1
		{
			get => _param1;
			set
			{
				_param1 = value;
				//UpdateIcdWord(nameof(Param1), value);
			}
		}
		/// <summary>
		/// 信号标定参数
		/// </summary>
		public double Param2
		{
			get => _param2;
			set
			{
				_param2 = value;
				//UpdateIcdWord(nameof(Param2), value);
			}
		}
		/// <summary>
		/// 信号标定参数
		/// </summary>
		public double Param3
		{
			get => _param3;
			set
			{
				_param3 = value;
				//UpdateIcdWord(nameof(Param3), value);
			}
		}
		/// <summary>
		/// 信号标定参数
		/// </summary>
		public double Param4
		{
			get => _param4;
			set
			{
				_param4 = value;
				//UpdateIcdWord(nameof(Param4), value);
			}
		}

		/// <summary>
		/// 信号标定算法
		/// </summary>
		public string FunctionName
		{
			get => _functionName;
			set
			{
				_functionName = value;
				//UpdateIcdWord(nameof(FunctionName), value);
			}
		}
		/// <summary>
		/// 信号最小值
		/// </summary>
		public double Min { get => _min; set => _min = value; }
		/// <summary>
		/// 信号最大值
		/// </summary>
		public double Max { get => _max; set => _max = value; }
		/// <summary>
		/// 信号的物理单位
		/// </summary>
		public string Unit { get => _unit; set => _unit = value; }
		public string ValueTextList { get => _valueTextList; set => _valueTextList = value; }
		public string DefaultValue { get => _defaultValue; set => _defaultValue = value; }
		public bool DataValid { get => _dataValid; set => _dataValid = value; }
		public string Tag { get => _tag; set => _tag = value; }
		public AccessMode Access { get => _access; set => _access = value; }

		#endregion

		#region MyRegion

		public ICDWord()
		{
			
		}

		//public ICDWord(ICDWords icdFile)
		//{
		//	IcdFile = icdFile;
		//}

		#endregion

		//private void UpdateIcdWord(string prop, object value)
		//{
		//	if(EnableSet)
		//	{
		//		try
		//		{
		//			if(!File.Exists(IcdFile.FileName))
		//			{
		//				_logger.Error($"ICD文件[{IcdFile.FileName}]不存在。");
		//				return;
		//			}

		//			if(Path.GetExtension(IcdFile.FileName) == ".xlsx")
		//			{
		//				UpdateExcel2007(IcdFile, prop, value);
		//			}
		//			else if(Path.GetExtension(IcdFile.FileName) == ".xls")
		//			{
		//				UpdateExcel2003(IcdFile, prop, value);
		//			}
		//			else if(Path.GetExtension(IcdFile.FileName) == ".xml")
		//			{
		//				UpdateXML(IcdFile.FileName, prop, value);
		//			}
		//			else
		//			{
		//				throw new Exception($"文件格式不正确：{IcdFile.FileName}");
		//			}
		//		}
		//		catch(Exception ex)
		//		{
		//			_logger.Error($"更新信号值失败：{ex.Message}");
		//			throw;
		//		}
		//	}
		//}
		
		#region excel

		//private void UpdateExcel2003(ICDWords icdFile, string prop, object value)
		//{
		//	using(FileStream fs = new FileStream(icdFile.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
		//	{
		//		HSSFWorkbook workbook = new HSSFWorkbook(fs);
		//		ISheet sheet = workbook.GetSheet(icdFile.Name);
		//		UpdateExcel(sheet, prop, value);

		//		workbook.Write(fs);
		//		fs.Close();
		//	}
		//}

		//private void UpdateExcel2007(ICDWords icdFile, string prop, object value)
		//{
		//	FileStream fs = new FileStream(icdFile.FileName, FileMode.Open, FileAccess.ReadWrite);
		//	IWorkbook workbook = new XSSFWorkbook(fs);

		//	ISheet sheet = workbook.GetSheet(icdFile.Name);
		//	UpdateExcel(sheet, prop, value);

		//	fs.Dispose();

		//	using(FileStream wook = new FileStream(icdFile.FileName, FileMode.Create, FileAccess.Write))
		//	{
		//		workbook.Write(wook);
		//	}

		//	//XSSFWorkbook book = new XSSFWorkbook(icdFile.FileName);

		//	//ISheet sheet = book.GetSheet(icdFile.Name);
		//	//UpdateExcel(sheet, prop, value);

		//	////转为字节数组
		//	//MemoryStream stream = new MemoryStream();
		//	//book.Write(stream);
		//	//book.Close();
		//	//byte[] buf = stream.ToArray();
		//	//stream.Dispose();

		//	////保存为Excel文件
		//	//using(FileStream fs = new FileStream(icdFile.FileName, FileMode.Create, FileAccess.Write))
		//	//{
		//	//	fs.Write(buf, 0, buf.Length);
		//	//	fs.Flush();
		//	//}
		//}

		//private void UpdateExcel(ISheet sheet, string prop, object value)
		//{
		//	for(int i = sheet.FirstRowNum + 3; i <= sheet.LastRowNum; i++)
		//	{
		//		IRow row = sheet.GetRow(i);
		//		if(SelectWord(row))
		//		{
		//			SetCellValue(sheet.GetRow(2), row, prop, value);
		//			break;
		//		}
		//	}
		//}

		//private bool SelectWord(IRow row)
		//{
		//	bool result = false;

		//	result = row.GetCell(5).ToString() == Name;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	result = Convert.ToUInt32(row.GetCell(0).ToString()) == Offset;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	result = Convert.ToUInt32(row.GetCell(1).ToString()) == StartBit;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	result = Convert.ToUInt32(row.GetCell(2).ToString()) == BitWidth;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	return true;
		//}

		//private void SetCellValue(IRow headerRow, IRow row, string field, object value)
		//{
		//	bool found = false;
		//	int colIndex = headerRow.FirstCellNum;
		//	for(; colIndex < headerRow.LastCellNum; colIndex++)
		//	{
		//		if(headerRow.GetCell(colIndex).StringCellValue.Equals(field))
		//		{
		//			found = true;
		//			break;
		//		}
		//	}

		//	if(!found)
		//		throw new Exception($"未找到列[{field}]。");

		//	ICell cell = row.GetCell(colIndex);
		//	if(cell == null)
		//		throw new Exception($"查找单元格[col:{colIndex}]错误。");

		//	switch(value)
		//	{
		//		case null:
		//			cell.SetCellValue("");
		//			break;
		//		case int intValue:
		//			cell.SetCellValue(intValue);
		//			break;
		//		case double dblValue:
		//			cell.SetCellValue(dblValue);
		//			break;
		//		case string strValue:
		//			cell.SetCellValue(strValue);
		//			break;
		//		case DateTime dtValue:
		//			cell.SetCellValue(dtValue);
		//			break;
		//		case bool bValue:
		//			cell.SetCellValue(bValue);
		//			break;
		//		default:
		//			cell.SetCellValue(value.ToString());
		//			break;
		//	}
		//}

		#endregion

		#region xml

		//private void UpdateXML(string fileName, string prop, object value)
		//{
		//	XElement element = XElement.Load(fileName);
		//	XElement frameElement = element.Element("ICDFrame");
		//	XElement word = frameElement.Elements("ICDWord").Where(SelectWord).FirstOrDefault();
		//	word.Element(prop).Value = value.ToString();

		//	element.Save(fileName);
		//}

		//private bool SelectWord(XElement e)
		//{
		//	bool result = false;

		//	result = e.Element("Name").Value == Name;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	result = Convert.ToUInt32(e.Element("Offset").Value) == Offset;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	result = Convert.ToUInt32(e.Element("StartBit").Value) == StartBit;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	result = Convert.ToUInt32(e.Element("BitWidth").Value) == BitWidth;
		//	if(!result)
		//	{
		//		return false;
		//	}

		//	return true;
		//}

		#endregion
	}
}