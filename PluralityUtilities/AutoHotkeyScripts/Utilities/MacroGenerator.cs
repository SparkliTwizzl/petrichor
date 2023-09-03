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
			//TODO trawl token, count how many times each field name appears
			return result;
		}

		private Dictionary<string, string> CreateFindAndReplaceTableForToken( Token entry )
		{
			var result = new Dictionary<string, string>();
			//TODO build token field dictionary (key = field name, value = field value)
			return result;
		}

		private Token[] FlattenEntryFields( Token entry )
		{
			var result = new List<Token>();
			//TODO prep batches of token data to be parsed into macros
			//TODO convert single token with duplicate fields into many copies with only one instance of each field, ie copy token and remove duplicates at every level
			return result.ToArray();
		}

		private List<string> GenerateMacrosFromEntry( Token entry )
		{
			var result = new List<string>();
			var flattenedEntry = FlattenEntryFields( entry );
			result.AddRange( GenerateMacrosFromFields( flattenedEntry ) );
			return result;
		}

		private List<string> GenerateMacrosFromFields( Token[] flattenedEntry )
		{
			var result = new List<string>();
			foreach ( var token in flattenedEntry )
			{
				var findAndReplaceTable = CreateFindAndReplaceTableForToken( token );
			}
			//TODO loop to create batches of macros
			return result;
		}
	}
}
