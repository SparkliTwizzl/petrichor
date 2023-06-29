using PluralityUtilities.Common.Enums;

namespace PluralityUtilities.Common.Containers
{
	public class Tag
	{
		public const string TagStart = "<";
		public const string TagEnd = ">";
		public const string TagRegexMatch = $"{ TagStart }.+{ TagEnd }.*";

		public const string OpenTagStart = TagStart;
		public const string OpenTagEnd = TagEnd;
		public const string OpenTagRegexMatch = $"{ OpenTagStart }.+{ OpenTagEnd }.*";

		public const string CloseTagStart = $"{ TagStart }/";
		public const string CloseTagEnd = TagEnd;
		public const string CloseTagRegexMatch = $"{ CloseTagStart }.+{ CloseTagEnd }.*";

		public const string SelfCloseTagStart = TagStart;
		public const string SelfCloseTagEnd = $"/{ TagEnd }";
		public const string SelfCloseTagRegexMatch = $"{ SelfCloseTagStart }.+{ SelfCloseTagEnd }.*";

		public static Tag Empty => new Tag( TagTypes.Invalid, "" );


		public TagTypes Type { get; set; } = TagTypes.Invalid;
		public string Value { get; set; } = string.Empty;


		public Tag() {}
		public Tag( Tag other )
		{
			Type = other.Type;
			Value = other.Value;
		}
		public Tag( TagTypes type, string value )
		{
			Type = type;
			Value = value;
		}


		public static string GetTagEndString( TagTypes type )
		{
			switch ( type )
			{
				case TagTypes.Open:
					return OpenTagEnd;
				case TagTypes.Close:
					return CloseTagEnd;
				case TagTypes.SelfClose:
					return SelfCloseTagEnd;
				case TagTypes.Invalid:
				default:
					return string.Empty;
			}
		}

		public static string GetTagStartString( TagTypes type )
		{
			switch ( type )
			{
				case TagTypes.Open:
					return OpenTagStart;
				case TagTypes.Close:
					return CloseTagStart;
				case TagTypes.SelfClose:
					return SelfCloseTagStart;
				case TagTypes.Invalid:
				default:
					return string.Empty;
			}
		}
	}
}
