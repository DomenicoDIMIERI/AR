Imports DMD
Imports DMD.Forms
Imports DMD.Databases
Imports DMD.WebSite

Imports DMD.CQSPD
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace Forms
 
    Public Class CQSPDInfoAnalisiAnomalie
        Implements IComparable, DMD.XML.IDMDXMLSerializable


        Public IDCliente As Integer
        Public NomeCliente As String
        Private m_Cliente As CPersonaFisica
        Public Oggetti As CCollection(Of OggettoAnomalo)

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.IDCliente = 0
            Me.NomeCliente = ""
            Me.m_Cliente = Nothing
            Me.Oggetti = New CCollection(Of OggettoAnomalo)
        End Sub

        Public Property Cliente As CPersonaFisica
            Get
                If (Me.m_Cliente Is Nothing) Then Me.m_Cliente = Anagrafica.Persone.GetItemById(Me.IDCliente)
                Return Me.m_Cliente
            End Get
            Set(value As CPersonaFisica)
                Me.m_Cliente = value
                Me.IDCliente = GetID(value)
                If (value Is Nothing) Then
                    Me.NomeCliente = ""
                Else
                    Me.NomeCliente = value.Nominativo
                End If
            End Set
        End Property

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            Dim o As CQSPDInfoAnalisiAnomalie = obj
            Return Strings.Compare(Me.NomeCliente, o.NomeCliente, CompareMethod.Text)
        End Function

        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDCliente" : Me.IDCliente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomeCliente" : Me.NomeCliente = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Oggetti" : Me.Oggetti.Clear() : Me.Oggetti.AddRange(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDCliente", Me.IDCliente)
            writer.WriteAttribute("NomeCliente", Me.NomeCliente)
            writer.WriteTag("Oggetti", Me.Oggetti)
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class

    Public Class CQSPDAnomalieHandler
        Inherits CQSPDBaseStatsHandler

        Public Sub New()
            Me.UseLocal = False
        End Sub


        Public Function GetStats(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("list") Then Throw New PermissionDeniedException(Me.Module, "list")
            Dim po As Integer = RPC.n2int(GetParameter(renderer, "po", "0"))
            Dim op As Integer = RPC.n2int(GetParameter(renderer, "op", "0"))
            Dim di As Date? = RPC.n2date(GetParameter(renderer, "di", ""))
            Dim df As Date? = RPC.n2date(GetParameter(renderer, "df", ""))
            Dim ret As New CKeyCollection
            ret.Add("richieste", CQSPD.RichiesteFinanziamento.GetAnomalie(po, op, di, df, 1))
            ret.Add("consulenze", CQSPD.StudiDiFattibilita.GetAnomalie(po, op, di, df, 1))
            ret.Add("pratiche", CQSPD.Pratiche.GetAnomalie(po, op, di, df, 1))
            Return XML.Utils.Serializer.Serialize(ret)
        End Function

    End Class


End Namespace