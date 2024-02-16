﻿// <auto-generated>This is auto-generated code by a DIS Macro. Do not modify.</auto-generated>
namespace Skyline.Protocol.Tables
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages;
	using Skyline.DataMiner.Core.InterAppCalls.Common.CallSingle;
	using Skyline.DataMiner.Core.InterAppCalls.Common.Serializing;
	using Skyline.DataMiner.Net.Helper;
	using Skyline.DataMiner.Scripting;

	using SLNetMessages = Skyline.DataMiner.Net.Messages;

	public enum IAC_MessageStatus
	{
		Bufferred = 1,
		InProgress = 2,
		Confirmed = 3,
	}

	public class IAC_MessagesTableRow
	{
		public IAC_MessagesTableRow() { }

		public IAC_MessagesTableRow(params object[] row)
		{
			Guid = Guid.Parse(Convert.ToString(row[0]));
			Status = (IAC_MessageStatus)Convert.ToInt32(row[1]);
			Request = MessageFactory.CreateFromRaw(Convert.ToString(row[2]), Types.KnownTypes);
			RequestType = Type.GetType(Convert.ToString(row[3]));
			Response = MessageFactory.CreateFromRaw(Convert.ToString(row[4]), Types.KnownTypes);
			ResponseType = Type.GetType(Convert.ToString(row[5]));
		}

		public Guid Guid { get; set; }

		public IAC_MessageStatus Status { get; set; }

		public Message Request { get; set; }

		public Type RequestType { get; set; }

		public Message Response { get; set; }

		public Type ResponseType { get; set; }

		public static IAC_MessagesTableRow FromPK(SLProtocol protocol, string pk)
		{
			var row = (object[])protocol.GetRow(Parameter.Iac_messages.tablePid, pk);
			if (row[0] == null)
			{
				return default;
			}

			return new IAC_MessagesTableRow(row);
		}

		public object[] ToProtocolRow()
		{
			return new Iac_messagesQActionRow
			{
				Iac_messagesguid_9000101 = Guid.ToString(),
				Iac_messagesstatus_9000102 = (int)Status,
				Iac_messagesrequest_9000103 = SerializerFactory.CreateInterAppSerializer(new List<Type> { RequestType }).SerializeToString(Request),
				Iac_messagesrequesttype_9000104 = RequestType.AssemblyQualifiedName,
				Iac_messagesresponse_9000105 = SerializerFactory.CreateInterAppSerializer(new List<Type> { ResponseType }).SerializeToString(Response),
				Iac_messagesresponsetype_9000106 = ResponseType.AssemblyQualifiedName,
			};
		}

		public void SaveToProtocol(SLProtocol protocol)
		{
			if (!protocol.Exists(Parameter.Iac_messages.tablePid, Guid.ToString()))
			{
				protocol.AddRow(Parameter.Iac_messages.tablePid, ToProtocolRow());
			}
			else
			{
				protocol.SetRow(Parameter.Iac_messages.tablePid, Guid.ToString(), ToProtocolRow());
			}
		}
	}

	public class IAC_MessagesTable
	{
		public IAC_MessagesTable() { }

		public IAC_MessagesTable(SLProtocol protocol)
		{
			uint[] iAC_MessagesIdx = new uint[]
			{
				Parameter.Iac_messages.Idx.iac_messagesguid_9000101,
				Parameter.Iac_messages.Idx.iac_messagesstatus_9000102,
				Parameter.Iac_messages.Idx.iac_messagesrequest_9000103,
				Parameter.Iac_messages.Idx.iac_messagesrequesttype_9000104,
				Parameter.Iac_messages.Idx.iac_messagesresponse_9000105,
				Parameter.Iac_messages.Idx.iac_messagesresponsetype_9000106,
			};
			object[] iac_messages = (object[])protocol.NotifyProtocol((int)SLNetMessages.NotifyType.NT_GET_TABLE_COLUMNS, Parameter.Iac_messages.tablePid, iAC_MessagesIdx);
			object[] gUIDIDX = (object[])iac_messages[0];
			object[] status = (object[])iac_messages[1];
			object[] request = (object[])iac_messages[2];
			object[] requestType = (object[])iac_messages[3];
			object[] response = (object[])iac_messages[4];
			object[] responseType = (object[])iac_messages[5];

			for (int i = 0; i < gUIDIDX.Length; i++)
			{
				Rows.Add(new IAC_MessagesTableRow(
				gUIDIDX[i],
				status[i],
				request[i],
				requestType[i],
				response[i],
				responseType[i]));
			}
		}

		public List<IAC_MessagesTableRow> Rows { get; set; } = new List<IAC_MessagesTableRow>();

		public void SaveToProtocol(SLProtocol protocol, bool partial = false)
		{
			// Calculate the batch size, recommended 25000 cells max per fill array, divided by the number of columns.
			var batchSize = 25000 / 6;

			// If full then the first batch needs to be a SaveOption.Full.
			var first = !partial;
			foreach (var batch in Rows.Select(x => x.ToProtocolRow()).Batch(batchSize))
			{
				if (first)
				{
					protocol.FillArray(Parameter.Iac_messages.tablePid, batch.ToList(), NotifyProtocol.SaveOption.Full);
				}
				else
				{
					protocol.FillArray(Parameter.Iac_messages.tablePid, batch.ToList(), NotifyProtocol.SaveOption.Partial);
				}
			}
		}
	}
}
