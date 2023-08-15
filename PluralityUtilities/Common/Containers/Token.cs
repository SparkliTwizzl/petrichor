using PluralityUtilities.Logging;

namespace PluralityUtilities.Common.Containers
{
	public class Token
	{
		public List< Token > Body { get; set; } = new List< Token >();
		public string Name { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;


		public static Token Empty => new Token();


		public Token() { }
		public Token( Token other )
		{
			Name = other.Name;
			Value = other.Value;
			Body = other.Body;
		}
		public Token( string name, string value, List< Token > body )
		{
			Name = name;
			Value = value;
			Body = body;
		}


		public static bool operator==( Token left, Token right )
		{
			return left.Equals( right );
		}

		public static bool operator!=( Token left, Token right )
		{
			return !left.Equals( right );
		}

		public static bool Equals( Token left, Token right )
		{
			Logger.WriteLine( $"[[ { left } ]], [[ { right } ]]" );
			var leftBody = left.Body.ToArray();
			var rightBody = right.Body.ToArray();
			if ( leftBody.Length != rightBody.Length )
			{
				return false;
			}

			var areBodiesEqual = true;
			for ( var i = 0; i < leftBody.Length; ++i )
			{
				if ( leftBody[ i ] != rightBody[ i ] )
				{
					areBodiesEqual = false;
					break;
				}
			}

			var areNamesEqual = left.Name == right.Name;
			var areValuesEqual = left.Value == right.Value;
			return areBodiesEqual && areNamesEqual && areValuesEqual;
		}
	}
}
