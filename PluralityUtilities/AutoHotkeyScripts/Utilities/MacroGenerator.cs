using PluralityUtilities.Common.Containers;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class MacroGenerator
	{
		private Dictionary<string, string[]> Fields { get; set; } = new();
		private string[] Templates { get; set; } = Array.Empty<string>();
		private Token[] Entries { get; set; } = Array.Empty<Token>();


		public MacroGenerator() { }
		public MacroGenerator( Dictionary<string, string[]> fields, string[] templates, Token[] entries )
		{
			Fields = fields;
			Templates = templates;
			Entries = entries;
		}


		public string[] GenerateMacros()
		{
			var result = new List<string>();
			foreach ( var entry in Entries )
			{
				result.AddRange( GenerateMacrosFromEntry( entry ) );
			}
			return result.ToArray();
		}


		private Dictionary<string, int> CountFieldInstancesInEntry( Token entry )
		{
			var result = new Dictionary<string, int>();
			//TODO trawl entry, count how many times each field name appears
			return result;
		}

		private Dictionary<string, string> CreateFindAndReplaceTableForEntry( Token entry )
		{
			var result = new Dictionary<string, string>();
			//TODO build entry field dictionary (key = field name, value = field value)
			return result;
		}

		private List<string> GenerateMacrosFromEntry( Token entry )
		{
			var result = new List<string>();
			var findAndReplaceTable = CreateFindAndReplaceTableForEntry( entry );
			var fieldInstanceCounts = CountFieldInstancesInEntry( entry );
			//TODO use field instance counts and field dictionary to determine how many batches of macros to create - one batch per instance of a field name, ie number of batches will be the highest count of field instances
			//TODO prep batches of entry data to be parsed into macros
			//TODO loop to create batches of macros

			return result;
		}


	}
}
