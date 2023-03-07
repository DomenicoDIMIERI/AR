Imports DMD
Imports DMD.Sistema
Imports DMD.Forms
Imports DMD.WebSite
Imports DMD.Databases
Imports DMD.Forms.Utils



Namespace Forms

   
 
    <Serializable> _
    Public Class ExportableColumnInfo
        Implements DMD.XML.IDMDXMLSerializable, IComparable

        Public Key As String
        Public Value As String
        Public Posizione As Integer
        Public TipoValore As TypeCode
        Public Selected As Boolean

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
        End Sub

        Public Sub New(ByVal nome As String, ByVal descrizione As String, ByVal tipoValore As TypeCode, Optional ByVal selected As Boolean = True)
            Me.New
            Me.Key = nome
            Me.Value = descrizione
            Me.TipoValore = tipoValore
            Me.Selected = selected
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Key & "/" & Me.Value
        End Function

        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Key" : Me.Key = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Value" : Me.Value = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Posizione" : Me.Posizione = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoValore" : Me.TipoValore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Selected" : Me.Selected = XML.Utils.Serializer.DeserializeBoolean(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteTag("Key", Me.Key)
            writer.WriteTag("Value", Me.Value)
            writer.WriteTag("Posizione", Me.Posizione)
            writer.WriteTag("TipoValore", Me.TipoValore)
            writer.WriteTag("Selected", Me.Selected)
        End Sub

        Public Function CompareTo(obj As ExportableColumnInfo) As Integer
            Dim ret As Integer = Me.Posizione - obj.Posizione
            If (ret = 0) Then ret = Strings.Compare(Me.Value, obj.Value, CompareMethod.Text)
            Return ret
        End Function

        Private Function CompareTo1(obj As Object) As Integer Implements IComparable.CompareTo
            Return Me.CompareTo(obj)
        End Function

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class




End Namespace