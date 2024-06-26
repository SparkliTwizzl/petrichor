﻿using Petrichor.Common.Containers;

namespace Petrichor.Common.Utilities
{
	public interface ITokenBodyParser<T> where T : class, new()
	{
		static Func<IndexedString[], int, T, ProcessedRegionData<T>> InertHandler => ( IndexedString[] regionData, int tokenStartIndex, T result ) => new() { Value = result };


		int LinesParsed { get; }
		DataToken RegionToken { get; }
		Dictionary<string, int> TokenInstancesParsed { get; }


		void AddTokenHandler( DataToken token, Func<IndexedString[], int, T, ProcessedRegionData<T>> handler );
		void CancelParsing();
		T Parse( IndexedString[] regionData, T? input = null );
		void Reset();
	}
}
