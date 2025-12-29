// Ignore Spelling: Pid

namespace QAction_990
{
	using System;

	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.InterAppMessages;
	using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages.MyTable;
	using Skyline.DataMiner.Scripting;
	using Skyline.DataMiner.Utils.Table.ContextMenu;
	using Skyline.Protocol.InterApp;

	public enum MyTableAction
	{
		SimpleAdd = 1,
		AdvancedAdd = 2,
		AdvancedAddWrong = 3,
		DelayedAdd = 4,
	}

	internal class ContextMenuTableManagerMyTable : ContextMenu<MyTableAction>
	{
		public ContextMenuTableManagerMyTable(SLProtocol protocol, object contextMenuData, int tablePid)
			: base(protocol, contextMenuData, tablePid)
		{
		}

		public override void ProcessContextMenuAction()
		{
			switch (this.Action)
			{
				case MyTableAction.SimpleAdd:
					SimpleCreate();
					break;

				case MyTableAction.AdvancedAdd:
					AdvancedCreate();
					break;

				case MyTableAction.AdvancedAddWrong:
					AdvancedCreateWrong();
					break;

				case MyTableAction.DelayedAdd:
					DelayedCreate();
					break;

				default:
					Protocol.Log($"QA{Protocol.QActionID}|ContextMenuTableManagerMyTable|ProcessContextMenuAction|Unknown action.", LogType.Error, LogLevel.NoLogging);
					return;
			}
		}

		protected void SimpleCreate()
		{
			// Prepare
			var value1 = Convert.ToDouble(Data[0]);
			var value2 = Convert.ToString(Data[1]);
			var value3 = (DiscreetColumnOption)Convert.ToInt32(Data[2]);

			// Create the InterApp Message
			var message = new GenericInterAppMessage<SimpleCreateExampleRow>(
				new SimpleCreateExampleRow
				{
					ExampleData = new MyTableData
					{
						MyNumericColumn = value1,
						MyStringColumn = value2,
						MyDiscreetColumn = value3,
					},
				});

			// Since the InterApp message is for the current element can't use the InterAppFactory to build our message.
			// We can execute it immediately, without going through SLNet
			message.TryExecute(Protocol, Protocol, Mapping.MessageToExecutorMapping, out var response);

			// Log the result
			var result = response as GenericInterAppMessage<SimpleCreateExampleRowResult>;
			Protocol.ShowInformationMessage(result?.Data.Description);
		}

		protected void AdvancedCreate()
		{
			// Prepare
			var value1 = Convert.ToDouble(Data[0]);
			var value2 = Convert.ToString(Data[1]);
			var value3 = (DiscreetColumnOption)Convert.ToInt32(Data[2]);

			// Create the InterApp Message
			var message = new GenericInterAppMessage<AdvancedCreateExampleRow>(
				new AdvancedCreateExampleRow
				{
					ExampleData = new MyTableData
					{
						MyNumericColumn = value1,
						MyStringColumn = value2,
						MyDiscreetColumn = value3,
					},
				});

			// Since the InterApp message is for the current element can't use the InterAppFactory to build our message.
			// We can execute it immediately, without going through SLNet
			message.TryExecute(Protocol, Protocol, Mapping.MessageToExecutorMapping, out var response);

			// Log the result
			var result = response as GenericInterAppMessage<AdvancedCreateExampleRowResult>;
			Protocol.ShowInformationMessage(result?.Data.Description);
		}

		protected void AdvancedCreateWrong()
		{
			// Prepare
			var value1 = 110;
			var value2 = Convert.ToString(Data[0]);
			var value3 = (DiscreetColumnOption)Convert.ToInt32(Data[1]);

			// Create the InterApp Message
			var message = new GenericInterAppMessage<AdvancedCreateExampleRow>(
				new AdvancedCreateExampleRow
				{
					ExampleData = new MyTableData
					{
						MyNumericColumn = value1,
						MyStringColumn = value2,
						MyDiscreetColumn = value3,
					},
				});

			// Since the InterApp message is for the current element can't use the InterAppFactory to build our message.
			// We can execute it immediately, without going through SLNet
			message.TryExecute(Protocol, Protocol, Mapping.MessageToExecutorMapping, out var response);

			// Log the result
			var result = response as GenericInterAppMessage<AdvancedCreateExampleRowResult>;
			Protocol.ShowInformationMessage(result?.Data.Description);
		}

		protected void DelayedCreate()
		{
			// Prepare
			var value1 = Convert.ToDouble(Data[0]);
			var value2 = Convert.ToString(Data[1]);
			var value3 = (DiscreetColumnOption)Convert.ToInt32(Data[2]);

			// Create the InterApp Message
			var message = new GenericInterAppMessage<DelayedCreateExampleRow>(
				new DelayedCreateExampleRow
				{
					ExampleData = new MyTableData
					{
						MyNumericColumn = value1,
						MyStringColumn = value2,
						MyDiscreetColumn = value3,
					},
				});

			// Since the InterApp message is for the current element can't use the InterAppFactory to build our message.
			// We can execute it immediately, without going through SLNet
			message.TryExecute(Protocol, Protocol, Mapping.MessageToExecutorMapping, out _);
		}
	}
}