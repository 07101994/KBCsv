using System;
using System.Collections.Generic;
using System.Globalization;

namespace Kent.Boogaart.KBCsv
{
	/// <summary>
	/// Represents a header record from a CSV source.
	/// </summary>
	/// <remarks>
	/// An instance of this class represents the header record in a CSV data source. Such a record defines only the column names for the CSV data.
	/// </remarks>
	[Serializable]
	public sealed class HeaderRecord : RecordBase
	{
		/// <summary>
		/// Maps column names to indexes.
		/// </summary>
		private IDictionary<string, int> _columnToIndexMap;

		/// <summary>
		/// Gets the index of the specified column in this header record.
		/// </summary>
		/// <remarks>
		/// This indexer can be used to determine the corresponding index of a named column in this header record. If the specified column is not found
		/// in this header record, an exception is thrown.
		/// </remarks>
		public int this[string column]
		{
			get
			{
				if (!_columnToIndexMap.ContainsKey(column))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No column named '{0}' exists in this header record.", column));
				}

				return _columnToIndexMap[column];
			}
		}

		/// <summary>
		/// Creates and returns an instance of <c>HeaderRecord</c> by parsing via the provided CSV parser.
		/// </summary>
		/// <param name="parser">
		/// The CSV parser to use.
		/// </param>
		/// <returns>
		/// The CSV header record, or <see langword="null"/> if no record was found in the parser provided.
		/// </returns>
		internal static HeaderRecord FromParser(CsvParser parser)
		{
			HeaderRecord retVal = null;
			string[] values = parser.ParseRecord();

			if (values != null)
			{
				retVal = new HeaderRecord(values, true);
			}

			return retVal;
		}

		/// <summary>
		/// Constructs an empty instance of <c>HeaderRecord</c>.
		/// </summary>
		public HeaderRecord()
		{
		}

		/// <summary>
		/// Constructs an instance of <c>HeaderRecord</c> with the columns specified.
		/// </summary>
		/// <param name="columns">
		/// The columns in the CSV header.
		/// </param>
		public HeaderRecord(string[] columns) : this(columns, false)
		{
		}

		/// <summary>
		/// Constructs an instance of <c>HeaderRecord</c> with the columns specified, optionally making the values in the header record read-only.
		/// </summary>
		/// <param name="columns">
		/// The columns in the CSV header.
		/// </param>
		/// <param name="readOnly">
		/// If <see langword="true"/>, the values in this header record are read-only.
		/// </param>
		public HeaderRecord(string[] columns, bool readOnly) : base(columns, readOnly)
		{
			_columnToIndexMap = new Dictionary<string, int>();

			//populate the dictionary with column name -> index mappings
			for (int i = 0; i < Values.Count; ++i)
			{
				string columnName = Values[i];

				if (_columnToIndexMap.ContainsKey(columnName))
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "A column named '{0}' appears more than once in the header record.", columnName));
				}

				_columnToIndexMap[columnName] = i;
			}
		}
	}
}