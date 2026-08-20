using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.Messages;
using Skyline.DataMiner.Core.InterAppCalls.Common.CallBulk;
using Skyline.DataMiner.Core.InterAppCalls.Common.CallSingle;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.Protocol.InterApp;
using Skyline.Protocol.Tables;

using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

/// <summary>
/// DataMiner QAction Class.
/// This QAction will parse incoming InterApp Calls and execute the correct MessageExecutor.
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
				case Parameter.iac_generic_receiver_9000000:
					HandleIncomingMessage(protocol);
					break;

				case Dummies.inter_app_messages_execute_next_9000099:
					HandleNextMessage(protocol);
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

	private static void HandleIncomingMessage(SLProtocol protocol)
	{
		// Get the incoming InterApp Message
		var raw = Convert.ToString(protocol.GetParameter(protocol.GetTriggerParameter()));
		var receivedCall = InterAppCallFactory.CreateFromRaw(raw, Types.KnownTypes);

		// Handle the message
		foreach (var message in receivedCall.Messages)
		{
			// Save to InterApp table with status Confirmed
			var interAppRow = InterAppMessagesRecord.CreateFromMessage(message);
			interAppRow.SaveToProtocol(protocol);

			ExecuteSingleInterAppMessage(protocol, interAppRow);
		}
	}

	private static void HandleNextMessage(SLProtocol protocol)
	{
		var interAppTable = new InterAppMessagesRecords(protocol);
		var nextRow = interAppTable.Rows
			.Where(r => r.Status == IAC_MessageStatus.Confirmed)
			.OrderBy(r => r.ReceivedAt)
			.FirstOrDefault();

		if (nextRow is null)
		{
			// No more messages to process
			return;
		}

		ExecuteSingleInterAppMessage(protocol, nextRow);
	}

	private static void ExecuteSingleInterAppMessage(SLProtocol protocol, InterAppMessagesRecord interAppRow)
	{
		interAppRow.Request.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var returnMessage);
		if (returnMessage is null)
		{
			return;
		}

		// If completed directly we can mark it as complete and send the response back to the sender
		interAppRow.Request.Reply(protocol.SLNet.RawConnection, returnMessage, Types.KnownTypes);
		interAppRow.Status = IAC_MessageStatus.Completed;
		interAppRow.CompletedAt = DateTime.UtcNow;
		interAppRow.Response = returnMessage;
		interAppRow.SaveToProtocol(protocol);
	}
}
