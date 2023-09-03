using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;

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


		private Dictionary<string, string> CreateFindAndReplaceTableForToken( Token token )
		{
			var result = new Dictionary<string, string>();
			var flattenedToken = TokenDataParser.FlattenTokenTree( token );
			foreach ( var field in flattenedToken )
			{
				result.Add( field.Name, field.Value );
			}
			return result;
		}

		private Token[] SeparateFieldInstances( Token entry )
		{
			var result = new List<Token>();
			//TODO prep batches of field data to be parsed into macros
			//TODO convert single field with duplicate fields into many copies with only one instance of each field, ie copy field and remove duplicates at every level
			return result.ToArray();
		}

		private List<string> GenerateMacrosFromEntry( Token entry )
		{
			var result = new List<string>();
			var separatedFields = SeparateFieldInstances( entry );
			result.AddRange( GenerateMacrosFromFields( separatedFields ) );
			return result;
		}

		private List<string> GenerateMacrosFromFields( Token[] flattenedEntry )
		{
			var result = new List<string>();
			//TODO loop to create batches of macros
			foreach ( var token in flattenedEntry )
			{
				var findAndReplaceTable = CreateFindAndReplaceTableForToken( token );
			}
			return result;
		}
	}
}
