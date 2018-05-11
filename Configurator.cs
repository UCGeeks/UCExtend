using System;
using System.Data;
using System.IO;
using System.Linq;

namespace UCExtend
{
	public sealed class Configurator
	{
		#region Declarations
		
		public DataTable Settings; //this is the main Table

		private string fileName = ""; //this is the filename that was loaded

		public enum FileType //this specifies if the file is an xml or an ini
		{
			Ini, Xml
		}

		#endregion

		#region Public Methods

		public Configurator() //creates the settings
		{
			initializeDataTable();
		}

		public void LoadFromFile(string file, FileType ft) //loads settings from a file (xml or ini)
		{
			fileName = Path.GetFullPath(file); //saves the filename for future use

			if (ft == FileType.Ini)
				LoadFromIni();
			else
				LoadFromXml();
		}

		public void AddValue(string Category, string Key, string Value, bool OverwriteExisting) //adds a new setting to the table
		{
			if (OverwriteExisting)
			{
				foreach (DataRow row in Settings.Rows.Cast<DataRow>().Where(row => (string) row[0] == Category && (string) row[1] == Key))
				{
					row[2] = Value;
					return;
				}

				Settings.Rows.Add(Category, Key, Value);
			}
			else
				Settings.Rows.Add(Category, Key, Value);
		}

		public string GetValue(string Category, string Key, string DefaultValue) //gets a value or returns a default value
		{
			foreach (DataRow row in Settings.Rows.Cast<DataRow>().Where(row => (string)row[0] == Category && (string)row[1] == Key))
			{
				return (string)row[2];
			}

			return DefaultValue;
		}

		public void Save(FileType ft) //saves the file to the previously loaded file
		{
			//sorts the table for saving

			if (fileName == "") throw new FileNotFoundException("The file name was not previously defined");

			DataView dv = Settings.DefaultView;
			dv.Sort = "Category asc";
			DataTable sortedDT = dv.ToTable();

			if (ft == FileType.Xml)
				sortedDT.WriteXml(fileName);
			else
			{
				StreamWriter sw = new StreamWriter(fileName);

				string lastCategory ="";

				foreach (DataRow row in sortedDT.Rows)
				{
					if ((string) row[0] != lastCategory)
					{
						lastCategory = (string) row[0];
						sw.WriteLine("[" + lastCategory + "]");
					}

					sw.WriteLine((string) row[1] + "=" + (string)row[2]);
				}

				sw.Close();
			}
		}

		public void Save(string file, FileType ft) //saves the file to a file
		{
			fileName = Path.GetFullPath(file); //saves the filename for future use

			Save(ft);
		}

		#endregion

		#region Private Methods

		private void LoadFromIni() //loads settings from ini
		{
			if (!File.Exists(fileName))return;

			StreamReader sr = new StreamReader(fileName); //stream reader that will read the settings

			string currentCategory = ""; //holds the category we're at

			while (!sr.EndOfStream) //goes through the file
			{
				string currentLine = sr.ReadLine(); //reads the current file

				if (currentLine.Length < 3) continue; //checks that the line is usable

				if (currentLine.StartsWith("[") && currentLine.EndsWith("]")) //checks if the line is a category marker
				{
					currentCategory = currentLine.Substring(1, currentLine.Length - 2);
					continue;
				}

				if (!currentLine.Contains("=")) continue; //or an actual setting

				string currentKey = currentLine.Substring(0, currentLine.IndexOf("=", StringComparison.Ordinal));

				string currentValue = currentLine.Substring(currentLine.IndexOf("=", StringComparison.Ordinal) + 1);

				AddValue(currentCategory, currentKey, currentValue, true);
			}

			sr.Close(); //closes the stream
		}

		private void LoadFromXml() //loads the settings from an xml file
		{
			Settings.ReadXml(fileName);
		}

		private void initializeDataTable() //re-initializes the table with the proper columns
		{
			Settings = new DataTable {TableName = "Settings"};

			Settings.Columns.Add("Category", typeof(string));
			Settings.Columns.Add("SettingKey", typeof(string));
			Settings.Columns.Add("SettingsValue", typeof(string));
		}

		#endregion

	}
}
