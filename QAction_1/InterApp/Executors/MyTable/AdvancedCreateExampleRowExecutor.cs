// Ignore Spelling: App

namespace Skyline.Protocol.InterApp.Executors.MyTable
{
	using System;
	using Newtonsoft.Json;

	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.InterAppMessages;
	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages.MyTable;
	using Skyline.DataMiner.Core.InterAppCalls.Common.CallSingle;
	using Skyline.DataMiner.Core.InterAppCalls.Common.MessageExecution;
	using Skyline.DataMiner.Scripting;

	public class AdvancedCreateExampleRowExecutor : MessageExecutor<GenericInterAppMessage<AdvancedCreateExampleRow>>
	{
		private AdvancedCreateExampleRowResult result;

		public AdvancedCreateExampleRowExecutor(GenericInterAppMessage<AdvancedCreateExampleRow> message) : base(message)
		{
		}

		// Step 1, Is always executed
		public override void DataGets(object dataSource)
		{
			// Can be used to fetch other data needed to handle this InterApp Call.
		}

		// Step 2, Is always executed
		public override void Parse()
		{
			/* If you need to parse, some of the data you can do this here.
			 * For example a property id in the following format: {AgentID}/{ElementID}
			 */

			// We are going to use this to start building our response message. But you could do this in any of the methods.
			result = new AdvancedCreateExampleRowResult
			{
				Success = false,
				RowKey = string.Empty,
				Description = string.Empty,
			};
		}

		// Step 3, Is always executed
		public override bool Validate()
		{
			// Here you can validate the request, Check if all the necessary data is present.

			// We are going to check if value1 is within the range we specified in the protocol.xml
			// Value 1 is a percentage, which means it can't be lower than 0 and can't be higher then 100.
			if (!Message.Data.ExampleData.MyNumericColumn.HasValue ||
				Message.Data.ExampleData.MyNumericColumn.Value < 0 ||
				Message.Data.ExampleData.MyNumericColumn.Value > 100)
			{
				result.Description = $"The request should contain a value between 0 and 100, for property {nameof(Message.Data.ExampleData.MyNumericColumn)}. Instead got '{Message.Data.ExampleData.MyNumericColumn}'.";
				return false;
			}

			return true;
		}

		// Step 4, Only if the validate was successful
		public override void Modify()
		{
			/* Here you can modify the InterApp Call into something the device can understand.
			 * For example the device wants an XML. You can create that object here.
			 */
		}

		// Step 5, Only if the validate was successful
		public override void DataSets(object dataDestination)
		{
			// Here you do the actual set, in our case this is the adding of a new row to the Example Table.
			var protocol = (SLProtocol)dataDestination;

			var newId = Guid.NewGuid().ToString();
			if (!protocol.Exists(Parameter.Mytable.tablePid, newId))
			{
				// Mimic for example setting a http body and triggering a group.
				Message.Data.ExampleData.Instance = newId;
				protocol.SetParameter(Parameter.commandbody, JsonConvert.SerializeObject(Message.Data.ExampleData));
				protocol.CheckTrigger(11);

				result.Description = "Successfully send a create new MyTable row message to the simulated device.";
				result.RowKey = newId;
				result.Success = true;
			}
			else
			{
				result.Description = $"An error occurred while trying to add a new row. There is already a row with ID '{newId}' in the Example Table.";
				result.RowKey = string.Empty;
				result.Success = false;
			}
		}

		// Step 6, Is always executed
		public override Message CreateReturnMessage()
		{
			// Here you can build the return message. If you don't need it you can return null.
			if(result != null)
			{
				return new GenericInterAppMessage<AdvancedCreateExampleRowResult>(result);
			}

			return null;
		}
	}
}
