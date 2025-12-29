using System;
using System.Linq;

using Newtonsoft.Json;

using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.InterAppMessages;
using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages;
using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages.MyTable;
using Skyline.DataMiner.Scripting;
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
			/* Get simulated device response.
			 * which in our case is just the same message we send out, but would normally be a response on a command.
			 * for example the response of a HTTP Post request.
			 */
			var raw = Convert.ToString(protocol.GetParameter(Parameter.commandbody));
			if(String.IsNullOrEmpty(raw))
			{
				return;
			}

			var response = JsonConvert.DeserializeObject<MyTableData>(raw);

			// Add the row to the table, just like you normally would.
			var row = new MyTableRow
			{
				Instance = response.Instance,
				MyNumericColumn = response.MyNumericColumn.GetValueOrDefault(0),
				MyStringColumn = response.MyStringColumn,
				MyDiscreetColumn = response.MyDiscreetColumn.GetValueOrDefault(DiscreetColumnOption.Discreet1),
			};

			protocol.AddRow(Parameter.Mytable.tablePid, row.ToProtocolRow());

			// Check the InterApp Table for messages that still need a response
			// Get all the buffered InterApp Messages that are connected to the DelayedCreateExampleRow call, the other ones are for other tables.
			var iapBuffer = new IAC_MessagesTable(protocol);
			var iapBufferRow = iapBuffer.Rows
				.Where(message => message.ResponseType == typeof(GenericInterAppMessage<DelayedCreateExampleRowResult>))
				.FirstOrDefault(message => message.Info == row.Instance);

			if (iapBufferRow == null)
			{
				// Found no message that was waiting on this response
				return;
			}

			// Get the already partially build response, and complete it.
			var iapResponse = iapBufferRow.Response as GenericInterAppMessage<DelayedCreateExampleRowResult>;
			iapResponse.Data.Success = true;
			iapResponse.Data.Description = "Successfully created a new MyTable example row.";

			// Reply to the InterApp Message, and mark this row completed.
			iapBufferRow.Request.Reply(protocol.SLNet.RawConnection, iapResponse, Types.KnownTypes);
			iapBufferRow.Status = IAC_MessageStatus.Confirmed;
			iapBuffer.SaveToProtocol(protocol);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
