Imports DMD
Imports DMD.Sistema
Imports DMD.Forms

Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.XML

Namespace Forms



    Public Class CCalendarModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
        End Sub

        Public Function getPersonInfo(ByVal renderer As Object) As String
            Dim pid As Integer
            Dim ret As CCalendarActivityInfo
            pid = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            ret = Calendar.GetPersonInfo(pid)
            Return XML.Utils.Serializer.Serialize(ret, XMLSerializeMethod.Document)
        End Function

        Public Overrides Function CreateCursor() As Databases.DBObjectCursorBase
            Return Nothing
        End Function


        Public Function GetAvailableGroups(ByVal renderer As Object) As String
            Dim col As CCollection(Of CGroup) = Calendar.GetAvailableGroups
            If (col.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function GetAvailableGroupOperators(ByVal renderer As Object) As String
            Dim gid As Integer = Me.GetParameter(renderer, "gid", 0)
            Dim col As CCollection(Of CUser) = Calendar.GetAvailableGroupOperators(gid)
            If (col.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(col.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function GetCompleanniPerMese(ByVal renderer As Object) As String
            Dim year As Integer = RPC.n2int(Me.GetParameter(renderer, "y", ""))
            Dim month As Integer = RPC.n2int(Me.GetParameter(renderer, "m", ""))
            Dim compleanni() As CCollection(Of CPersonaInfo) = Calendar.GetCompleanniPerMese(year, month)
            Return XML.Utils.Serializer.Serialize(compleanni, XMLSerializeMethod.Document)
        End Function

        Public Function GetCompleanniPerGiorno(ByVal renderer As Object) As String
            Dim d As Date = RPC.n2date(Me.GetParameter(renderer, "d", ""))
            Dim compleanni() As CPersonaInfo = Calendar.GetCompleanniPerGiorno(d)
            Return XML.Utils.Serializer.Serialize(compleanni, XMLSerializeMethod.Document)
        End Function

        Public Function GetToDoList(ByVal renderer As Object) As String
            Dim uid As Integer = RPC.n2int(GetParameter(renderer, "uid", "0"))
            Dim user As CUser = Sistema.Users.GetItemById(uid)
            Dim items As CCollection = New CCollection(Sistema.Calendar.GetToDoList(user))
            Return XML.Utils.Serializer.Serialize(items)
        End Function


    End Class



End Namespace