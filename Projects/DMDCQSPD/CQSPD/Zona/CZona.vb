Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema

Partial Public Class CQSPD

    Public Class CZona
        Inherits DBObjectPO

        Private m_Nome As String
        Private m_Comuni As CCollection(Of CComune)
        Private m_Responsabili As CCollection(Of CUser)

        Public Sub New()
            Me.m_Nome = vbNullString
            Me.m_Comuni = New CCollection(Of CComune)
            Me.m_Responsabili = New CCollection(Of CUser)
        End Sub

        Public ReadOnly Property Comuni As CCollection(Of CComune)
            Get
                Return Me.m_Comuni
            End Get
        End Property

        ''' <summary>
        ''' Restituisce o imposta il nome della zona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nome As String
            Get
                Return Me.m_Nome
            End Get
            Set(value As String)
                value = Trim(value)
                Dim oldValue As String = Me.m_Nome
                If (oldValue = value) Then Exit Property
                Me.m_Nome = value
                Me.DoChanged("Nome", value, oldValue)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce la collezione degli utente definiti come responsabili di zona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Responsabili As CCollection(Of CUser)
            Get
                Return Me.m_Responsabili
            End Get
        End Property

        ' ''' <summary>
        ' ''' Aggiunge il comune specificato alla zona
        ' ''' </summary>
        ' ''' <param name="comune"></param>
        ' ''' <remarks></remarks>
        'Public Sub AddComune(ByVal comune As CComune)
        '    If (comune Is Nothing) Then Throw New ArgumentNullException("comune")
        '    If Me.Comuni.GetItemById(GetID(comune)) IsNot Nothing Then
        '        Throw New Exception("Il comune [" & comune.Nome & "] è già presente nella zona [" & Me.m_Nome & "]")
        '    End If
        '    Me.GetConnection.ExecuteCommand("INSERT INTO [tbl_XCQSPDZoneXComune] ([Zona], [Comune]) VALUES (" & GetID(Me) & ", " & GetID(comune) & ")")
        '    Me.m_Comuni.Add(comune)
        'End Sub

        ' ''' <summary>
        ' ''' Rimuove il comune dalla zona
        ' ''' </summary>
        ' ''' <param name="comune"></param>
        ' ''' <remarks></remarks>
        'Public Sub RemoveComune(ByVal comune As CComune)
        '    If (comune Is Nothing) Then Throw New ArgumentNullException("comune")
        '    If Not Me.Comuni.GetItemById(GetID(comune)) IsNot Nothing Then
        '        Throw New Exception("Il comune [" & comune.Nome & "] non è presente nella zona [" & Me.m_Nome & "]")
        '    End If
        '    Me.GetConnection.ExecuteCommand("DELETE * FROM [tbl_XCQSPDZoneXComune] WHERE [Zona]=" & GetID(Me) & " And [Comune]=" & GetID(comune))
        '    Me.m_Comuni.Add(comune)
        'End Sub

        Protected Overrides Function GetConnection() As CDBConnection
            Return CQSPD.Database
        End Function

        Public Overrides Function GetModule() As Sistema.CModule
            Return Zone.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CQSPDZone"
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("Nome", Me.m_Nome)
            writer.Write("Comuni", Me.GetIDStr(Me.m_Comuni))
            writer.Write("Responsabili", Me.GetIDStr(Me.m_Responsabili))
            Return MyBase.SaveToRecordset(writer)
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            reader.Read("Nome", Me.m_Nome)
            Me.LoadComuniIDs(reader.GetValue(Of String)("Comuni", ""))
            Me.LoadResponsabiliIDs(reader.GetValue(Of String)("Responsabili", ""))
            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("Nome", Me.m_Nome)
            writer.WriteAttribute("Comuni", Me.GetIDStr(Me.m_Comuni))
            writer.WriteAttribute("Responsabili", Me.GetIDStr(Me.m_Responsabili))
            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Nome" : Me.m_Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Comuni" : Me.LoadComuniIDs(fieldValue)
                Case "Responsabili" : Me.LoadResponsabiliIDs(fieldValue)
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Private Function GetIDStr(ByVal col As IEnumerable) As String
            Dim ret As New System.Text.StringBuilder
            For Each c As Object In col
                If ret.Length > 0 Then ret.Append(",")
                ret.Append(GetID(c))
            Next
            Return ret.ToString
        End Function

        Private Sub LoadComuniIDs(ByVal value As String)
            Me.m_Comuni.Clear()
            value = Trim(value)
            If (value <> vbNullString) Then
                Dim items() As String = Split(value, ",")
                If (items IsNot Nothing) Then
                    For Each s As String In items
                        Me.m_Comuni.Add(Luoghi.Comuni.GetItemById(CInt(s)))
                    Next
                End If
            End If
        End Sub

        Private Sub LoadResponsabiliIDs(ByVal value As String)
            Me.m_Responsabili.Clear()
            value = Trim(value)
            If (value <> vbNullString) Then
                Dim items() As String = Split(value, ",")
                If (items IsNot Nothing) Then
                    For Each s As String In items
                        Me.m_Responsabili.Add(Users.GetItemById(CInt(s)))
                    Next
                End If
            End If
        End Sub

        ''' <summary>
        ''' Restituisce la somma del numero di abitanti appartenenti ai comuni inclusi nella zona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroAbitanti As Integer
            Get
                Dim ret As Integer = 0
                For i As Integer = 0 To Me.m_Comuni.Count - 1
                    Dim c As CComune = Me.m_Comuni(i)
                    ret += c.NumeroAbitanti
                Next
                Return ret
            End Get
        End Property

        ''' <summary>
        ''' Restituisce il numero dei comuni compresi nella zona
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NumeroComuni As Integer
            Get
                Return Me.m_Comuni.Count
            End Get
        End Property

        Protected Overrides Sub OnAfterSave(e As SystemEvent)
            MyBase.OnAfterSave(e)
            CQSPD.Zone.UpdateCached(Me)
        End Sub

    End Class


End Class