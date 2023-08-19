namespace PluralityUtilities.Common.Exceptions
{
	[ Serializable ]
	public class IndentImbalanceException : Exception
	{
		public IndentImbalanceException() : base() { }
		public IndentImbalanceException( string message ) : base( message ) { }
		public IndentImbalanceException( string message, Exception inner ) : base( message, inner ) { }
	}
}
