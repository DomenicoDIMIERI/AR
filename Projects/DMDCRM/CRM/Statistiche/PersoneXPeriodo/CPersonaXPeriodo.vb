Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    
    Public Class CPersonaXPeriodo
        Implements DMD.XML.IDMDXMLSerializable, IComparable

        Public IDPersona As Integer
        Public NomePersona As String
        Private m_Persona As CPersona
        Public Data As Date
        Public ConteggioChiamate As Integer
        Public ConteggioRisposte As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.IDPersona = 0
            Me.NomePersona = ""
            Me.m_Persona = Nothing
            Me.Data = Nothing
        End Sub

        Public Property Persona As CPersona
            Get
                If (Me.m_Persona Is Nothing) Then Me.m_Persona = Anagrafica.Persone.GetItemById(Me.IDPersona)
                Return Me.m_Persona
            End Get
            Set(value As CPersona)
                Me.m_Persona = value
                Me.IDPersona = GetID(value)
                Me.NomePersona = ""
                If (value IsNot Nothing) Then Me.NomePersona = value.Nominativo
            End Set
        End Property


        Private Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDPersona" : Me.IDPersona = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NomePersona" : Me.NomePersona = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Data" : Me.Data = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "ConteggioChiamate" : Me.ConteggioChiamate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ConteggioRisposte" : Me.ConteggioRisposte = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Private Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDPersona", Me.IDPersona)
            writer.WriteAttribute("NomePersona", Me.NomePersona)
            writer.WriteAttribute("Data", Me.Data)
            writer.WriteAttribute("ConteggioChiamate", Me.ConteggioChiamate)
            writer.WriteAttribute("ConteggioRisposte", Me.ConteggioRisposte)
        End Sub

        Public Function CompareTo(ByVal obj As CPersonaXPeriodo) As Integer
            Dim ret As Integer = DateUtils.Compare(Me.Data, obj.Data)
            If (ret = 0) Then ret = Strings.Compare(Me.NomePersona, obj.NomePersona)
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



End Class