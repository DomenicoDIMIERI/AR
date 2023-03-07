Imports FAXCOMEXLib

Namespace Drivers

    Public Structure FaxDevice

        Public ID As Integer
        Public DeviceName As String
        Public ProviderUniqueName As String
        Public PoweredOff As Boolean
        Public ReceivingNow As Boolean
        Public RingingNow As Boolean
        Public SendingNow As Boolean
        Public UsedRoutingMethods As String()


    End Structure

End Namespace