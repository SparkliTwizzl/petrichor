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
	}
}
