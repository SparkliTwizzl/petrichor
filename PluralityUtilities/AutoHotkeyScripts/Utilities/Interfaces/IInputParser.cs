using PluralityUtilities.AutoHotkeyScripts.Containers;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Interfaces
{
	public interface IInputParser
	{
		Input ParseInputFile( string inputFilePath );
	}
}
