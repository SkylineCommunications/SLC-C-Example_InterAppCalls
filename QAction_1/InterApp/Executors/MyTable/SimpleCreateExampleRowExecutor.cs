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

	public class SimpleCreateExampleRowExecutor : SimpleMessageExecutor<GenericInterAppMessage<SimpleCreateExampleRow>>
	{
		public SimpleCreateExampleRowExecutor(GenericInterAppMessage<SimpleCreateExampleRow> message) : base(message)
		{
		}

		public override bool TryExecute(object dataSource, object dataDestination, out Message optionalReturnMessage)
		{
			var protocol = (SLProtocol)dataSource;

			var returnMessage = new SimpleCreateExampleRowResult
			{
				Request = Message.Data,
			};

			var newId = Guid.NewGuid().ToString();
			if (!protocol.Exists(Parameter.Mytable.tablePid, newId))
			{
				// Mimic for example setting a HTTP body and triggering a group.
				Message.Data.ExampleData.Instance = newId;
				protocol.SetParameter(Parameter.commandbody, JsonConvert.SerializeObject(Message.Data.ExampleData));
				protocol.CheckTrigger(11);

				returnMessage.Description = "Successfully send a create new MyTable row message to the simulated device.";
				returnMessage.RowKey = newId;
				returnMessage.Success = true;
			}
			else
			{
				returnMessage.Description = $"An error occurred while trying to add a new row. There is already a row with ID '{newId}' in the Example Table.";
				returnMessage.RowKey = string.Empty;
				returnMessage.Success = false;
			}

			optionalReturnMessage = new GenericInterAppMessage<SimpleCreateExampleRowResult>(returnMessage);
			return true;
		}
	}
}
