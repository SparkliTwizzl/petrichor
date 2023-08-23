namespace PluralityUtilities.AutoHotkeyScripts.Exceptions
{
	[ Serializable ]
	public class MissingValueException : Exception
	{
		public MissingValueException() : base() { }
		public MissingValueException( string message ) : base( message ) { }
		public MissingValueException( string message, Exception inner ) : base( message, inner ) { }
	}
}
