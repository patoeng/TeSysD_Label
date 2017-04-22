'TCP/IP communications to a Modbus based PLC.

'Intended to be of general purpose, used without specific implementations.

Imports System.Net
Imports System.Net.Sockets

Imports System.Text

Public Class PLC2ModbusComms

    Public ipPLC As String = "192.168.178.51"

    Private Const c_PLCPort As Integer = 502
    Private plcIPAddress As String
    Private plcIPPort As Integer = 502
    Private plcIPTimeout As Integer
    Private plcIPNoDelay As Boolean

    Private localBuffer As List(Of Byte)
    Private firstMWAtIndex As Integer

    Private errMsg As String

    Private _sck As Socket

    Public ReadOnly Property IsConnected As Boolean
        Get
            If _sck IsNot Nothing Then Return _sck.Connected

            Return False

        End Get
    End Property

    Public Property BytesSent As Integer
    Public Property BytesRecv As Integer

    ''' <summary>
    ''' Constructors.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

        plcIPTimeout = 4000
        plcIPNoDelay = True

        localBuffer = New List(Of Byte)

    End Sub

    Public Sub New(ByVal ip As String, ByVal port As Integer)
        Me.New()

        plcIPAddress = ip
        plcIPPort = port

    End Sub

    ''' <summary>
    ''' Properties.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ErrorMessage() As String
        Get
            Return errMsg
        End Get
    End Property

    Public Property HostIP() As String
        Get
            Return ipPLC
        End Get
        Set(ByVal value As String)
            plcIPAddress = value
        End Set
    End Property

    Public Property Port() As Integer
        Get
            Return plcIPPort
        End Get
        Set(ByVal value As Integer)
            plcIPPort = value
        End Set
    End Property

    Public Property Timeout() As Integer
        Get
            Return plcIPTimeout
        End Get
        Set(ByVal value As Integer)
            plcIPTimeout = value
        End Set
    End Property

    'Creates and opens a connection to the PLC.
    Public Sub Open()
        'Dim b(0) As Byte

        'Check socket status. Discard any existing connection.
        If _sck IsNot Nothing Then
            'Not invoking .Shutdown as we're creating a new connection, ignoring existing data.
            If _sck.Connected Then _sck.Close()
        End If

        _sck = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        _sck.NoDelay = plcIPNoDelay
        _sck.SendTimeout = plcIPTimeout
        _sck.ReceiveTimeout = plcIPTimeout

        _sck.Connect(IPAddress.Parse(ipPLC), plcIPPort)

        'Update Connected status.
        'Try
        '    _sck.Blocking = False
        '    _sck.Send(b, 0, 0)

        'Catch ex As SocketException

        'End Try
        '_sck.Blocking = True
    End Sub

    'Closes the connection to the PLC.
    Public Sub Close()
        If _sck IsNot Nothing Then
            If _sck.Connected Then
                _sck.Shutdown(SocketShutdown.Both)
            End If

            _sck.Close()
        End If
    End Sub

    ''' <summary>
    ''' Internal buffer holding PLC MWs always starts at 0. This Property allows the user to
    ''' retrieve the MW by its index as known on the PLC.
    ''' 
    ''' Modbus integer values are always in big endian format (257 = 01FF, little endian is FF01)
    ''' </summary>
    ''' <param name="index"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property GetMWAt(ByVal index As Integer) As Byte()
        Get
            Dim word(1) As Byte
            Dim offset As Integer

            offset = ((index - firstMWAtIndex) << 1) + 9

            If BitConverter.IsLittleEndian Then
                word(0) = localBuffer.Item(offset + 1)
                word(1) = localBuffer.Item(offset)
            Else
                word(0) = localBuffer.Item(offset)
                word(1) = localBuffer.Item(offset + 1)
            End If

            Return word

            'Return localBuffer.GetRange(((index - firstMWAtIndex) << 1) + 9, 2)
        End Get
    End Property

    Public ReadOnly Property GetMWAsIntAt(ByVal index As Integer) As UInteger
        Get
            Dim bytes(1) As Byte
            Dim offset As Integer

            offset = ((index - firstMWAtIndex) << 1) + 9

            If BitConverter.IsLittleEndian Then
                bytes(1) = localBuffer.Item(offset)
                bytes(0) = localBuffer.Item(offset + 1)
            Else
                bytes(0) = localBuffer.Item(offset)
                bytes(1) = localBuffer.Item(offset + 1)
            End If

            Return BitConverter.ToUInt16(bytes, 0)
        End Get
    End Property

    Public ReadOnly Property GetMWBitAt(ByVal index As Integer, ByVal bitIndex As Integer) As Boolean
        Get
            Return BitConverter.ToBoolean( _
                localBuffer.GetRange(((index - firstMWAtIndex) << 1) + 9, 2).ToArray(), _
                bitIndex)
        End Get
    End Property

    Public ReadOnly Property Buffer() As List(Of Byte)
        Get
            Return localBuffer
        End Get
    End Property

    ''' <summary>
    ''' Methods.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReadSingleRegister(ByVal index As Int32)

    End Sub

    'Possible bug: MWs in the PLC (at least for the Twido) are signed integers. The conversion done
    'in the code below may not handle negative values correctly.
    Public Sub ReadMultipleRegisters(ByVal index As Integer, ByVal count As Integer)
        Dim result As Integer

        Dim addrHi As Byte
        Dim addrLo As Byte
        Dim lenHi As Byte
        Dim lenLo As Byte

        Dim modbusQuery(11) As Byte
        Dim modbusBuffer(256) As Byte

        Dim timeStart As Long

        addrLo = CByte(index Mod 256)
        addrHi = CByte(index \ 256)

        lenLo = CByte(count Mod 256)
        lenHi = CByte(count \ 256)

        modbusQuery(0) = 0 'Transaction Identifier - High Byte.
        modbusQuery(1) = 0 'Low Byte.
        modbusQuery(2) = 0 'Protocol Identifier - High Byte.
        modbusQuery(3) = 0 'Low Byte.
        modbusQuery(4) = 0 'Length including Unit Identifier - High Byte.
        modbusQuery(5) = 6 'Low Byte.
        modbusQuery(6) = 1 'Unit Identifier.
        modbusQuery(7) = 3 'Modbus Function Code.
        modbusQuery(8) = addrHi
        modbusQuery(9) = addrLo
        modbusQuery(10) = lenHi
        modbusQuery(11) = lenLo

        result = 0

        'Try
        timeStart = Now.Ticks
        While True
            If _sck IsNot Nothing AndAlso _sck.Connected Then

                'Warning, ensure that modbusQuery contains no additional bytes. Check that the
                'declaration of modbusQuery is exactly the length required (in VB, an array
                'declared as byte(11) contains 12 bytes, not 11 as in C).
                BytesSent = _sck.Send(modbusQuery)
                BytesRecv = _sck.Receive(modbusBuffer)

                localBuffer.Clear()
                localBuffer.AddRange(modbusBuffer)

                firstMWAtIndex = index

                errMsg = "No errors."
                result = 1

                Exit While
            End If

            'Attempt to connect to the PLC.
            Open()

            'Stay in loop until timeout.
            If ((Now.Ticks - timeStart) / 10000) > plcIPTimeout Then Exit While
        End While

        'Catch e As SocketException
        'errMsg = e.ToString() & vbCrLf & e.ErrorCode

        'End Try

        'Return result

    End Sub

    'Possible bug: MWs in the PLC (at least for the Twido) are signed integers. The conversion done
    'in the code below may not handle negative values correctly.
    Public Sub ReadMultipleRegisters1(ByVal index As Integer, ByVal count As Integer)
        Dim result As Integer

        Dim addrHi As Byte
        Dim addrLo As Byte
        Dim lenHi As Byte
        Dim lenLo As Byte

        Dim modbusQuery(12) As Byte
        Dim modbusBuffer(256) As Byte

        Dim sentCount As Integer
        Dim recvCount As Integer

        Dim sck As Socket
        Dim ns As NetworkStream

        addrLo = CByte(index Mod 256)
        addrHi = CByte(index \ 256)

        lenLo = CByte(count Mod 256)
        lenHi = CByte(count \ 256)

        modbusQuery(0) = 0 'Transaction Identifier - High Byte.
        modbusQuery(1) = 0 'Low Byte.
        modbusQuery(2) = 0 'Protocol Identifier - High Byte.
        modbusQuery(3) = 0 'Low Byte.
        modbusQuery(4) = 0 'Length including Unit Identifier - High Byte.
        modbusQuery(5) = 6 'Low Byte.
        modbusQuery(6) = 1 'Unit Identifier.
        modbusQuery(7) = 3 'Modbus Function Code.
        modbusQuery(8) = addrHi
        modbusQuery(9) = addrLo
        modbusQuery(10) = lenHi
        modbusQuery(11) = lenLo

        sentCount = 0
        recvCount = 0

        result = 0

        'Try
        '''''''''''''''''''''''''''''''''''''''''''''''
        '' Connect and transceive.
        sck = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        sck.NoDelay = plcIPNoDelay
        sck.SendTimeout = plcIPTimeout
        sck.ReceiveTimeout = plcIPTimeout
        sck.Connect(IPAddress.Parse(ipPLC), plcIPPort)

        ns = New NetworkStream(sck)
        ns.Write(modbusQuery, 0, modbusQuery.Length)
        Me.BytesRecv = ns.Read(modbusBuffer, 0, modbusBuffer.Length)
        ns.Close()

        sck.Shutdown(SocketShutdown.Both)
        sck.Close()
        '''''''''''''''''''''''''''''''''''''''''''''''

        localBuffer.Clear()
        localBuffer.AddRange(modbusBuffer)

        firstMWAtIndex = index

        errMsg = "No errors."
        result = 1

        'Catch e As SocketException
        'errMsg = e.ToString() & vbCrLf & e.ErrorCode

        'End Try

        'Return result

    End Sub

    Public Sub WriteRegister(ByVal index As Int16, ByVal value() As Byte)
        Dim result As Integer

        Dim addrHi As Byte
        Dim addrLo As Byte

        Dim modbusQuery(11) As Byte
        Dim modbusBuffer(256) As Byte

        Dim sentCount As Integer
        Dim recvCount As Integer

        Dim timeStart As Long

        addrLo = CByte(index Mod 256)
        addrHi = CByte(index \ 256)

        modbusQuery(0) = 0 'Transaction Identifier - High Byte.
        modbusQuery(1) = 0 'Low Byte.
        modbusQuery(2) = 0 'Protocol Identifier - High Byte.
        modbusQuery(3) = 0 'Low Byte.
        modbusQuery(4) = 0 'Length including Unit Identifier - High Byte.
        modbusQuery(5) = 6 'Low Byte.
        modbusQuery(6) = 1 'Unit Identifier.
        modbusQuery(7) = 6 'Modbus Function Code.
        modbusQuery(8) = addrHi
        modbusQuery(9) = addrLo
        modbusQuery(10) = value(1)
        modbusQuery(11) = value(0)

        sentCount = 0
        recvCount = 0

        result = 0
        'Try
        '''''''''''''''''''''''''''''''''''''''''''''''
        '' Connect and transceive.
        timeStart = Now.Ticks
        While True
            If _sck IsNot Nothing AndAlso _sck.Connected Then
                BytesSent = _sck.Send(modbusQuery)
                BytesRecv = _sck.Receive(modbusBuffer)

                localBuffer.Clear()
                localBuffer.AddRange(modbusBuffer)

                errMsg = "No errors."
                result = 1

                Exit While
            End If

            'Attempt to connect to the PLC.
            Open()

            'Stay in loop until timeout.
            If ((Now.Ticks - timeStart) / 10000) > plcIPTimeout Then Exit While
        End While

        'Catch e As SocketException
        'errMsg = e.ToString()

        'End Try

        'Return result

    End Sub

    Public Sub WriteRegisterInt(ByVal index As Int16, ByVal value As Int16)
        Dim word(1) As Byte

        word(1) = CByte(value \ 256)
        word(0) = CByte(value Mod 256)

        WriteRegister(index, word)

    End Sub

    Public Sub WriteRegister_old(ByVal index As Int16, ByVal value() As Byte)
        Dim result As Integer

        Dim addrHi As Byte
        Dim addrLo As Byte

        Dim modbusQuery(12) As Byte
        Dim modbusBuffer(256) As Byte

        Dim sentCount As Integer
        Dim recvCount As Integer

        Dim sck As Socket
        Dim ns As NetworkStream

        addrLo = CByte(index Mod 256)
        addrHi = CByte(index \ 256)

        modbusQuery(0) = 0 'Transaction Identifier - High Byte.
        modbusQuery(1) = 0 'Low Byte.
        modbusQuery(2) = 0 'Protocol Identifier - High Byte.
        modbusQuery(3) = 0 'Low Byte.
        modbusQuery(4) = 0 'Length including Unit Identifier - High Byte.
        modbusQuery(5) = 6 'Low Byte.
        modbusQuery(6) = 1 'Unit Identifier.
        modbusQuery(7) = 6 'Modbus Function Code.
        modbusQuery(8) = addrHi
        modbusQuery(9) = addrLo
        modbusQuery(10) = value(1)
        modbusQuery(11) = value(0)

        sentCount = 0
        recvCount = 0

        result = 0
        'Try
        '''''''''''''''''''''''''''''''''''''''''''''''
        '' Connect and transceive.
        sck = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        sck.NoDelay = plcIPNoDelay
        sck.SendTimeout = plcIPTimeout
        sck.ReceiveTimeout = plcIPTimeout
        sck.Connect(IPAddress.Parse(ipPLC), plcIPPort)

        ns = New NetworkStream(sck)
        ns.Write(modbusQuery, 0, modbusQuery.Length)

        recvCount = ns.Read(modbusBuffer, 0, modbusBuffer.Length)
        ns.Close()

        sck.Shutdown(SocketShutdown.Both)
        sck.Close()
        '''''''''''''''''''''''''''''''''''''''''''''''

        localBuffer.Clear()
        localBuffer.AddRange(modbusBuffer)

        errMsg = "No errors."
        result = 1

        'Catch e As SocketException
        'errMsg = e.ToString()

        'End Try

        'Return result

    End Sub

    Public Sub WriteRegisterInt_old(ByVal index As Int16, ByVal value As Int16)
        Dim result As Integer

        Dim addrHi As Byte
        Dim addrLo As Byte
        Dim valHi As Byte
        Dim valLo As Byte

        Dim modbusQuery(12) As Byte
        Dim modbusBuffer(256) As Byte

        Dim sentCount As Integer
        Dim recvCount As Integer

        Dim sck As Socket
        Dim ns As NetworkStream

        addrLo = CByte(index Mod 256)
        addrHi = CByte(index \ 256)
        valLo = CByte(value Mod 256)
        valHi = CByte(value \ 256)

        modbusQuery(0) = 0 'Transaction Identifier - High Byte.
        modbusQuery(1) = 0 'Low Byte.
        modbusQuery(2) = 0 'Protocol Identifier - High Byte.
        modbusQuery(3) = 0 'Low Byte.
        modbusQuery(4) = 0 'Length including Unit Identifier - High Byte.
        modbusQuery(5) = 6 'Low Byte.
        modbusQuery(6) = 1 'Unit Identifier.
        modbusQuery(7) = 6 'Modbus Function Code.
        modbusQuery(8) = addrHi
        modbusQuery(9) = addrLo
        modbusQuery(10) = valHi
        modbusQuery(11) = valLo

        sentCount = 0
        recvCount = 0

        result = 0
        'Try
        '''''''''''''''''''''''''''''''''''''''''''''''
        '' Connect and transceive.
        sck = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        sck.NoDelay = plcIPNoDelay
        sck.SendTimeout = plcIPTimeout
        sck.ReceiveTimeout = plcIPTimeout
        sck.Connect(IPAddress.Parse(ipPLC), plcIPPort)

        ns = New NetworkStream(sck)
        ns.Write(modbusQuery, 0, modbusQuery.Length)

        recvCount = ns.Read(modbusBuffer, 0, modbusBuffer.Length)
        ns.Close()

        sck.Shutdown(SocketShutdown.Both)
        sck.Close()
        '''''''''''''''''''''''''''''''''''''''''''''''

        localBuffer.Clear()
        localBuffer.AddRange(modbusBuffer)

        errMsg = "No errors."
        result = 1

        'Catch e As SocketException
        'errMsg = e.ToString()

        'End Try

        'Return result

    End Sub

End Class
