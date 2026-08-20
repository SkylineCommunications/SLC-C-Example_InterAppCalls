// Ignore Spelling: App

namespace Skyline.Protocol.InterApp
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages;
	using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages.GenericDataMiner;
	using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages.GenericDataSource;
	using Skyline.Protocol.InterApp.Executors.DataMiner;

	public static class Mapping
	{
		public static Dictionary<Type, Type> InternalMessageToExecutorMapping { get; } = new Dictionary<Type, Type>
		{
			{ typeof(GenericInterAppMessage<DataMinerStringConfigRequest>),    typeof(DataMinerStringConfigExecutor) },
			{ typeof(GenericInterAppMessage<DataMinerDiscreetConfigRequest>),  typeof(DataMinerDiscreetConfigExecutor) },
			{ typeof(GenericInterAppMessage<DataMinerNumberConfigRequest>),    typeof(DataMinerNumberConfigExecutor) },
			{ typeof(GenericInterAppMessage<DataMinerBooleanConfigRequest>),   typeof(DataMinerBooleanConfigExecutor) },

			{ typeof(GenericInterAppMessage<DataSourceStringConfigRequest>),   typeof(DataSourceStringConfigExecutor) },
			{ typeof(GenericInterAppMessage<DataSourceDiscreetConfigRequest>), typeof(DataSourceDiscreetConfigExecutor) },
			{ typeof(GenericInterAppMessage<DataSourceNumberConfigRequest>),   typeof(DataSourceNumberConfigExecutor) },
			{ typeof(GenericInterAppMessage<DataSourceBooleanConfigRequest>),  typeof(DataSourceBooleanConfigExecutor) },
		};
	}
}
