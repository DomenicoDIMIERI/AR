Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils


Imports DMD.Anagrafica

Namespace Forms

 
    Public Class CSysEventsModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            Me.UseLocal = True
        End Sub

        Public Function getEventsArray(ByVal renderer As Object) As String
            Dim filter As CEventsFilter
            Dim cursor As CEventsCursor
            Dim col As New CCollection(Of CEventLog)
            Dim i As Integer
            filter = XML.Utils.Serializer.Deserialize(RPC.n2str(Me.GetParameter(renderer, "filter", "")), "CEventsFilter")
            cursor = New CEventsCursor
            If filter.fromDate.HasValue Then
                cursor.Data.Value = filter.fromDate
                cursor.Data.Operator = OP.OP_GE
            End If
            cursor.Data.SortOrder = SortEnum.SORT_DESC
            If filter.operatorName <> "" Then
                cursor.UserName.Value = filter.operatorName
                cursor.UserName.Operator = OP.OP_LIKE
            End If
            If filter.moduleName <> "" Then
                cursor.Source.Value = filter.moduleName
                cursor.Source.Operator = OP.OP_LIKE
            End If
            i = 0
            While (Not cursor.EOF) And (i < filter.maxCount)
                col.Add(cursor.Item)
                cursor.MoveNext()
                i = i + 1
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            If col.Count > 0 Then
                Return XML.Utils.Serializer.Serialize(col.ToArray, 0)
            Else
                Return vbNullString
            End If
        End Function


    End Class
     
End Namespace