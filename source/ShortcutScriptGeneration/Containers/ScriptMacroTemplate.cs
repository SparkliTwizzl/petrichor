﻿namespace Petrichor.ShortcutScriptGeneration.Containers
{
	public sealed class ScriptMacroTemplate : IEquatable<ScriptMacroTemplate>
	{
		public string TemplateString { get; set; } = string.Empty;
		public Dictionary<string, string> FindAndReplace { get; set; } = new();


		public ScriptMacroTemplate() { }
		public ScriptMacroTemplate( string templateString, Dictionary<string, string> findAndReplace )
		{
			TemplateString = templateString;
			FindAndReplace = findAndReplace;
		}
		public ScriptMacroTemplate( ScriptMacroTemplate other )
		{
			TemplateString = other.TemplateString;
			FindAndReplace = other.FindAndReplace;
		}


		public override bool Equals( object? obj )
		{
			if ( obj is null )
			{
				return false;
			}
			var other = obj as ScriptMacroTemplate;
			return Equals( other );
		}

		public bool Equals( ScriptMacroTemplate? other )
		{
			if ( other is null )
			{
				return false;
			}
			return TemplateString.Equals( other.TemplateString ) && FindAndReplace.Equals( other.FindAndReplace );
		}

		public override int GetHashCode() => TemplateString.GetHashCode() ^ FindAndReplace.GetHashCode();

		public override string ToString() => TemplateString;

		public static bool operator ==( ScriptMacroTemplate a, ScriptMacroTemplate b ) => a.Equals( b );

		public static bool operator !=( ScriptMacroTemplate a, ScriptMacroTemplate b ) => !a.Equals( b );
	}
}