using Auxilia.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace NMag.Persistence
{
	internal partial class DataSetSerializer
	{
		public DataSet Deserialize(string filePath)
		{
			if (!File.Exists(filePath))
				throw new FileNotFoundException(nameof(filePath), filePath);

			return Deserialize(File.OpenRead(filePath));
		}

		public DataSet Deserialize(FileStream fileStream)
		{
			using StreamReader reader = new StreamReader(fileStream, Encoding);
			_currentLineIndex = 0;
			DataSet dataSet = new DataSet(Path.GetFileNameWithoutExtension(fileStream.Name));
			ProjectSettings projectSettings = new ProjectSettings();
			
			try
			{
				DeserializeMetaData(reader, projectSettings);
				
				while (!reader.EndOfStream)
				{
					switch (ReadLine(reader).Left(3))
					{
						case "*MO":
						{
							DeserializeMode(reader, projectSettings);
							break;
						}
						case "*CO":
						{
							DeserializeComments(reader, projectSettings);
							break;
						}
						case "*MA":
						{
							DeserializeReservoirs(reader, dataSet);
							break;
						}
						case "*KR":
						{
							DeserializePowerPlants(reader, dataSet);
							break;
						}
						case "*OF":
						{
							DeserializeTransfers(reader, dataSet);
							break;
						}
						case "*KO":
						{
							DeserializeControlPoints(reader, dataSet);
							break;
						}
					}
				}
				
				return dataSet;
			}
			catch (DeserializationException deserializationException)
			{
				deserializationException.DataBlock = _currentBlock;
				throw;
			}
		}

		private string ReadLine(StreamReader reader)
		{
			if (reader.EndOfStream)
				throw new DeserializationException("End of file.", _currentLineIndex, _currentLine, _currentBlock);

			_currentLineIndex++;
			return _currentLine = reader.ReadLine();
		}

		private static T ConvertValue<T>(object value) where T : struct
		{
			if (typeof(T) == typeof(bool))
				value = value.Equals("1");

			return (T)Convert.ChangeType(value, typeof(T));
		}

		private T ReadValue<T>(StreamReader reader) where T : struct
		{
			return ReadValue<T>(ReadLine(reader));
		}
		private T ReadValue<T>(string line) where T : struct
		{
			string data = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.First()
				.Trim();

			if (data.StartsWith(CommentIndicator))
				throw new DeserializationException("No data found.", _currentLineIndex, _currentLine, _currentBlock);
			
			try
			{
				return ConvertValue<T>(data);
			}
			catch
			{
				throw new DeserializationException($"Cannot parse value {data}.", _currentLineIndex, _currentLine, _currentBlock);
			}
		}

		private T[] ReadValues<T>(StreamReader reader, int count = int.MaxValue) where T : struct
		{
			return ReadValues<T>(ReadLine(reader), count);
		}
		private T[] ReadValues<T>(string line, int count = int.MaxValue) where T : struct
		{
			List<string> data = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.Take(count)
				.Select(v => v.Trim())
				.Where(v => !v.StartsWith(CommentIndicator) && !string.IsNullOrEmpty(v))
				.ToList();

			if (data.Count != count)
				throw new DeserializationException($"Found {data.Count} values, expected {count}.", _currentLineIndex, _currentLine, _currentBlock);

			T[] values = new T[count];

			for (int i = 0; i < data.Count; i++)
			{
				try
				{
					values[i] = ConvertValue<T>(data[i]);
				}
				catch
				{
					throw new DeserializationException($"Cannot parse value {data[i]}.", _currentLineIndex, _currentLine, _currentBlock);
				}
			}

			return values;
		}

		private void DeserializeMetaData(StreamReader reader, ProjectSettings projectSettings)
		{
			_currentBlock = "meta data";

			for (int i = 0; i < 6; i++)
				ReadLine(reader);

			projectSettings.MaxCurveSize = ReadValue<int>(reader);
		}

		private void DeserializeMode(StreamReader reader, ProjectSettings projectSettings)
		{
			_currentBlock = "mode";
			projectSettings.SingleReservoir = ReadValue<bool>(reader);
			ReadLine(reader);
		}

		private void DeserializeComments(StreamReader reader, ProjectSettings projectSettings)
		{
			_currentBlock = "comments";
			StringBuilder comments = new StringBuilder();

			while (!reader.EndOfStream)
			{
				if(ReadLine(reader).Equals(BlockDelimiter))
					break;

				comments.AppendLine(_currentLine);
			}

			projectSettings.Comments = comments.ToString();
		}
		
		private void SetConnectedModules(Module module, StreamReader reader)
		{
			int[] routedIds = ReadValues<int>(reader, 3);
			
			module.ReleaseModuleId = routedIds[0];
			module.BypassModuleId = routedIds[1];
			module.SpillModuleId = routedIds[2];
		}

		private IList<Point> ReadCurvePoints(StreamReader reader)
		{
			List<Point> points = new List<Point>();

			int pointCount = ReadValue<int>(reader);
			for (int i = 0; i < pointCount; i++)
			{
				double[] values = ReadValues<double>(reader, 2);
				points.Add(new Point(values[0], values[1]));
			}

			return points;
		}

		private void DeserializeReservoirs(StreamReader reader, DataSet dataSet)
		{
			_currentBlock = "reservoirs";
			
			ReadLine(reader);
			while (!_currentLine.Equals(BlockDelimiter))
			{
				Module module = dataSet.Modules.Add(ModuleType.Reservoir, ReadValue<int>(_currentLine));
				ReservoirUnit reservoir = module.Reservoir;

				reservoir.Name = ReadLine(reader);

				reservoir.LowestRegulatedWaterLevel = ReadValue<double>(reader);
				reservoir.HighestRegulatedWaterLevel = ReadValue<double>(reader);
				reservoir.Volume = ReadValue<double>(reader);

				SetConnectedModules(module, reader);
				
				while (!ReadLine(reader).Contains(CommentIndicator))
				{
					int curveCode = ReadValue<int>(_currentLine);

					if (curveCode.Equals(CurveType.Discharge.GetCode()))
						reservoir.OutletLevel = ReadValue<double>(reader);

					reservoir.SetCurve(CurveFactory.CreateUnitCurve(module.Reservoir, curveCode, ReadCurvePoints(reader)));
				}
			}
		}

		private void DeserializePowerPlants(StreamReader reader, DataSet dataSet)
		{
			_currentBlock = "power plants";
			
			ReadLine(reader);
			while (!_currentLine.Equals(BlockDelimiter))
			{
				Module module = dataSet.Modules.Add(ModuleType.PowerPlant, ReadValue<int>(_currentLine));
				PowerPlantUnit powerPlant = module.PowerPlant;

				powerPlant.Name = ReadLine(reader);

				SetConnectedModules(module, reader);

				powerPlant.MaximumDischarge = ReadValue<double>(reader);
				powerPlant.EnergyEquivalent = ReadValue<double>(reader);
				
				while (!ReadLine(reader).Contains(CommentIndicator))
				{
					int curveCode = ReadValue<int>(_currentLine);

					if (curveCode.Equals(CurveType.LoadEfficiency.GetCode()))
					{
						powerPlant.NominalHead = ReadValue<double>(reader);
						powerPlant.IntakeLevel = ReadValue<double>(reader);
						powerPlant.TailWaterLevel = ReadValue<double>(reader);
						powerPlant.HeadLossCoefficient = ReadValue<double>(reader);
					}

					powerPlant.SetCurve(CurveFactory.CreateUnitCurve(powerPlant, curveCode, ReadCurvePoints(reader)));
				}
			}
		}

		private void DeserializeTransfers(StreamReader reader, DataSet dataSet)
		{
			_currentBlock = "transfers";
			
			ReadLine(reader);
			while (!_currentLine.Equals(BlockDelimiter))
			{
				Module module = dataSet.Modules.Add(ModuleType.Transfer, ReadValue<int>(_currentLine));
				TransferUnit transfer = module.Transfer;

				transfer.Name = ReadLine(reader);

				SetConnectedModules(module, reader);
				
				while (!ReadLine(reader).Contains(CommentIndicator))
				{
					int code = ReadValue<int>(_currentLine);

					switch (code)
					{
						case (int)TransferMode.FixedCapacity:
						{
							transfer.Mode = TransferMode.FixedCapacity;
							transfer.Capacity = ReadValue<double>(reader);
							break;
						}
						case (int)TransferMode.VariableCapacity:
						{
							transfer.Mode = TransferMode.VariableCapacity;
							double[] values = ReadValues<double>(reader, 3);

							transfer.InletElevation = values[0];
							transfer.OutletElevation = values[1];
							transfer.HeadLossCoefficient = values[2];
							break;
						}
						case (int)TransferMode.HeadFlowCapacity:
						{
							transfer.Mode = TransferMode.HeadFlowCapacity;
							transfer.IntakeElevation = ReadValue<double>(reader);
							transfer.TailWaterElevation = ReadValue<double>(reader);

							transfer.SetCurve(CurveFactory.CreateUnitCurve(transfer, (int)transfer.Mode, ReadCurvePoints(reader)));
							break;
						}
						default:
						{
							if (code.Equals(CurveType.Routing.GetCode()))
								transfer.SetCurve(CurveFactory.CreateUnitCurve(transfer, code, ReadCurvePoints(reader)));

							break;
						}
					}
				}
			}
		}

		private void DeserializeControlPoints(StreamReader reader, DataSet dataSet)
		{
			_currentBlock = "control points";
			
			ReadLine(reader);
			while (!_currentLine.Equals(BlockDelimiter))
			{
				Module module = dataSet.Modules.Add(ModuleType.ControlPoint, ReadValue<int>(_currentLine));
				ControlPointUnit controlPoint = module.ControlPoint;

				controlPoint.Name = ReadLine(reader);

				SetConnectedModules(module, reader);

				if(!ReadLine(reader).Contains(CommentIndicator))
					controlPoint.SetCurve(CurveFactory.CreateUnitCurve(controlPoint, ReadValue<int>(_currentLine), ReadCurvePoints(reader)));
			}
		}
	}
}