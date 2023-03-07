Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica


 

    <Serializable> _
    Public Class CUtentiXUfficioCollection
        Inherits CCollection(Of CUser)

        <NonSerialized> _
        Private m_Ufficio As CUfficio

        Public Sub New()
            Me.m_Ufficio = Nothing
        End Sub

        Public Sub New(ByVal ufficio As CUfficio)
            Me.New()
            Me.Load(ufficio)
        End Sub

        Public ReadOnly Property Ufficio As CUfficio
            Get
                Return Me.m_Ufficio
            End Get
        End Property

        Protected Friend Sub Load(ByVal value As CUfficio)
            If (value Is Nothing) Then Throw New ArgumentNullException("Ufficio")
            MyBase.Clear()
            Me.m_Ufficio = value
            If (GetID(value) = 0) Then Return

            For i As Integer = 0 To Anagrafica.Uffici.UfficiConsentiti.Count - 1
                Dim item As CUtenteXUfficio = Anagrafica.Uffici.UfficiConsentiti(i)
                If (item.IDUfficio = GetID(value)) AndAlso (item.Utente IsNot Nothing) AndAlso (item.Utente.Stato = ObjectStatus.OBJECT_VALID) Then
                    MyBase.Add(item.Utente)
                End If
            Next
        End Sub


    End Class

End Class