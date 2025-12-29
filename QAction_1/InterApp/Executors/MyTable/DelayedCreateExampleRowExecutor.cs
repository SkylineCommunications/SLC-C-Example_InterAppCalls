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
	using Skyline.Protocol.Tables;

	public class DelayedCreateExampleRowExecutor
		: SimpleMessageExecutor<GenericInterAppMessage<DelayedCreateExampleRow>>
	{
		private readonly DelayedCreateExampleRowResult result;

		public DelayedCreateExampleRowExecutor(GenericInterAppMessage<DelayedCreateExampleRow> message)
			: base(message)
		{
			result = new DelayedCreateExampleRowResult
			{
				Request = message.Data,
			};
		}

		public override bool TryExecute(object dataSource, object dataDestination, out Message optionalReturnMessage)
		{
			var protocol = (SLProtocol)dataSource;

			// Create a new unique primary key to be added
			var id = Guid.NewGuid().ToString();
			Message.Data.ExampleData.Instance = id;

			// Add the delayed InterApp Message to an internal buffer
			new IAC_MessagesTableRow
			{
				Guid = Guid.Parse(Message.Guid),
				Status = IAC_MessageStatus.Bufferred,
				Request = Message,
				RequestType = typeof(GenericInterAppMessage<DelayedCreateExampleRow>),
				Response = new GenericInterAppMessage<DelayedCreateExampleRowResult>(result),
				ResponseType = typeof(GenericInterAppMessage<DelayedCreateExampleRowResult>),
				Info = id,
			}.SaveToProtocol(protocol);

			// Mimic for example setting a HTTP body and triggering a group.
			protocol.SetParameter(Parameter.commandbody, JsonConvert.SerializeObject(Message.Data.ExampleData));
			protocol.CheckTrigger(11);

			optionalReturnMessage = null;
			return true;
		}
	}
}
