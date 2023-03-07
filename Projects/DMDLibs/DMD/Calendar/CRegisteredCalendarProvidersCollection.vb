Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    Public Class CRegisteredCalendarProvidersCollection
        Inherits CCollection(Of CRegisteredCalendarProvider)


        ''' <summary>
        ''' Registra il nome della classe come nuove provider per il calendario
        ''' </summary>
        ''' <param name="pName"></param>
        ''' <param name="type"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RegisterProvider(ByVal pName As String, ByVal type As System.Type) As CRegisteredCalendarProvider
            If type Is Nothing Then Throw New ArgumentException("type non può essere null")
            Dim ret As New CRegisteredCalendarProvider(pName, type)
            ret.Save()
            MyBase.Add(ret)
            Return ret
        End Function

        ''' <summary>
        ''' Registra il nome della classe come nuove provider per il calendario
        ''' </summary>
        ''' <param name="pName"></param>
        ''' <param name="typeName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RegisterProvider(ByVal pName As String, ByVal typeName As String) As CRegisteredCalendarProvider
            Dim t As System.Type = Reflection.Assembly.GetCallingAssembly.GetType(typeName)
            Return RegisterProvider(pName, t)
        End Function

        ''' <summary>
        ''' Rimuove il nome della classe come nuove provider per il calendario
        ''' </summary>
        ''' <param name="pName"></param>
        ''' <remarks></remarks>
        Public Sub UnregisterProvider(ByVal pName As String)
            Dim p As CRegisteredCalendarProvider = Me(pName)
            p.Delete()
            MyBase.Remove(p)
        End Sub

        Friend Function Load() As Boolean
            MyBase.Clear()
            Dim dbSQL = "SELECT * FROM [tbl_RegisteredCalendarProviders]"
            Dim reader As New DBReader(APPConn.Tables("tbl_RegisteredCalendarProviders"), dbSQL)
            While reader.Read
                Dim item As New CRegisteredCalendarProvider
                If APPConn.Load(item, reader) Then MyBase.Add(item)
            End While
            reader.Dispose()
            Return True
        End Function

        Public Overloads Function IndexOf(ByVal providerName As String) As Integer
            For i As Integer = 0 To Me.Count - 1
                If Me(i).Nome = providerName Then Return i
            Next
            Return -1
        End Function

        Public Overloads Function Contains(ByVal providerName As String) As Boolean
            Return Me.IndexOf(providerName) >= 0
        End Function

        Default Public Overloads ReadOnly Property Item(ByVal providerName As String) As CRegisteredCalendarProvider
            Get
                Return MyBase.Item(Me.IndexOf(providerName))
            End Get
        End Property

    End Class


End Class