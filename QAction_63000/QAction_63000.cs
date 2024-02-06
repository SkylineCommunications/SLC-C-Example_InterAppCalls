// --- auto-generated code --- do not modify ---

/*
{{StartPackageInfo}}
<PackageInfo xmlns="http://www.skyline.be/ClassLibrary">
	<BasePackage>
		<Identity>
			<Name>Class Library</Name>
			<Version>1.2.0.2</Version>
		</Identity>
	</BasePackage>
	<CustomPackages />
</PackageInfo>
{{EndPackageInfo}}
*/

namespace Skyline.DataMiner.Library.Common
{
    namespace Attributes
    {
        /// <summary>
        /// This attribute indicates a DLL is required.
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
        public sealed class DllImportAttribute : System.Attribute
        {
            /// <summary>
            /// Initializes a new instance of the <see cref = "DllImportAttribute"/> class.
            /// </summary>
            /// <param name = "dllImport">The name of the DLL to be imported.</param>
            public DllImportAttribute(string dllImport)
            {
                DllImport = dllImport;
            }

            /// <summary>
            /// Gets the name of the DLL to be imported.
            /// </summary>
            public string DllImport
            {
                get;
                private set;
            }
        }
    }

    /// <summary>
    /// Represents a DataMiner Agent.
    /// </summary>
    internal class Dma : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDma
    {
        /// <summary>
        /// The object used for DMS communication.
        /// </summary>
        private new readonly Skyline.DataMiner.Library.Common.IDms dms;
        /// <summary>
        /// The DataMiner Agent ID.
        /// </summary>
        private readonly int id;
        private string versionInfo;
        private string hostName;
        private string name;
        /// <summary>
        /// Initializes a new instance of the <see cref = "Dma"/> class.
        /// </summary>
        /// <param name = "dms">The DataMiner System.</param>
        /// <param name = "id">The ID of the DataMiner Agent.</param>
        /// <exception cref = "ArgumentNullException">The <see cref = "IDms"/> reference is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException">The DataMiner Agent ID is negative.</exception>
        internal Dma(Skyline.DataMiner.Library.Common.IDms dms, int id): base(dms)
        {
            if (id < 1)
            {
                throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID: {0}", id), "id");
            }

            this.dms = dms;
            this.id = id;
        }

        internal Dma(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage infoMessage): base(dms)
        {
            if (infoMessage == null)
            {
                throw new System.ArgumentNullException("infoMessage");
            }

            Parse(infoMessage);
        }

