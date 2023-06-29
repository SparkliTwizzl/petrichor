namespace PluralityUtilities.Common.Exceptions
{
	[ Serializable ]
	public class InvalidInputException : Exception
	{
		public InvalidInputException() : base() { }
		public InvalidInputException( string message ) : base( message ) { }
		public InvalidInputException( string message, Exception inner ) : base( message, inner ) { }
	}
}
