using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using HOTINST.COMMON.Bitwise;
using HOTINST.COMMON.Serialization;
using HOTINST.ICD.Codec;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HOTINST.ICD
{
	public class ICDManager : IICDManager, IDisposable
	{
		#region 字段
		private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(ICDManager));
		private string ConfigPath { get; set; }
		private string ConfigFile { get; set; }
		/// <summary>
		/// ICD配置文件
		/// </summary>
		private List<ICDFile> ICDConfig { get; set; }

		private readonly List<ICDWords> _icds = new List<ICDWords>();
		private readonly IList<ICDWords> _editedIcds = new List<ICDWords>();
		private readonly IList<ICDWords> _removedIcds = new List<ICDWords>();

		#endregion

		public ICDManager(string confieName)
		{
			ConfigPath = Path.GetDirectoryName(confieName);
			Debug.Assert(!string.IsNullOrEmpty(ConfigPath));

			ConfigFile = confieName;

			ICDConfig = SerializationHelper.LoadFromXml<List<ICDFile>>(confieName, "ArrayOfICDFile") ?? new List<ICDFile>();
			foreach(ICDFile file in ICDConfig)
			{
				string icdFilePath = Path.Combine(ConfigPath, file.FileName);
				if(!File.Exists(icdFilePath))
				{
					_logger.Error($"ICD配置[{confieName}]中配置的ICD文件[{icdFilePath}]不存在。");
					continue;
				}

				try
				{
					if(Path.GetExtension(file.FileName) == ".xlsx")
					{
						IList<ICDWords> words = LoadFromeExcell2007(icdFilePath);
						foreach(ICDWords word in words)
						{
							Add(word);
						}
					}
					else if(Path.GetExtension(file.FileName) == ".xls")
					{
						IList<ICDWords> words = LoadFromeExcell2003(icdFilePath);
						foreach(ICDWords word in words)
						{
							Add(word);
						}
					}
					else
					{
						ICDWords words = CreatIcdWordsFromFile(icdFilePath);
						words.Id = file.ID;
						words.Name = file.Name;
						words.FileName = icdFilePath;
						Add(words);
					}
				}
				catch(Exception ex)
				{
					_logger.Error($"加载[{file.FileName}]失败：{ex.Message}");
				}
			}
			if(_icds.Count == 0)
			{
				_logger.Warn($"{confieName} 没有加载任何内容！");
			}

			DeleteTempFile();
		}

		private void Add(ICDWords icd)
		{
			if(_icds.Any(d => d.Name == icd.Name))
			{
				_logger.Error($"跳过：{icd.FileName}===>{icd.Name}，因为相同名称的ICD已经加载。");
			}
			else
			{
				_icds.Add(icd);
				_logger.Debug($"已加载：{icd.FileName}===>{icd.Name}");

				_removedIcds.Remove(_removedIcds.FirstOrDefault(e => e.Name == icd.Name));
			}
		}

		private void Remove(string name)
		{
			ICDWords icd = _icds.FirstOrDefault(d => d.Name == name);
			if(icd == null)
			{
				_logger.Error($"移除失败，因为没有找到名称为[{name}]的ICD。");
			}
			else
			{
				_icds.Remove(icd);
				_logger.Debug($"已移除：{name}");

				_editedIcds.Remove(_editedIcds.FirstOrDefault(e => e.Name == name));
				_removedIcds.Add(icd);
			}
		}

		private void AddConfig(string name, string file)
		{
			ICDConfig.Add(new ICDFile
			{
				ID = ICDConfig.Count,
				Name = name,
				FileName = file
			});
		}

		#region IICDManager
		public IList<ICDWords> GetAllICD()
		{
			return _icds;
		}

		public void AddICDWords(ICDWords icdWords)
		{
			Add(icdWords);
		}

		public void RemoveICDWords(string name)
		{
			Remove(name);
		}

		public void RemoveICDWords(int id)
		{
			ICDConfig.Remove(ICDConfig.Find(n => n.ID == id));
			_icds.Remove(_icds.Find(n => n.Id == id));
		}

		public ICDWords GetICD(string name)
		{
			return _icds.FirstOrDefault(n => n.Name == name);
		}

		public ICDWords GetICD(int id)
		{
			return _icds.FirstOrDefault(n => n.Id == id);
		}

		public void Edit(ICDWords icdWords)
		{
			ICDWords exist = GetICD(icdWords.Name);
			if(exist == null)
			{
				_logger.Error($"编辑失败：不存在名为[{icdWords.Name}]的ICD。");
				return;
			}
			RemoveICDWords(icdWords.Name);
			AddICDWords(icdWords);
			_editedIcds.Add(icdWords);
		}

		public void Save()
		{
			RemoveIcd();

			ICDConfig.Clear();
			foreach(ICDWords icd in _icds)
			{
				string icdFilePath = Path.Combine(ConfigPath, icd.FileName);

				try
				{
					string ext = Path.GetExtension(icd.FileName);

					if(ext == ".xlsx" || ext == ".xls")
					{
						SaveIcdExcel(icdFilePath, icd);
					}
					else if(ext == ".icd")
					{
						SaveIcdXml(icdFilePath, icd);
					}
					else
					{
						_logger.Error($"保存[{icd.FileName}]失败：文件扩展名格式不正确。");
					}
				}
				catch(Exception ex)
				{
					_logger.Error($"保存[{icd.FileName}]失败：{ex.Message}");
				}
			}

			SerializationHelper.SaveToXml(ConfigFile, ICDConfig, "ArrayOfICDFile");
			
			DeleteTempFile();
		}

		private void RemoveSheet(IWorkbook workbook, string path, string name)
		{
			int id = workbook.GetSheetIndex(name);
			if(id != -1)
			{
				workbook.RemoveSheetAt(id);
				if(workbook.NumberOfSheets == 0)
				{
					File.Delete(path);
				}
				else
				{
					Save(workbook, path);
				}
			}
		}

		private void RemoveIcd()
		{
			foreach(ICDWords icd in _removedIcds)
			{
				string icdFilePath = Path.Combine(ConfigPath, icd.FileName);

				try
				{
					string ext = Path.GetExtension(icd.FileName);

					if(ext == ".xlsx" || ext == ".xls")
					{
						RemoveSheet(GetWorkbook(icdFilePath), icdFilePath, icd.Name);
					}
					else if(ext == ".icd")
					{
						File.Delete(icdFilePath);
					}
				}
				catch(Exception ex)
				{
					_logger.Error($"删除[{icd.FileName}]失败：{ex.Message}");
				}
			}
		}

		#endregion

		#region 私有方法

		private void DeleteTempFile()
		{
			string path = Path.GetDirectoryName(ConfigPath);
			DirectoryInfo di = new DirectoryInfo(path);
			FileInfo[] tempFiles = di.GetFiles("OpenXml*.tmp", SearchOption.TopDirectoryOnly);
			for(int i = tempFiles.Length - 1; i >= 0; i--)
			{
				File.Delete(tempFiles[i].FullName);
			}
		}
		
		private IList<ICDWords> LoadFromeExcell2007(string fileName)
		{
			//FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			XSSFWorkbook workbook = new XSSFWorkbook(fileName);

			IList<ICDWords> frames = new List<ICDWords>();

			for(int i = 0; i < workbook.NumberOfSheets; i++)
			{
				ISheet sheet = workbook.GetSheetAt(i);
				ICDWords frame = LoadFrameFromSheet(workbook.GetSheetAt(i), fileName, i);
				frame.FileName = fileName;
				frame.Id = frames.Count;
				frame.Name = sheet.SheetName;
				frames.Add(frame);
			}

			workbook.Close();

			return frames;
		}

		private IList<ICDWords> LoadFromeExcell2003(string fileName)
		{
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			HSSFWorkbook workbook = new HSSFWorkbook(fs);
			fs.Dispose();

			IList<ICDWords> frames = new List<ICDWords>();

			for(int i = 0; i < workbook.NumberOfSheets; i++)
			{
				ISheet sheet = workbook.GetSheetAt(i);
				ICDWords frame = LoadFrameFromSheet(workbook.GetSheetAt(i), fileName, i);
				frame.FileName = fileName;
				frame.Id = frames.Count;
				frame.Name = sheet.SheetName;
				frames.Add(frame);
			}

			workbook.Close();

			return frames;
		}


		private ICDWords CreatIcdWordsFromFile(string fileName)
		{
			XElement element = XElement.Load(fileName);

			ICDWords words = ICDWordFormElement(element);
			words.FileName = fileName;
			return words;
		}
		private ICDHead HeaderFromElement(XElement element)
		{
			ICDHead header = new ICDHead();

			#region assign head

			try
			{
				XElement eleNodeIdSize = element.Element("NodeIdSize");
				XElement eleTotalSize = element.Element("TotalSize");
				XElement eleCheckPosition = element.Element("CheckPosition");
				XElement eleFixedSize = element.Element("FixedSize");
				XElement eleBeginFlag = element.Element("BeginFlag");
				XElement eleCheckLengthSize = element.Element("CheckLengthSize");
				XElement eleCheckMode = element.Element("CheckMode");
				XElement eleCheckStartPosition = element.Element("CheckStartPosition");
				XElement eleLengthPosition = element.Element("LengthPosition");
				XElement eleCheckSize = element.Element("CheckSize");
				XElement eleNodeIdPosition = element.Element("NodeIdPosition");
				XElement eleLengthSize = element.Element("LengthSize");
				XElement eleEndian = element.Element("Endian");

				header.BeginFlag = eleBeginFlag.Value;
				header.NodeIdSize = uint.Parse(eleNodeIdSize.Value);
				header.TotalSize = int.Parse(eleTotalSize.Value);
				header.CheckPosition = uint.Parse(eleCheckPosition.Value);
				header.FixedSize = int.Parse(eleFixedSize.Value);
				header.CheckLengthSize = uint.Parse(eleCheckLengthSize.Value);
				Enum.TryParse(eleCheckMode.Value, true, out ECheckMode cm);
				header.CheckMode = cm;
				header.CheckStartPosition = uint.Parse(eleCheckStartPosition.Value);
				header.LengthPosition = uint.Parse(eleLengthPosition.Value);
				header.CheckSize = uint.Parse(eleCheckSize.Value);
				header.NodeIdPosition = uint.Parse(eleNodeIdPosition.Value);
				header.LengthSize = uint.Parse(eleLengthSize.Value);
				Enum.TryParse(eleEndian.Value, true, out Endian et);
				header.Endian = et;
			}
			catch(Exception e)
			{
				_logger.Error($"加载ICD头错误：{e.Message}");
			}
			#endregion

			return header;
		}

		private List<ICDWord> ICDWordListFromElement(XElement frameElement, ICDWords icdWords)
		{
			List<ICDWord> wordList = new List<ICDWord>(512);

			foreach(XElement element in frameElement.Elements("ICDWord"))
			{
				#region get word

				string offset = element.Element("Offset") == null ? null : element.Element("Offset").Value;
				string startBit = element.Element("StartBit") == null ? null : element.Element("StartBit").Value;
				string bitWidth = element.Element("BitWidth") == null ? null : element.Element("BitWidth").Value;
				string innertype = element.Element("InnerType") == null ? null : element.Element("InnerType").Value;
				string type = element.Element("Type") == null ? null : element.Element("Type").Value;
				string min = element.Element("Min") == null ? null : element.Element("Min").Value;
				string max = element.Element("Max") == null ? null : element.Element("Max").Value;
				string param1 = element.Element("Param1") == null ? null : element.Element("Param1").Value;
				string param2 = element.Element("Param2") == null ? null : element.Element("Param2").Value;
				string param3 = element.Element("Param3") == null ? null : element.Element("Param3").Value;
				string param4 = element.Element("Param4") == null ? null : element.Element("Param4").Value;
				string endian = element.Element("Endian") == null ? null : element.Element("Endian").Value;
				string isCallFunction = element.Element("IsCallFunction") == null ? null : element.Element("IsCallFunction").Value;
				string dataValid = element.Element("DataValid") == null ? null : element.Element("DataValid").Value;
				string access = element.Element("Access") == null ? null : element.Element("Access").Value;

				#endregion
				#region assign word

				try
				{
					if(string.IsNullOrEmpty(innertype))
					{
						throw new ArgumentNullException(nameof(innertype), @"内部数据类型不能为空");
					}

					if(innertype.Equals("int8"))
					{
						innertype = "uint8";
					}
					DataType dinnerType;
					if(int.TryParse(innertype, out int innerType))
					{
						innertype = ((DataType)innerType).ToString().ToLower();
					}
					if(!Enum.TryParse(innertype, true, out dinnerType))
					{
						throw new ArgumentException($@"数据类型转换失败:{innertype}", nameof(innertype));
					}

					if(string.IsNullOrEmpty(type))
					{
						throw new ArgumentNullException(nameof(type), @"数据类型不能为空");
					}
					if(type.Equals("int8"))
					{
						type = "uint8";
					}
					DataType dType;
					if(int.TryParse(type, out int datatype))
					{
						type = ((DataType)datatype).ToString().ToLower();
					}
					if(!Enum.TryParse(type, true, out dType))
					{
						throw new ArgumentException($@"数据类型转换失败:{type}", nameof(type));
					}

					Enum.TryParse(access, true, out AccessMode accessMode);

					Enum.TryParse(endian, true, out Endian et);

					ICDWord word = new ICDWord
					{
						Offset = Convert.ToUInt32(string.IsNullOrEmpty(offset) ? "0" : offset),
						StartBit = Convert.ToUInt32(string.IsNullOrEmpty(startBit) ? "0" : startBit),
						BitWidth = Convert.ToUInt32(string.IsNullOrEmpty(bitWidth) ? "0" : bitWidth),
						InnerType = dinnerType,
						Type = dType,
						Name = element.Element("Name") == null ? null : element.Element("Name").Value.Trim(),
						Min = Convert.ToDouble(string.IsNullOrEmpty(min) ? "0" : min),
						Max = Convert.ToDouble(string.IsNullOrEmpty(max) ? "0" : max),
						Description = element.Element("Description") == null ? null : element.Element("Description").Value.Trim(),
						Contract = element.Element("Contract") == null ? null : element.Element("Contract").Value.Trim(),
						SystemName = element.Element("SystemName") == null ? null : element.Element("SystemName").Value.Trim(),
						GroupName = element.Element("GroupName") == null ? null : element.Element("GroupName").Value.Trim(),
						Unit = element.Element("Unit") == null ? null : element.Element("Unit").Value,
						IsCallFunction = Convert.ToBoolean(isCallFunction),
						Param1 = Convert.ToDouble(string.IsNullOrEmpty(param1) ? "0" : param1),
						Param2 = Convert.ToDouble(string.IsNullOrEmpty(param2) ? "0" : param2),
						Param3 = Convert.ToDouble(string.IsNullOrEmpty(param3) ? "0" : param3),
						Param4 = Convert.ToDouble(string.IsNullOrEmpty(param4) ? "0" : param4),
						FunctionName = element.Element("FunctionName") == null ? null : element.Element("FunctionName").Value.Trim(),
						Tag = element.Element("Tag") == null ? null : element.Element("Tag").Value,
						ValueTextList = element.Element("ValueTextList") == null ? null : element.Element("ValueTextList").Value,
						DefaultValue = element.Element("DefaultValue") == null ? null : element.Element("DefaultValue").Value,
						Endian = et,
						DataValid = Convert.ToBoolean(dataValid),
						Access = accessMode
					};
					wordList.Add(word);
				}
				catch(Exception e)
				{
					_logger.Error($"加载ICD内容错误：{e.Message}");
				}

				#endregion
			}

			return wordList;
		}

		private ICDWords ICDWordFormElement(XElement element)
		{
			ICDWords oneWords = new ICDWords();

			XElement headerElement = element.Element("ICDHead");
			XElement frameElement = element.Element("ICDFrame");

			if(headerElement != null)
				oneWords.ICDHead = HeaderFromElement(headerElement);
			if(frameElement != null)
				oneWords.ICDFrame = ICDWordListFromElement(frameElement, oneWords);

			return oneWords;
		}


		#endregion

		#region 加载excell

		private ICDWords LoadFrameFromSheet(ISheet sheet, string file, int id)
		{
			// ICD头标题行 + ICD头内容行 + ICD信号行 至少三行
			if(sheet.LastRowNum - sheet.FirstRowNum < 2)
			{
				throw new Exception($"工作表[{sheet.SheetName}]格式错误");
			}

			ICDWords frame = new ICDWords
			{
				Id = id,
				Name = sheet.SheetName,
				FileName = file,
				ICDHead = LoadHeadFromSheet(sheet)
			};
			frame.ICDFrame = LoadWordsFromSheet(sheet, frame);

			return frame;
		}

		private ICDHead LoadHeadFromSheet(ISheet sheet)
		{
			ICDHead head = new ICDHead();

			IRow titleRow = sheet.GetRow(sheet.FirstRowNum) ?? throw new Exception($"解析ICD头的标题行失败. 工作表: {sheet.SheetName}");
			IRow valueRow = sheet.GetRow(sheet.FirstRowNum + 1) ?? throw new Exception($"解析ICD头的数据行失败. 工作表: {sheet.SheetName}");

			for(int i = titleRow.FirstCellNum; i < titleRow.LastCellNum; i++)
			{
				string field = titleRow.GetCell(i)?.StringCellValue;
				if(string.IsNullOrEmpty(field))
				{
					continue;
				}

				object value;

				#region assign value

				switch(field)
				{
					case nameof(head.BeginFlag):
						value = valueRow.GetCell(i)?.StringCellValue ?? string.Empty;
						break;
					case nameof(head.TotalSize):
					case nameof(head.FixedSize):
						value = Convert.ToInt32(valueRow.GetCell(i)?.ToString());
						break;
					case nameof(head.CheckMode):
						value = valueRow.GetCell(i)?.ToString();
						if(Enum.TryParse(value?.ToString(), true, out ECheckMode cm))
						{
							value = cm;
						}
						else
						{
							throw new Exception($"解析校验方式失败. 值: {value}");
						}

						break;
					case nameof(head.Endian):
						value = valueRow.GetCell(i)?.ToString();
						if(Enum.TryParse(value?.ToString(), true, out Endian ed))
						{
							value = ed;
						}
						else
						{
							throw new Exception($"解析ICD头大小端类型失败. 值: {value}");
						}

						break;
					default:
						value = Convert.ToUInt32(valueRow.GetCell(i)?.ToString());
						break;
				}

				#endregion

				PropertyInfo prop = typeof(ICDHead).GetProperty(field);
				if(prop == null)
				{
					throw new Exception($"未定义的属性. 名称: {field}");
				}

				prop.SetValue(head, value, null);
			}

			return head;
		}

		private List<ICDWord> LoadWordsFromSheet(ISheet sheet, ICDWords icdWords)
		{
			List<ICDWord> words = new List<ICDWord>();

			IRow rowWordTitle = sheet.GetRow(sheet.FirstRowNum + 2) ?? throw new Exception($"解析ICD标题行失败. 工作表: {sheet.SheetName}");

			//LastrowNum也是有内容的
			for(int i = sheet.FirstRowNum + 3; i <= sheet.LastRowNum; i++)
			{
				ICDWord word = LoadWordFromRow(rowWordTitle, sheet.GetRow(i), icdWords);
				if(word != null)
				{
					words.Add(word);
				}
			}

			return words;
		}

		private ICDWord LoadWordFromRow(IRow titleRow, IRow valueRow, ICDWords icdWords)
		{
			if(valueRow == null)
			{
				return null;
			}

			//Debug.Assert(titleRow.FirstCellNum == valueRow.FirstCellNum && titleRow.LastCellNum == valueRow.LastCellNum);

			ICDWord word = new ICDWord();

			for(int i = titleRow.FirstCellNum; i < titleRow.LastCellNum; i++)
			{
				string field = titleRow.GetCell(i)?.StringCellValue;
				if(string.IsNullOrEmpty(field))
				{
					continue;
				}

				object value;

				#region assign value

				switch(field)
				{
					case nameof(word.Offset):
					case nameof(word.StartBit):
					case nameof(word.BitWidth):
						value = Convert.ToUInt32(valueRow.GetCell(i)?.ToString());
						break;
					case nameof(word.Param1):
					case nameof(word.Param2):
					case nameof(word.Param3):
					case nameof(word.Param4):
					case nameof(word.Min):
					case nameof(word.Max):
						value = valueRow.GetCell(i)?.NumericCellValue;
						break;
					case nameof(word.InnerType):
					case nameof(word.Type):
						value = valueRow.GetCell(i)?.ToString();
						if(Enum.TryParse(value?.ToString(), true, out DataType dt))
						{
							value = dt;
						}
						else
						{
							throw new Exception($"解析数据类型失败. 值: {value}, 行: {valueRow.RowNum}");
						}
						break;
					case nameof(word.Endian):
						value = valueRow.GetCell(i)?.ToString();
						if(Enum.TryParse(value?.ToString(), true, out Endian ed))
						{
							value = ed;
						}
						else
						{
							throw new Exception($"解析大小端类型失败. 值: {value}, 行: {valueRow.RowNum}");
						}
						break;
					case nameof(word.Access):
						value = valueRow.GetCell(i)?.ToString();
						if(Enum.TryParse(value?.ToString(), true, out AccessMode access))
						{
							value = access;
						}
						else
						{
							throw new Exception($"解析读写类型失败. 值: {value}, 行: {valueRow.RowNum}");
						}

						break;
					case nameof(word.DataValid):
					case nameof(word.IsCallFunction):
						value = valueRow.GetCell(i)?.BooleanCellValue;
						break;
					default:
						value = valueRow.GetCell(i)?.ToString() ?? string.Empty;
						break;
				}

				#endregion

				if(value == null)
				{
					throw new Exception($"解析ICD头失败. 行: {valueRow.RowNum}, 列: {field}, 值: {null}");
				}

				PropertyInfo prop = typeof(ICDWord).GetProperty(field);
				if(prop == null)
				{
					throw new Exception($"未定义的属性. 名称: {field}, 行: {valueRow.RowNum}");
				}

				prop.SetValue(word, value, null);
			}


			return word;
		}

		#endregion

		#region 保存Xml

		private void SaveIcdXml(string path, ICDWords icd)
		{
			string fileName = icd.FileName;
			if(Path.IsPathRooted(icd.FileName))
			{
				fileName = Path.GetFileName(icd.FileName);
			}

			AddConfig(icd.Name, Path.GetFileName(fileName));
			if(!File.Exists(path) || _editedIcds.Contains(icd))
			{
				SerializationHelper.SaveToXml(path, icd, "ICDWords");
				_editedIcds.Remove(icd);
			}
		}

		#endregion

		#region 保存Excel

		private IWorkbook GetWorkbook(string file)
		{
			IWorkbook workbook = null;

			string ext = Path.GetExtension(file);
			switch(ext)
			{
				case ".xlsx":
					workbook = File.Exists(file)
						? new XSSFWorkbook(new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
						: new XSSFWorkbook();
					break;
				case ".xls":
					workbook = File.Exists(file)
						? new HSSFWorkbook(new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
						: new HSSFWorkbook();
					break;
				default:
					throw new Exception($"文件[{file}]格式不正确");
			}

			return workbook;
		}

		private void CreateHead(ISheet sheet, ICDHead head)
		{
			// 创建ICD头的标题行
			IRow rowHeaderTitle = sheet.CreateRow(0);
			// 创建ICD头的值行
			IRow rowHeaderValue = sheet.CreateRow(1);
			// 获取ICD头的所有属性
			PropertyInfo[] props = typeof(ICDHead).GetProperties();
			for(int i = 0; i < props.Length; i++)
			{
				// 创建ICD头标题行的单元格并写入属性名
				ICell cellht = rowHeaderTitle.CreateCell(i, CellType.String);
				cellht.SetCellValue(props[i].Name);
				// 创建ICD头值行的单元格并写入属性值
				if(props[i].PropertyType == typeof(string))
				{
					ICell cellhtv = rowHeaderValue.CreateCell(i, CellType.String);
					cellhtv.SetCellValue(props[i].GetValue(head, null)?.ToString());
				}
				else
				{
					ICell cellhtv = rowHeaderValue.CreateCell(i, CellType.Numeric);
					cellhtv.SetCellValue(Convert.ToInt32(props[i].GetValue(head, null)));
				}
			}
		}

		private void CreateFrame(ISheet sheet, IList<ICDWord> frame)
		{
			// 创建ICD信号的标题行
			IRow rowWordTitle = sheet.CreateRow(2);
			// 获取ICD信号的所有属性
			PropertyInfo[] props = typeof(ICDWord).GetProperties().Where(p => !(p.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).FirstOrDefault() is XmlIgnoreAttribute)).ToArray();
			for(int i = 0; i < props.Length; i++)
			{
				// 创建ICD信号标题行的单元格并写入属性名
				ICell cellht = rowWordTitle.CreateCell(i, CellType.String);
				cellht.SetCellValue(props[i].Name);
			}
			for(int i = 0; i < frame.Count; i++)
			{
				// 创建ICD信号行
				IRow rowWord = sheet.CreateRow(i + 3);
				for(int j = 0; j < props.Length; j++)
				{
					// 创建ICD信号行的单元格
					rowWord.CreateCell(j);
				}
				// 设置信号值
				foreach(PropertyInfo prop in props)
				{
					SetCellValue(rowWordTitle, rowWord, prop.Name, prop.GetValue(frame[i], null));
				}
			}
		}

		private void SetCellValue(IRow headerRow, IRow row, string field, object value)
		{
			bool found = false;
			int colIndex = headerRow.FirstCellNum;
			for(; colIndex < headerRow.LastCellNum; colIndex++)
			{
				if(headerRow.GetCell(colIndex).StringCellValue.Equals(field))
				{
					found = true;
					break;
				}
			}

			if(!found)
				throw new Exception($"未找到列[{field}]。");

			ICell cell = row.GetCell(colIndex);
			if(cell == null)
				throw new Exception($"查找单元格[col:{colIndex}]错误。");

			switch(value)
			{
				case null:
					cell.SetCellValue("");
					break;
				case int intValue:
					cell.SetCellValue(intValue);
					break;
				case double dblValue:
					cell.SetCellValue(dblValue);
					break;
				case string strValue:
					cell.SetCellValue(strValue);
					break;
				case DateTime dtValue:
					cell.SetCellValue(dtValue);
					break;
				case bool bValue:
					cell.SetCellValue(bValue);
					break;
				default:
					cell.SetCellValue(value.ToString());
					break;
			}
		}

		private void Save(IWorkbook workbook, string file)
		{
			using(FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
			{
				workbook.Write(fs);
			}
		}

		private void SaveIcdExcel(string path, ICDWords icd)
		{
			string fileName = icd.FileName;
			if(Path.IsPathRooted(icd.FileName))
			{
				fileName = Path.GetFileName(icd.FileName);
			}

			if(ICDConfig.All(c => c.FileName != fileName))
			{
				AddConfig(string.Empty, Path.GetFileName(fileName));
			}

			IWorkbook workbook = GetWorkbook(path);
			if(workbook.GetSheet(icd.Name) == null || _editedIcds.Contains(icd))
			{
				RemoveSheet(workbook, path, icd.Name);

				ISheet sheet = workbook.CreateSheet(icd.Name);

				CreateHead(sheet, icd.ICDHead);
				CreateFrame(sheet, icd.ICDFrame);

				Save(workbook, path);

				_editedIcds.Remove(icd);
			}
		}

		#endregion

		#region Implementation of IDisposable

		private bool _disposed;

		/// <summary>
		/// 执行与释放或重置非托管资源相关的应用程序定义的任务。
		/// </summary>
		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// 释放资源
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if(!_disposed)
			{
				if(disposing)
				{
					//托管资源释放
					OnDispose();
				}

				//非托管资源释放
			}

			_disposed = true;
		}

		public virtual void OnDispose()
		{
			DeleteTempFile();
		}

		/// <summary>
		/// Useful for ensuring that DockingViewModel objects are properly garbage collected.
		/// </summary>
		~ICDManager()
		{
			Dispose(false);
		}

		#endregion
	}
}