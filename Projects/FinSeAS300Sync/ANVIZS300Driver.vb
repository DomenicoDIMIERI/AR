Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Databases
Imports DMD.S300


Public Class ANVIZS300Driver
    Inherits DMD.Office.DriverRilevatorePresenze

    Public Sub New()

    End Sub

    Protected Overrides Sub SincronizzaOrario(rilevatore As Office.RilevatorePresenze)
        Dim dev As DMD.S300.S300Device = Nothing

        Try
            Dim deviceID As Integer = Formats.ToInteger(rilevatore.Parametri.GetItemByKey("DeviceID"))
            Dim address As String = Formats.ToString(rilevatore.Parametri.GetItemByKey("Address"))
            dev = DMD.S300.S300Devices.RegisterDevice(deviceID, address)
            If (dev Is Nothing) Then Throw New ArgumentNullException("Rilevatore non trovato")
            If (dev.IsConnected = False) Then dev.Start()
            dev.SetDeviceTime(Calendar.Now)

        Catch ex As Exception
            Throw
        Finally
            If (dev IsNot Nothing AndAlso dev.IsConnected) Then dev.Stop()
        End Try
    End Sub

    Public Overrides Function GetName() As String
        Return "ANVIZ S300 Device Driver"
    End Function

    Public Overrides Function GetVersion() As String
        Return "1.00"
    End Function

    Private Function GetAllMarcature(rilevatore As Office.RilevatorePresenze) As CCollection(Of MarcaturaIngressoUscita)
        Dim ret As New CCollection(Of MarcaturaIngressoUscita)
        If (GetID(rilevatore) = 0) Then Return ret

        Dim cursor As New MarcatureIngressoUscitaCursor
        cursor.Stato.Value = Databases.ObjectStatus.OBJECT_VALID
        cursor.IgnoreRights = True
        cursor.IDDispositivo.Value = GetID(rilevatore)
        While Not cursor.EOF
            ret.Add(cursor.Item)
            cursor.MoveNext()
        End While
        cursor.Dispose()

        ret.Sort()

        Return ret
    End Function

    Private Function DevToSav(ByVal rilevatore As RilevatorePresenze, ByVal devItem As S300Clocking) As MarcaturaIngressoUscita
        Dim savItem As New MarcaturaIngressoUscita
        savItem.Dispositivo = rilevatore
        savItem.Data = devItem.Time
        savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
        savItem.Stato = ObjectStatus.OBJECT_VALID
        savItem.Operatore = Me.MapOperatore(rilevatore, devItem)
        savItem.PuntoOperativo = rilevatore.PuntoOperativo
        savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
        savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
        Return savItem
    End Function

    Private Function CheckEquals(ByVal devItem As S300Clocking, ByVal savItem As MarcaturaIngressoUscita) As Boolean
        Dim ret As Boolean = (savItem.Data = devItem.Time)
        If (devItem.Type = S300ClockingType.In) Then
            ret = ret AndAlso savItem.Operazione = TipoMarcaturaIO.INGRESSO
        Else
            ret = ret AndAlso savItem.Operazione = TipoMarcaturaIO.USCITA
        End If

        'savItem.Operatore = Me.MapOperatore(rilevatore, devItem)
        'savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
        'savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
        ret = ret AndAlso Formats.ToInteger(savItem.Parametri.GetItemByKey("PersonID")) = devItem.PersonID

        Return ret
    End Function

    Protected Overrides Function ScaricaNuoveMarcature(rilevatore As Office.RilevatorePresenze) As CCollection(Of Office.MarcaturaIngressoUscita)
        Dim dev As DMD.S300.S300Device = Nothing
        Try
            If (Not Me.Supports(rilevatore)) Then Throw New NotSupportedException("Rilevatore non supportato")
            Dim deviceID As Integer = Formats.ToInteger(rilevatore.Parametri.GetItemByKey("DeviceID"))
            Dim address As String = Formats.ToString(rilevatore.Parametri.GetItemByKey("Address"))
            dev = DMD.S300.S300Devices.RegisterDevice(deviceID, address)
            If (dev Is Nothing) Then Throw New ArgumentNullException("Rilevatore non trovato")
            If (dev.IsConnected = False) Then dev.Start()

            Dim ret As New CCollection(Of MarcaturaIngressoUscita)

            Dim devItems As S300Clocking() = dev.GetAllClockings
            Dim devItem As S300Clocking
            Dim savItem As MarcaturaIngressoUscita

            If (devItems IsNot Nothing AndAlso devItems.Length > 0) Then
                Dim savedItems As CCollection(Of MarcaturaIngressoUscita) = Me.GetAllMarcature(rilevatore)
                Array.Sort(devItems)

                Dim i, j As Integer
                Dim toSave As Boolean
                i = 0 : j = 0
                While (i < devItems.Length AndAlso j < savedItems.Count)
                    devItem = devItems(i)
                    savItem = savedItems(j)
                    toSave = False
                    If (savItem.Data < devItem.Time) Then
                        j += 1
                    ElseIf (savItem.Data > devItem.Time) Then
                        toSave = True
                    Else
                        If Me.CheckEquals(devItem, savItem) Then
                            'Marcatura già registrata
                            i += 1 : j += 1
                        Else
                            toSave = True
                        End If
                    End If

                    If (toSave) Then
                        savItem = New MarcaturaIngressoUscita
                        savItem.Dispositivo = rilevatore
                        savItem.Data = devItem.Time
                        savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                        savItem.Stato = ObjectStatus.OBJECT_VALID
                        savItem.Operatore = Me.MapOperatore(rilevatore, devItem)
                        savItem.PuntoOperativo = rilevatore.PuntoOperativo
                        savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
                        savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
                        savItem.Save()
                        ret.Add(savItem)
                        i += 1
                    End If
                End While

                While (i < devItems.Length)
                    devItem = devItems(i)

                    savItem = New MarcaturaIngressoUscita
                    savItem.Dispositivo = rilevatore
                    savItem.Data = devItem.Time
                    savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                    savItem.Stato = ObjectStatus.OBJECT_VALID
                    savItem.Operatore = Me.MapOperatore(rilevatore, devItem)
                    savItem.PuntoOperativo = rilevatore.PuntoOperativo
                    savItem.Parametri.SetItemByKey("DeviceID", devItem.DeviceID)
                    savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
                    savItem.Save()
                    ret.Add(savItem)
                    i += 1
                End While
            End If

            Return ret
        Catch ex As Exception
            Throw
        Finally
            If (dev IsNot Nothing AndAlso dev.IsConnected) Then dev.Stop()
        End Try
    End Function

    Protected Overrides Function Supports(rilevatore As Office.RilevatorePresenze) As Boolean
        Return rilevatore.Tipo = "ANVIZ" AndAlso rilevatore.Modello = "S300"
    End Function

    Private Function MapOperatore(ByVal rilevatore As RilevatorePresenze, ByVal dev As S300Clocking) As CUser
        If (rilevatore.Parametri.ContainsKey("UserMapping")) Then
            Dim mappings As String() = Split(CStr(rilevatore.Parametri.GetItemByKey("UserMapping")), ";")
            If mappings Is Nothing OrElse mappings.Length <= 0 Then Return Nothing

            For Each mapping As String In mappings
                Dim piduname As String() = Split(mapping, ":")
                If (piduname.Length >= 2) Then
                    Dim pid As Integer = Formats.ToInteger(piduname(0))
                    Dim uname As String = Trim(piduname(1))
                    If (pid = dev.PersonID) Then
                        Return Sistema.Users.GetItemByName(uname)
                    End If
                End If
            Next

            Return Nothing
        End If

        Select Case dev.PersonID
            Case 1 : Return Sistema.Users.GetItemByName("admin")
            Case 202 : Return Sistema.Users.GetItemByName("OperatoreAC")
            Case 204 : Return Sistema.Users.GetItemByName("Francesca.Lomanto")
            Case 213 : Return Sistema.Users.GetItemByName("Vincenza.Novellino")
            Case 211 : Return Sistema.Users.GetItemByName("admin")
            Case 208 : Return Sistema.Users.GetItemByName("Giovanna.Biscotti")
            Case 216 : Return Sistema.Users.GetItemByName("Mina.Greco")
            Case 301 : Return Sistema.Users.GetItemByName("Annalisa.Ragone")
            Case 302 : Return Sistema.Users.GetItemByName("Lorenzo.Lepore")
            Case 303 : Return Sistema.Users.GetItemByName("Valentina.Vignola")
            Case 305 : Return Sistema.Users.GetItemByName("Marco.Voto")
            Case 401 : Return Sistema.Users.GetItemByName("OperatoreCV")
            Case 402 : Return Sistema.Users.GetItemByName("Rosa.Fiore")
            Case 403 : Return Sistema.Users.GetItemByName("Caterina.Donisi")
            Case 404 : Return Sistema.Users.GetItemByName("Raffaele.Tomasetta")
            Case 405 : Return Sistema.Users.GetItemByName("Katiuscia.Coppola")
            Case 406 : Return Sistema.Users.GetItemByName("Maurilio.Calandro")
            Case 502 : Return Sistema.Users.GetItemByName("Daniela.Iacicco")
            Case 503 : Return Sistema.Users.GetItemByName("Daniela.Sasso")
            Case 504 : Return Sistema.Users.GetItemByName("OperatoreE")
            Case 506 : Return Sistema.Users.GetItemByName("Maurilio.Calandro")
            Case Else : Return Nothing
        End Select

    End Function


End Class
