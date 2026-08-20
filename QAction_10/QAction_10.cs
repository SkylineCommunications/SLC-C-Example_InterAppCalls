using System;

using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages;
using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages.GenericDataMiner;
using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages.GenericDataSource;
using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.Messages;
using Skyline.DataMiner.Core.InterAppCalls.Common.CallSingle;
using Skyline.DataMiner.Scripting;
using Skyline.Protocol.InterApp;
using Skyline.Protocol.Tables;

/// <summary>
/// DataMiner QAction Class.
/// This QAction is triggered when a simulated response comes in.
/// </summary>
public static class QAction
{
	/// <summary>
	/// The QAction entry point.
	/// </summary>
	/// <param name="protocol">Link with SLProtocol process.</param>
	public static void Run(SLProtocol protocol)
	{
		try
		{
			switch (protocol.GetTriggerParameter())
			{
				case Parameter.Write.generic_dm_directvalidation_buttontest_1001:
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Triggered by generic_dm_directvalidation_buttontest_1001", LogType.Information, LogLevel.NoLogging);
					GenericDataMiner(protocol);
					break;

				case Parameter.Write.generic_datasource_postvalidation_buttontest_5001:
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Triggered by generic_datasource_postvalidation_buttontest_5001", LogType.Information, LogLevel.NoLogging);
					GenericDataSource(protocol);
					break;

				case Parameter.Write.customer1_dm_directvalidation_buttontest_10001:
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Triggered by customer1_dm_directvalidation_buttontest_10001", LogType.Information, LogLevel.NoLogging);
					break;

				default:
					protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Triggered by unknown parameter", LogType.Error, LogLevel.NoLogging);
					break;
			}
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static void GenericDataMiner(SLProtocol protocol)
	{
		var rnd = new Random();

		// String
		var stringInterApp = new GenericInterAppMessage<DataMinerStringConfigRequest>(
			new DataMinerStringConfigRequest
			{
				Config = $"Hello from InterApp with random double {rnd.NextDouble()}",
			});
		stringInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var stringReturnMessage);

		// Number
		var numberInterApp = new GenericInterAppMessage<DataMinerNumberConfigRequest>(
			new DataMinerNumberConfigRequest
			{
				Config = rnd.Next(0, 101), // Upper bounds is exclusive, so 101 is used to include 100.
			});
		numberInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var numberReturnMessage);

		// Discreet
		var discreetInterApp = new GenericInterAppMessage<DataMinerDiscreetConfigRequest>(
			new DataMinerDiscreetConfigRequest
			{
				Config = (DataMinerDiscreet)rnd.Next(0, 3),
			});
		discreetInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var discreetReturnMessage);

		// Boolean
		var booleanInterApp = new GenericInterAppMessage<DataMinerBooleanConfigRequest>(
			new DataMinerBooleanConfigRequest
			{
				Config = rnd.Next(0, 2) == 0,
			});
		booleanInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var booleanReturnMessage);
	}

	private static void GenericDataSource(SLProtocol protocol)
	{
		var rnd = new Random();

		// String
		var stringInterApp = new GenericInterAppMessage<DataSourceStringConfigRequest>(
			new DataSourceStringConfigRequest
			{
				Config = $"Hello from InterApp with random double {rnd.NextDouble()}",
			});
		InterAppMessagesRecord.CreateFromMessage(stringInterApp).SaveToProtocol(protocol);
		stringInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var stringReturnMessage);

		// Number
		var numberInterApp = new GenericInterAppMessage<DataSourceNumberConfigRequest>(
			new DataSourceNumberConfigRequest
			{
				Config = rnd.Next(0, 101), // Upper bounds is exclusive, so 101 is used to include 100.
			});
		InterAppMessagesRecord.CreateFromMessage(numberInterApp).SaveToProtocol(protocol);
		numberInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var numberReturnMessage);

		// Discreet
		var discreetInterApp = new GenericInterAppMessage<DataSourceDiscreetConfigRequest>(
			new DataSourceDiscreetConfigRequest
			{
				Config = (DataSourceDiscreet)rnd.Next(0, 3),
			});
		InterAppMessagesRecord.CreateFromMessage(discreetInterApp).SaveToProtocol(protocol);
		discreetInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var discreetReturnMessage);

		// Boolean
		var booleanInterApp = new GenericInterAppMessage<DataSourceBooleanConfigRequest>(
			new DataSourceBooleanConfigRequest
			{
				Config = rnd.Next(0, 2) == 0,
			});
		InterAppMessagesRecord.CreateFromMessage(booleanInterApp).SaveToProtocol(protocol);
		booleanInterApp.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var booleanReturnMessage);
	}
}
