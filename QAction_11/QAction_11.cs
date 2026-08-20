using System;
using System.Linq;

using Skyline.DataMiner.ConnectorAPI.ExampleInterAppCalls.Messages;
using Skyline.DataMiner.Scripting;
using Skyline.DataMiner.Utils.Protocol.Extension;
using Skyline.Protocol.InterApp;
using Skyline.Protocol.Tables;

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
			var messageGuid = Convert.ToString(protocol.GetParameter(Parameter.generic_datasource_postvalidation_simulatedmessage_5007));
			var interAppRow = InterAppMessagesRecord.FromPK(protocol, messageGuid);

			interAppRow.Request.TryExecute(protocol, protocol, Mapping.InternalMessageToExecutorMapping, out var returnMessage);
			interAppRow.Status = IAC_MessageStatus.Completed;
			interAppRow.CompletedAt = DateTime.UtcNow;
			if (returnMessage is null)
			{
				protocol.SetParameter(Parameter.generic_datasource_postvalidation_simulatedmessage_5007, String.Empty);
				protocol.RunAction(Actions.execute_next_inter_app_message_9000099);
				interAppRow.SaveToProtocol(protocol);
				return;
			}

			interAppRow.Response = returnMessage;
			interAppRow.SaveToProtocol(protocol);

			if (interAppRow.Request.ExpectsReply)
			{
				interAppRow.Request.Reply(protocol.SLNet.RawConnection, returnMessage, Types.KnownTypes);
			}

			protocol.SetParameter(Parameter.generic_datasource_postvalidation_simulatedmessage_5007, String.Empty);
			protocol.RunAction(Actions.execute_next_inter_app_message_9000099);
		}
		catch (Exception ex)
		{
			protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
		}
	}
}
