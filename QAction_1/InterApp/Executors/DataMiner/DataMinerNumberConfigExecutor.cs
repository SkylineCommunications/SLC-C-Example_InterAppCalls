namespace Skyline.Protocol.InterApp.Executors.DataMiner
{
	using System;

	using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages;
	using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.InterAppMessages.GenericDataMiner;
	using Skyline.DataMiner.Core.InterAppCalls.Common.CallSingle;
	using Skyline.DataMiner.Core.InterAppCalls.Common.MessageExecution;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Utils.Protocol.Extension;
	using Skyline.Protocol.Tables;

	public class DataMinerNumberConfigExecutor : MessageExecutor<GenericInterAppMessage<DataMinerNumberConfigRequest>>
	{
		private DataMinerNumberConfigResponse _response;

		public DataMinerNumberConfigExecutor(GenericInterAppMessage<DataMinerNumberConfigRequest> message) : base(message)
		{
			_response = new DataMinerNumberConfigResponse
			{
				Success = false,
				Description = $"An unexpected error occurred while processing the request.",
			};
		}

		/// <summary>
		/// Step 1 (Is always executed) : Reads data from SLProtocol, Engine or other data sources.
		/// </summary>
		/// <param name="dataSource">SLProtocol, Engine, or other data sources.</param>
		public override void DataGets(object dataSource)
		{
			/* Can be used to fetch other data needed to handle this InterApp Call.
			 */

			var protocol = dataSource as SLProtocol;
			if (protocol is null)
			{
				_response.Description = "The provided data source is not of type SLProtocol.";
				return;
			}

			// Can happen when executed by the element itself
			if (protocol.Exists(Parameter.Iac_messages.tablePid, Message.Guid))
			{
				protocol.SetCell(Parameter.Iac_messages.tablePid, Message.Guid, Parameter.Iac_messages.Idx.iac_messagesstatus_9000102, (int)IAC_MessageStatus.InProgress);
			}
		}

		/// <summary>
		/// Step 2 (Is always executed) : Parses the data retrieved from a data source in DataGets.
		/// </summary>
		public override void Parse()
		{
			/* If you need to parse, some of the data you can do this here.
			 * For example a property id in the following format: {AgentID}/{ElementID}
			 */
		}

		/// <summary>
		/// Step 3 (Is always executed) : Validates received data for validity.
		/// </summary>
		/// <returns>A boolean indicating if the received data is valid.</returns>
		public override bool Validate()
		{
			/* Here you can validate the request, Check if all the necessary data is present.
			 * We are going to check if the NumberValue property is not null or empty or N/A.
			 */

			if (Message.Data.Config < 0)
			{
				_response.Description = "The provided config value cannot be negative.";
				return false;
			}

			if (Message.Data.Config > 100)
			{
				_response.Description = "The provided config value cannot be greater than 100.";
				return false;
			}

			return true;
		}

		/// <summary>
		/// Step 4 (Only if the Validate was successful) : Modifies retrieved data and Message data into a correct format for setting.
		/// </summary>
		public override void Modify()
		{
			/* Here you can modify the InterApp Call into something the device can understand.
			 * For example the device wants an XML. You can create that object here.
			 */
		}

		/// <summary>
		/// Step 5 (Only if the Validate was successful): Writes data to SLProtocol, Engine, or another data destination.
		/// </summary>
		/// <param name="dataDestination">SLProtocol, Engine, or another data destination.</param>
		public override void DataSets(object dataDestination)
		{
			/* Here you do the actual set, in our case this is setting the standalone parameter
			 */
			var protocol = dataDestination as SLProtocol;
			if (protocol is null)
			{
				_response.Description = "The provided data destination is not of type SLProtocol.";
				return;
			}

			try
			{
				protocol.SetParameter(Parameter.generic_dm_directvalidation_confignumber_1003, Message.Data.Config);
				_response.Success = true;
				_response.Description = $"Successfully set the parameter value to '{Message.Data.Config}'.";
			}
			catch (Exception ex)
			{
				_response.Description = $"An error occurred while trying to set the parameter value. Exception details: {ex}";
				protocol.Log($"QA{protocol.QActionID}|{nameof(DataMinerNumberConfigExecutor)}.{nameof(DataSets)}|{Message.Guid}|An error occurred while trying to set the parameter value. Exception details: {ex}", LogType.Error, LogLevel.NoLogging);
			}
		}

		/// <summary>
		/// Step 6 (Is always executed)	: Creates a return Message.
		/// </summary>
		/// <returns>A message representing the response for the processed message, or <see langword="null"/> in case no response should be sent.</returns>
		public override Message CreateReturnMessage()
		{
			if (_response != null)
			{
				return new GenericInterAppMessage<DataMinerNumberConfigResponse>(_response);
			}

			return null;
		}
	}
}
