Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica




    <Serializable>
    Public Class CUtentiXPostazioneCollection
        Inherits CCollection(Of CUtenteXPostazione)

        <NonSerialized> Private m_Postazione As CPostazione

        Public Sub New()
            Me.m_Postazione = Nothing
        End Sub

        Public Sub New(ByVal postazione As CPostazione)
            Me.New()
            If (postazione Is Nothing) Then Throw New ArgumentNullException("postazione")
            Me.SetPostazione(postazione)
        End Sub

        Public ReadOnly Property Postazione As CPostazione
            Get
                Return Me.m_Postazione
            End Get
        End Property

        Protected Friend Sub SetPostazione(ByVal value As CPostazione)
            Me.m_Postazione = value
            If (value IsNot Nothing) Then
                For Each item As CUtenteXPostazione In Me
                    item.SetPostazione(value)
                Next
            End If
        End Sub

        'Protected Friend Sub Load(ByVal value As CUfficio)
        '    If (value Is Nothing) Then Throw New ArgumentNullException("Ufficio")
        '    MyBase.Clear()
        '    Me.m_Ufficio = value
        '    If (GetID(value) = 0) Then Return

        '    For i As Integer = 0 To Anagrafica.Uffici.UfficiConsentiti.Count - 1
        '        Dim item As CUtenteXUfficio = Anagrafica.Uffici.UfficiConsentiti(i)
        '        If (item.IDUfficio = GetID(value)) AndAlso (item.Utente IsNot Nothing) AndAlso (item.Utente.Stato = ObjectStatus.OBJECT_VALID) Then
        '            MyBase.Add(item.Utente)
        '        End If
        '    Next
        'End Sub


    End Class

End Class