        /// <summary>
        /// Gets the ID of this DataMiner Agent.
        /// </summary>
        /// <value>The ID of this DataMiner Agent.</value>
        public int Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "DataMiner agent ID: {0}", id);
        }

        internal override void Load()
        {
            try
            {
                Skyline.DataMiner.Net.Messages.GetDataMinerByIDMessage message = new Skyline.DataMiner.Net.Messages.GetDataMinerByIDMessage(id);
                Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage infoResponseMessage = Dms.Communication.SendSingleResponseMessage(message) as Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage;
                if (infoResponseMessage != null)
                {
                    Parse(infoResponseMessage);
                }
                else
                {
                    throw new Skyline.DataMiner.Library.Common.AgentNotFoundException(id);
                }

                Skyline.DataMiner.Net.Messages.GetAgentBuildInfo buildInfoMessage = new Skyline.DataMiner.Net.Messages.GetAgentBuildInfo(id);
                Skyline.DataMiner.Net.Messages.BuildInfoResponse buildInfoResponse = (Skyline.DataMiner.Net.Messages.BuildInfoResponse)Dms.Communication.SendSingleResponseMessage(buildInfoMessage);
                if (buildInfoResponse != null)
                {
                    Parse(buildInfoResponse);
                }

                Skyline.DataMiner.Net.Messages.RSAPublicKeyRequest rsapkr;
                rsapkr = new Skyline.DataMiner.Net.Messages.RSAPublicKeyRequest(id)
                {HostingDataMinerID = id};
                Skyline.DataMiner.Net.Messages.RSAPublicKeyResponse resp = Dms.Communication.SendSingleResponseMessage(rsapkr) as Skyline.DataMiner.Net.Messages.RSAPublicKeyResponse;
                Skyline.DataMiner.Library.Common.RSA.PublicKey = new System.Security.Cryptography.RSAParameters{Modulus = resp.Modulus, Exponent = resp.Exponent};
                IsLoaded = true;
            }
            catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
            {
                if (e.ErrorCode == -2146233088)
                {
                    // 0x80131500, No agent available with ID.
                    throw new Skyline.DataMiner.Library.Common.AgentNotFoundException(id);
                }
                else
                {
                    throw;
                }
            }
        }

        private void Parse(Skyline.DataMiner.Net.Messages.GetDataMinerInfoResponseMessage infoMessage)
        {
            name = infoMessage.AgentName;
            hostName = infoMessage.ComputerName;
        }

        /// <summary>
        /// Parses the version information of the DataMiner Agent.
        /// </summary>
        /// <param name = "response"></param>
        private void Parse(Skyline.DataMiner.Net.Messages.BuildInfoResponse response)
        {
            if (response == null || response.Agents == null || response.Agents.Length == 0)
            {
                throw new System.ArgumentException("Agent build information cannot be null or empty");
            }

            string rawVersion = response.Agents[0].RawVersion;
            this.versionInfo = rawVersion;
        }
    }

    /// <summary>
    /// DataMiner Agent interface.
    /// </summary>
    public interface IDma
    {
        /// <summary>
        /// Gets the DataMiner System this Agent is part of.
        /// </summary>
        /// <value>The DataMiner system this Agent is part of.</value>
        Skyline.DataMiner.Library.Common.IDms Dms
        {
            get;
        }

        /// <summary>
        /// Gets the ID of this DataMiner Agent.
        /// </summary>
        /// <value>The ID of this DataMiner Agent.</value>
        int Id
        {
            get;
        }
    }

    /// <summary>
    /// Represents a communication interface implementation using the <see cref = "IConnection"/> interface.
    /// </summary>
    internal class ConnectionCommunication : Skyline.DataMiner.Library.Common.ICommunication
    {
        /// <summary>
        /// The SLNet connection.
        /// </summary>
        private readonly Skyline.DataMiner.Net.IConnection connection;
        /// <summary>
        /// Initializes a new instance of the <see cref = "ConnectionCommunication"/> class using an instance of the <see cref = "IConnection"/> class.
        /// </summary>
        /// <param name = "connection">The connection.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "connection"/> is <see langword = "null"/>.</exception>
        public ConnectionCommunication(Skyline.DataMiner.Net.IConnection connection)
        {
            if (connection == null)
            {
                throw new System.ArgumentNullException("connection");
            }

            this.connection = connection;
        }

        /// <summary>
        /// Sends a message to the SLNet process.
        /// </summary>
        /// <param name = "message">The message to be sent.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "message"/> is <see langword = "null"/>.</exception>
        /// <returns>The message responses.</returns>
        public Skyline.DataMiner.Net.Messages.DMSMessage[] SendMessage(Skyline.DataMiner.Net.Messages.DMSMessage message)
        {
            if (message == null)
            {
                throw new System.ArgumentNullException("message");
            }

            return connection.HandleMessage(message);
        }

        /// <summary>
        /// Sends a message to the SLNet process.
        /// </summary>
        /// <param name = "message">The message to be sent.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "message"/> is <see langword = "null"/>.</exception>
        /// <returns>The message response.</returns>
        public Skyline.DataMiner.Net.Messages.DMSMessage SendSingleResponseMessage(Skyline.DataMiner.Net.Messages.DMSMessage message)
        {
            if (message == null)
            {
                throw new System.ArgumentNullException("message");
            }

            return connection.HandleSingleResponseMessage(message);
        }
    }

    /// <summary>
    /// Defines methods to send messages to a DataMiner System.
    /// </summary>
    public interface ICommunication
    {
        /// <summary>
        /// Sends a message to the SLNet process that can have multiple responses.
        /// </summary>
        /// <param name = "message">The message to be sent.</param>
        /// <exception cref = "ArgumentNullException">The message cannot be null.</exception>
        /// <returns>The message responses.</returns>
        Skyline.DataMiner.Net.Messages.DMSMessage[] SendMessage(Skyline.DataMiner.Net.Messages.DMSMessage message);
        /// <summary>
        /// Sends a message to the SLNet process that returns a single response.
        /// </summary>
        /// <param name = "message">The message to be sent.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "message"/> is <see langword = "null"/>.</exception>
        /// <returns>The message response.</returns>
        Skyline.DataMiner.Net.Messages.DMSMessage SendSingleResponseMessage(Skyline.DataMiner.Net.Messages.DMSMessage message);
    }

    /// <summary>
    /// A collection of IElementConnection objects.
    /// </summary>
    public class ElementConnectionCollection
    {
        private readonly Skyline.DataMiner.Library.Common.IElementConnection[] connections;
        private readonly bool canBeValidated;
        private readonly System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> protocolConnectionInfo;
        /// <summary>
        /// initiates a new instance.
        /// </summary>
        /// <param name = "protocolConnectionInfo"></param>
        internal ElementConnectionCollection(System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> protocolConnectionInfo)
        {
            int amountOfConnections = protocolConnectionInfo.Count;
            this.connections = new Skyline.DataMiner.Library.Common.IElementConnection[amountOfConnections];
            this.protocolConnectionInfo = protocolConnectionInfo;
            canBeValidated = true;
        }

        /// <summary>
        /// Initiates a new instance.
        /// </summary>
        /// <param name = "message"></param>
        internal ElementConnectionCollection(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage message)
        {
            int amountOfConnections = 1;
            if (message != null && message.ExtraPorts != null)
            {
                amountOfConnections += message.ExtraPorts.Length;
            }

            this.connections = new Skyline.DataMiner.Library.Common.IElementConnection[amountOfConnections];
            canBeValidated = false;
        }

        /// <summary>
        /// Gets or sets an entry in the collection.
        /// </summary>
        /// <param name = "index"></param>
        /// <returns></returns>
        public IElementConnection this[int index]
        {
            get
            {
                return connections[index];
            }

            set
            {
                bool valid = ValidateConnectionTypeAtPos(index, value);
                if (valid)
                {
                    connections[index] = value;
                }
                else
                {
                    throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Invalid connection type provided at index " + index);
                }
            }
        }

        /// <summary>
        /// Validates the provided <see cref = "IElementConnection"/> against the provided Protocol.
        /// </summary>
        /// <param name = "index">The index position of the connection to validate.</param>
        /// <param name = "conn">The IElementConnection connection.</param>
        /// <exception cref = "ArgumentOutOfRangeException"><paramref name = "index"/> is out of range.</exception>
        /// <returns></returns>
        private bool ValidateConnectionTypeAtPos(int index, Skyline.DataMiner.Library.Common.IElementConnection conn)
        {
            if (!canBeValidated)
            {
                return true;
            }

            if (index < 0 || ((index + 1) > protocolConnectionInfo.Count))
            {
                throw new System.ArgumentOutOfRangeException("index", "Index needs to be between 0 and the amount of connections in the protocol minus 1.");
            }

            return ValidateConnectionInfo(conn, protocolConnectionInfo[index]);
        }

        /// <summary>
        /// Validates a single connection.
        /// </summary>
        /// <param name = "conn"><see cref = "IElementConnection"/> object.</param>
        /// <param name = "connectionInfo"><see cref = "IDmsConnectionInfo"/> object.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "conn"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentNullException"><paramref name = "connectionInfo"/> is <see langword = "null"/>.</exception>
        /// <returns></returns>
        private static bool ValidateConnectionInfo(Skyline.DataMiner.Library.Common.IElementConnection conn, Skyline.DataMiner.Library.Common.IDmsConnectionInfo connectionInfo)
        {
            if (conn == null)
            {
                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("conn: Invalid data , ElementConfiguration does not contain connection info");
            }

            if (connectionInfo == null)
            {
                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("connectionInfo: Invalid data , Protocol does not contain connection info");
            }

            switch (connectionInfo.Type)
            {
                case Skyline.DataMiner.Library.Common.ConnectionType.SnmpV1:
                    return ValidateAsSnmpV1(conn);
                case Skyline.DataMiner.Library.Common.ConnectionType.SnmpV2:
                    return ValidateAsSnmpV2(conn);
                case Skyline.DataMiner.Library.Common.ConnectionType.SnmpV3:
                    return ValidateAsSnmpV3(conn);
                case Skyline.DataMiner.Library.Common.ConnectionType.Virtual:
                    return conn is Skyline.DataMiner.Library.Common.IVirtualConnection;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Validate a connection for SNMPv1
        /// </summary>
        /// <param name = "conn">object of type <see cref = "IElementConnection"/> to validate.</param>
        /// <returns></returns>
        private static bool ValidateAsSnmpV1(Skyline.DataMiner.Library.Common.IElementConnection conn)
        {
            return conn is Skyline.DataMiner.Library.Common.ISnmpV1Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV2Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV3Connection;
        }

        /// <summary>
        /// Validate a connection for SNMPv2
        /// </summary>
        /// <param name = "conn">object of type <see cref = "IElementConnection"/> to validate.</param>
        /// <returns></returns>
        private static bool ValidateAsSnmpV2(Skyline.DataMiner.Library.Common.IElementConnection conn)
        {
            return conn is Skyline.DataMiner.Library.Common.ISnmpV2Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV3Connection;
        }

        /// <summary>
        /// Validate a connection for SNMPv3
        /// </summary>
        /// <param name = "conn">object of type <see cref = "IElementConnection"/> to validate.</param>
        /// <returns></returns>
        private static bool ValidateAsSnmpV3(Skyline.DataMiner.Library.Common.IElementConnection conn)
        {
            return conn is Skyline.DataMiner.Library.Common.ISnmpV3Connection || conn is Skyline.DataMiner.Library.Common.ISnmpV2Connection;
        }
    }

    /// <summary>
    /// Represents information about a connection.
    /// </summary>
    internal class DmsConnectionInfo : Skyline.DataMiner.Library.Common.IDmsConnectionInfo
    {
        /// <summary>
        /// The name of the connection.
        /// </summary>
        private readonly string name;
        /// <summary>
        /// The connection type.
        /// </summary>
        private readonly Skyline.DataMiner.Library.Common.ConnectionType type;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsConnectionInfo"/> class.
        /// </summary>
        /// <param name = "name">The connection name.</param>
        /// <param name = "type">The connection type.</param>
        internal DmsConnectionInfo(string name, Skyline.DataMiner.Library.Common.ConnectionType type)
        {
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// Gets the connection type.
        /// </summary>
        /// <value>The connection type.</value>
        public Skyline.DataMiner.Library.Common.ConnectionType Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Connection with Name:{0} and Type:{1}.", name, type);
        }
    }

    /// <summary>
    /// DataMiner element connection information interface.
    /// </summary>
    public interface IDmsConnectionInfo
    {
        /// <summary>
        /// Gets the connection type.
        /// </summary>
        /// <value>The connection type.</value>
        Skyline.DataMiner.Library.Common.ConnectionType Type
        {
            get;
        }
    }

    /// <summary>
    /// Represents a system-wide element ID.
    /// </summary>
    /// <remarks>This is a combination of a DataMiner Agent ID (the ID of the Agent on which the element was created) and an element ID.</remarks>
    [System.Serializable]
    public struct DmsElementId : System.IEquatable<Skyline.DataMiner.Library.Common.DmsElementId>, System.IComparable, System.IComparable<Skyline.DataMiner.Library.Common.DmsElementId>
    {
        /// <summary>
        /// The DataMiner Agent ID.
        /// </summary>
        private readonly int agentId;
        /// <summary>
        /// The element ID.
        /// </summary>
        private readonly int elementId;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsElementId"/> structure using the specified string.
        /// </summary>
        /// <param name = "id">String representing the system-wide element ID.</param>
        /// <remarks>The provided string must be formatted as follows: "DataMiner Agent ID/element ID (e.g. 400/201)".</remarks>
        /// <exception cref = "ArgumentNullException"><paramref name = "id"/> is <see langword = "null"/> .</exception>
        /// <exception cref = "ArgumentException"><paramref name = "id"/> is the empty string ("") or white space.</exception>
        /// <exception cref = "ArgumentException">The ID does not match the mandatory format.</exception>
        /// <exception cref = "ArgumentException">The DataMiner Agent ID is not an integer.</exception>
        /// <exception cref = "ArgumentException">The element ID is not an integer.</exception>
        /// <exception cref = "ArgumentException">Invalid DataMiner Agent ID.</exception>
        /// <exception cref = "ArgumentException">Invalid element ID.</exception>
        public DmsElementId(string id)
        {
            if (id == null)
            {
                throw new System.ArgumentNullException("id");
            }

            if (System.String.IsNullOrWhiteSpace(id))
            {
                throw new System.ArgumentException("The provided ID must not be empty.", "id");
            }

            string[] idParts = id.Split('/');
            if (idParts.Length != 2)
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid ID. Value: {0}. The string must be formatted as follows: \"agent ID/element ID\".", id);
                throw new System.ArgumentException(message, "id");
            }

            if (!System.Int32.TryParse(idParts[0], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out agentId))
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID. \"{0}\" is not an integer value", id);
                throw new System.ArgumentException(message, "id");
            }

            if (!System.Int32.TryParse(idParts[1], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out elementId))
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid Element ID. \"{0}\" is not an integer value", id);
                throw new System.ArgumentException(message, "id");
            }

            if (!IsValidAgentId())
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid agent ID. Value: {0}.", agentId);
                throw new System.ArgumentException(message, "id");
            }

            if (!IsValidElementId())
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid element ID. Value: {0}.", elementId);
                throw new System.ArgumentException(message, "id");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsElementId"/> structure using the specified element ID and DataMiner Agent ID.
        /// </summary>
        /// <param name = "agentId">The DataMiner Agent ID.</param>
        /// <param name = "elementId">The element ID.</param>
        /// <remarks>The hosting DataMiner Agent ID value will be set to the same value as the specified DataMiner Agent ID.</remarks>
        /// <exception cref = "ArgumentException"><paramref name = "agentId"/> is invalid.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "elementId"/> is invalid.</exception>
        public DmsElementId(int agentId, int elementId)
        {
            if ((elementId == -1 && agentId != -1) || agentId < -1)
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid agent ID. Value: {0}.", agentId);
                throw new System.ArgumentException(message, "agentId");
            }

            if ((agentId == -1 && elementId != -1) || elementId < -1)
            {
                string message = System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid element ID. Value: {0}.", elementId);
                throw new System.ArgumentException(message, "elementId");
            }

            this.elementId = elementId;
            this.agentId = agentId;
        }

        /// <summary>
        /// Gets the DataMiner Agent ID.
        /// </summary>
        /// <remarks>The DataMiner Agent ID is the ID of the DataMiner Agent this element has been created on.</remarks>
        public int AgentId
        {
            get
            {
                return agentId;
            }
        }

        /// <summary>
        /// Gets the element ID.
        /// </summary>
        public int ElementId
        {
            get
            {
                return elementId;
            }
        }

        /// <summary>
        /// Gets the DataMiner Agent ID/element ID string representation.
        /// </summary>
        public string Value
        {
            get
            {
                return agentId + "/" + elementId;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the
        /// current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name = "other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings: Less than zero means this instance precedes <paramref name = "other"/> in the sort order.
        /// Zero means this instance occurs in the same position in the sort order as <paramref name = "other"/>.
        /// Greater than zero means this instance follows <paramref name = "other"/> in the sort order.</returns>
        /// <remarks>The order of the comparison is as follows: DataMiner Agent ID, element ID.</remarks>
        public int CompareTo(Skyline.DataMiner.Library.Common.DmsElementId other)
        {
            int result = agentId.CompareTo(other.AgentId);
            if (result == 0)
            {
                result = elementId.CompareTo(other.ElementId);
            }

            return result;
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name = "obj">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Less than zero means this instance precedes <paramref name = "obj"/> in the sort order. Zero means this instance occurs in the same position in the sort order as <paramref name = "obj"/>. Greater than zero means this instance follows <paramref name = "obj"/> in the sort order.</returns>
        /// <remarks>The order of the comparison is as follows: DataMiner Agent ID, element ID.</remarks>
        /// <exception cref = "ArgumentException">The obj is not of type <see cref = "DmsElementId"/></exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Skyline.DataMiner.Library.Common.DmsElementId))
            {
                throw new System.ArgumentException("The provided object must be of type DmsElementId.", "obj");
            }

            return CompareTo((Skyline.DataMiner.Library.Common.DmsElementId)obj);
        }

        /// <summary>
        /// Compares the object to another object.
        /// </summary>
        /// <param name = "obj">The object to compare against.</param>
        /// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Skyline.DataMiner.Library.Common.DmsElementId))
            {
                return false;
            }

            return Equals((Skyline.DataMiner.Library.Common.DmsElementId)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name = "other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
        public bool Equals(Skyline.DataMiner.Library.Common.DmsElementId other)
        {
            if (elementId == other.elementId && agentId == other.agentId)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return elementId ^ agentId;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "agent ID: {0}, element ID: {1}", agentId, elementId);
        }

        /// <summary>
        /// Returns a value determining whether the agent ID is valid.
        /// </summary>
        /// <returns><c>true</c> if the agent ID is valid; otherwise, <c>false</c>.</returns>
        private bool IsValidAgentId()
        {
            bool isValid = true;
            if ((elementId == -1 && agentId != -1) || agentId < -1)
            {
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Returns a value determining whether the element ID is valid.
        /// </summary>
        /// <returns><c>true</c> if the element ID is valid; otherwise, <c>false</c>.</returns>
        private bool IsValidElementId()
        {
            bool isValid = true;
            if ((agentId == -1 && elementId != -1) || elementId < -1)
            {
                isValid = false;
            }

            return isValid;
        }
    }

    /// <summary>
    /// Represents a DataMiner System.
    /// </summary>
    internal class Dms : Skyline.DataMiner.Library.Common.IDms
    {
        /// <summary>
        /// Cached element information message.
        /// </summary>
        private Skyline.DataMiner.Net.Messages.ElementInfoEventMessage cachedElementInfoMessage;
        /// <summary>
        /// The object used for DMS communication.
        /// </summary>
        private Skyline.DataMiner.Library.Common.ICommunication communication;
        /// <summary>
        /// Initializes a new instance of the <see cref = "Dms"/> class.
        /// </summary>
        /// <param name = "communication">An object implementing the ICommunication interface.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "communication"/> is <see langword = "null"/>.</exception>
        internal Dms(Skyline.DataMiner.Library.Common.ICommunication communication)
        {
            if (communication == null)
            {
                throw new System.ArgumentNullException("communication");
            }

            this.communication = communication;
        }

        /// <summary>
        /// Gets the communication interface.
        /// </summary>
        /// <value>The communication interface.</value>
        public Skyline.DataMiner.Library.Common.ICommunication Communication
        {
            get
            {
                return communication;
            }
        }

        /// <summary>
        /// Determines whether an element with the specified Agent ID/element ID exists in the DataMiner System.
        /// </summary>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
        /// <returns><c>true</c> if the element exists; otherwise, <c>false</c>.</returns>
        /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is invalid.</exception>
        public bool ElementExists(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId)
        {
            int dmaId = dmsElementId.AgentId;
            int elementId = dmsElementId.ElementId;
            if (dmaId < 1)
            {
                throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner agent ID: {0}", dmaId), "dmsElementId");
            }

            if (elementId < 1)
            {
                throw new System.ArgumentException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid DataMiner element ID: {0}", elementId), "dmsElementId");
            }

            try
            {
                Skyline.DataMiner.Net.Messages.GetElementByIDMessage message = new Skyline.DataMiner.Net.Messages.GetElementByIDMessage(dmaId, elementId);
                Skyline.DataMiner.Net.Messages.ElementInfoEventMessage response = (Skyline.DataMiner.Net.Messages.ElementInfoEventMessage)Communication.SendSingleResponseMessage(message);
                // Cache the response of SLNet.
                // Could be useful when this call is used within a "GetElement" this makes sure that we do not double call SLNet.
                if (response != null)
                {
                    cachedElementInfoMessage = response;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
            {
                if (e.ErrorCode == -2146233088)
                {
                    // 0x80131500, Element "[element name]" is unavailable.
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Retrieves the element with the specified ID.
        /// </summary>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
        /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is invalid.</exception>
        /// <exception cref = "ElementNotFoundException">The element with the specified ID was not found in the DataMiner System.</exception>
        /// <returns>The element with the specified ID.</returns>
        public Skyline.DataMiner.Library.Common.IDmsElement GetElement(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId)
        {
            if (!ElementExists(dmsElementId))
            {
                throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(dmsElementId);
            }

            return new Skyline.DataMiner.Library.Common.DmsElement(this, cachedElementInfoMessage);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return "DataMiner System";
        }
    }

    /// <summary>
    /// Helper class to convert from enumeration value to string or vice versa.
    /// </summary>
    internal static class EnumMapper
    {
        /// <summary>
        /// The connection type map.
        /// </summary>
        private static readonly System.Collections.Generic.Dictionary<string, Skyline.DataMiner.Library.Common.ConnectionType> ConnectionTypeMapping = new System.Collections.Generic.Dictionary<string, Skyline.DataMiner.Library.Common.ConnectionType>{{"SNMP", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV1}, {"SNMPV1", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV1}, {"SNMPV2", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV2}, {"SNMPV3", Skyline.DataMiner.Library.Common.ConnectionType.SnmpV3}, {"SERIAL", Skyline.DataMiner.Library.Common.ConnectionType.Serial}, {"SERIAL SINGLE", Skyline.DataMiner.Library.Common.ConnectionType.SerialSingle}, {"SMART-SERIAL", Skyline.DataMiner.Library.Common.ConnectionType.SmartSerial}, {"SMART-SERIAL SINGLE", Skyline.DataMiner.Library.Common.ConnectionType.SmartSerialSingle}, {"HTTP", Skyline.DataMiner.Library.Common.ConnectionType.Http}, {"GPIB", Skyline.DataMiner.Library.Common.ConnectionType.Gpib}, {"VIRTUAL", Skyline.DataMiner.Library.Common.ConnectionType.Virtual}, {"OPC", Skyline.DataMiner.Library.Common.ConnectionType.Opc}, {"SLA", Skyline.DataMiner.Library.Common.ConnectionType.Sla}, {"WEBSOCKET", Skyline.DataMiner.Library.Common.ConnectionType.WebSocket}};
        /// <summary>
        /// Converts a string denoting a connection type to the corresponding value of the <see cref = "ConnectionType"/> enumeration.
        /// </summary>
        /// <param name = "type">The connection type.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "type"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "type"/> is the empty string ("") or white space</exception>
        /// <exception cref = "KeyNotFoundException"></exception>
        /// <returns>The corresponding <see cref = "ConnectionType"/> value.</returns>
        internal static Skyline.DataMiner.Library.Common.ConnectionType ConvertStringToConnectionType(string type)
        {
            if (type == null)
            {
                throw new System.ArgumentNullException("type");
            }

            if (System.String.IsNullOrWhiteSpace(type))
            {
                throw new System.ArgumentException("The type must not be empty.", "type");
            }

            string valueLower = type.ToUpperInvariant();
            Skyline.DataMiner.Library.Common.ConnectionType result;
            if (!ConnectionTypeMapping.TryGetValue(valueLower, out result))
            {
                throw new System.Collections.Generic.KeyNotFoundException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The key {0} could not be found.", valueLower));
            }

            return result;
        }
    }

    /// <summary>
    /// Specifies the connection type.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// Undefined connection type.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// SNMPv1 connection.
        /// </summary>
        SnmpV1 = 1,
        /// <summary>
        /// Serial connection.
        /// </summary>
        Serial = 2,
        /// <summary>
        /// Smart-serial connection.
        /// </summary>
        SmartSerial = 3,
        /// <summary>
        /// Virtual connection.
        /// </summary>
        Virtual = 4,
        /// <summary>
        /// GBIP (General Purpose Interface Bus) connection.
        /// </summary>
        Gpib = 5,
        /// <summary>
        /// OPC (OLE for Process Control) connection.
        /// </summary>
        Opc = 6,
        /// <summary>
        /// SLA (Service Level Agreement).
        /// </summary>
        Sla = 7,
        /// <summary>
        /// SNMPv2 connection.
        /// </summary>
        SnmpV2 = 8,
        /// <summary>
        /// SNMPv3 connection.
        /// </summary>
        SnmpV3 = 9,
        /// <summary>
        /// HTTP connection.
        /// </summary>
        Http = 10,
        /// <summary>
        /// Service.
        /// </summary>
        Service = 11,
        /// <summary>
        /// Serial single connection.
        /// </summary>
        SerialSingle = 12,
        /// <summary>
        /// Smart-serial single connection.
        /// </summary>
        SmartSerialSingle = 13,
        /// <summary>
        /// Web Socket connection.
        /// </summary>
        WebSocket = 14
    }

    /// <summary>
    /// Specifies the state of the element.
    /// </summary>
    public enum ElementState
    {
        /// <summary>
        /// Specifies the undefined element state.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Specifies the active element state.
        /// </summary>
        Active = 1,
        /// <summary>
        /// Specifies the hidden element state.
        /// </summary>
        Hidden = 2,
        /// <summary>
        /// Specifies the paused element state.
        /// </summary>
        Paused = 3,
        /// <summary>
        /// Specifies the stopped element state.
        /// </summary>
        Stopped = 4,
        /// <summary>
        /// Specifies the deleted element state.
        /// </summary>
        Deleted = 6,
        /// <summary>
        /// Specifies the error element state.
        /// </summary>
        Error = 10,
        /// <summary>
        /// Specifies the restart element state.
        /// </summary>
        Restart = 11,
        /// <summary>
        /// Specifies the masked element state.
        /// </summary>
        Masked = 12
    }

    /// <summary>
    /// The exception that is thrown when an action is performed on a DataMiner Agent that was not found.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class AgentNotFoundException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class.
        /// </summary>
        public AgentNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with a specified DataMiner Agent ID.
        /// </summary>
        /// <param name = "id">The ID of the DataMiner Agent that was not found.</param>
        public AgentNotFoundException(int id): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The agent with ID '{0}' was not found.", id))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public AgentNotFoundException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AgentNotFoundException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AgentNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected AgentNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when a requested alarm template was not found.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class AlarmTemplateNotFoundException : Skyline.DataMiner.Library.Common.TemplateNotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
        /// </summary>
        public AlarmTemplateNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public AlarmTemplateNotFoundException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AlarmTemplateNotFoundException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "templateName">The name of the template.</param>
        /// <param name = "protocol">The protocol this template relates to.</param>
        public AlarmTemplateNotFoundException(string templateName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(templateName, protocol)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "templateName">The name of the template.</param>
        /// <param name = "protocolName">The name of the protocol.</param>
        /// <param name = "protocolVersion">The version of the protocol.</param>
        public AlarmTemplateNotFoundException(string templateName, string protocolName, string protocolVersion): base(templateName, protocolName, protocolVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "AlarmTemplateNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected AlarmTemplateNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when an exception occurs in a DataMiner System.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class DmsException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsException"/> class.
        /// </summary>
        public DmsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public DmsException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DmsException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected DmsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when performing actions on an element that was not found.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class ElementNotFoundException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
        /// </summary>
        public ElementNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
        /// </summary>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element that was not found.</param>
        public ElementNotFoundException(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element with DMA ID '{0}' and element ID '{1}' was not found.", dmsElementId.AgentId, dmsElementId.ElementId))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
        /// </summary>
        /// <param name = "dmaId">The ID of the DataMiner Agent that was not found.</param>
        /// <param name = "elementId">The ID of the element that was not found.</param>
        public ElementNotFoundException(int dmaId, int elementId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element with DMA ID '{0}' and element ID '{1}' was not found.", dmaId, elementId))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public ElementNotFoundException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ElementNotFoundException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element that was not found.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ElementNotFoundException(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Element with DMA ID '{0}' and element ID '{1}' was not found.", dmsElementId.AgentId, dmsElementId.ElementId), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected ElementNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when an operation is performed on a stopped element.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class ElementStoppedException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class.
        /// </summary>
        public ElementStoppedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public ElementStoppedException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "dmsElementId">The ID of the element.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ElementStoppedException(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The element with ID '{0}' is stopped.", dmsElementId.Value), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ElementStoppedException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementStoppedException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected ElementStoppedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when invalid data was provided.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class IncorrectDataException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class.
        /// </summary>
        public IncorrectDataException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public IncorrectDataException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public IncorrectDataException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "IncorrectDataException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected IncorrectDataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when an action is performed on a DataMiner element parameter that was not found.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class ParameterNotFoundException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ParameterNotFoundException"/> class.
        /// </summary>
        public ParameterNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ParameterNotFoundException"/> class with a specified DataMiner element parameter ID.
        /// </summary>
        /// <param name = "id">The ID of the DataMiner Agent that was not found.</param>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element the parameter belongs to.</param>
        public ParameterNotFoundException(int id, Skyline.DataMiner.Library.Common.DmsElementId dmsElementId): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The parameter with ID '{0}' was not found on the element with agent ID '{1}' and element ID '{2}'.", id, dmsElementId.AgentId, dmsElementId.ElementId))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ParameterNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public ParameterNotFoundException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ParameterNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ParameterNotFoundException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ParameterNotFoundException"/> class with a specified DataMiner element parameter ID.
        /// </summary>
        /// <param name = "id">The ID of the DataMiner agent that was not found.</param>
        /// <param name = "dmsElementId">The DataMiner agent ID/element ID of the element the parameter belongs to.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ParameterNotFoundException(int id, Skyline.DataMiner.Library.Common.DmsElementId dmsElementId, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "The parameter with ID '{0}' was not found on the element with agent ID '{1}' and element ID '{2}'.", id, dmsElementId.AgentId, dmsElementId.ElementId), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ParameterNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected ParameterNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when a requested protocol was not found.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class ProtocolNotFoundException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
        /// </summary>
        public ProtocolNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
        /// </summary>
        /// <param name = "protocolName">The name of the protocol.</param>
        /// <param name = "protocolVersion">The version of the protocol.</param>
        public ProtocolNotFoundException(string protocolName, string protocolVersion): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Protocol with name '{0}' and version '{1}' was not found.", protocolName, protocolVersion))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public ProtocolNotFoundException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class.
        /// </summary>
        /// <param name = "protocolName">The name of the protocol.</param>
        /// <param name = "protocolVersion">The version of the protocol.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ProtocolNotFoundException(string protocolName, string protocolVersion, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Protocol with name '{0}' and version '{1}' was not found.", protocolName, protocolVersion), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ProtocolNotFoundException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ProtocolNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected ProtocolNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when a requested template was not found.
    /// </summary>
    [System.Serializable]
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("System.Runtime.Serialization.dll")]
    public class TemplateNotFoundException : Skyline.DataMiner.Library.Common.DmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
        /// </summary>
        public TemplateNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "templateName">The name of the template.</param>
        /// <param name = "protocol">The protocol this template relates to.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
        public TemplateNotFoundException(string templateName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol): base(BuildMessageString(templateName, protocol))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "templateName">The name of the template.</param>
        /// <param name = "protocolName">The name of the protocol.</param>
        /// <param name = "protocolVersion">The version of the protocol.</param>
        public TemplateNotFoundException(string templateName, string protocolName, string protocolVersion): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template \"{0}\" for protocol \"{1}\" version \"{2}\" was not found.", templateName, protocolName, protocolVersion))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        public TemplateNotFoundException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class.
        /// </summary>
        /// <param name = "templateName">The name of the template.</param>
        /// <param name = "protocolName">The name of the protocol.</param>
        /// <param name = "protocolVersion">The version of the protocol.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TemplateNotFoundException(string templateName, string protocolName, string protocolVersion, System.Exception innerException): base(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template \"{0}\" for protocol \"{1}\" version \"{2}\" was not found.", templateName, protocolName, protocolVersion), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name = "message">The error message that explains the reason for the exception.</param>
        /// <param name = "innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public TemplateNotFoundException(string message, System.Exception innerException): base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "TemplateNotFoundException"/> class with serialized data.
        /// </summary>
        /// <param name = "info">The serialization info.</param>
        /// <param name = "context">The streaming context.</param>
        /// <exception cref = "ArgumentNullException">The <paramref name = "info"/> parameter is <see langword = "null"/>.</exception>
        /// <exception cref = "SerializationException">The class name is <see langword = "null"/> or HResult is zero (0).</exception>
        /// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
        protected TemplateNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context)
        {
        }

        private static string BuildMessageString(string templateName, Skyline.DataMiner.Library.Common.IDmsProtocol protocol)
        {
            if (protocol == null)
            {
                throw new System.ArgumentNullException("protocol");
            }

            return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template \"{0}\" for protocol \"{1}\" version \"{2}\" was not found.", templateName, protocol.Name, protocol.Version);
        }
    }

    /// <summary>
    /// Class containing helper methods.
    /// </summary>
    internal static class HelperClass
    {
        /// <summary>
        /// Checks the element state and throws an exception if no operation is possible due to the current element state.
        /// </summary>
        /// <param name = "element">The element on which to check the state.</param>
        /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner system.</exception>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        internal static void CheckElementState(Skyline.DataMiner.Library.Common.IDmsElement element)
        {
            if (element.State == Skyline.DataMiner.Library.Common.ElementState.Deleted)
            {
                throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Failed to perform an operation on the element: {0} because it has been deleted.", element.Name));
            }

            if (element.State == Skyline.DataMiner.Library.Common.ElementState.Stopped)
            {
                throw new Skyline.DataMiner.Library.Common.ElementStoppedException(System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Failed to perform an operation on the element: {0} because it has been stopped.", element.Name));
            }
        }

        /// <summary>
        /// Using the description attribute on an enum, we can easily find back a corresponding enum value.
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "description"></param>
        /// <returns></returns>
        internal static T GetEnumFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new System.InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = System.Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new System.ArgumentException(description + "Not found as Enum.");
        }
    }

    /// <summary>
    /// DataMiner System interface.
    /// </summary>
    public interface IDms
    {
        /// <summary>
        /// Gets the communication interface.
        /// </summary>
        /// <value>The communication interface.</value>
        Skyline.DataMiner.Library.Common.ICommunication Communication
        {
            get;
        }

        /// <summary>
        /// Determines whether an element with the specified DataMiner Agent ID/element ID exists in the DataMiner System.
        /// </summary>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
        /// <returns><c>true</c> if the element exists; otherwise, <c>false</c>.</returns>
        bool ElementExists(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId);
        /// <summary>
        /// Retrieves the element with the specified element ID.
        /// </summary>
        /// <param name = "dmsElementId">The DataMiner Agent ID/element ID of the element.</param>
        /// <exception cref = "ArgumentException"><paramref name = "dmsElementId"/> is empty.</exception>
        /// <exception cref = "ElementNotFoundException">No element with the specified ID exists in the DataMiner System.</exception>
        /// <returns>The element with the specified ID.</returns>
        Skyline.DataMiner.Library.Common.IDmsElement GetElement(Skyline.DataMiner.Library.Common.DmsElementId dmsElementId);
    }

    /// <summary>
    /// Contains methods for input validation.
    /// </summary>
    internal static class InputValidator
    {
        /// <summary>
        /// Validates the name of an element, service, redundancy group, template or folder.
        /// </summary>
        /// <param name = "name">The element name.</param>
        /// <param name = "parameterName">The name of the parameter that is passing the name.</param>
        /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
        /// <returns><c>true</c> if the name is valid; otherwise, <c>false</c>.</returns>
        /// <remarks>Forbidden characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</remarks>
        public static string ValidateName(string name, string parameterName)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (parameterName == null)
            {
                throw new System.ArgumentNullException("parameterName");
            }

            if (System.String.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("The name must not be null or white space.", parameterName);
            }

            string trimmedName = name.Trim();
            if (trimmedName.Length > 200)
            {
                throw new System.ArgumentException("The name must not exceed 200 characters.", parameterName);
            }

            // White space is trimmed.
            if (trimmedName[0].Equals('.'))
            {
                throw new System.ArgumentException("The name must not start with a dot ('.').", parameterName);
            }

            if (trimmedName[trimmedName.Length - 1].Equals('.'))
            {
                throw new System.ArgumentException("The name must not end with a dot ('.').", parameterName);
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(trimmedName, @"^[^/\\:;\*\?<>\|°""]+$"))
            {
                throw new System.ArgumentException("The name contains a forbidden character.", parameterName);
            }

            if (System.Linq.Enumerable.Count(trimmedName, x => x == '%') > 1)
            {
                throw new System.ArgumentException("The name must not contain more than one '%' characters.", parameterName);
            }

            return trimmedName;
        }
    }

    /// <summary>
    /// Updateable interface.
    /// </summary>
    public interface IUpdateable
    {
    }

    /// <summary>
    /// Represents the parent for every type of object that can be present on a DataMiner system.
    /// </summary>
    internal abstract class DmsObject
    {
        /// <summary>
        /// The DataMiner system the object belongs to.
        /// </summary>
        protected readonly Skyline.DataMiner.Library.Common.IDms dms;
        /// <summary>
        /// Flag stating whether the DataMiner system object has been loaded.
        /// </summary>
        private bool isLoaded;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsObject"/> class.
        /// </summary>
        /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
        protected DmsObject(Skyline.DataMiner.Library.Common.IDms dms)
        {
            if (dms == null)
            {
                throw new System.ArgumentNullException("dms");
            }

            this.dms = dms;
        }

        /// <summary>
        /// Gets the DataMiner system this object belongs to.
        /// </summary>
        public Skyline.DataMiner.Library.Common.IDms Dms
        {
            get
            {
                return dms;
            }
        }

        /// <summary>
        /// Gets the communication object.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.ICommunication Communication
        {
            get
            {
                return dms.Communication;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the DMS object has been loaded.
        /// </summary>
        internal bool IsLoaded
        {
            get
            {
                return isLoaded;
            }

            set
            {
                isLoaded = value;
            }
        }

        /// <summary>
        /// Loads DMS object data in case the object exists and is not already loaded.
        /// </summary>
        internal void LoadOnDemand()
        {
            if (!IsLoaded)
            {
                Load();
            }
        }

        /// <summary>
        /// Loads the object.
        /// </summary>
        internal abstract void Load();
    }

    /// <summary>
    /// Base class for all connection related objects.
    /// </summary>
    public abstract class ConnectionSettings
    {
        /// <summary>
        /// Enum used to track changes on properties of classes implementing this abstract class
        /// </summary>
        protected enum ConnectionSetting
        {
            /// <summary>
            /// GetCommunityString
            /// </summary>
            GetCommunityString = 0,
            /// <summary>
            /// SetCommunityString
            /// </summary>
            SetCommunityString = 1,
            /// <summary>
            /// DeviceAddress
            /// </summary>
            DeviceAddress = 2,
            /// <summary>
            /// Timeout
            /// </summary>
            Timeout = 3,
            /// <summary>
            /// Retries
            /// </summary>
            Retries = 4,
            /// <summary>
            /// ElementTimeout
            /// </summary>
            ElementTimeout = 5,
            /// <summary>
            /// PortConnection (e.g.Udp , Tcp)
            /// </summary>
            PortConnection = 6,
            /// <summary>
            /// SecurityConfiguration
            /// </summary>
            SecurityConfig = 7,
            /// <summary>
            /// SNMPv3 Encryption Algorithm
            /// </summary>
            EncryptionAlgorithm = 8,
            /// <summary>
            /// SNMPv3 AuthenticationProtocol
            /// </summary>
            AuthenticationProtocol = 9,
            /// <summary>
            /// SNMPv3 EncryptionKey
            /// </summary>
            EncryptionKey = 10,
            /// <summary>
            /// SNMPv3 AuthenticationKey
            /// </summary>
            AuthenticationKey = 11,
            /// <summary>
            /// SNMPv3 Username
            /// </summary>
            Username = 12,
            /// <summary>
            /// SNMPv3 Security Level and Protocol
            /// </summary>
            SecurityLevelAndProtocol = 13,
            /// <summary>
            /// Local port
            /// </summary>
            LocalPort = 14,
            /// <summary>
            /// Remote port
            /// </summary>
            RemotePort = 15,
            /// <summary>
            /// Is SSL/TLS enabled
            /// </summary>
            IsSslTlsEnabled = 16,
            /// <summary>
            /// Remote host
            /// </summary>
            RemoteHost = 17,
            /// <summary>
            /// Network interface card
            /// </summary>
            NetworkInterfaceCard = 18
        }

        /// <summary>
        /// The list of changed properties.
        /// </summary>
        private readonly System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting> changedPropertyList = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting>();
        /// <summary>
        /// Gets the list of updated properties.
        /// </summary>
        protected System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting> ChangedPropertyList
        {
            get
            {
                return changedPropertyList;
            }
        }
    }

    /// <summary>
    /// Specifies the SNMPv3 authentication protocol.
    /// </summary>
    public enum SnmpV3AuthenticationAlgorithm
    {
        /// <summary>
        /// Message Digest 5 (MD5).
        /// </summary>
        [System.ComponentModel.Description("MD5")]
        Md5 = 0,
        /// <summary>
        /// Secure Hash Algorithm (SHA).
        /// </summary>
        [System.ComponentModel.Description("SHA")]
        Sha1 = 1,
        /// <summary>
        /// Secure Hash Algorithm (SHA) 224.
        /// </summary>
        [System.ComponentModel.Description("SHA224")]
        Sha224 = 2,
        /// <summary>
        /// Secure Hash Algorithm (SHA) 256.
        /// </summary>
        [System.ComponentModel.Description("SHA256")]
        Sha256 = 3,
        /// <summary>
        /// Secure Hash Algorithm (SHA) 384.
        /// </summary>
        [System.ComponentModel.Description("SHA384")]
        Sha384 = 4,
        /// <summary>
        /// Secure Hash Algorithm (SHA) 512.
        /// </summary>
        [System.ComponentModel.Description("SHA512")]
        Sha512 = 5,
        /// <summary>
        /// Algorithm is defined in the Credential Library.
        /// </summary>
        DefinedInCredentialsLibrary = 6,
        /// <summary>
        /// No algorithm selected.
        /// </summary>
        [System.ComponentModel.Description("None")]
        None = 7,
    }

    /// <summary>
    /// Specifies the SNMPv3 encryption algorithm.
    /// </summary>
    public enum SnmpV3EncryptionAlgorithm
    {
        /// <summary>
        /// Data Encryption Standard (DES).
        /// </summary>
        [System.ComponentModel.Description("DES")]
        Des = 0,
        /// <summary>
        /// Advanced Encryption Standard (AES) 128 bit.
        /// </summary>
        [System.ComponentModel.Description("AES128")]
        Aes128 = 1,
        /// <summary>
        /// Advanced Encryption Standard (AES) 192 bit.
        /// </summary>
        [System.ComponentModel.Description("AES192")]
        Aes192 = 2,
        /// <summary>
        /// Advanced Encryption Standard (AES) 256 bit.
        /// </summary>
        [System.ComponentModel.Description("AES256")]
        Aes256 = 3,
        /// <summary>
        /// Advanced Encryption Standard is defined in the Credential Library.
        /// </summary>
        DefinedInCredentialsLibrary = 4,
        /// <summary>
        /// No algorithm selected.
        /// </summary>
        [System.ComponentModel.Description("None")]
        None = 5,
    }

    /// <summary>
    /// Specifies the SNMP v3 security level and protocol.
    /// </summary>
    public enum SnmpV3SecurityLevelAndProtocol
    {
        /// <summary>
        /// Authentication and privacy.
        /// </summary>
        [System.ComponentModel.Description("authPriv")]
        AuthenticationPrivacy = 0,
        /// <summary>
        /// Authentication but no privacy.
        /// </summary>
        [System.ComponentModel.Description("authNoPriv")]
        AuthenticationNoPrivacy = 1,
        /// <summary>
        /// No authentication and no privacy.
        /// </summary>
        [System.ComponentModel.Description("noAuthNoPriv")]
        NoAuthenticationNoPrivacy = 2,
        /// <summary>
        /// Security Level and Protocol is defined in the Credential library.
        /// </summary>
        DefinedInCredentialsLibrary = 3
    }

    /// <summary>
    /// Represents a connection of a DataMiner element.
    /// </summary>
    public interface IElementConnection
    {
    }

    /// <summary>
    /// Defines a non-virtual interface.
    /// </summary>
    public interface IRealConnection : Skyline.DataMiner.Library.Common.IElementConnection
    {
    }

    /// <summary>
    /// Defines an SNMP connection.
    /// </summary>
    public interface ISnmpConnection : Skyline.DataMiner.Library.Common.IRealConnection
    {
    }

    /// <summary>
    /// Defines an SNMPv1 Connection
    /// </summary>
    public interface ISnmpV1Connection : Skyline.DataMiner.Library.Common.ISnmpConnection
    {
    }

    /// <summary>
    /// Defines an SNMPv2 Connection.
    /// </summary>
    public interface ISnmpV2Connection : Skyline.DataMiner.Library.Common.ISnmpConnection
    {
    }

    /// <summary>
    /// Defines an SNMPv3 Connection.
    /// </summary>
    public interface ISnmpV3Connection : Skyline.DataMiner.Library.Common.ISnmpConnection
    {
        /// <summary>
        /// Gets or sets the SNMPv3 security configuration.
        /// </summary>
        Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig SecurityConfig
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface for SnmpV3 Security configurations.
    /// </summary>
    public interface ISnmpV3SecurityConfig
    {
    }

    /// <summary>
    /// Defines a Virtual Connection
    /// </summary>
    public interface IVirtualConnection : Skyline.DataMiner.Library.Common.IElementConnection
    {
    }

    /// <summary>
    /// Represents a connection using the Internet Protocol (IP).
    /// </summary>
    public interface IIpBased : Skyline.DataMiner.Library.Common.IPortConnection
    {
    }

    /// <summary>
    /// interface IPortConnection for which all connections will inherit from.
    /// </summary>
    public interface IPortConnection
    {
    }

    /// <summary>
    /// Represents a UDP/IP connection.
    /// </summary>
    public interface IUdp : Skyline.DataMiner.Library.Common.IIpBased
    {
    }

    /// <summary>
    /// Class representing an UDP connection.
    /// </summary>
    public class Udp : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IUdp
    {
        private int? localPort;
        private int remotePort;
        private bool isSslTlsEnabled;
        private readonly bool isDedicated;
        private string remoteHost;
        private int networkInterfaceCard;
        /// <summary>
        /// Initializes a new instance, using default values for localPort (null=Auto) SslTlsEnabled (false), IsDedicated (false) and NetworkInterfaceCard (0=Auto)
        /// </summary>
        /// <param name = "remoteHost">The IP or name of the remote host.</param>
        /// <param name = "remotePort">The port number of the remote host.</param>
        public Udp(string remoteHost, int remotePort)
        {
            this.localPort = null;
            this.remotePort = remotePort;
            this.isSslTlsEnabled = false;
            this.isDedicated = false;
            this.remoteHost = remoteHost;
            this.networkInterfaceCard = 0;
        }

        /// <summary>
        /// Initializes a new instance using a <see cref = "ElementPortInfo"/> object.
        /// </summary>
        /// <param name = "info"></param>
        internal Udp(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            if (!info.LocalIPPort.Equals(""))
                localPort = System.Convert.ToInt32(info.LocalIPPort);
            remoteHost = info.PollingIPAddress;
            remotePort = System.Convert.ToInt32(info.PollingIPPort);
            isSslTlsEnabled = info.IsSslTlsEnabled;
            isDedicated = IsDedicatedConnection(info);
            networkInterfaceCard = System.Convert.ToInt32(info.Number);
        }

        /// <summary>
        /// Determines if a connection is using a dedicated connection or not (e.g serial single, smart serial single).
        /// </summary>
        /// <param name = "info">ElementPortInfo</param>
        /// <returns>Whether a connection is marked as single or not.</returns>
        private static bool IsDedicatedConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            bool isDedicatedConnection = false;
            switch (info.ProtocolType)
            {
                case Skyline.DataMiner.Net.Messages.ProtocolType.SerialSingle:
                case Skyline.DataMiner.Net.Messages.ProtocolType.SmartSerialRawSingle:
                case Skyline.DataMiner.Net.Messages.ProtocolType.SmartSerialSingle:
                    isDedicatedConnection = true;
                    break;
                default:
                    isDedicatedConnection = false;
                    break;
            }

            return isDedicatedConnection;
        }
    }

    /// <summary>
    /// Class representing any non-virtual connection.
    /// </summary>
    public class RealConnection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IRealConnection
    {
        private readonly int portId;
        private System.TimeSpan timeout;
        private int retries;
        /// <summary>
        /// Initiates a new RealConnection class.
        /// </summary>
        /// <param name = "info"></param>
        internal RealConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            this.portId = info.PortID;
            this.retries = info.Retries;
            this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
        }
    }

    /// <summary>
    /// Class used to Encrypt data in DataMiner.
    /// </summary>
    internal class RSA
    {
        private static System.Security.Cryptography.RSAParameters publicKey;
        /// <summary>
        /// Get or Sets the Public Key.
        /// </summary>
        internal static System.Security.Cryptography.RSAParameters PublicKey
        {
            get
            {
                return publicKey;
            }

            set
            {
                publicKey = value;
            }
        }
    }

    /// <summary>
    /// Class representing an SNMPv1 connection.
    /// </summary>
    public class SnmpV1Connection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV1Connection
    {
        private string getCommunityString;
        private string setCommunityString;
        private string deviceAddress;
        private System.TimeSpan timeout;
        private int retries;
        private Skyline.DataMiner.Library.Common.IUdp udpIpConfiguration;
        private readonly int id;
        private readonly System.Guid libraryCredentials;
        private System.TimeSpan? elementTimeout;
        /// <summary>
        /// Initiates an new instance.
        /// </summary>
        internal SnmpV1Connection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            this.deviceAddress = info.BusAddress;
            this.retries = info.Retries;
            this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
            this.libraryCredentials = info.LibraryCredential;
            //this.elementTimeout = new TimeSpan(0, 0, info.ElementTimeoutTime / 1000);
            if (this.libraryCredentials == System.Guid.Empty)
            {
                this.getCommunityString = info.GetCommunity;
                this.setCommunityString = info.SetCommunity;
            }
            else
            {
                this.getCommunityString = System.String.Empty;
                this.setCommunityString = System.String.Empty;
            }

            this.id = info.PortID;
            this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
            this.udpIpConfiguration = new Skyline.DataMiner.Library.Common.Udp(info);
        }

        /// <summary>
        /// /// Initiates a new instance with default settings for Get Community String (public), Set Community String (private), Device Address (empty),
        /// Command Timeout (1500ms), Retries (3) and Element Timeout (30s).
        /// </summary>
        /// <param name = "udpConfiguration">The UDP configuration parameters.</param>
        public SnmpV1Connection(Skyline.DataMiner.Library.Common.IUdp udpConfiguration)
        {
            if (udpConfiguration == null)
            {
                throw new System.ArgumentNullException("udpConfiguration");
            }

            this.id = -1;
            this.udpIpConfiguration = udpConfiguration;
            this.getCommunityString = "public";
            this.setCommunityString = "private";
            this.deviceAddress = System.String.Empty;
            this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
            this.retries = 3;
            this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
        }
    }

    /// <summary>
    /// Class representing an SnmpV2 Connection.
    /// </summary>
    public class SnmpV2Connection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV2Connection
    {
        private System.TimeSpan timeout;
        private int retries;
        private string deviceAddress;
        private Skyline.DataMiner.Library.Common.IUdp udpIpConfiguration;
        private string getCommunityString;
        private string setCommunityString;
        private readonly int portId;
        private readonly System.Guid libraryCredentials;
        private System.TimeSpan? elementTimeout;
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal SnmpV2Connection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            this.deviceAddress = info.BusAddress;
            this.retries = info.Retries;
            this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
            this.getCommunityString = info.GetCommunity;
            this.setCommunityString = info.SetCommunity;
            this.libraryCredentials = info.LibraryCredential;
            if (info.LibraryCredential == System.Guid.Empty)
            {
                this.getCommunityString = info.GetCommunity;
                this.setCommunityString = info.SetCommunity;
            }
            else
            {
                this.getCommunityString = System.String.Empty;
                this.setCommunityString = System.String.Empty;
            }

            this.portId = info.PortID;
            this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
            this.udpIpConfiguration = new Skyline.DataMiner.Library.Common.Udp(info);
        }

        /// <summary>
        ///	Initiates a new instance with default settings for Get Community String (public), Set Community String (private), Device Address (empty),
        /// Command Timeout (1500ms), Retries (3) and Element Timeout (30s).
        /// </summary>
        /// <param name = "udpConfiguration">The UDP Connection settings.</param>
        public SnmpV2Connection(Skyline.DataMiner.Library.Common.IUdp udpConfiguration)
        {
            if (udpConfiguration == null)
            {
                throw new System.ArgumentNullException("udpConfiguration");
            }

            this.portId = -1;
            this.udpIpConfiguration = udpConfiguration;
            //this.udpIpConfiguration = udpIpIpConfiguration;
            this.deviceAddress = System.String.Empty;
            this.getCommunityString = "public";
            this.setCommunityString = "private";
            this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
            this.retries = 3;
            this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
            this.libraryCredentials = System.Guid.Empty;
        }
    }

    /// <summary>
    /// Class representing a SNMPv3 class.
    /// </summary>
    public class SnmpV3Connection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV3Connection
    {
        private System.TimeSpan? elementTimeout;
        private System.TimeSpan timeout;
        private int retries;
        private string deviceAddress;
        private Skyline.DataMiner.Library.Common.IUdp udpIpConfiguration;
        private Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig securityConfig;
        private readonly int id;
        private readonly System.Guid libraryCredentials;
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal SnmpV3Connection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            this.deviceAddress = info.BusAddress;
            this.retries = info.Retries;
            this.timeout = new System.TimeSpan(0, 0, 0, 0, info.TimeoutTime);
            this.elementTimeout = new System.TimeSpan(0, 0, info.ElementTimeoutTime / 1000);
            if (this.libraryCredentials == System.Guid.Empty)
            {
                Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol securityLevelAndProtocol = Skyline.DataMiner.Library.Common.HelperClass.GetEnumFromDescription<Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol>(info.StopBits);
                Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm = Skyline.DataMiner.Library.Common.HelperClass.GetEnumFromDescription<Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm>(info.FlowControl);
                Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationProtocol = Skyline.DataMiner.Library.Common.HelperClass.GetEnumFromDescription<Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm>(info.Parity);
                string authenticationKey = info.GetCommunity;
                string encryptionKey = info.SetCommunity;
                string username = info.DataBits;
                this.securityConfig = new Skyline.DataMiner.Library.Common.SnmpV3SecurityConfig(securityLevelAndProtocol, username, authenticationKey, encryptionKey, authenticationProtocol, encryptionAlgorithm);
            }
            else
            {
                this.SecurityConfig = new Skyline.DataMiner.Library.Common.SnmpV3SecurityConfig(Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.DefinedInCredentialsLibrary, "", "", "", Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary, Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary);
            }

            this.id = info.PortID;
            this.elementTimeout = new System.TimeSpan(0, 0, 0, 0, info.ElementTimeoutTime);
            this.udpIpConfiguration = new Skyline.DataMiner.Library.Common.Udp(info);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name = "udpConfiguration">The udp configuration settings.</param>
        ///<param name = "securityConfig">The SNMPv3 security configuration.</param>
        public SnmpV3Connection(Skyline.DataMiner.Library.Common.IUdp udpConfiguration, Skyline.DataMiner.Library.Common.SnmpV3SecurityConfig securityConfig)
        {
            if (udpConfiguration == null)
            {
                throw new System.ArgumentNullException("udpConfiguration");
            }

            if (securityConfig == null)
            {
                throw new System.ArgumentNullException("securityConfig");
            }

            this.libraryCredentials = System.Guid.Empty;
            this.id = -1;
            this.udpIpConfiguration = udpConfiguration;
            this.deviceAddress = System.String.Empty;
            this.securityConfig = securityConfig;
            this.timeout = new System.TimeSpan(0, 0, 0, 0, 1500);
            this.retries = 3;
            this.elementTimeout = new System.TimeSpan(0, 0, 0, 30);
        }

        /// <summary>
        /// Gets or sets the SNMPv3 security configuration.
        /// </summary>
        public Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig SecurityConfig
        {
            get
            {
                return securityConfig;
            }

            set
            {
                ChangedPropertyList.Add(Skyline.DataMiner.Library.Common.ConnectionSettings.ConnectionSetting.SecurityConfig);
                securityConfig = value;
            }
        }
    }

    /// <summary>
    /// Represents the Security settings linked to SNMPv3.
    /// </summary>
    public class SnmpV3SecurityConfig : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.ISnmpV3SecurityConfig
    {
        private Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol securityLevelAndProtocol;
        private string username;
        private string authenticationKey;
        private string encryptionKey;
        private Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationAlgorithm;
        private Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm;
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name = "securityLevelAndProtocol">The security Level and Protocol.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "authenticationKey">The authenticationKey</param>
        /// <param name = "encryptionKey">The encryptionKey</param>
        /// <param name = "authenticationAlgorithm">The authentication Algorithm.</param>
        /// <param name = "encryptionAlgorithm">The encryption Algorithm.</param>
        /// <exception cref = "System.ArgumentNullException">When username, authenticationKey or encryptionKey is null.</exception>
        internal SnmpV3SecurityConfig(Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol securityLevelAndProtocol, string username, string authenticationKey, string encryptionKey, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationAlgorithm, Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm)
        {
            if (username == null)
            {
                throw new System.ArgumentNullException("username");
            }

            if (authenticationKey == null)
            {
                throw new System.ArgumentNullException("authenticationKey");
            }

            if (encryptionKey == null)
            {
                throw new System.ArgumentNullException("encryptionKey");
            }

            this.securityLevelAndProtocol = securityLevelAndProtocol;
            this.username = username;
            this.authenticationKey = authenticationKey;
            this.encryptionKey = encryptionKey;
            this.authenticationAlgorithm = authenticationAlgorithm;
            this.encryptionAlgorithm = encryptionAlgorithm;
        }

        /// <summary>
        /// Initializes a new instance using No Authentication and No Privacy.
        /// </summary>
        /// <param name = "username">The username.</param>
        /// <exception cref = "System.ArgumentNullException">When the username is null.</exception>
        public SnmpV3SecurityConfig(string username)
        {
            if (username == null)
            {
                throw new System.ArgumentNullException("username");
            }

            this.securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.NoAuthenticationNoPrivacy;
            this.username = username;
            this.authenticationKey = "";
            this.encryptionKey = "";
            this.authenticationAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None;
            this.encryptionAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None;
        }

        /// <summary>
        /// Initializes a new instance using Authentication No Privacy.
        /// </summary>
        /// <param name = "username">The username.</param>
        /// <param name = "authenticationKey">The Authentication key.</param>
        /// <param name = "authenticationAlgorithm">The Authentication Algorithm.</param>
        /// <exception cref = "System.ArgumentNullException">When username, authenticationKey is null.</exception>
        /// <exception cref = "IncorrectDataException">When None or DefinedInCredentialsLibrary is selected as authentication algorithm.</exception>
        public SnmpV3SecurityConfig(string username, string authenticationKey, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationAlgorithm)
        {
            if (username == null)
            {
                throw new System.ArgumentNullException("username");
            }

            if (authenticationKey == null)
            {
                throw new System.ArgumentNullException("authenticationKey");
            }

            if ((authenticationAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None) || (authenticationAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary))
            {
                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Authentication Algorithm 'None' and 'DefinedInCredentialsLibrary' is Invalid when choosing 'Authentication No Privacy' as Security Level and Protocol.");
            }

            this.securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationNoPrivacy;
            this.username = username;
            this.authenticationKey = authenticationKey;
            this.encryptionKey = "";
            this.authenticationAlgorithm = authenticationAlgorithm;
            this.encryptionAlgorithm = Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None;
        }

        /// <summary>
        /// Initializes a new instance using Authentication and Privacy.
        /// </summary>
        /// <param name = "username">The username.</param>
        /// <param name = "authenticationKey">The authentication key.</param>
        /// <param name = "encryptionKey">The encryptionKey.</param>
        /// <param name = "authenticationProtocol">The authentication algorithm.</param>
        /// <param name = "encryptionAlgorithm">The encryption algorithm.</param>
        /// <exception cref = "System.ArgumentNullException">When username, authenticationKey or encryptionKey is null.</exception>
        /// <exception cref = "IncorrectDataException">When None or DefinedInCredentialsLibrary is selected as authentication algorithm or encryption algorithm.</exception>
        public SnmpV3SecurityConfig(string username, string authenticationKey, Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm authenticationProtocol, string encryptionKey, Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm encryptionAlgorithm)
        {
            if (username == null)
            {
                throw new System.ArgumentNullException("username");
            }

            if (authenticationKey == null)
            {
                throw new System.ArgumentNullException("authenticationKey");
            }

            if (encryptionKey == null)
            {
                throw new System.ArgumentNullException("encryptionKey");
            }

            if ((authenticationProtocol == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.None) || (authenticationProtocol == Skyline.DataMiner.Library.Common.SnmpV3AuthenticationAlgorithm.DefinedInCredentialsLibrary))
            {
                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Authentication Algorithm 'None' and 'DefinedInCredentialsLibrary' is Invalid when choosing 'Authentication No Privacy' as Security Level and Protocol.");
            }

            if ((encryptionAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.None) || (encryptionAlgorithm == Skyline.DataMiner.Library.Common.SnmpV3EncryptionAlgorithm.DefinedInCredentialsLibrary))
            {
                throw new Skyline.DataMiner.Library.Common.IncorrectDataException("Encryption Algorithm 'None' and 'DefinedInCredentialsLibrary' is Invalid when choosing 'Authentication and Privacy' as Security Level and Protocol.");
            }

            this.securityLevelAndProtocol = Skyline.DataMiner.Library.Common.SnmpV3SecurityLevelAndProtocol.AuthenticationPrivacy;
            this.username = username;
            this.authenticationKey = authenticationKey;
            this.encryptionKey = encryptionKey;
            this.authenticationAlgorithm = authenticationProtocol;
            this.encryptionAlgorithm = encryptionAlgorithm;
        }
    }

    /// <summary>
    /// Class representing a Virtual connection. 
    /// </summary>
    public class VirtualConnection : Skyline.DataMiner.Library.Common.ConnectionSettings, Skyline.DataMiner.Library.Common.IVirtualConnection
    {
        private readonly int id;
        /// <summary>
        /// Initiates a new VirtualConnection class.
        /// </summary>
        /// <param name = "info"></param>
        internal VirtualConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            this.id = info.PortID;
        }

        /// <summary>
        /// Initiates a new VirtualConnection class.
        /// </summary>
        public VirtualConnection()
        {
            this.id = -1;
        }
    }

    /// <summary>
    /// Represents a DataMiner element.
    /// </summary>
    internal class DmsElement : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDmsElement
    {
        // Keep this message in case we need to parse the element properties when the user wants to use these.
        private Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo;
        /// <summary>
        /// The advanced settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.AdvancedSettings advancedSettings;
        /// <summary>
        /// The device settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.DeviceSettings deviceSettings;
        /// <summary>
        /// The DVE settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.DveSettings dveSettings;
        /// <summary>
        /// The failover settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.FailoverSettings failoverSettings;
        /// <summary>
        /// The general settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.GeneralSettings generalSettings;
        /// <summary>
        /// The redundancy settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.RedundancySettings redundancySettings;
        /// <summary>
        /// The replication settings.
        /// </summary>
        private Skyline.DataMiner.Library.Common.ReplicationSettings replicationSettings;
        /// <summary>
        /// The element components.
        /// </summary>
        private System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.ElementSettings> settings;
        /// <summary>
        /// Collection of connections available on the element.
        /// </summary>
        private Skyline.DataMiner.Library.Common.ElementConnectionCollection elementCommunicationConnections;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsElement"/> class.
        /// </summary>
        /// <param name = "dms">Object implementing <see cref = "IDms"/> interface.</param>
        /// <param name = "dmsElementId">The system-wide element ID.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
        internal DmsElement(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Library.Common.DmsElementId dmsElementId): base(dms)
        {
            Initialize();
            generalSettings.DmsElementId = dmsElementId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsElement"/> class.
        /// </summary>
        /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
        /// <param name = "elementInfo">The element information.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentNullException"><paramref name = "elementInfo"/> is <see langword = "null"/>.</exception>
        internal DmsElement(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo): base(dms)
        {
            if (elementInfo == null)
            {
                throw new System.ArgumentNullException("elementInfo");
            }

            Initialize(elementInfo);
            Parse(elementInfo);
        }

        /// <summary>
        /// Gets or sets the element description.
        /// </summary>
        /// <value>The element description.</value>
        public string Description
        {
            get
            {
                return GeneralSettings.Description;
            }

            set
            {
                GeneralSettings.Description = value;
            }
        }

        /// <summary>
        /// Gets the system-wide element ID of the element.
        /// </summary>
        /// <value>The system-wide element ID of the element.</value>
        public Skyline.DataMiner.Library.Common.DmsElementId DmsElementId
        {
            get
            {
                return generalSettings.DmsElementId;
            }
        }

        /// <summary>
        /// Gets the DVE settings of this element.
        /// </summary>
        /// <value>The DVE settings of this element.</value>
        public Skyline.DataMiner.Library.Common.IDveSettings DveSettings
        {
            get
            {
                return dveSettings;
            }
        }

        /// <summary>
        /// Gets the DataMiner Agent that hosts this element.
        /// </summary>
        /// <value>The DataMiner Agent that hosts this element.</value>
        public Skyline.DataMiner.Library.Common.IDma Host
        {
            get
            {
                return generalSettings.Host;
            }
        }

        /// <summary>
        /// Gets or sets the element name.
        /// </summary>
        /// <value>The element name.</value>
        /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
        /// <remarks>
        /// <para>The following restrictions apply to element names:</para>
        /// <list type = "bullet">
        ///		<item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
        ///		<item><para>Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</para></item>
        ///		<item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
        /// </list>
        /// </remarks>
        public string Name
        {
            get
            {
                return generalSettings.Name;
            }

            set
            {
                generalSettings.Name = Skyline.DataMiner.Library.Common.InputValidator.ValidateName(value, "value");
            }
        }

        /// <summary>
        /// Gets the protocol executed by this element.
        /// </summary>
        /// <value>The protocol executed by this element.</value>
        public Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
        {
            get
            {
                return generalSettings.Protocol;
            }
        }

        /// <summary>
        /// Gets the redundancy settings.
        /// </summary>
        /// <value>The redundancy settings.</value>
        public Skyline.DataMiner.Library.Common.IRedundancySettings RedundancySettings
        {
            get
            {
                return redundancySettings;
            }
        }

        /// <summary>
        /// Gets the element state.
        /// </summary>
        /// <value>The element state.</value>
        public Skyline.DataMiner.Library.Common.ElementState State
        {
            get
            {
                return GeneralSettings.State;
            }

            internal set
            {
                GeneralSettings.State = value;
            }
        }

        /// <summary>
        /// Gets the general settings of the element.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.GeneralSettings GeneralSettings
        {
            get
            {
                return generalSettings;
            }
        }

        /// <summary>
        /// Gets the specified standalone parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the parameter. Currently supported types: int?, double?, DateTime? and string.</typeparam>
        /// <param name = "parameterId">The parameter ID.</param>
        /// <exception cref = "ArgumentException"><paramref name = "parameterId"/> is invalid.</exception>
        /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        /// <exception cref = "NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
        /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
        public Skyline.DataMiner.Library.Common.IDmsStandaloneParameter<T> GetStandaloneParameter<T>(int parameterId)
        {
            if (parameterId < 1)
            {
                throw new System.ArgumentException("Invalid parameter ID.", "parameterId");
            }

            System.Type type = typeof(T);
            if (type != typeof(string) && type != typeof(int? ) && type != typeof(double? ) && type != typeof(System.DateTime? ))
            {
                throw new System.NotSupportedException("Only one of the following types is supported: string, int?, double? or DateTime?.");
            }

            Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(this);
            return new Skyline.DataMiner.Library.Common.DmsStandaloneParameter<T>(this, parameterId);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Name: {0}{1}", Name, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "agent ID/element ID: {0}{1}", DmsElementId.Value, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Description: {0}{1}", Description, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol name: {0}{1}", Protocol.Name, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol version: {0}{1}", Protocol.Version, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Hosting agent ID: {0}{1}", Host.Id, System.Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Loads all the data and properties found related to the element.
        /// </summary>
        /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner system.</exception>
        internal override void Load()
        {
            try
            {
                IsLoaded = true;
                Skyline.DataMiner.Net.Messages.GetElementByIDMessage message = new Skyline.DataMiner.Net.Messages.GetElementByIDMessage(generalSettings.DmsElementId.AgentId, generalSettings.DmsElementId.ElementId);
                Skyline.DataMiner.Net.Messages.ElementInfoEventMessage response = (Skyline.DataMiner.Net.Messages.ElementInfoEventMessage)Communication.SendSingleResponseMessage(message);
                Parse(response);
            }
            catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
            {
                if (e.ErrorCode == -2146233088)
                {
                    // 0x80131500, Element "[element ID]" is unavailable.
                    throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(DmsElementId, e);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Parses all of the element info.
        /// </summary>
        /// <param name = "elementInfo">The element info message.</param>
        internal void Parse(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            IsLoaded = true;
            try
            {
                ParseElementInfo(elementInfo);
            }
            catch
            {
                IsLoaded = false;
                throw;
            }
        }

        /// <summary>
        /// Initializes the element.
        /// </summary>
        private void Initialize(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            this.elementInfo = elementInfo;
            generalSettings = new Skyline.DataMiner.Library.Common.GeneralSettings(this);
            deviceSettings = new Skyline.DataMiner.Library.Common.DeviceSettings(this);
            replicationSettings = new Skyline.DataMiner.Library.Common.ReplicationSettings(this);
            advancedSettings = new Skyline.DataMiner.Library.Common.AdvancedSettings(this);
            failoverSettings = new Skyline.DataMiner.Library.Common.FailoverSettings(this);
            redundancySettings = new Skyline.DataMiner.Library.Common.RedundancySettings(this);
            dveSettings = new Skyline.DataMiner.Library.Common.DveSettings(this);
            elementCommunicationConnections = new Skyline.DataMiner.Library.Common.ElementConnectionCollection(this.elementInfo);
            settings = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementSettings>{generalSettings, deviceSettings, replicationSettings, advancedSettings, failoverSettings, redundancySettings, dveSettings};
        }

        /// <summary>
        /// Initializes the element.
        /// </summary>
        private void Initialize()
        {
            generalSettings = new Skyline.DataMiner.Library.Common.GeneralSettings(this);
            deviceSettings = new Skyline.DataMiner.Library.Common.DeviceSettings(this);
            replicationSettings = new Skyline.DataMiner.Library.Common.ReplicationSettings(this);
            advancedSettings = new Skyline.DataMiner.Library.Common.AdvancedSettings(this);
            failoverSettings = new Skyline.DataMiner.Library.Common.FailoverSettings(this);
            redundancySettings = new Skyline.DataMiner.Library.Common.RedundancySettings(this);
            dveSettings = new Skyline.DataMiner.Library.Common.DveSettings(this);
            elementCommunicationConnections = new Skyline.DataMiner.Library.Common.ElementConnectionCollection(this.elementInfo);
            settings = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.ElementSettings>{generalSettings, deviceSettings, replicationSettings, advancedSettings, failoverSettings, redundancySettings, dveSettings};
        }

        /// <summary>
        /// Parses the element info.
        /// </summary>
        /// <param name = "elementInfo">The element info.</param>
        private void ParseElementInfo(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            // Keep this object in case properties are accessed.
            //this.elementInfo = elementInfo;
            foreach (Skyline.DataMiner.Library.Common.ElementSettings component in settings)
            {
                component.Load(elementInfo);
            }

            ParseConnections(elementInfo);
        }

        /// <summary>
        /// Parse an ElementInfoEventMessage object.
        /// </summary>
        /// <param name = "elementInfo"></param>
        private void ParseConnections(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            // Keep this object in case properties are accessed.
            this.elementInfo = elementInfo;
            ParseConnection(elementInfo.MainPort);
            if (elementInfo.ExtraPorts != null)
            {
                foreach (Skyline.DataMiner.Net.Messages.ElementPortInfo info in elementInfo.ExtraPorts)
                {
                    ParseConnection(info);
                }
            }
        }

        /// <summary>
        /// Parse an ElementPortInfo object in order to add IElementConnection objects to the ElementConnectionCollection.
        /// </summary>
        /// <param name = "info">The ElementPortInfo object.</param>
        private void ParseConnection(Skyline.DataMiner.Net.Messages.ElementPortInfo info)
        {
            switch (info.ProtocolType)
            {
                case Skyline.DataMiner.Net.Messages.ProtocolType.Virtual:
                    Skyline.DataMiner.Library.Common.VirtualConnection myVirtualConnection = new Skyline.DataMiner.Library.Common.VirtualConnection(info);
                    elementCommunicationConnections[info.PortID] = myVirtualConnection;
                    break;
                case Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV1:
                    Skyline.DataMiner.Library.Common.SnmpV1Connection mySnmpV1Connection = new Skyline.DataMiner.Library.Common.SnmpV1Connection(info);
                    elementCommunicationConnections[info.PortID] = mySnmpV1Connection;
                    break;
                case Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV2:
                    Skyline.DataMiner.Library.Common.SnmpV2Connection mySnmpv2Connection = new Skyline.DataMiner.Library.Common.SnmpV2Connection(info);
                    elementCommunicationConnections[info.PortID] = mySnmpv2Connection;
                    break;
                case Skyline.DataMiner.Net.Messages.ProtocolType.SnmpV3:
                    Skyline.DataMiner.Library.Common.SnmpV3Connection mySnmpV3Connection = new Skyline.DataMiner.Library.Common.SnmpV3Connection(info);
                    elementCommunicationConnections[info.PortID] = mySnmpV3Connection;
                    break;
                default:
                    Skyline.DataMiner.Library.Common.RealConnection myConnection = new Skyline.DataMiner.Library.Common.RealConnection(info);
                    elementCommunicationConnections[info.PortID] = myConnection;
                    break;
            }
        }
    }

    /// <summary>
    /// DataMiner element interface.
    /// </summary>
    public interface IDmsElement : Skyline.DataMiner.Library.Common.IDmsObject, Skyline.DataMiner.Library.Common.IUpdateable
    {
        /// <summary>
        /// Gets or sets the element description.
        /// </summary>
        /// <value>The element description.</value>
        string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the system-wide element ID of the element.
        /// </summary>
        /// <value>The system-wide element ID of the element.</value>
        Skyline.DataMiner.Library.Common.DmsElementId DmsElementId
        {
            get;
        }

        /// <summary>
        /// Gets the DVE settings of this element.
        /// </summary>
        /// <value>The DVE settings of this element.</value>
        Skyline.DataMiner.Library.Common.IDveSettings DveSettings
        {
            get;
        }

        ///// <summary>
        ///// Gets the failover settings of this element.
        ///// </summary>
        ///// <value>The failover settings of this element.</value>
        //IFailoverSettings FailoverSettings { get; }
        /// <summary>
        /// Gets the DataMiner Agent that hosts this element.
        /// </summary>
        /// <value>The DataMiner Agent that hosts this element.</value>
        Skyline.DataMiner.Library.Common.IDma Host
        {
            get;
        }

        /// <summary>
        /// Gets or sets the element name.
        /// </summary>
        /// <value>The element name.</value>
        /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation is empty or white space.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation exceeds 200 characters.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation contains a forbidden character.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation contains more than one '%' character.</exception>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
        /// <remarks>
        /// <para>The following restrictions apply to element names:</para>
        /// <list type = "bullet">
        ///		<item><para>Names may not start or end with the following characters: '.' (dot), ' ' (space).</para></item>
        ///		<item><para>Names may not contain the following characters: '\', '/', ':', '*', '?', '"', '&lt;', '&gt;', '|', '°', ';'.</para></item>
        ///		<item><para>The following characters may not occur more than once within a name: '%' (percentage).</para></item>
        /// </list>
        /// </remarks>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the protocol executed by this element.
        /// </summary>
        /// <value>The protocol executed by this element.</value>
        Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
        {
            get;
        }

        /// <summary>
        /// Gets the redundancy settings.
        /// </summary>
        /// <value>The redundancy settings.</value>
        Skyline.DataMiner.Library.Common.IRedundancySettings RedundancySettings
        {
            get;
        }

        /// <summary>
        /// Gets the element state.
        /// </summary>
        /// <value>The element state.</value>
        Skyline.DataMiner.Library.Common.ElementState State
        {
            get;
        }

        /// <summary>
        /// Gets the specified standalone parameter.
        /// </summary>
        /// <typeparam name = "T">The type of the parameter. Currently supported types: int?, double?, DateTime? and string.</typeparam>
        /// <param name = "parameterId">The parameter ID.</param>
        /// <exception cref = "ArgumentException"><paramref name = "parameterId"/> is invalid.</exception>
        /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
        /// <exception cref = "ElementStoppedException">The element is not active.</exception>
        /// <exception cref = "NotSupportedException">A type other than string, int?, double? or DateTime? was provided.</exception>
        /// <returns>The standalone parameter that corresponds with the specified ID.</returns>
        Skyline.DataMiner.Library.Common.IDmsStandaloneParameter<T> GetStandaloneParameter<T>(int parameterId);
    }

    /// <summary>
    /// Represents the advanced element information.
    /// </summary>
    internal class AdvancedSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IAdvancedSettings
    {
        /// <summary>
        /// Value indicating whether the element is hidden.
        /// </summary>
        private bool isHidden;
        /// <summary>
        /// Value indicating whether the element is read-only.
        /// </summary>
        private bool isReadOnly;
        /// <summary>
        /// Indicates whether this is a simulated element.
        /// </summary>
        private bool isSimulation;
        /// <summary>
        /// The element timeout value.
        /// </summary>
        private System.TimeSpan timeout = new System.TimeSpan(0, 0, 30);
        /// <summary>
        /// Initializes a new instance of the <see cref = "AdvancedSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the <see cref = "DmsElement"/> instance this object is part of.</param>
        internal AdvancedSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is hidden.
        /// </summary>
        /// <value><c>true</c> if the element is hidden; otherwise, <c>false</c>.</value>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a derived element.</exception>
        public bool IsHidden
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isHidden;
            }

            set
            {
                DmsElement.LoadOnDemand();
                if (DmsElement.RedundancySettings.IsDerived)
                {
                    throw new System.NotSupportedException("This operation is not supported on a derived element.");
                }

                if (isHidden != value)
                {
                    ChangedPropertyList.Add("IsHidden");
                    isHidden = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is read-only.
        /// </summary>
        /// <value><c>true</c> if the element is read-only; otherwise, <c>false</c>.</value>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
        public bool IsReadOnly
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isReadOnly;
            }

            set
            {
                if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
                {
                    throw new System.NotSupportedException("This operation is not supported on a DVE child or derived element.");
                }

                DmsElement.LoadOnDemand();
                if (isReadOnly != value)
                {
                    ChangedPropertyList.Add("IsReadOnly");
                    isReadOnly = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the element is running a simulation.
        /// </summary>
        /// <value><c>true</c> if the element is running a simulation; otherwise, <c>false</c>.</value>
        public bool IsSimulation
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isSimulation;
            }
        }

        /// <summary>
        /// Gets or sets the element timeout value.
        /// </summary>
        /// <value>The timeout value.</value>
        /// <exception cref = "ArgumentOutOfRangeException">The value specified for a set operation is not in the range of [0,120] s.</exception>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
        /// <remarks>Fractional seconds are ignored. For example, setting the timeout to a value of 3.5s results in setting it to 3s.</remarks>
        public System.TimeSpan Timeout
        {
            get
            {
                DmsElement.LoadOnDemand();
                return timeout;
            }

            set
            {
                if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
                {
                    throw new System.NotSupportedException("Setting the timeout is not supported on a DVE child or derived element.");
                }

                DmsElement.LoadOnDemand();
                int timeoutInSeconds = (int)value.TotalSeconds;
                if (timeoutInSeconds < 0 || timeoutInSeconds > 120)
                {
                    throw new System.ArgumentOutOfRangeException("value", "The timeout value must be in the range of [0,120] s.");
                }

                if ((int)timeout.TotalSeconds != (int)value.TotalSeconds)
                {
                    ChangedPropertyList.Add("Timeout");
                    timeout = value;
                }
            }
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("ADVANCED SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Timeout: {0}{1}", Timeout, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Hidden: {0}{1}", IsHidden, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Simulation: {0}{1}", IsSimulation, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Read-only: {0}{1}", IsReadOnly, System.Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            timeout = new System.TimeSpan(0, 0, 0, 0, elementInfo.ElementTimeoutTime);
            isHidden = elementInfo.Hidden;
            isReadOnly = elementInfo.IsReadOnly;
            isSimulation = elementInfo.IsSimulated;
        }
    }

    /// <summary>
    ///  Represents a class containing the device details of an element.
    /// </summary>
    internal class DeviceSettings : Skyline.DataMiner.Library.Common.ElementSettings
    {
        /// <summary>
        /// The type of the element.
        /// </summary>
        private string type = System.String.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DeviceSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
        internal DeviceSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("DEVICE SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendLine("Type: " + type);
            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            type = elementInfo.Type ?? System.String.Empty;
        }
    }

    /// <summary>
    /// Represents DVE information of an element.
    /// </summary>
    internal class DveSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IDveSettings
    {
        /// <summary>
        /// Value indicating whether DVE creation is enabled.
        /// </summary>
        private bool isDveCreationEnabled = true;
        /// <summary>
        /// Value indicating whether this element is a parent DVE.
        /// </summary>
        private bool isParent;
        /// <summary>
        /// The parent element.
        /// </summary>
        private Skyline.DataMiner.Library.Common.IDmsElement parent;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DveSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
        internal DveSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Gets a value indicating whether this element is a DVE child.
        /// </summary>
        /// <value><c>true</c> if this element is a DVE child element; otherwise, <c>false</c>.</value>
        public bool IsChild
        {
            get
            {
                return parent != null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether DVE creation is enabled for this element.
        /// </summary>
        /// <value><c>true</c> if the element DVE generation is enabled; otherwise, <c>false</c>.</value>
        /// <exception cref = "NotSupportedException">The set operation is not supported: The element is not a DVE parent element.</exception>
        public bool IsDveCreationEnabled
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isDveCreationEnabled;
            }

            set
            {
                DmsElement.LoadOnDemand();
                if (!DmsElement.DveSettings.IsParent)
                {
                    throw new System.NotSupportedException("This operation is only supported on DVE parent elements.");
                }

                if (isDveCreationEnabled != value)
                {
                    ChangedPropertyList.Add("IsDveCreationEnabled");
                    isDveCreationEnabled = value;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this element is a DVE parent.
        /// </summary>
        /// <value><c>true</c> if the element is a DVE parent element; otherwise, <c>false</c>.</value>
        public bool IsParent
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isParent;
            }
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("DVE SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "DVE creation enabled: {0}{1}", IsDveCreationEnabled, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Is parent DVE: {0}{1}", IsParent, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Is child DVE: {0}{1}", IsChild, System.Environment.NewLine);
            if (IsChild)
            {
                sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Parent DataMiner agent ID/element ID: {0}{1}", parent.DmsElementId.Value, System.Environment.NewLine);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            if (elementInfo.IsDynamicElement && elementInfo.DveParentDmaId != 0 && elementInfo.DveParentElementId != 0)
            {
                parent = new Skyline.DataMiner.Library.Common.DmsElement(DmsElement.Dms, new Skyline.DataMiner.Library.Common.DmsElementId(elementInfo.DveParentDmaId, elementInfo.DveParentElementId));
            }

            isParent = elementInfo.IsDveMainElement;
            isDveCreationEnabled = elementInfo.CreateDVEs;
        }
    }

    /// <summary>
    /// Represents a class containing the failover settings for an element.
    /// </summary>
    internal class FailoverSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IFailoverSettings
    {
        /// <summary>
        /// In failover configurations, this can be used to force an element to run only on one specific agent.
        /// </summary>
        private string forceAgent = System.String.Empty;
        /// <summary>
        /// Is true when the element is a failover element and is online on the backup agent instead of this agent; otherwise, false.
        /// </summary>
        private bool isOnlineOnBackupAgent;
        /// <summary>
        /// Is true when the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, false.
        /// </summary>
        private bool keepOnline;
        /// <summary>
        /// Initializes a new instance of the <see cref = "FailoverSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
        internal FailoverSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether to force agent.
        /// Local IP address of the agent which will be running the element.
        /// </summary>
        /// <value>Value indicating whether to force agent.</value>
        public string ForceAgent
        {
            get
            {
                DmsElement.LoadOnDemand();
                return forceAgent;
            }

            set
            {
                DmsElement.LoadOnDemand();
                var newValue = value == null ? System.String.Empty : value;
                if (!forceAgent.Equals(newValue, System.StringComparison.Ordinal))
                {
                    ChangedPropertyList.Add("ForceAgent");
                    forceAgent = newValue;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the element is a failover element and is online on the backup agent instead of this agent.
        /// </summary>
        /// <value><c>true</c> if the element is a failover element and is online on the backup agent instead of this agent; otherwise, <c>false</c>.</value>
        public bool IsOnlineOnBackupAgent
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isOnlineOnBackupAgent;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is a failover element that needs to keep running on the same DataMiner agent event after switching.
        /// keepOnline="true" indicates that the element needs to keep running even when the agent is offline.
        /// </summary>
        /// <value><c>true</c> if the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, <c>false</c>.</value>
        public bool KeepOnline
        {
            get
            {
                DmsElement.LoadOnDemand();
                return keepOnline;
            }

            set
            {
                DmsElement.LoadOnDemand();
                if (keepOnline != value)
                {
                    ChangedPropertyList.Add("KeepOnline");
                    keepOnline = value;
                }
            }
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("FAILOVER SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Keep online: {0}{1}", KeepOnline, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Force agent: {0}{1}", ForceAgent, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Online on backup agent: {0}{1}", IsOnlineOnBackupAgent, System.Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            keepOnline = elementInfo.KeepOnline;
            forceAgent = elementInfo.ForceAgent ?? System.String.Empty;
            isOnlineOnBackupAgent = elementInfo.IsOnlineOnBackupAgent;
        }
    }

    /// <summary>
    /// Represents general element information.
    /// </summary>
    internal class GeneralSettings : Skyline.DataMiner.Library.Common.ElementSettings
    {
        /// <summary>
        /// The name of the alarm template.
        /// </summary>
        private string alarmTemplateName;
        /// <summary>
        /// The alarm template assigned to this element.
        /// </summary>
        private Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate alarmTemplate;
        /// <summary>
        /// Element description.
        /// </summary>
        private string description = System.String.Empty;
        /// <summary>
        /// The hosting DataMiner agent.
        /// </summary>
        private Skyline.DataMiner.Library.Common.Dma host;
        /// <summary>
        /// The element state.
        /// </summary>
        private Skyline.DataMiner.Library.Common.ElementState state = Skyline.DataMiner.Library.Common.ElementState.Active;
        /// <summary>
        /// Instance of the protocol this element executes.
        /// </summary>
        private Skyline.DataMiner.Library.Common.DmsProtocol protocol;
        /// <summary>
        /// The trend template assigned to this element.
        /// </summary>
        private Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate trendTemplate;
        /// <summary>
        /// The name of the element.
        /// </summary>
        private string name;
        /// <summary>
        /// Initializes a new instance of the <see cref = "GeneralSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
        internal GeneralSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Gets or sets the alarm template definition of the element.
        /// This can either be an alarm template or an alarm template group.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate AlarmTemplate
        {
            get
            {
                DmsElement.LoadOnDemand();
                return alarmTemplate;
            }

            set
            {
                DmsElement.LoadOnDemand();
                bool updateRequired = false;
                if (alarmTemplate == null)
                {
                    if (value != null)
                    {
                        updateRequired = true;
                    }
                }
                else
                {
                    if (value == null || !alarmTemplate.Equals(value))
                    {
                        updateRequired = true;
                    }
                }

                if (updateRequired)
                {
                    ChangedPropertyList.Add("AlarmTemplate");
                    alarmTemplateName = value == null ? System.String.Empty : value.Name;
                    alarmTemplate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the element description.
        /// </summary>
        internal string Description
        {
            get
            {
                DmsElement.LoadOnDemand();
                return description;
            }

            set
            {
                DmsElement.LoadOnDemand();
                string newValue = value == null ? System.String.Empty : value;
                if (!description.Equals(newValue, System.StringComparison.Ordinal))
                {
                    ChangedPropertyList.Add("Description");
                    description = newValue;
                }
            }
        }

        /// <summary>
        /// Gets or sets the system-wide element ID.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.DmsElementId DmsElementId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the DataMiner agent that hosts the element.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.Dma Host
        {
            get
            {
                DmsElement.LoadOnDemand();
                return host;
            }
        }

        /// <summary>
        /// Gets or sets the state of the element.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.ElementState State
        {
            get
            {
                DmsElement.LoadOnDemand();
                return state;
            }

            set
            {
                DmsElement.LoadOnDemand();
                state = value;
            }
        }

        /// <summary>
        /// Gets or sets the trend template assigned to this element.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate TrendTemplate
        {
            get
            {
                DmsElement.LoadOnDemand();
                return trendTemplate;
            }

            set
            {
                DmsElement.LoadOnDemand();
                bool updateRequired = false;
                if (trendTemplate == null)
                {
                    if (value != null)
                    {
                        updateRequired = true;
                    }
                }
                else
                {
                    if (value == null || !trendTemplate.Equals(value))
                    {
                        updateRequired = true;
                    }
                }

                if (updateRequired)
                {
                    ChangedPropertyList.Add("TrendTemplate");
                    trendTemplate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE child or a derived element.</exception>
        internal string Name
        {
            get
            {
                DmsElement.LoadOnDemand();
                return name;
            }

            set
            {
                DmsElement.LoadOnDemand();
                if (DmsElement.DveSettings.IsChild || DmsElement.RedundancySettings.IsDerived)
                {
                    throw new System.NotSupportedException("Setting the name of a DVE child or a derived element is not supported.");
                }

                if (!name.Equals(value, System.StringComparison.Ordinal))
                {
                    ChangedPropertyList.Add("Name");
                    name = value.Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the instance of the protocol.
        /// </summary>
        /// <exception cref = "ArgumentNullException">The value of a set operation is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException">The value of a set operation is empty.</exception>
        internal Skyline.DataMiner.Library.Common.DmsProtocol Protocol
        {
            get
            {
                DmsElement.LoadOnDemand();
                return protocol;
            }

            set
            {
                if (value == null)
                {
                    throw new System.ArgumentNullException("value");
                }

                DmsElement.LoadOnDemand();
                ChangedPropertyList.Add("Protocol");
                protocol = value;
            }
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("GENERAL SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Name: {0}{1}", DmsElement.Name, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Description: {0}{1}", Description, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol name: {0}{1}", Protocol.Name, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Protocol version: {0}{1}", Protocol.Version, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "DMA ID: {0}{1}", DmsElementId.AgentId, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Element ID: {0}{1}", DmsElementId.ElementId, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Hosting DMA ID: {0}{1}", Host.Id, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Alarm template: {0}{1}", AlarmTemplate, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Trend template: {0}{1}", TrendTemplate, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "State: {0}{1}", State, System.Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            DmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(elementInfo.DataMinerID, elementInfo.ElementID);
            description = elementInfo.Description ?? System.String.Empty;
            protocol = new Skyline.DataMiner.Library.Common.DmsProtocol(DmsElement.Dms, elementInfo.Protocol, elementInfo.ProtocolVersion);
            alarmTemplateName = elementInfo.ProtocolTemplate;
            trendTemplate = System.String.IsNullOrWhiteSpace(elementInfo.Trending) ? null : new Skyline.DataMiner.Library.Common.Templates.DmsTrendTemplate(DmsElement.Dms, elementInfo.Trending, protocol);
            state = (Skyline.DataMiner.Library.Common.ElementState)elementInfo.State;
            name = elementInfo.Name ?? System.String.Empty;
            host = new Skyline.DataMiner.Library.Common.Dma(DmsElement.Dms, elementInfo.HostingAgentID);
            LoadAlarmTemplateDefinition();
        }

        /// <summary>
        /// Loads the alarm template definition.
        /// This method checks whether there is a group or a template assigned to the element.
        /// </summary>
        private void LoadAlarmTemplateDefinition()
        {
            if (alarmTemplate == null && !System.String.IsNullOrWhiteSpace(alarmTemplateName))
            {
                Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = protocol.Name, Version = protocol.Version, Template = alarmTemplateName};
                Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage response = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)DmsElement.Dms.Communication.SendSingleResponseMessage(message);
                if (response != null)
                {
                    switch (response.Type)
                    {
                        case Skyline.DataMiner.Net.Messages.AlarmTemplateType.Template:
                            alarmTemplate = new Skyline.DataMiner.Library.Common.Templates.DmsStandaloneAlarmTemplate(DmsElement.Dms, response);
                            break;
                        case Skyline.DataMiner.Net.Messages.AlarmTemplateType.Group:
                            alarmTemplate = new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroup(DmsElement.Dms, response);
                            break;
                        default:
                            throw new System.InvalidOperationException("Unexpected value: " + response.Type);
                    }
                }
            }
        }
    }

    /// <summary>
    /// DataMiner element advanced settings interface.
    /// </summary>
    public interface IAdvancedSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether the element is hidden.
        /// </summary>
        /// <value><c>true</c> if the element is hidden; otherwise, <c>false</c>.</value>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a derived element.</exception>
        bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is read-only.
        /// </summary>
        /// <value><c>true</c> if the element is read-only; otherwise, <c>false</c>.</value>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
        bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the element is running a simulation.
        /// </summary>
        /// <value><c>true</c> if the element is running a simulation; otherwise, <c>false</c>.</value>
        bool IsSimulation
        {
            get;
        }

        /// <summary>
        /// Gets or sets the element timeout value.
        /// </summary>
        /// <value>The timeout value.</value>
        /// <exception cref = "NotSupportedException">A set operation is not supported on a DVE or derived element.</exception>
        /// <exception cref = "ArgumentOutOfRangeException">The value specified for a set operation is not in the range of [0,120] s.</exception>
        /// <remarks>Fractional seconds are ignored. For example, setting the timeout to a value of 3.5s results in setting it to 3s.</remarks>
        System.TimeSpan Timeout
        {
            get;
            set;
        }
    }

    /// <summary>
    /// DataMiner element DVE settings interface.
    /// </summary>
    public interface IDveSettings
    {
        /// <summary>
        /// Gets a value indicating whether this element is a DVE child.
        /// </summary>
        /// <value><c>true</c> if this element is a DVE child element; otherwise, <c>false</c>.</value>
        bool IsChild
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether DVE creation is enabled for this element.
        /// </summary>
        /// <value><c>true</c> if the element DVE generation is enabled; otherwise, <c>false</c>.</value>
        /// <exception cref = "NotSupportedException">The element is not a DVE parent element.</exception>
        bool IsDveCreationEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this element is a DVE parent.
        /// </summary>
        /// <value><c>true</c> if the element is a DVE parent element; otherwise, <c>false</c>.</value>
        bool IsParent
        {
            get;
        }
    }

    /// <summary>
    /// DataMiner element failover settings interface.
    /// </summary>
    internal interface IFailoverSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to force agent.
        /// Local IP address of the agent which will be running the element.
        /// </summary>
        /// <value>Value indicating whether to force agent.</value>
        string ForceAgent
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the element is a failover element and is online on the backup agent instead of this agent.
        /// </summary>
        /// <value><c>true</c> if the element is a failover element and is online on the backup agent instead of this agent; otherwise, <c>false</c>.</value>
        bool IsOnlineOnBackupAgent
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is a failover element that needs to keep running on the same DataMiner agent event after switching.
        /// </summary>
        /// <value><c>true</c> if the element is a failover element that needs to keep running on the same DataMiner agent event after switching; otherwise, <c>false</c>.</value>
        bool KeepOnline
        {
            get;
            set;
        }
    }

    /// <summary>
    /// DataMiner element redundancy settings interface.
    /// </summary>
    public interface IRedundancySettings
    {
        /// <summary>
        /// Gets a value indicating whether the element is derived from another element.
        /// </summary>
        /// <value><c>true</c> if the element is derived from another element; otherwise, <c>false</c>.</value>
        bool IsDerived
        {
            get;
        }
    }

    /// <summary>
    /// DataMiner element replication settings interface.
    /// </summary>
    public interface IReplicationSettings
    {
    }

    /// <summary>
    /// Represents the redundancy settings for a element.
    /// </summary>
    internal class RedundancySettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IRedundancySettings
    {
        /// <summary>
        /// Value indicating whether or not this element is derived from another element.
        /// </summary>
        private bool isDerived;
        /// <summary>
        /// Initializes a new instance of the <see cref = "RedundancySettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the <see cref = "DmsElement"/> instance this object is part of.</param>
        internal RedundancySettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the element is derived from another element.
        /// </summary>
        /// <value><c>true</c> if the element is derived from another element; otherwise, <c>false</c>.</value>
        public bool IsDerived
        {
            get
            {
                DmsElement.LoadOnDemand();
                return isDerived;
            }

            internal set
            {
                isDerived = value;
            }
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("REDUNDANCY SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Derived: {0}{1}", isDerived, System.Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            isDerived = elementInfo.IsDerivedElement;
        }
    }

    /// <summary>
    /// Represents the replication information of an element.
    /// </summary>
    internal class ReplicationSettings : Skyline.DataMiner.Library.Common.ElementSettings, Skyline.DataMiner.Library.Common.IReplicationSettings
    {
        /// <summary>
        /// The domain the specified user belongs to.
        /// </summary>
        private string domain = System.String.Empty;
        /// <summary>
        /// External DMP engine.
        /// </summary>
        private bool connectsToExternalDmp;
        /// <summary>
        /// IP address of the source DataMiner Agent.
        /// </summary>
        private string ipAddressSourceDma = System.String.Empty;
        /// <summary>
        /// Value indicating whether this element is replicated.
        /// </summary>
        private bool isReplicated;
        /// <summary>
        /// The options string.
        /// </summary>
        private string options = System.String.Empty;
        /// <summary>
        /// The password.
        /// </summary>
        private string password = System.String.Empty;
        /// <summary>
        /// The ID of the source element.
        /// </summary>
        private Skyline.DataMiner.Library.Common.DmsElementId sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
        /// <summary>
        /// The user name.
        /// </summary>
        private string userName = System.String.Empty;
        /// <summary>
        /// Initializes a new instance of the <see cref = "ReplicationSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the DmsElement where this object will be used in.</param>
        internal ReplicationSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement): base(dmsElement)
        {
        }

        /// <summary>
        /// Returns the string representation of the object.
        /// </summary>
        /// <returns>String representation of the object.</returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("REPLICATION SETTINGS:");
            sb.AppendLine("==========================");
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Replicated: {0}{1}", isReplicated, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Source DMA ID: {0}{1}", sourceDmsElementId.AgentId, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Source element ID: {0}{1}", sourceDmsElementId.ElementId, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "IP address source DMA: {0}{1}", ipAddressSourceDma, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Domain: {0}{1}", domain, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "User name: {0}{1}", userName, System.Environment.NewLine);
            sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "Password: {0}{1}", password, System.Environment.NewLine);
            //sb.AppendFormat(CultureInfo.InvariantCulture, "Options: {0}{1}", options, Environment.NewLine);
            //sb.AppendFormat(CultureInfo.InvariantCulture, "Replication DMP engine: {0}{1}", connectsToExternalDmp, Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>
        /// Loads the information to the component.
        /// </summary>
        /// <param name = "elementInfo">The element information.</param>
        internal override void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo)
        {
            isReplicated = elementInfo.ReplicationActive;
            if (!isReplicated)
            {
                options = System.String.Empty;
                ipAddressSourceDma = System.String.Empty;
                password = System.String.Empty;
                domain = System.String.Empty;
                sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
                userName = System.String.Empty;
                connectsToExternalDmp = false;
            }

            options = elementInfo.ReplicationOptions ?? System.String.Empty;
            ipAddressSourceDma = elementInfo.ReplicationDmaIP ?? System.String.Empty;
            password = elementInfo.ReplicationPwd ?? System.String.Empty;
            domain = elementInfo.ReplicationDomain ?? System.String.Empty;
            bool isEmpty = System.String.IsNullOrWhiteSpace(elementInfo.ReplicationRemoteElement) || elementInfo.ReplicationRemoteElement.Equals("/", System.StringComparison.Ordinal);
            if (isEmpty)
            {
                sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
            }
            else
            {
                try
                {
                    sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(elementInfo.ReplicationRemoteElement);
                }
                catch (System.Exception ex)
                {
                    string logMessage = "Failed parsing replication element info for element " + System.Convert.ToString(elementInfo.Name) + " (" + System.Convert.ToString(elementInfo.DataMinerID) + "/" + System.Convert.ToString(elementInfo.ElementID) + "). Replication remote element is: " + System.Convert.ToString(elementInfo.ReplicationRemoteElement) + System.Environment.NewLine + ex;
                    Skyline.DataMiner.Library.Common.Logger.Log(logMessage);
                    sourceDmsElementId = new Skyline.DataMiner.Library.Common.DmsElementId(-1, -1);
                }
            }

            userName = elementInfo.ReplicationUser ?? System.String.Empty;
            connectsToExternalDmp = elementInfo.ReplicationIsExternalDMP;
        }
    }

    /// <summary>
    /// Represents a base class for all of the components in a DmsElement object.
    /// </summary>
    internal abstract class ElementSettings
    {
        /// <summary>
        /// The list of changed properties.
        /// </summary>
        private readonly System.Collections.Generic.List<string> changedPropertyList = new System.Collections.Generic.List<string>();
        /// <summary>
        /// Instance of the DmsElement class where these classes will be used for.
        /// </summary>
        private readonly Skyline.DataMiner.Library.Common.DmsElement dmsElement;
        /// <summary>
        /// Initializes a new instance of the <see cref = "ElementSettings"/> class.
        /// </summary>
        /// <param name = "dmsElement">The reference to the <see cref = "DmsElement"/> instance this object is part of.</param>
        protected ElementSettings(Skyline.DataMiner.Library.Common.DmsElement dmsElement)
        {
            this.dmsElement = dmsElement;
        }

        /// <summary>
        /// Gets the element this object belongs to.
        /// </summary>
        internal Skyline.DataMiner.Library.Common.DmsElement DmsElement
        {
            get
            {
                return dmsElement;
            }
        }

        /// <summary>
        /// Gets the list of updated properties.
        /// </summary>
        protected internal System.Collections.Generic.List<string> ChangedPropertyList
        {
            get
            {
                return changedPropertyList;
            }
        }

        /// <summary>
        /// Based on the array provided from the DmsNotify call, parse the data to the correct fields.
        /// </summary>
        /// <param name = "elementInfo">Object containing all the required information. Retrieved by DmsClass.</param>
        internal abstract void Load(Skyline.DataMiner.Net.Messages.ElementInfoEventMessage elementInfo);
    }

    /// <summary>
    /// DataMiner object interface.
    /// </summary>
    public interface IDmsObject
    {
    }

    /// <summary>
    /// Represents a DataMiner protocol.
    /// </summary>
    internal class DmsProtocol : Skyline.DataMiner.Library.Common.DmsObject, Skyline.DataMiner.Library.Common.IDmsProtocol
    {
        /// <summary>
        /// The constant value 'Production'.
        /// </summary>
        private const string Production = "Production";
        /// <summary>
        /// The protocol name.
        /// </summary>
        private string name;
        /// <summary>
        /// The protocol version.
        /// </summary>
        private string version;
        /// <summary>
        /// The protocol referenced version.
        /// </summary>
        private string referencedVersion = null;
        /// <summary>
        /// Whether the version is 'Production'.
        /// </summary>
        private bool isProduction;
        /// <summary>
        /// The connection info of the protocol.
        /// </summary>
        private System.Collections.Generic.IList<Skyline.DataMiner.Library.Common.IDmsConnectionInfo> connectionInfo = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.IDmsConnectionInfo>();
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsProtocol"/> class.
        /// </summary>
        /// <param name = "dms">The DataMiner System.</param>
        /// <param name = "name">The protocol name.</param>
        /// <param name = "version">The protocol version.</param>
        /// <param name = "referencedVersion">The protocol referenced version.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentNullException"><paramref name = "version"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "version"/> is the empty string ("") or white space.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "version"/> is not 'Production' and <paramref name = "referencedVersion"/> is not the empty string ("") or white space.</exception>
        internal DmsProtocol(Skyline.DataMiner.Library.Common.IDms dms, string name, string version, string referencedVersion = ""): base(dms)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (version == null)
            {
                throw new System.ArgumentNullException("version");
            }

            if (System.String.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "name");
            }

            if (System.String.IsNullOrWhiteSpace(version))
            {
                throw new System.ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "version");
            }

            this.name = name;
            this.version = version;
            this.isProduction = CheckIsProduction(this.version);
            if (!this.isProduction && !System.String.IsNullOrWhiteSpace(referencedVersion))
            {
                throw new System.ArgumentException("The version of the protocol is not referenced version of the protocol is not the empty string (\"\") or white space.", "referencedVersion");
            }

            this.referencedVersion = referencedVersion;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsProtocol"/> class.
        /// </summary>
        /// <param name = "dms">The DataMiner system.</param>
        /// <param name = "infoMessage">The information message received from SLNet.</param>
        /// <param name = "requestedProduction">The version requested to SLNet.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "infoMessage"/> is <see langword = "null"/>.</exception>
        internal DmsProtocol(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage infoMessage, bool requestedProduction): base(dms)
        {
            if (infoMessage == null)
            {
                throw new System.ArgumentNullException("infoMessage");
            }

            this.isProduction = requestedProduction;
            Parse(infoMessage);
        }

        /// <summary>
        /// Gets the protocol name.
        /// </summary>
        /// <value>The protocol name.</value>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        /// <value>The protocol version.</value>
        public string Version
        {
            get
            {
                return version;
            }
        }

        /// <summary>
        /// Gets the alarm template with the specified name defined for this protocol.
        /// </summary>
        /// <param name = "templateName">The name of the alarm template.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
        /// <exception cref = "AlarmTemplateNotFoundException">No alarm template with the specified name was found.</exception>
        /// <returns>The alarm template with the specified name defined for this protocol.</returns>
        public Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate GetAlarmTemplate(string templateName)
        {
            Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = this.Name, Version = this.Version, Template = templateName};
            Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage alarmTemplateEventMessage = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)dms.Communication.SendSingleResponseMessage(message);
            if (alarmTemplateEventMessage == null)
            {
                throw new Skyline.DataMiner.Library.Common.AlarmTemplateNotFoundException(templateName, this);
            }

            if (alarmTemplateEventMessage.Type == Skyline.DataMiner.Net.Messages.AlarmTemplateType.Template)
            {
                return new Skyline.DataMiner.Library.Common.Templates.DmsStandaloneAlarmTemplate(dms, alarmTemplateEventMessage);
            }
            else if (alarmTemplateEventMessage.Type == Skyline.DataMiner.Net.Messages.AlarmTemplateType.Group)
            {
                return new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroup(dms, alarmTemplateEventMessage);
            }
            else
            {
                throw new System.NotSupportedException("Support for " + alarmTemplateEventMessage.Type + " has not yet been implemented.");
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Protocol name: {0}, version: {1}", Name, Version);
        }

        /// <summary>
        /// Loads the object.
        /// </summary>
        /// <exception cref = "ProtocolNotFoundException">No protocol with the specified name and version exists in the DataMiner system.</exception>
        internal override void Load()
        {
            isProduction = CheckIsProduction(version);
            Skyline.DataMiner.Net.Messages.GetProtocolMessage getProtocolMessage = new Skyline.DataMiner.Net.Messages.GetProtocolMessage{Protocol = name, Version = version};
            Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage protocolInfo = (Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage)Communication.SendSingleResponseMessage(getProtocolMessage);
            if (protocolInfo != null)
            {
                Parse(protocolInfo);
            }
            else
            {
                throw new Skyline.DataMiner.Library.Common.ProtocolNotFoundException(name, version);
            }
        }

        /// <summary>
        /// Parses the <see cref = "GetProtocolInfoResponseMessage"/> message.
        /// </summary>
        /// <param name = "protocolInfo">The protocol information.</param>
        private void Parse(Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage protocolInfo)
        {
            IsLoaded = true;
            name = protocolInfo.Name;
            if (isProduction)
            {
                version = Production;
                referencedVersion = protocolInfo.Version;
            }
            else
            {
                version = protocolInfo.Version;
                referencedVersion = System.String.Empty;
            }

            ParseConnectionInfo(protocolInfo);
        }

        /// <summary>
        /// Parses the <see cref = "GetProtocolInfoResponseMessage"/> message.
        /// </summary>
        /// <param name = "protocolInfo">The protocol information.</param>
        private void ParseConnectionInfo(Skyline.DataMiner.Net.Messages.GetProtocolInfoResponseMessage protocolInfo)
        {
            System.Collections.Generic.List<Skyline.DataMiner.Library.Common.DmsConnectionInfo> info = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.DmsConnectionInfo>();
            info.Add(new Skyline.DataMiner.Library.Common.DmsConnectionInfo(System.String.Empty, Skyline.DataMiner.Library.Common.EnumMapper.ConvertStringToConnectionType(protocolInfo.Type)));
            if (protocolInfo.AdvancedTypes != null && protocolInfo.AdvancedTypes.Length > 0 && !System.String.IsNullOrWhiteSpace(protocolInfo.AdvancedTypes))
            {
                string[] split = protocolInfo.AdvancedTypes.Split(';');
                foreach (string part in split)
                {
                    if (part.Contains(":"))
                    {
                        string[] connectionSplit = part.Split(':');
                        Skyline.DataMiner.Library.Common.ConnectionType connectionType = Skyline.DataMiner.Library.Common.EnumMapper.ConvertStringToConnectionType(connectionSplit[0]);
                        string connectionName = connectionSplit[1];
                        info.Add(new Skyline.DataMiner.Library.Common.DmsConnectionInfo(connectionName, connectionType));
                    }
                    else
                    {
                        Skyline.DataMiner.Library.Common.ConnectionType connectionType = Skyline.DataMiner.Library.Common.EnumMapper.ConvertStringToConnectionType(part);
                        string connectionName = System.String.Empty;
                        info.Add(new Skyline.DataMiner.Library.Common.DmsConnectionInfo(connectionName, connectionType));
                    }
                }
            }

            connectionInfo = info.ToArray();
        }

        /// <summary>
        /// Validate if <paramref name = "version"/> is 'Production'.
        /// </summary>
        /// <param name = "version">The version.</param>
        /// <returns>Whether <paramref name = "version"/> is 'Production'.</returns>
        internal static bool CheckIsProduction(string version)
        {
            return System.String.Equals(version, Production, System.StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// DataMiner protocol interface.
    /// </summary>
    public interface IDmsProtocol : Skyline.DataMiner.Library.Common.IDmsObject
    {
        /// <summary>
        /// Gets the protocol name.
        /// </summary>
        /// <value>The protocol name.</value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the protocol version.
        /// </summary>
        /// <value>The protocol version.</value>
        string Version
        {
            get;
        }

        /// <summary>
        /// Gets the alarm template with the specified name defined for this protocol.
        /// </summary>
        /// <param name = "templateName">The name of the alarm template.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "templateName"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "templateName"/> is the empty string ("") or white space.</exception>
        /// <exception cref = "AlarmTemplateNotFoundException">No alarm template with the specified name was found.</exception>
        /// <returns>The alarm template with the specified name defined for this protocol.</returns>
        Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate GetAlarmTemplate(string templateName);
    }

    namespace Templates
    {
        /// <summary>
        /// Base class for standalone alarm templates and alarm template groups.
        /// </summary>
        internal abstract class DmsAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.DmsTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsAlarmTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "name">The name of the alarm template.</param>
            /// <param name = "protocol">Instance of the protocol.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            protected DmsAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.DmsProtocol protocol): base(dms, name, protocol)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsAlarmTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "name">The name of the alarm template.</param>
            /// <param name = "protocolName">The name of the protocol.</param>
            /// <param name = "protocolVersion">The version of the protocol.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocolName"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocolVersion"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "protocolName"/> is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "protocolVersion"/> is the empty string ("") or white space.</exception>
            protected DmsAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, string protocolName, string protocolVersion): base(dms, name, protocolName, protocolVersion)
            {
            }

            /// <summary>
            /// Loads all the data and properties found related to the alarm template.
            /// </summary>
            /// <exception cref = "TemplateNotFoundException">The template does not exist in the DataMiner system.</exception>
            internal override void Load()
            {
                Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage message = new Skyline.DataMiner.Net.Messages.GetAlarmTemplateMessage{AsOneObject = true, Protocol = Protocol.Name, Version = Protocol.Version, Template = Name};
                Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage response = (Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage)Dms.Communication.SendSingleResponseMessage(message);
                if (response != null)
                {
                    Parse(response);
                }
                else
                {
                    throw new Skyline.DataMiner.Library.Common.TemplateNotFoundException(Name, Protocol.Name, Protocol.Version);
                }
            }

            /// <summary>
            /// Parses the alarm template event message.
            /// </summary>
            /// <param name = "message">The message received from SLNet.</param>
            internal abstract void Parse(Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage message);
        }

        /// <summary>
        /// Represents an alarm template group.
        /// </summary>
        internal class DmsAlarmTemplateGroup : Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroup
        {
            /// <summary>
            /// The entries of the alarm group.
            /// </summary>
            private readonly System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry> entries = new System.Collections.Generic.List<Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry>();
            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsAlarmTemplateGroup"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "name">The name of the alarm template.</param>
            /// <param name = "protocol">The protocol this alarm template group corresponds with.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            internal DmsAlarmTemplateGroup(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.DmsProtocol protocol): base(dms, name, protocol)
            {
                IsLoaded = false;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsAlarmTemplateGroup"/> class.
            /// </summary>
            /// <param name = "dms">Instance of <see cref = "Dms"/>.</param>
            /// <param name = "alarmTemplateEventMessage">An instance of AlarmTemplateEventMessage.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "alarmTemplateEventMessage"/> is invalid.</exception>
            internal DmsAlarmTemplateGroup(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage alarmTemplateEventMessage): base(dms, alarmTemplateEventMessage.Name, alarmTemplateEventMessage.Protocol, alarmTemplateEventMessage.Version)
            {
                IsLoaded = true;
                foreach (Skyline.DataMiner.Net.Messages.AlarmTemplateGroupEntry entry in alarmTemplateEventMessage.GroupEntries)
                {
                    Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template = Protocol.GetAlarmTemplate(entry.Name);
                    entries.Add(new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroupEntry(template, entry.IsEnabled, entry.IsScheduled));
                }
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Template Group Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
            }

            /// <summary>
            /// Parses the alarm template event message.
            /// </summary>
            /// <param name = "message">The message received from the SLNet process.</param>
            internal override void Parse(Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage message)
            {
                IsLoaded = true;
                entries.Clear();
                foreach (Skyline.DataMiner.Net.Messages.AlarmTemplateGroupEntry entry in message.GroupEntries)
                {
                    Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template = Protocol.GetAlarmTemplate(entry.Name);
                    entries.Add(new Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplateGroupEntry(template, entry.IsEnabled, entry.IsScheduled));
                }
            }
        }

        /// <summary>
        /// Represents an alarm group entry.
        /// </summary>
        internal class DmsAlarmTemplateGroupEntry : Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplateGroupEntry
        {
            /// <summary>
            /// The template which is an entry of the alarm group.
            /// </summary>
            private readonly Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template;
            /// <summary>
            /// Specifies whether this entry is enabled.
            /// </summary>
            private readonly bool isEnabled;
            /// <summary>
            /// Specifies whether this entry is scheduled.
            /// </summary>
            private readonly bool isScheduled;
            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsAlarmTemplateGroupEntry"/> class.
            /// </summary>
            /// <param name = "template">The alarm template.</param>
            /// <param name = "isEnabled">Specifies if the entry is enabled.</param>
            /// <param name = "isScheduled">Specifies if the entry is scheduled.</param>
            internal DmsAlarmTemplateGroupEntry(Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate template, bool isEnabled, bool isScheduled)
            {
                if (template == null)
                {
                    throw new System.ArgumentNullException("template");
                }

                this.template = template;
                this.isEnabled = isEnabled;
                this.isScheduled = isScheduled;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Alarm template group entry:{0}", template.Name);
            }
        }

        /// <summary>
        /// Represents a standalone alarm template.
        /// </summary>
        internal class DmsStandaloneAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.DmsAlarmTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsStandaloneAlarmTemplate
        {
            /// <summary>
            /// The description of the alarm definition.
            /// </summary>
            private string description;
            /// <summary>
            /// Indicates whether this alarm template is used in a group.
            /// </summary>
            private bool isUsedInGroup;
            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsStandaloneAlarmTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "name">The name of the alarm template.</param>
            /// <param name = "protocol">The protocol this standalone alarm template corresponds with.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            internal DmsStandaloneAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.DmsProtocol protocol): base(dms, name, protocol)
            {
                IsLoaded = false;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsStandaloneAlarmTemplate"/> class.
            /// </summary>
            /// <param name = "dms">The DataMiner system reference.</param>
            /// <param name = "alarmTemplateEventMessage">An instance of AlarmTemplateEventMessage.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "dms"/> is invalid.</exception>
            internal DmsStandaloneAlarmTemplate(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage alarmTemplateEventMessage): base(dms, alarmTemplateEventMessage.Name, alarmTemplateEventMessage.Protocol, alarmTemplateEventMessage.Version)
            {
                IsLoaded = true;
                description = alarmTemplateEventMessage.Description;
                isUsedInGroup = alarmTemplateEventMessage.IsUsedInGroup;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Alarm Template Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
            }

            /// <summary>
            /// Parses the alarm template event message.
            /// </summary>
            /// <param name = "message">The message received from SLNet.</param>
            internal override void Parse(Skyline.DataMiner.Net.Messages.AlarmTemplateEventMessage message)
            {
                IsLoaded = true;
                description = message.Description;
                isUsedInGroup = message.IsUsedInGroup;
            }
        }

        /// <summary>
        /// Represents an alarm template.
        /// </summary>
        internal abstract class DmsTemplate : Skyline.DataMiner.Library.Common.DmsObject
        {
            /// <summary>
            /// Alarm template name.
            /// </summary>
            private readonly string name;
            /// <summary>
            /// The protocol this alarm template corresponds with.
            /// </summary>
            private readonly Skyline.DataMiner.Library.Common.DmsProtocol protocol;
            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "name">The name of the alarm template.</param>
            /// <param name = "protocol">Instance of the protocol.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocol"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            protected DmsTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.DmsProtocol protocol): base(dms)
            {
                if (name == null)
                {
                    throw new System.ArgumentNullException("name");
                }

                if (protocol == null)
                {
                    throw new System.ArgumentNullException("protocol");
                }

                if (System.String.IsNullOrWhiteSpace(name))
                {
                    throw new System.ArgumentException("The name of the template is the empty string (\"\") or white space.");
                }

                this.name = name;
                this.protocol = protocol;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsTemplate"/> class.
            /// </summary>
            /// <param name = "dms">The DataMiner System reference.</param>
            /// <param name = "name">The template name.</param>
            /// <param name = "protocolName">The name of the protocol.</param>
            /// <param name = "protocolVersion">The version of the protocol.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "name"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocolName"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException"><paramref name = "protocolVersion"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "protocolName"/> is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "protocolVersion"/> is the empty string ("") or white space.</exception>
            protected DmsTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, string protocolName, string protocolVersion): base(dms)
            {
                if (name == null)
                {
                    throw new System.ArgumentNullException("name");
                }

                if (protocolName == null)
                {
                    throw new System.ArgumentNullException("protocolName");
                }

                if (protocolVersion == null)
                {
                    throw new System.ArgumentNullException("protocolVersion");
                }

                if (System.String.IsNullOrWhiteSpace(name))
                {
                    throw new System.ArgumentException("The name of the template is the empty string(\"\") or white space.", "name");
                }

                if (System.String.IsNullOrWhiteSpace(protocolName))
                {
                    throw new System.ArgumentException("The name of the protocol is the empty string (\"\") or white space.", "protocolName");
                }

                if (System.String.IsNullOrWhiteSpace(protocolVersion))
                {
                    throw new System.ArgumentException("The version of the protocol is the empty string (\"\") or white space.", "protocolVersion");
                }

                this.name = name;
                protocol = new Skyline.DataMiner.Library.Common.DmsProtocol(dms, protocolName, protocolVersion);
            }

            /// <summary>
            /// Gets the template name.
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }

            /// <summary>
            /// Gets the protocol this template corresponds with.
            /// </summary>
            public Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
            {
                get
                {
                    return protocol;
                }
            }
        }

        /// <summary>
        /// Represents a trend template.
        /// </summary>
        internal class DmsTrendTemplate : Skyline.DataMiner.Library.Common.Templates.DmsTemplate, Skyline.DataMiner.Library.Common.Templates.IDmsTrendTemplate
        {
            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsTrendTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "name">The name of the alarm template.</param>
            /// <param name = "protocol">The instance of the protocol.</param>
            /// <exception cref = "ArgumentNullException">Dms is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">Name is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">Protocol is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException"><paramref name = "name"/> is the empty string ("") or white space.</exception>
            internal DmsTrendTemplate(Skyline.DataMiner.Library.Common.IDms dms, string name, Skyline.DataMiner.Library.Common.DmsProtocol protocol): base(dms, name, protocol)
            {
                IsLoaded = true;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsTrendTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "templateInfo">The template info received by SLNet.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">name is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">protocolName is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">protocolVersion is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException">name is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException">ProtocolName is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException">ProtocolVersion is the empty string ("") or white space.</exception>
            internal DmsTrendTemplate(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.GetTrendingTemplateInfoResponseMessage templateInfo): base(dms, templateInfo.Name, templateInfo.Protocol, templateInfo.Version)
            {
                IsLoaded = true;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref = "DmsTrendTemplate"/> class.
            /// </summary>
            /// <param name = "dms">Object implementing the <see cref = "IDms"/> interface.</param>
            /// <param name = "templateInfo">The template info received by SLNet.</param>
            /// <exception cref = "ArgumentNullException"><paramref name = "dms"/> is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">Name is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">ProtocolName is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentNullException">ProtocolVersion is <see langword = "null"/>.</exception>
            /// <exception cref = "ArgumentException">Name is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException">ProtocolName is the empty string ("") or white space.</exception>
            /// <exception cref = "ArgumentException">ProtocolVersion is the empty string ("") or white space.</exception>
            internal DmsTrendTemplate(Skyline.DataMiner.Library.Common.IDms dms, Skyline.DataMiner.Net.Messages.TrendTemplateMetaInfo templateInfo): base(dms, templateInfo.Name, templateInfo.ProtocolName, templateInfo.ProtocolVersion)
            {
                IsLoaded = true;
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Trend Template Name: {0}, Protocol Name: {1}, Protocol Version: {2}", Name, Protocol.Name, Protocol.Version);
            }

            /// <summary>
            /// Loads this object.
            /// </summary>
            internal override void Load()
            {
            }
        }

        /// <summary>
        /// DataMiner alarm template interface.
        /// </summary>
        public interface IDmsAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.IDmsTemplate
        {
        }

        /// <summary>
        /// DataMiner alarm template group interface.
        /// </summary>
        public interface IDmsAlarmTemplateGroup : Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate
        {
        }

        /// <summary>
        /// DataMiner alarm template group entry interface.
        /// </summary>
        public interface IDmsAlarmTemplateGroupEntry
        {
        }

        /// <summary>
        /// DataMiner standalone alarm template interface.
        /// </summary>
        public interface IDmsStandaloneAlarmTemplate : Skyline.DataMiner.Library.Common.Templates.IDmsAlarmTemplate
        {
        }

        /// <summary>
        /// DataMiner template interface.
        /// </summary>
        public interface IDmsTemplate : Skyline.DataMiner.Library.Common.IDmsObject
        {
            /// <summary>
            /// Gets the template name.
            /// </summary>
            string Name
            {
                get;
            }

            /// <summary>
            /// Gets the protocol this template corresponds with.
            /// </summary>
            Skyline.DataMiner.Library.Common.IDmsProtocol Protocol
            {
                get;
            }
        }

        /// <summary>
        /// DataMiner trend template interface.
        /// </summary>
        public interface IDmsTrendTemplate : Skyline.DataMiner.Library.Common.Templates.IDmsTemplate
        {
        }
    }

    /// <summary>
    /// Base class for parameters.
    /// </summary>
    /// <typeparam name = "T">The parameter type.</typeparam>
    internal class DmsParameter<T>
    {
        /// <summary>
        /// Setter delegates.
        /// </summary>
        private static readonly System.Collections.Generic.Dictionary<System.Type, System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool>> Setters = new System.Collections.Generic.Dictionary<System.Type, System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool>>{{typeof(string), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool>(AddStringValueToSetParameterMessage)}, {typeof(int? ), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool>(AddNullableIntValueToSetParameterMessage)}, {typeof(double? ), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool>(AddNullableDoubleValueToSetParameterMessage)}, {typeof(System.DateTime? ), new System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool>(AddNullableDateTimeValueToSetParameterMessage)}};
        /// <summary>
        /// The parameter ID.
        /// </summary>
        private readonly int id;
        /// <summary>
        /// The type of the parameter.
        /// </summary>
        /// <remarks>Currently supported types: int?, double?, string, DateTime?.</remarks>
        private readonly System.Type type;
        /// <summary>
        /// The underlying type (in case of Nullable&lt;T&gt;).
        /// </summary>
        private readonly System.Type underlyingType;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsParameter{T}"/> class.
        /// </summary>
        /// <param name = "id">The parameter ID.</param>
        /// <exception cref = "ArgumentException"><paramref name = "id"/> is invalid.</exception>
        protected DmsParameter(int id)
        {
            if (id < 0)
            {
                throw new System.ArgumentException("Invalid parameter ID", "id");
            }

            this.id = id;
            type = typeof(T);
            underlyingType = System.Nullable.GetUnderlyingType(type);
        }

        /// <summary>
        /// Gets the parameter ID.
        /// </summary>
        /// <value>The parameter ID.</value>
        public int Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Processes the response.
        /// </summary>
        /// <param name = "response">The response message.</param>
        /// <returns>The parameter value.</returns>
        internal T ProcessResponse(Skyline.DataMiner.Net.Messages.GetParameterResponseMessage response)
        {
            object interopValue = response.Value.InteropValue;
            T value = underlyingType == null ? ProcessResponseNonNullable(interopValue) : ProcessResponseNullable(interopValue);
            return value;
        }

        /// <summary>
        /// Adds the value to set to the SetParameterMessage.
        /// </summary>
        /// <param name = "message">The message to update with the parameter value to set.</param>
        /// <param name = "value">The parameter value to set.</param>
        /// <returns>Whether the SetParameterMessage needs to be sent.</returns>
        protected bool AddValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
        {
            System.Func<Skyline.DataMiner.Net.Messages.SetParameterMessage, T, bool> setter;
            if (Setters.TryGetValue(type, out setter))
            {
                return setter(message, value);
            }
            else
            {
                throw new System.NotSupportedException("Type " + typeof(T) + " is not supported.");
            }
        }

        /// <summary>
        /// Adds a nullable DateTime value to the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "value">The value.</param>
        /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
        private static bool AddNullableDateTimeValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
        {
            bool executeSet = true;
            if (!value.Equals(default(T)))
            {
                System.DateTime valueToSet = (System.DateTime)System.Convert.ChangeType(value, typeof(System.DateTime), System.Globalization.CultureInfo.CurrentCulture);
                message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue(valueToSet);
            }
            else
            {
                executeSet = false;
            }

            return executeSet;
        }

        /// <summary>
        /// Adds a nullable double value to the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "value">The value.</param>
        /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
        private static bool AddNullableDoubleValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
        {
            bool executeSet = true;
            if (!value.Equals(default(T)))
            {
                double valueToSet = (double)System.Convert.ChangeType(value, typeof(double), System.Globalization.CultureInfo.CurrentCulture);
                message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue(valueToSet);
            }
            else
            {
                executeSet = false;
            }

            return executeSet;
        }

        /// <summary>
        /// Adds a nullable int value to the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "value">The string value.</param>
        /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
        private static bool AddNullableIntValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
        {
            bool executeSet = true;
            if (!value.Equals(default(T)))
            {
                int valueToSet = (int)System.Convert.ChangeType(value, typeof(int), System.Globalization.CultureInfo.CurrentCulture);
                message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue(valueToSet);
            }
            else
            {
                executeSet = false;
            }

            return executeSet;
        }

        /// <summary>
        /// Adds a string value to the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "value">The string value.</param>
        /// <returns><c>true</c> if the set message needs to be executed; otherwise, <c>false</c>.</returns>
        private static bool AddStringValueToSetParameterMessage(Skyline.DataMiner.Net.Messages.SetParameterMessage message, T value)
        {
            message.Value = new Skyline.DataMiner.Net.Messages.ParameterValue((string)System.Convert.ChangeType(value, typeof(string), System.Globalization.CultureInfo.CurrentCulture));
            return true;
        }

        /// <summary>
        /// Processes the response for a non-nullable type.
        /// </summary>
        /// <param name = "interopValue">The value.</param>
        /// <returns>The parameter value.</returns>
        private T ProcessResponseNonNullable(object interopValue)
        {
            return Skyline.DataMiner.Library.Common.SLNetHelper.SLNetUtility.ProcessResponseNonNullable<T>(interopValue, type);
        }

        /// <summary>
        /// Processes the response for a nullable type.
        /// </summary>
        /// <param name = "interopValue">The value.</param>
        /// <returns>The parameter value.</returns>
        private T ProcessResponseNullable(object interopValue)
        {
            return Skyline.DataMiner.Library.Common.SLNetHelper.SLNetUtility.ProcessResponseNullable<T>(interopValue, underlyingType);
        }
    }

    /// <summary>
    /// Represents a standalone parameter.
    /// </summary>
    /// <typeparam name = "T">The type of the standalone parameter.</typeparam>
    /// <remarks>
    /// In case T equals int?, double? or DateTime?, extension methods are available. Refer to <see 
    ///cref = "ExtensionsIDmsStandaloneParameter"/> for more information.
    /// </remarks>
    internal class DmsStandaloneParameter<T> : Skyline.DataMiner.Library.Common.DmsParameter<T>, Skyline.DataMiner.Library.Common.IDmsStandaloneParameter<T>
    {
        /// <summary>
        /// The element this parameter is part of.
        /// </summary>
        private readonly Skyline.DataMiner.Library.Common.IDmsElement element;
        /// <summary>
        /// Initializes a new instance of the <see cref = "DmsStandaloneParameter{T}"/> class.
        /// </summary>
        /// <param name = "element">The element that the parameter belongs to.</param>
        /// <param name = "id">The ID of the parameter.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "element"/> is <see langword = "null"/>.</exception>
        /// <exception cref = "ArgumentException"><paramref name = "id"/> is invalid.</exception>
        internal DmsStandaloneParameter(Skyline.DataMiner.Library.Common.IDmsElement element, int id): base(id)
        {
            if (element == null)
            {
                throw new System.ArgumentNullException("element");
            }

            this.element = element;
        }

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <exception cref = "ParameterNotFoundException">The parameter was not found.</exception>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        /// <exception cref = "ElementNotFoundException">
        /// The element was not found in the DataMiner System.
        /// </exception>
        /// <returns>The parameter value.</returns>
        public T GetValue()
        {
            var response = SendGetParameterMessage();
            T value = ProcessResponse(response);
            return value;
        }

        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        /// <exception cref = "ElementNotFoundException">
        /// The element was not found in the DataMiner System.
        /// </exception>
        public void SetValue(T value)
        {
            Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(element);
            Skyline.DataMiner.Net.Messages.SetParameterMessage message = new Skyline.DataMiner.Net.Messages.SetParameterMessage{DataMinerID = element.DmsElementId.AgentId, ElId = element.DmsElementId.ElementId, ParameterId = Id, DisableInformationEventMessage = true};
            if (AddValueToSetParameterMessage(message, value))
            {
                element.Host.Dms.Communication.SendMessage(message);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return System.String.Format(System.Globalization.CultureInfo.InvariantCulture, "Standalone Parameter:{0}", Id);
        }

        /// <summary>
        /// Sends a <see cref = "GetParameterMessage"/> SLNet message.
        /// </summary>
        /// <exception cref = "ParameterNotFoundException">The parameter was not found.</exception>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        /// <exception cref = "ElementNotFoundException">
        /// The element was not found in the DataMiner System.
        /// </exception>
        /// <returns>The response message.</returns>
        private Skyline.DataMiner.Net.Messages.GetParameterResponseMessage SendGetParameterMessage()
        {
            Skyline.DataMiner.Library.Common.HelperClass.CheckElementState(element);
            try
            {
                var message = new Skyline.DataMiner.Net.Messages.GetParameterMessage{DataMinerID = element.DmsElementId.AgentId, ElId = element.DmsElementId.ElementId, ParameterId = Id, };
                var response = (Skyline.DataMiner.Net.Messages.GetParameterResponseMessage)element.Host.Dms.Communication.SendSingleResponseMessage(message);
                return response;
            }
            catch (Skyline.DataMiner.Net.Exceptions.DataMinerException e)
            {
                if (e.ErrorCode == -2147220935)
                {
                    // 0x80040239, SL_FAILED_NOT_FOUND, The object or file was not found.
                    throw new Skyline.DataMiner.Library.Common.ParameterNotFoundException(Id, element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147024891 && e.Message == "No such element.")
                {
                    // 0x80070005: Access is denied.
                    throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(element.DmsElementId, e);
                }
                else if (e.ErrorCode == -2147220916)
                {
                    // 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
                    throw new Skyline.DataMiner.Library.Common.ElementNotFoundException(element.DmsElementId, e);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// DataMiner standalone parameter interface.
    /// </summary>
    public interface IDmsStandaloneParameter
    {
        /// <summary>
        /// Gets the ID of this parameter.
        /// </summary>
        /// <value>The ID of this parameter.</value>
        int Id
        {
            get;
        }
    }

    /// <summary>
    /// DataMiner standalone parameter interface for a parameter of a specific type.
    /// </summary>
    /// <typeparam name = "T">The type of the standalone parameter.</typeparam>
    public interface IDmsStandaloneParameter<T> : Skyline.DataMiner.Library.Common.IDmsStandaloneParameter
    {
        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <exception cref = "ParameterNotFoundException">The parameter was not found.</exception>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
        /// <returns>The parameter value.</returns>
        T GetValue();
        /// <summary>
        /// Sets the value of this parameter.
        /// </summary>
        /// <param name = "value">The value to set.</param>
        /// <exception cref = "ElementStoppedException">The element is stopped.</exception>
        /// <exception cref = "ElementNotFoundException">The element was not found in the DataMiner System.</exception>
        void SetValue(T value);
    }

    /// <summary>
    /// Defines extension methods on the <see cref = "IConnection"/> class.
    /// </summary>
    ///
    [Skyline.DataMiner.Library.Common.Attributes.DllImport("SLNetTypes.dll")]
    public static class ConnectionInterfaceExtensions
    {
        /// <summary>
        /// Gets an object implementing the <see cref = "IDms"/> interface using an object that implements the  <see cref = "IConnection"/> class.
        /// </summary>
        /// <param name = "connection">The connection interface.</param>
        /// <exception cref = "ArgumentNullException"><paramref name = "connection"/> is <see langword = "null"/>.</exception>
        /// <returns>Object implementing the <see cref = "IDms"/> interface.</returns>
        public static Skyline.DataMiner.Library.Common.IDms GetDms(this Skyline.DataMiner.Net.IConnection connection)
        {
            if (connection == null)
            {
                throw new System.ArgumentNullException("connection");
            }

            return new Skyline.DataMiner.Library.Common.Dms(new Skyline.DataMiner.Library.Common.ConnectionCommunication(connection));
        }
    }

    internal static class Logger
    {
        private const long SizeLimit = 3 * 1024 * 1024;
        private const string LogFileName = @"C:\Skyline DataMiner\logging\ClassLibrary.txt";
        private const string LogPositionPlaceholder = "**********";
        private const int PlaceHolderSize = 10;
        private static long logPositionPlaceholderStart = -1;
        private static System.Threading.Mutex loggerMutex = new System.Threading.Mutex(false, "clpMutex");
        public static void Log(string message)
        {
            try
            {
                loggerMutex.WaitOne();
                string logPrefix = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "|";
                long messageByteCount = System.Text.Encoding.UTF8.GetByteCount(message);
                // Safeguard for large messages.
                if (messageByteCount > SizeLimit)
                {
                    message = "WARNING: message \"" + message.Substring(0, 100) + " not logged as it is too large (over " + SizeLimit + " bytes).";
                }

                long limit = SizeLimit / 2; // Safeguard: limit messages. If safeguard removed, the limit would be: SizeLimit - placeholder size - prefix length - 4 (2 * CR LF).
                if (messageByteCount > limit)
                {
                    long overhead = messageByteCount - limit;
                    int partToRemove = (int)overhead / 4; // In worst case, each char takes 4 bytes.
                    if (partToRemove == 0)
                    {
                        partToRemove = 1;
                    }

                    while (messageByteCount > limit)
                    {
                        message = message.Substring(0, message.Length - partToRemove);
                        messageByteCount = System.Text.Encoding.UTF8.GetByteCount(message);
                    }
                }

                int byteCount = System.Text.Encoding.UTF8.GetByteCount(message);
                long positionOfPlaceHolder = GetPlaceHolderPosition();
                System.IO.Stream fileStream = null;
                try
                {
                    fileStream = new System.IO.FileStream(LogFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream))
                    {
                        fileStream = null;
                        if (positionOfPlaceHolder == -1)
                        {
                            sw.BaseStream.Position = 0;
                            sw.Write(logPrefix);
                            sw.WriteLine(message);
                            logPositionPlaceholderStart = byteCount + logPrefix.Length;
                            sw.WriteLine(LogPositionPlaceholder);
                        }
                        else
                        {
                            sw.BaseStream.Position = positionOfPlaceHolder;
                            if (positionOfPlaceHolder + byteCount + 4 + PlaceHolderSize > SizeLimit)
                            {
                                // Overwrite previous placeholder.
                                byte[] placeholder = System.Text.Encoding.UTF8.GetBytes("          ");
                                sw.BaseStream.Write(placeholder, 0, placeholder.Length);
                                sw.BaseStream.Position = 0;
                            }

                            sw.Write(logPrefix);
                            sw.WriteLine(message);
                            sw.Flush();
                            logPositionPlaceholderStart = sw.BaseStream.Position;
                            sw.WriteLine(LogPositionPlaceholder);
                        }
                    }
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                    }
                }
            }
            catch
            {
            // Do nothing.
            }
            finally
            {
                loggerMutex.ReleaseMutex();
            }
        }

        private static long SetToStartOfLine(System.IO.StreamReader streamReader, long startPosition)
        {
            System.IO.Stream stream = streamReader.BaseStream;
            for (long position = startPosition - 1; position > 0; position--)
            {
                stream.Position = position;
                if (stream.ReadByte() == '\n')
                {
                    return position + 1;
                }
            }

            return 0;
        }

        private static long GetPlaceHolderPosition()
        {
            long result = -1;
            System.IO.Stream fileStream = null;
            try
            {
                fileStream = System.IO.File.Open(LogFileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream))
                {
                    fileStream = null;
                    streamReader.DiscardBufferedData();
                    long startOfLinePosition = SetToStartOfLine(streamReader, logPositionPlaceholderStart);
                    streamReader.DiscardBufferedData();
                    streamReader.BaseStream.Position = startOfLinePosition;
                    string line;
                    long postionInFile = startOfLinePosition;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line == LogPositionPlaceholder)
                        {
                            streamReader.DiscardBufferedData();
                            result = postionInFile;
                            break;
                        }
                        else
                        {
                            postionInFile = postionInFile + System.Text.Encoding.UTF8.GetByteCount(line) + 2;
                        }
                    }

                    // If this point is reached, it means the placeholder was still not found.
                    if (result == -1 && startOfLinePosition > 0)
                    {
                        streamReader.DiscardBufferedData();
                        streamReader.BaseStream.Position = 0;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            if (line == LogPositionPlaceholder)
                            {
                                streamReader.DiscardBufferedData();
                                result = streamReader.BaseStream.Position - PlaceHolderSize - 2;
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }

            return result;
        }
    }

    namespace SLNetHelper
    {
        internal static class SLNetUtility
        {
            internal static T ProcessResponseNonNullable<T>(object interopValue, System.Type type)
            {
                T value;
                if (type == typeof(System.DateTime))
                {
                    if (interopValue == null)
                    {
                        value = default(T);
                    }
                    else
                    {
                        double oleAutomationDate = System.Convert.ToDouble(interopValue, System.Globalization.CultureInfo.CurrentCulture);
                        value = (T)System.Convert.ChangeType(System.DateTime.FromOADate(oleAutomationDate), type, System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    value = interopValue == null ? default(T) : (T)System.Convert.ChangeType(interopValue, type, System.Globalization.CultureInfo.CurrentCulture);
                }

                return value;
            }

            internal static T ProcessResponseNullable<T>(object interopValue, System.Type underlyingType)
            {
                T value;
                // Nullable type.
                if (underlyingType == typeof(System.DateTime))
                {
                    if (interopValue == null)
                    {
                        value = default(T);
                    }
                    else
                    {
                        double oleAutomationDate = System.Convert.ToDouble(interopValue, System.Globalization.CultureInfo.CurrentCulture);
                        System.DateTime dateTime = System.DateTime.FromOADate(oleAutomationDate);
                        value = (T)System.Convert.ChangeType(dateTime, underlyingType, System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
                else
                {
                    value = interopValue == null ? default(T) : (T)System.Convert.ChangeType(interopValue, underlyingType, System.Globalization.CultureInfo.CurrentCulture);
                }

                return value;
            }
#pragma warning restore S4018 // Generic methods should provide type parameters

#pragma warning restore S3242 // Method parameters should be declared with base types

        }
    }
}