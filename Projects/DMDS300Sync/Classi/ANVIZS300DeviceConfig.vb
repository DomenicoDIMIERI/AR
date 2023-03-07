Imports Microsoft.VisualBasic
Imports DMD
Imports DMD.Sistema
Imports DMD.Anagrafica
Imports DMD.Office
Imports DMD.Databases
Imports DMD.S300
Imports DMD.XML

<Serializable>
Public Class ANVIZS300DeviceConfig
    Inherits DMDObject
    Implements DMD.XML.IDMDXMLSerializable

    Public Property Nome As String
    Public Property Indirizzo As String
    Public Property DeviceID As Integer
    Public Property SincronizzaOgni As Integer
    Public Property Mapping As String
    Private m_Device As S300.S300Device


    Public Sub New()

    End Sub

    Public Sub XMLSerialize(writer As XMLWriter) Implements IDMDXMLSerializable.XMLSerialize
        writer.WriteAttribute("Nome", Me.Nome)
        writer.WriteAttribute("Indirizzo", Me.Indirizzo)
        writer.WriteAttribute("DeviceID", Me.DeviceID)
        writer.WriteAttribute("SincronizzaOgni", Me.SincronizzaOgni)
        writer.WriteTag("Mapping", Me.Mapping)
    End Sub

    Public Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements IDMDXMLSerializable.SetFieldInternal
        Select Case fieldName
            Case "Nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "Indirizzo" : Me.Indirizzo = XML.Utils.Serializer.DeserializeString(fieldValue)
            Case "DeviceID" : Me.DeviceID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "SincronizzaOgni" : Me.SincronizzaOgni = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            Case "Mapping" : Me.Mapping = XML.Utils.Serializer.DeserializeString(fieldValue)
        End Select
    End Sub

    Public Function GetDevice() As S300.S300Device
        If (Me.m_Device Is Nothing) Then
            Me.m_Device = DMD.S300.S300Devices.RegisterDevice(Me.DeviceID, Me.Indirizzo)
        End If
        Return Me.m_Device
    End Function

    Public Sub UnregisterDevice()
        If Me.m_Device Is Nothing Then Return
        DMD.S300.S300Devices.UnregisterDevice(Me.m_Device)
        Me.m_Device = Nothing
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Nome
    End Function


    Public Sub SincronizzaOrologio()
        Dim dev As S300.S300Device = Me.GetDevice
        If (Not dev.IsConnected) Then dev.Start()
        dev.SetDeviceTime(Now)
        Me.Log("Sincronizzazione Ora sul Dispositivo " & dev.DeviceID & " : " & dev.GetDeviceTime().ToString)
        dev.Stop()
    End Sub

    Private Sub Log(ByVal text As String)
        DMD.Sistema.ApplicationContext.Log(text)
    End Sub

    Public Function MapOperatore(ByVal dev As S300Clocking) As String
        Dim mappings As String() = Split(Me.Mapping, ";")
        If mappings Is Nothing OrElse mappings.Length <= 0 Then Return Nothing
        For Each mapping As String In mappings
            Dim piduname As String() = Split(mapping, ":")
            If (piduname.Length >= 2) Then
                Dim pid As Integer = Formats.ToInteger(piduname(0))
                Dim uname As String = Trim(piduname(1))
                If (pid = dev.PersonID) Then
                    Return uname
                End If
            End If
        Next

        Return dev.PersonID
    End Function

    Public Function GetAllMarcature() As CCollection(Of MarcaturaIngressoUscita)
        Dim ret As New CCollection(Of MarcaturaIngressoUscita)

        Dim dbSQL As String = "SELECT * FROM [tbl_OfficeUserIO] WHERE [Stato]=1"
        Dim dbRis As System.Data.IDataReader = APPConn.ExecuteReader(dbSQL)
        While dbRis.Read
            Dim item As New MarcaturaIngressoUscita
            APPConn.Load(item, dbRis)
            ret.Add(item)
        End While
        dbRis.Dispose()
        ret.Sort()

        Return ret
    End Function


    Private Function CheckEquals(ByVal devItem As S300Clocking, ByVal savItem As MarcaturaIngressoUscita) As Boolean
        Dim ret As Boolean = (savItem.Data = devItem.Time)
        If (devItem.Type = S300ClockingType.In) Then
            ret = ret AndAlso savItem.Operazione = TipoMarcaturaIO.INGRESSO
        Else
            ret = ret AndAlso savItem.Operazione = TipoMarcaturaIO.USCITA
        End If

        'savItem.Operatore = Me.MapOperatore(rilevatore, devItem)
        ret = ret AndAlso Formats.ToInteger(savItem.Parametri.GetItemByKey("DeviceID")) = devItem.DeviceID
        ret = ret AndAlso Formats.ToInteger(savItem.Parametri.GetItemByKey("PersonID")) = devItem.PersonID

        Return ret
    End Function

    Public Function ScaricaNuoveMarcature() As CCollection(Of Office.MarcaturaIngressoUscita)
        Dim dev As S300.S300Device = Nothing

