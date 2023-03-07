Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports DIALTPLib
Imports LumiSoft.Media.Wave
Imports LumiSoft.Net.Codec

Public Enum InterfonoPayLoadType As Integer
    Handshake = 0
    AudioData = 1
    Disconnect = 255
End Enum

<Serializable>
Public Structure InterfonoPayLoad
    Public id As Integer
    Public time As DateTime
    Public type As InterfonoPayLoadType
    Public codec As Integer
    Public bufferSize As Integer
    Public buffer As Byte()
    Public crc As Integer
End Structure
