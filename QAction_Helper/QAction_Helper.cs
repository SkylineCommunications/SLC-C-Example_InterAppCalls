// <auto-generated>This is auto-generated code by DIS. Do not modify.</auto-generated>
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skyline.DataMiner.Scripting
{
public static class Parameter
{
	/// <summary>PID: 10 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int devicedelay_10 = 10;
	/// <summary>PID: 10 | Type: read</summary>
	public const int devicedelay = 10;
	/// <summary>PID: 101 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int datacreatelineup_101 = 101;
	/// <summary>PID: 101 | Type: read</summary>
	public const int datacreatelineup = 101;
	/// <summary>PID: 102 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int simulatedevicecommunication_102 = 102;
	/// <summary>PID: 102 | Type: read</summary>
	public const int simulatedevicecommunication = 102;
	/// <summary>PID: 103 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int currentactivemessage_103 = 103;
	/// <summary>PID: 103 | Type: read</summary>
	public const int currentactivemessage = 103;
	/// <summary>PID: 104 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int internaldevicecommunicationbuffer_104 = 104;
	/// <summary>PID: 104 | Type: read</summary>
	public const int internaldevicecommunicationbuffer = 104;
	/// <summary>PID: 200 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int automationscriptresponse_200 = 200;
	/// <summary>PID: 200 | Type: read</summary>
	public const int automationscriptresponse = 200;
	/// <summary>PID: 300 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int interappdebug_300 = 300;
	/// <summary>PID: 300 | Type: read</summary>
	public const int interappdebug = 300;
	/// <summary>PID: 9000000 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int clp_interapp_receive_9000000 = 9000000;
	/// <summary>PID: 9000000 | Type: read</summary>
	public const int clp_interapp_receive = 9000000;
	/// <summary>PID: 9000001 | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public const int clp_interapp_response_9000001 = 9000001;
	/// <summary>PID: 9000001 | Type: read</summary>
	public const int clp_interapp_response = 9000001;
	public class Write
	{
		/// <summary>PID: 11 | Type: write</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public const int devicedelay_11 = 11;
		/// <summary>PID: 11 | Type: write</summary>
		public const int devicedelay = 11;
		/// <summary>PID: 301 | Type: write</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public const int interappdebug_301 = 301;
		/// <summary>PID: 301 | Type: write</summary>
		public const int interappdebug = 301;
	}
}
public class WriteParameters
{
	/// <summary>PID: 11  | Type: write | DISCREETS: Instant = 0, 5 Seconds = 5</summary>
	public System.Object Devicedelay {get { return Protocol.GetParameter(11); }set { Protocol.SetParameter(11, value); }}
	/// <summary>PID: 301  | Type: write | DISCREETS: Disabled = 0, Enabled = 1</summary>
	public System.Object Interappdebug {get { return Protocol.GetParameter(301); }set { Protocol.SetParameter(301, value); }}
	public SLProtocolExt Protocol;
	public WriteParameters(SLProtocolExt protocol)
	{
		Protocol = protocol;
	}
}
public interface SLProtocolExt : SLProtocol
{
	object Devicedelay_10 { get; set; }
	object Devicedelay { get; set; }
	object Devicedelay_11 { get; set; }
	object Datacreatelineup_101 { get; set; }
	object Datacreatelineup { get; set; }
	object Simulatedevicecommunication_102 { get; set; }
	object Simulatedevicecommunication { get; set; }
	object Currentactivemessage_103 { get; set; }
	object Currentactivemessage { get; set; }
	object Internaldevicecommunicationbuffer_104 { get; set; }
	object Internaldevicecommunicationbuffer { get; set; }
	object Triggernextinbuffer_dummy { get; set; }
	object Automationscriptresponse_200 { get; set; }
	object Automationscriptresponse { get; set; }
	object Interappdebug_300 { get; set; }
	object Interappdebug { get; set; }
	object Interappdebug_301 { get; set; }
	object Clp_interapp_receive_9000000 { get; set; }
	object Clp_interapp_receive { get; set; }
	object Clp_interapp_response_9000001 { get; set; }
	object Clp_interapp_response { get; set; }
	WriteParameters Write { get; set; }
}
public class ConcreteSLProtocolExt : ConcreteSLProtocol, SLProtocolExt
{
	/// <summary>PID: 10  | Type: read | DISCREETS: Instant = 0, 5 Seconds = 5</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Devicedelay_10 {get { return GetParameter(10); }set { SetParameter(10, value); }}
	/// <summary>PID: 10  | Type: read | DISCREETS: Instant = 0, 5 Seconds = 5</summary>
	public System.Object Devicedelay {get { return GetParameter(10); }set { SetParameter(10, value); }}
	/// <summary>PID: 11  | Type: write | DISCREETS: Instant = 0, 5 Seconds = 5</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Devicedelay_11 {get { return GetParameter(11); }set { SetParameter(11, value); }}
	/// <summary>PID: 101  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Datacreatelineup_101 {get { return GetParameter(101); }set { SetParameter(101, value); }}
	/// <summary>PID: 101  | Type: read</summary>
	public System.Object Datacreatelineup {get { return GetParameter(101); }set { SetParameter(101, value); }}
	/// <summary>PID: 102  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Simulatedevicecommunication_102 {get { return GetParameter(102); }set { SetParameter(102, value); }}
	/// <summary>PID: 102  | Type: read</summary>
	public System.Object Simulatedevicecommunication {get { return GetParameter(102); }set { SetParameter(102, value); }}
	/// <summary>PID: 103  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Currentactivemessage_103 {get { return GetParameter(103); }set { SetParameter(103, value); }}
	/// <summary>PID: 103  | Type: read</summary>
	public System.Object Currentactivemessage {get { return GetParameter(103); }set { SetParameter(103, value); }}
	/// <summary>PID: 104  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Internaldevicecommunicationbuffer_104 {get { return GetParameter(104); }set { SetParameter(104, value); }}
	/// <summary>PID: 104  | Type: read</summary>
	public System.Object Internaldevicecommunicationbuffer {get { return GetParameter(104); }set { SetParameter(104, value); }}
	/// <summary>PID: 105  | Type: dummy</summary>
	public System.Object Triggernextinbuffer_dummy {get { return GetParameter(105); }set { SetParameter(105, value); }}
	/// <summary>PID: 200  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Automationscriptresponse_200 {get { return GetParameter(200); }set { SetParameter(200, value); }}
	/// <summary>PID: 200  | Type: read</summary>
	public System.Object Automationscriptresponse {get { return GetParameter(200); }set { SetParameter(200, value); }}
	/// <summary>PID: 300  | Type: read | DISCREETS: Disabled = 0, Enabled = 1</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Interappdebug_300 {get { return GetParameter(300); }set { SetParameter(300, value); }}
	/// <summary>PID: 300  | Type: read | DISCREETS: Disabled = 0, Enabled = 1</summary>
	public System.Object Interappdebug {get { return GetParameter(300); }set { SetParameter(300, value); }}
	/// <summary>PID: 301  | Type: write | DISCREETS: Disabled = 0, Enabled = 1</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Interappdebug_301 {get { return GetParameter(301); }set { SetParameter(301, value); }}
	/// <summary>PID: 9000000  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Clp_interapp_receive_9000000 {get { return GetParameter(9000000); }set { SetParameter(9000000, value); }}
	/// <summary>PID: 9000000  | Type: read</summary>
	public System.Object Clp_interapp_receive {get { return GetParameter(9000000); }set { SetParameter(9000000, value); }}
	/// <summary>PID: 9000001  | Type: read</summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public System.Object Clp_interapp_response_9000001 {get { return GetParameter(9000001); }set { SetParameter(9000001, value); }}
	/// <summary>PID: 9000001  | Type: read</summary>
	public System.Object Clp_interapp_response {get { return GetParameter(9000001); }set { SetParameter(9000001, value); }}
	public WriteParameters Write { get; set; }
	public ConcreteSLProtocolExt()
	{
		Write = new WriteParameters(this);
	}
}
}