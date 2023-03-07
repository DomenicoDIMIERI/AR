Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

     <Serializable> _
    Public Class ContattoInAttesaInfo
        Inherits CPersonaInfo

        Public Contatto As CContattoUtente

        Public Sub New()
        End Sub

        Public Sub New(ByVal contatto As CContattoUtente)
            MyBase.New()
            If (contatto Is Nothing) Then Throw New ArgumentNullException("contatto")
            If (contatto.Persona IsNot Nothing) Then Me.Parse(contatto.Persona)
            Me.Contatto = contatto
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Contatto", Me.Contatto)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Contatto" : Me.Contatto = fieldValue
                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub
    End Class



End Class