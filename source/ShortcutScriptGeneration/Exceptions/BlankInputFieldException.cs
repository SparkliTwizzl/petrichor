﻿namespace Petrichor.ShortcutScriptGeneration.Exceptions
{
	public class BlankInputFieldException : Exception
	{
		public BlankInputFieldException() : base() { }
		public BlankInputFieldException( string message ) : base( message ) { }
		public BlankInputFieldException( string message, Exception inner ) : base( message, inner ) { }
	}
}
