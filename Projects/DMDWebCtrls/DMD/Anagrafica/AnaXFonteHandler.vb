Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms

    Public Class AnaXFonteInfo
        Implements IComparable, DMD.XML.IDMDXMLSerializable

        Public TipoFonte As String
        Public Conteggio As Integer
        Private m_Fonte As Object
        Public IDFonte As Integer
        Public NomeFonte As String


        Public Sub New(ByVal tipo As String, ByVal fonte As Object, ByVal conteggio As Integer)
            DMD.DMDObject.IncreaseCounter(Me)
            Me.TipoFonte = tipo
            Me.Fonte = fonte
            Me.Conteggio = conteggio
        End Sub

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim tmp As AnaXFonteInfo = obj
            Return Arrays.Compare(tmp.Conteggio, Me.Conteggio)
        End Function

        Public Property Fonte As Object
            Get
                If (Me.m_Fonte Is Nothing) Then Me.m_Fonte = Anagrafica.Fonti.GetItemById(Me.TipoFonte, Me.TipoFonte, Me.IDFonte)
                Return Me.m_Fonte
            End Get
            Set(value As Object)
                If (Me.Fonte Is value) Then Exit Property
                Me.m_Fonte = value
                Me.IDFonte = GetID(value)
                If (value IsNot Nothing) Then
                    Me.NomeFonte = DirectCast(value, IFonte).Nome
                Else
                    Me.NomeFonte = ""
                End If
            End Set
        End Property

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "TipoFonte" : Me.TipoFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "NomeFonte" : Me.NomeFonte = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDFonte" : Me.IDFonte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Conteggio" : Me.Conteggio = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("TipoFonte", Me.TipoFonte)
            writer.WriteAttribute("IDFonte", Me.IDFonte)
            writer.WriteAttribute("NomeFonte", Me.NomeFonte)
            writer.WriteAttribute("Conteggio", Me.Conteggio)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


    Public Class AnaXFonteHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CPersonaCursor
        End Function

        Public Function GetStats() As String
            Dim dbRis As System.Data.IDataReader = Nothing
            Dim cursor As Anagrafica.CPersonaFisicaCursor = Nothing
            Dim dbSQL As String

            Try
                If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")

                cursor = New Anagrafica.CPersonaFisicaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                dbSQL = "SELECT [TipoFonte], [IDFonte], Count(*) As [Conteggio] FROM (" & cursor.GetSQL & ") GROUP BY [TipoFonte], [IDFonte]"
                cursor.Dispose() : cursor = Nothing

                dbRis = APPConn.ExecuteReader(dbSQL)


                Dim col As New CCollection(Of AnaXFonteInfo)
                While dbRis.Read
                    Dim tipoFonte As String = Formats.ToString(dbRis("TipoFonte"))
                    Dim idFonte As Integer = Formats.ToInteger(dbRis("IDFonte"))
                    Dim fonte As IFonte = Anagrafica.Fonti.GetItemById(tipoFonte, tipoFonte, idFonte)
                    Dim conteggio As Integer = Formats.ToInteger(dbRis("Conteggio"))
                    Dim info As New AnaXFonteInfo(tipoFonte, fonte, conteggio)
                    col.Add(info)
                End While
                dbRis.Dispose() : dbRis = Nothing

                Return XML.Utils.Serializer.Serialize(col)
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Function

    End Class


End Namespace