using PluralityUtilities.AutoHotkeyScripts.Containers;
using PluralityUtilities.AutoHotkeyScripts.LookUpTables;
using PluralityUtilities.AutoHotkeyScripts.Utilities.Interfaces;
using PluralityUtilities.Common;
using PluralityUtilities.Common.Utilities;
using PluralityUtilities.Logging;
using System.Text;

namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class AutoHotkeyScriptGenerator : IAutoHotkeyScriptGenerator
	{
		private IInputParser InputParser { get; set; }


		public AutoHotkeyScriptGenerator( IInputParser inputParser )
		{
			InputParser = inputParser;
		}


		public void GenerateScriptFromInputFile( string inputFile, string outputFile )
		{
			var input = InputParser.ParseInputFile( inputFile );
			var macros = GenerateMacrosFromInput( input );
			GenerateScriptFromMacros( outputFile, macros );
		}


		private static List< string > GenerateAllIdentityMacrosFromTemplates( string[] templates, Identity identity, string pronoun, string decoration )
		{
			var macros = new List< string >();
			foreach ( var template in templates )
			{
				macros.Add( GenerateIdentityMacroFromTemplate( template, identity, pronoun, decoration ) );
			}
			return macros;
		}

		private static List< string > GenerateAllEntryMacrosFromTemplates( string[] templates, Entry entry )
		{
			var macros = new List< string >();
			foreach ( var identity in entry.Identities )
			{
				macros.AddRange( GenerateAllIdentityMacrosFromTemplates( templates, identity, entry.Pronoun, entry.Decoration ) );
			}
			return macros;
		}

		private static string GenerateIdentityMacroFromTemplate( string template, Identity identity, string pronoun, string decoration )
		{
			var macro = template;
			Dictionary< string, string > fields = new()
			{
				{ "name", identity.Name },
				{ "tag", identity.Tag },
				{ "pronoun", pronoun },
				{ "decoration", decoration },
			 };
			foreach ( var marker in TemplateMarkers.LookUpTable )
			{
				macro = macro.Replace( $"`{ marker.Value }`", fields[ marker.Value ] );
			}
			return macro;
		}

		private static string[] GenerateMacrosFromInput( Input input )
		{
			var macros = new List< string >();
			foreach ( var entry in input.Entries )
			{
				macros.AddRange( GenerateAllEntryMacrosFromTemplates( input.Templates, entry ) );
			}
			return macros.ToArray();
		}

		private static void GenerateScriptFromMacros( string outputFile, string[] macros )
		{
			var outputFolder = GetNormalizedOutputFolder( outputFile );
			var outputFileName = GetNormalizedOutputFileName( outputFile );
			var outputFilePath = $"{ outputFolder }{ outputFileName }";

			Log.Info( $"started generating output file ({ outputFilePath })" );
			Directory.CreateDirectory( outputFolder );
			WriteByteOrderMarkToFile( outputFilePath );
			WriteHeaderToFile( outputFilePath );
			WriteLinesToFile( outputFilePath, macros );
			Log.Info( $"successfully generated output file ({ outputFilePath })" );
		}

		private static string GetNormalizedOutputFolder( string outputFile )
		{
			var outputFolder = outputFile.GetDirectory();
			if ( outputFolder == string.Empty )
			{
				return ProjectDirectories.OutputDir;
			}
			return outputFolder;
		}

		private static string GetNormalizedOutputFileName( string outputFile )
		{
			return $"{ outputFile.GetFileName().RemoveFileExtension() }.ahk";
		}

		private static void WriteByteOrderMarkToFile( string outputFilePath )
		{
			var encoding = Encoding.UTF8;
			using FileStream stream = new( outputFilePath, FileMode.Create );
			using BinaryWriter writer = new( stream, encoding );
			writer.Write(encoding.GetPreamble());
		}

		private static void WriteHeaderToFile( string outputFilePath )
		{
			var header = new string[]
			{
				"#SingleInstance Force",
				""
			 };
			WriteLinesToFile( outputFilePath, header );
		}

		private static void WriteLineToFile( string outputFilePath, string line = "" )
		{
			try
			{
				using StreamWriter writer = File.AppendText( outputFilePath );
				writer.WriteLine( line );
				Log.Info( $"wrote line to output file: { line }" );
			}
			catch ( Exception e )
			{
				var rethrow = new FileLoadException( "failed to write to output file", e );
				Log.Exception( rethrow );
				throw rethrow;
			}
		}

		private static void WriteLinesToFile( string outputFilePath, string[] data )
		{
			foreach ( string line in data )
			{
				WriteLineToFile( outputFilePath, line );
			}
		}
	}
}
