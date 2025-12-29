using System;

using Newtonsoft.Json;

using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls;
using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.InterAppMessages;
using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages.MyTable;
using Skyline.DataMiner.Scripting;

/// <summary>
/// DataMiner QAction Class.
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
			var selectedElement = Convert.ToString(protocol.GetParameter(Parameter.element));
			var selectedMessage = Convert.ToString(protocol.GetParameter(Parameter.messagetype));
			var numericValue = Convert.ToInt32(protocol.GetParameter(Parameter.mynumericvalue));
			var stringValue = Convert.ToString(protocol.GetParameter(Parameter.mystringvalue));
			var discreetValue = Convert.ToInt32(protocol.GetParameter(Parameter.mydiscreetvalue));
			var interAppElement = new ExampleInterAppCalls(protocol.SLNet.RawConnection, selectedElement);
			var interappData = new MyTableData
			{
				MyNumericColumn = numericValue,
				MyStringColumn = stringValue,
				MyDiscreetColumn = (DiscreetColumnOption)discreetValue,
			};
			var message = CreateMessage(selectedMessage, interappData);
			var response = interAppElement.SendSingleResponseMessage(message);
			protocol.Log($"QA{protocol.QActionID}|InterAppMessage|Response|{JsonConvert.SerializeObject(response)}", LogType.Information, LogLevel.NoLogging);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}

	private static IExampleRequest CreateMessage(string messageType, MyTableData data)
	{
		switch(messageType)
		{
			case nameof(SimpleCreateExampleRow):
				return new SimpleCreateExampleRow
				{
					ExampleData = data,
				};

			case nameof(AdvancedCreateExampleRow):
				return new AdvancedCreateExampleRow
				{
					ExampleData = data,
				};

			case nameof(DelayedCreateExampleRow):
				return new DelayedCreateExampleRow
				{
					ExampleData = data,
				};

			default:
				throw new NotSupportedException();
		}
	}
}
