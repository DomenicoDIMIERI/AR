Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public Class CEventsFilter
        Inherits DBObjectBase

        Public fromDate As Nullable(Of Date)
        Public toDate As Nullable(Of Date)
        Public operatorID As Integer
        Public operatorName As String
        Public moduleName As String
        Public text As String
        Public maxCount As Integer

        Public Sub New()
            Dim t As Date = Now
            Me.fromDate = DateUtils.MakeDate(Year(t), Month(t), Day(t), Hour(t) - 8, 0, 0)
            Me.toDate = Nothing
            Me.operatorID = 0
            Me.operatorName = ""
            Me.moduleName = ""
            Me.text = ""
            Me.maxCount = 10
        End Sub

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Protected Overrides Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("fromDate", Me.fromDate)
            writer.WriteTag("toDate", Me.toDate)
            writer.WriteTag("operatorID", Me.operatorID)
            writer.WriteTag("operatorName", Me.operatorName)
            writer.WriteTag("moduleName", Me.moduleName)
            writer.WriteTag("text", Me.text)
            writer.WriteTag("maxCount", Me.maxCount)
        End Sub

        Protected Overrides Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object)
            Select Case LCase(Trim(fieldName))
                Case "fromdate" : fromDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "todate" : toDate = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "operatorid" : operatorID = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "operatorname" : operatorName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "modulename" : moduleName = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "text" : text = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "maxcount" : maxCount = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case Else : Call MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub

        Public Overrides Function GetTableName() As String
            Return vbNullString
        End Function

        Protected Friend Overrides Function GetConnection() As Databases.CDBConnection
            Return Databases.LOGConn
        End Function
    End Class

End Class