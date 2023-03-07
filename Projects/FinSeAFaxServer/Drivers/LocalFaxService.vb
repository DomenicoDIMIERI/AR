Imports FAXCOMEXLib
Imports DMD.Sistema
Imports DMD

Namespace Drivers

    Public Class LocalFaxDriverConfiguration
        Inherits FaxDriverOptions
    End Class

    Public Class LocalFaxDriver
        Inherits DMD.Sistema.BaseFaxDriver

        Private faxSvr As FaxServer

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Local Fax Service"
            End Get
        End Property

        Public Overrides Function GetUniqueID() As String
            Return "Local Fax Service 1.0"
        End Function

       
        Protected Overrides Function InstantiateNewOptions() As DMD.Drivers.DriverOptions
            Return New LocalFaxDriverConfiguration
        End Function

#Region "Shared"

        ''' <summary>
        ''' Invia il fax e restituisce l'ID del lavoro
        ''' </summary>
        ''' <param name="documentName"></param>
        ''' <param name="body"></param>
        ''' <param name="subject"></param>
        ''' <param name="recipient"></param>
        ''' <param name="recipientName"></param>
        ''' <param name="attachFaxToRecipient"></param>
        ''' <param name="note"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SENDFAX(ByVal documentName As String, ByVal body As String, ByVal subject As String, ByVal recipient As String, ByVal recipientName As String, ByVal attachFaxToRecipient As Boolean, ByVal note As String) As String
            Dim faxDoc As New FaxDocument
            Dim JobID As Object



            faxDoc.Body = body
            faxDoc.DocumentName = documentName
            faxDoc.Recipients.Add(recipient)
            faxDoc.AttachFaxToReceipt = attachFaxToRecipient
            faxDoc.Note = note
            'faxDoc.Sender .
            faxDoc.Subject = subject
            JobID = faxDoc.ConnectedSubmit(faxSvr)
            'MsgBox("L'ID del lavoro inoltrato è:" & CStr(JobID, (0))

            Dim arr As Array = CType(JobID, Array)
            Return CStr(arr.GetValue(0))
        End Function

        Public Function REFRESHACTIVITIES() As FaxActivitiesInfo
            Dim objFaxServer As New FAXCOMEXLib.FaxServer
            Dim objFaxActivity As FAXCOMEXLib.FaxActivity
            Dim ret As New FaxActivitiesInfo

            objFaxActivity = objFaxServer.Activity
            ' Connect to the fax server. 
            ' "" defaults to the server on which the script is running.
            objFaxServer.Connect("")

            'Refresh the activity object
            objFaxActivity.Refresh()

            'Display the activity properties
            ret.IncomingMessages = objFaxActivity.IncomingMessages
            ret.OutgoingMessages = objFaxActivity.OutgoingMessages
            ret.RoutingMessages = objFaxActivity.RoutingMessages
            ret.QueuedMessages = objFaxActivity.QueuedMessages

            objFaxServer.Disconnect()

            Return ret
        End Function

        Public Sub SETFAXCONFIGURATION(ByVal config As FaxConfiguration)
            Dim objFaxServer As New FAXCOMEXLib.FaxServer
            Dim objFaxDevice As FAXCOMEXLib.FaxDevice

            'Connect to the fax server
            objFaxServer.Connect("")

            'Get the device
            objFaxDevice = objFaxServer.GetDevices.Item(1)

            'Set the device properties
            objFaxDevice.CSID = config.CSID
            objFaxDevice.Description = config.Description
            Select Case config.ReceiveMode
                Case FaxReceiveModeEnum.FDRM : objFaxDevice.ReceiveMode = FAXCOMEXLib.FAX_DEVICE_RECEIVE_MODE_ENUM.fdrmAUTO_ANSWER
            End Select
            objFaxDevice.RingsBeforeAnswer = config.RingsBeforeAnsware
            objFaxDevice.SendEnabled = config.SendEnabled
            objFaxDevice.TSID = config.TSID

            'Save the device configuration
            objFaxDevice.Save()

            objFaxServer.Disconnect()
        End Sub

        Public Function GETFAXDEVICES() As CCollection(Of FaxDevice)
            Dim objFaxServer As New FAXCOMEXLib.FaxServer
            Dim collFaxDevices As FAXCOMEXLib.FaxDevices
            Dim objFaxDevice As FAXCOMEXLib.FaxDevice
            Dim ret As New CCollection(Of FaxDevice)


            'Connect to the fax server
            objFaxServer.Connect("")

            collFaxDevices = objFaxServer.GetDevices
            'MsgBox("This server has " & collFaxDevices.Count & " fax devices.")

            'Dim I As Object
            For I As Integer = 1 To collFaxDevices.Count
                Dim item As New FaxDevice
                objFaxDevice = collFaxDevices.Item(I)
                'objFaxDevice = collFaxDevices.ItemById(ID)
                'Refresh the device information
                objFaxDevice.Refresh()
                item.ID = objFaxDevice.Id '
                item.DeviceName = objFaxDevice.DeviceName
                item.ProviderUniqueName = objFaxDevice.ProviderUniqueName
                item.PoweredOff = objFaxDevice.PoweredOff
                item.ReceivingNow = objFaxDevice.ReceivingNow
                item.RingingNow = objFaxDevice.RingingNow
                item.SendingNow = objFaxDevice.SendingNow
                'Display the routing methods for the device
                item.UsedRoutingMethods = DirectCast(objFaxDevice.UsedRoutingMethods, String())
                'Stop using the first routing method
                'objFaxDevice.UseRoutingMethod(RoutingMethods(0), False)
                'Save the change
                'objFaxDevice.Save()
                ret.Add(item)
            Next

            objFaxServer.Disconnect()

            Return ret
        End Function

#End Region

        
        Protected Overrides Sub InternalConnect()
            Me.faxSvr = New FaxServer
            faxSvr.Connect("") '"LAB1-STU15")
        End Sub

        Protected Overrides Sub InternalDisconnect()
            faxSvr.Disconnect()
        End Sub

        Protected Overrides Sub CancelJobInternal(jobID As String)

        End Sub

        Protected Overrides Sub InternalSend(job As FaxJob)
            Dim options As FaxDriverOptions = job.Options
            SENDFAX(options.DocumentName, options.FileName, options.Subject, options.TargetNumber, options.RecipientName, options.AttachFaxToRecipient, options.Comments)
        End Sub
    End Class

End Namespace