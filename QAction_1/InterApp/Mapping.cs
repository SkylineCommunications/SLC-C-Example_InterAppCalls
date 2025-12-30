// Ignore Spelling: App

namespace Skyline.Protocol.InterApp
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.InterAppMessages;
	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages.MyTable;
	using Skyline.Protocol.InterApp.Executors.MyTable;

	public static class Mapping
	{
		public static Dictionary<Type, Type> InternalMessageToExecutorMapping { get; } = new Dictionary<Type, Type>
		{
			{ typeof(GenericInterAppMessage<SimpleCreateExampleRow>),   typeof(SimpleCreateExampleRowExecutor) },
			{ typeof(GenericInterAppMessage<AdvancedCreateExampleRow>), typeof(AdvancedCreateExampleRowExecutor) },
			{ typeof(GenericInterAppMessage<DelayedCreateExampleRow>),  typeof(DelayedCreateExampleRowExecutor) },
		};
	}
}
