namespace PluralityUtilities.Common.Containers
{
	public class Token
	{
		public List< Token > Body { get; set; } = new List< Token >();
		public string Name { get; set; } = string.Empty;
		public string Value { get; set; } = string.Empty;


		public static Token Empty => new();


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


		public override bool Equals( object? obj )
		{
			if ( obj is Token )
			{
				return Equals( (Token) obj );
			}
			return base.Equals( obj );
		}

		public bool Equals( Token other )
		{
			var areBodiesEqual = Body.ToArray() == other.Body.ToArray();
			var areNamesEqual = Name == other.Name;
			var areValuesEqual = Value == other.Value;
			return areBodiesEqual && areNamesEqual && areValuesEqual;
		}

		public override int GetHashCode()
		{
			return Body.GetHashCode() ^ Name.GetHashCode() ^ Value.GetHashCode();
		}
	}
}
