Imports System.IO
Imports System.Xml.Serialization
Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Public NotInheritable Class DialTPConfigClass
    Inherits CGeneralClass(Of DialTPConfig)


    Public Sub New()
        MyBase.New("modDialTPConfig", GetType(DialTPConfigCursor), -1)
    End Sub

    Public Function GetConfiguration(ByVal machine As String, ByVal user As String) As DialTPConfig
        machine = Strings.Trim(machine)
        user = Strings.Trim(user)
        If (machine = "" Or user = "") Then Throw New ArgumentNullException()
        For Each c As DialTPConfig In Me.LoadAll
            If (c.Attiva AndAlso Strings.Compare(c.IDPostazione, machine) = 0 AndAlso Strings.Compare(c.UserName, user) = 0) Then
                Return c
            End If
        Next

        Dim cursor As DialTPConfigCursor = Nothing
        Try
            cursor = New DialTPConfigCursor
            'cursor.IDMacchina.Value = mn
            cursor.IDPostazione.Value = machine
            cursor.IDUtente.Value = user
            cursor.Attiva.SortOrder = SortEnum.SORT_ASC
            'cursor.MinVersion.Value = appver
            'cursor.MinVersion.Operator = OP.OP_GE
            'cursor.MinVersion.IncludeNulls = True

            'cursor.MaxVersion.Value = appver
            'cursor.MaxVersion.Operator = OP.OP_LE
            'cursor.MaxVersion.IncludeNulls = True
            cursor.IgnoreRights = True

            Return cursor.Item

        Catch ex As Exception
            Sistema.Events.NotifyUnhandledException(ex)
            Throw
        Finally
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
        End Try
    End Function


End Class

