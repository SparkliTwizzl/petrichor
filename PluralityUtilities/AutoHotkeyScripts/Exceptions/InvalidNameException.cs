namespace PluralityUtilities.AutoHotkeyScripts.Exceptions
{
	[ Serializable ]
	public class InvalidNameException : Exception
	{
		public InvalidNameException() : base() { }
		public InvalidNameException( string message ) : base( message ) { }
		public InvalidNameException( string message, Exception inner ) : base( message, inner ) { }
	}
}
