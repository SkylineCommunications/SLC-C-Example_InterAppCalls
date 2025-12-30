using System;
using System.Linq;

using Skyline.DataMiner.ConnectorAPI.SkylineCommunications.ExampleInterAppCalls.Messages;
using Skyline.DataMiner.Core.DataMinerSystem.Protocol;
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
            var elements = protocol.GetDms().GetElements().Where(element => element.Protocol.Name == protocol.ProtocolName);
            protocol.SetParameter(Parameter.element_discreetlist, String.Join(";", elements.Where(element => element.Name != protocol.ElementName).Select(element => element.Name)));

            protocol.SetParameter(Parameter.messagetype_discreetlist, String.Join(";", Types.KnownTypes.Where(type => !type.Name.EndsWith("Result")).Select(type => type.Name)));
        }
        catch (Exception ex)
        {
            protocol.Log($"QA{protocol.QActionID}|{protocol.GetTriggerParameter()}|Run|Exception thrown:{Environment.NewLine}{ex}", LogType.Error, LogLevel.NoLogging);
        }
    }
}
