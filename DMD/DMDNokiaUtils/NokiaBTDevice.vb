Imports System.Runtime.InteropServices
Imports DMD.Nokia
Imports DMD.Nokia.APIS

Partial Class Nokia

    Public Enum DeviceBTStatus As Integer
        Unpaired = CONAPI_UNPAIR_DEVICE
        Paired = CONAPI_PAIR_DEVICE
        Trusted = CONAPI_SET_PCSUITE_TRUSTED
        NotTrusted = CONAPI_SET_PCSUITE_UNTRUSTED
    End Enum
 

    ''' <summary>
    ''' Rappresenta un telefono che è possibile raggiungere tramite l'interfaccia Bluetooth del PC
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NokiaBTDevice
        Friend strName As String
        Friend strAddress As String
        Friend iDeviceID As Integer
        Friend iStatus As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public ReadOnly Property Name As String
            Get
                Return Me.strName
            End Get
        End Property

        Public ReadOnly Property Address As String
            Get
                Return Me.strAddress
            End Get
        End Property

        Public ReadOnly Property DeviceID As Integer
            Get
                Return Me.iDeviceID
            End Get
        End Property

        Public ReadOnly Property Status As DeviceBTStatus
            Get
                Return CType(Me.iStatus, DeviceBTStatus)
            End Get
        End Property

        Public ReadOnly Property StatusEx As String
            Get
                Dim ret As String = ""
                If CONAPI_IS_DEVICE_UNPAIRED(Me.Status) <> 0 Then
                    ret &= "Unpaired, "
                ElseIf CONAPI_IS_DEVICE_PAIRED(Me.Status) <> 0 Then
                    ret &= "Paired, "
                End If

                If CONAPI_IS_PCSUITE_TRUSTED(Me.Status) <> 0 Then
                    ret &= "Trusted "
                Else
                    ret &= "Not trusted "
                End If

                Return ret
            End Get
        End Property

        Friend Sub FromInfo(val As CONAPI_CONNECTION_INFO)
            Me.strName = val.pstrDeviceName
            Me.strAddress = val.pstrAddress
            Me.iDeviceID = val.iDeviceID
            Me.iStatus = val.iState
        End Sub

        Public Sub SetTrustedState(ByVal state As DeviceBTStatus, ByVal password As String)
            Dim iRet As Integer = ECONA_UNKNOWN_ERROR

            iRet = CONAChangeDeviceTrustedState(m_hDMHandle, state, Me.strAddress, password, vbNullString)
            If iRet = CONA_OK Then
                ' change the device status to the list according to return value
                ' re-searching all devices would take too long time
                If (state And CONAPI_PAIR_DEVICE) <> 0 Then
                    Me.iStatus = ((Me.iStatus And CONAPI_DEVICE_PCSUITE_TRUSTED) Or CONAPI_DEVICE_PAIRED)
                ElseIf (state And CONAPI_UNPAIR_DEVICE) <> 0 Then
                    Me.iStatus = ((Me.iStatus And CONAPI_DEVICE_PCSUITE_TRUSTED) Or CONAPI_DEVICE_UNPAIRED)
                End If
                If (state And CONAPI_SET_PCSUITE_TRUSTED) <> 0 Then
                    Me.iStatus = (Me.iStatus Or CONAPI_DEVICE_PCSUITE_TRUSTED)
                ElseIf (state And CONAPI_SET_PCSUITE_UNTRUSTED) <> 0 Then
                    Me.iStatus = (Me.iStatus And Not CONAPI_DEVICE_PCSUITE_TRUSTED)
                End If
            Else
                ShowErrorMessage("CONAChangeDeviceTrustedState failed.", iRet)
            End If

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

End Class