#If Not DEBUG Then
        Try
#End If
            dev = Me.GetDevice
            If (dev.IsConnected = False) Then dev.Start()

            Dim ret As New CCollection(Of MarcaturaIngressoUscita)

            Dim devItems As S300Clocking() = dev.GetAllClockings
            Dim devItem As S300Clocking
            Dim savItem As MarcaturaIngressoUscita

            If (devItems IsNot Nothing AndAlso devItems.Length > 0) Then
                Dim savedItems As CCollection(Of MarcaturaIngressoUscita) = Me.GetAllMarcature()
                Array.Sort(devItems)

                Dim i, j As Integer
                Dim toSave As Boolean
                i = 0 : j = 0
                While (i < devItems.Length AndAlso j < savedItems.Count)
                    devItem = devItems(i)
                    If (devItem IsNot Nothing) Then
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
                            savItem.Dispositivo = Nothing
                            savItem.Data = devItem.Time
                            savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                            savItem.Stato = ObjectStatus.OBJECT_VALID
                            savItem.NomeOperatore = Me.MapOperatore(devItem)
                            'savItem.PuntoOperativo = Nothing
                            savItem.NomePuntoOperativo = Me.Nome
                            savItem.Parametri.SetItemByKey("DeviceName", Me.Nome)
                            savItem.Parametri.SetItemByKey("DeviceID", Me.DeviceID)
                            savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
                            savItem.Save()
                            ret.Add(savItem)
                            i += 1
                        End If
                    Else
                        i += 1
                    End If

                End While

                While (i < devItems.Length)
                    devItem = devItems(i)
                    If (devItem IsNot Nothing) Then
                        savItem = New MarcaturaIngressoUscita
                        savItem.Dispositivo = Nothing
                        savItem.Data = devItem.Time
                        savItem.Operazione = IIf(devItem.Type = S300ClockingType.In, TipoMarcaturaIO.INGRESSO, TipoMarcaturaIO.USCITA)
                        savItem.Stato = ObjectStatus.OBJECT_VALID
                        savItem.NomeOperatore = Me.MapOperatore(devItem)
                        'savItem.PuntoOperativo = Nothing
                        savItem.NomePuntoOperativo = Me.Nome
                        savItem.Parametri.SetItemByKey("DeviceName", Me.Nome)
                        savItem.Parametri.SetItemByKey("DeviceID", Me.DeviceID)
                        savItem.Parametri.SetItemByKey("PersonID", devItem.PersonID)
                        savItem.Save()
                        ret.Add(savItem)
                    End If

                    i += 1
                End While
            End If

            If (dev IsNot Nothing AndAlso dev.IsConnected) Then dev.Stop() : dev = Nothing

            Return ret
#If Not DEBUG Then
        Catch ex As Exception
            Throw
        Finally
            If (dev IsNot Nothing AndAlso dev.IsConnected) Then dev.Stop() : dev = Nothing
        End Try
#End If
    End Function




End Class
