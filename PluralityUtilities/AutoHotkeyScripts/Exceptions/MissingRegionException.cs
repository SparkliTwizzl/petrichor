namespace PluralityUtilities.AutoHotkeyScripts.Exceptions
{
	[ Serializable ]
	public class MissingRegionException : Exception
	{
		public MissingRegionException() : base() { }
		public MissingRegionException( string message ) : base( message ) { }
		public MissingRegionException( string message, Exception inner ) : base( message, inner ) { }
	}
}
