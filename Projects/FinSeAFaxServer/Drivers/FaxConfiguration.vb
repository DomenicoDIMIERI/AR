Imports FAXCOMEXLib

Namespace Drivers

    Public Enum FaxReceiveModeEnum As Integer
        FDRM
    End Enum

    Public Structure FaxConfiguration

        Public CSID As String
        Public Description As String
        Public ReceiveMode As FaxReceiveModeEnum
        Public RingsBeforeAnsware As Integer
        Public SendEnabled As Boolean
        Public TSID As String

    End Structure

End Namespace