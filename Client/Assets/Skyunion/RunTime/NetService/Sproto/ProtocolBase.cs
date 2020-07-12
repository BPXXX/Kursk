using System;
using ILRuntime.Other;

namespace Sproto
{
    [NeedAdaptorAttribute]
	public abstract class ProtocolBase {
		private ProtocolFunctionDictionary _Protocol = new ProtocolFunctionDictionary ();
		public ProtocolFunctionDictionary Protocol {
			get { return _Protocol;}
		}
	}
}

