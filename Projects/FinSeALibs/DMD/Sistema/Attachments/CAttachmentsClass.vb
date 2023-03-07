Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases
Imports DMD.Internals

Namespace Internals

    Public Class CAttachmentsClass
        Inherits CGeneralClass(Of CAttachment)


        Private m_Database As CDBConnection

        Friend Sub New()
            MyBase.New("Attachments", GetType(CAttachmentsCursor))
        End Sub


        Public Property Database As CDBConnection
            Get
                If (Me.m_Database Is Nothing) Then Return APPConn
                Return Me.m_Database
            End Get
            Set(value As CDBConnection)
                Me.m_Database = value
            End Set
        End Property


        Public Function GetTipiContestoPerOggetto(ByVal obj As Object) As CCollection(Of String)
            If (obj Is Nothing) Then Throw New ArgumentNullException("obj")
            Return Me.GetTipiContestoPerOggetto(TypeName(obj), GetID(obj))
        End Function

        Public Function GetTipiContestoPerOggetto(ByVal objectType As String, ByVal objectID As Integer) As CCollection(Of String)
            Dim conn As CDBConnection = Me.Database
            Dim ret As New CCollection(Of String)

            If (conn.IsRemote) Then
                Dim temp As String = conn.InvokeMethod(Me.Module, "GetAttachmentsTipiContestoPerOggetto", "oid", RPC.int2n(objectID), "otp", RPC.str2n(objectType))
                ret.AddRange(XML.Utils.Serializer.Deserialize(temp))
            Else
                Dim dbRis As System.Data.IDataReader = Nothing
                Try
                    Dim dbSQL As New System.Text.StringBuilder
                    dbSQL.Append("SELECT [TipoContesto], [IDContesto] FROM [tbl_Attachments] WHERE [OwnerID]=")
                    dbSQL.Append(objectID)
                    dbSQL.Append(" AND [OwnerType]=")
                    dbSQL.Append(DBUtils.DBString(objectType))
                    dbSQL.Append(" AND Not ([TipoContesto] Is Null) AND [Stato]=")
                    dbSQL.Append(ObjectStatus.OBJECT_VALID)
                    dbSQL.Append(" GROUP BY [TipoContesto], [IDContesto]")

                    dbRis = conn.ExecuteReader(dbSQL.ToString)
                    While dbRis.Read
                        Dim tipoContesto As String = Trim(Formats.ToString(dbRis("TipoContesto")))
                        Dim idContesto As Integer = Formats.ToInteger(dbRis("IDContesto"))
                        Dim str As String = tipoContesto & " (" & Right("00000000" & Hex(idContesto), 8) & ")"
                        If (str <> "") Then ret.Add(str)
                    End While
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
                End Try
            End If

            ret.Sort()
            Return ret
        End Function

    End Class

End Namespace

Partial Public Class Sistema


    Private Shared m_Attachments As CAttachmentsClass = Nothing

    Public Shared ReadOnly Property Attachments As CAttachmentsClass
        Get
            If (m_Attachments Is Nothing) Then m_Attachments = New CAttachmentsClass
            Return m_Attachments
        End Get
    End Property
End Class

