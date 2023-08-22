namespace PluralityUtilities.AutoHotkeyScripts.Exceptions
{
	[ Serializable ]
	public class DuplicateValueException : Exception
	{
		public DuplicateValueException() : base() { }
		public DuplicateValueException( string message ) : base( message ) { }
		public DuplicateValueException( string message, Exception inner ) : base( message, inner ) { }
	}
}
