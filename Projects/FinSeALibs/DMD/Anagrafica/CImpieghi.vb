Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica
      

    ''' <summary>
    ''' Collezione di impieghi di una persona
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable> _
    Public Class CImpieghi
        Inherits CCollection(Of CImpiegato)

        <NonSerialized> _
        Private m_Persona As CPersonaFisica

        Public Sub New()
            Me.m_Persona = Nothing
        End Sub

        Public Sub New(ByVal persona As CPersonaFisica)
            Me.New()
            Me.Initialize(persona)
        End Sub

        Public ReadOnly Property Persona As CPersonaFisica
            Get
                Return Me.m_Persona
            End Get
        End Property

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(value, CImpiegato).SetPersona(Me.m_Persona)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Persona IsNot Nothing) Then DirectCast(newValue, CImpiegato).SetPersona(Me.m_Persona)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Protected Friend Sub SetPersona(ByVal value As CPersonaFisica)
            Me.m_Persona = value
            For Each impiego As CImpiegato In Me
                impiego.Persona = value
            Next
        End Sub

        Public Overloads Function Add() As CImpiegato
            Dim item As New CImpiegato
            item.Stato = ObjectStatus.OBJECT_VALID
            MyBase.Add(item)
            Return item
        End Function

        Public Function AddImpiego(ByVal azienda As CAzienda, ByVal ufficio As String, ByVal posizione As String) As CImpiegato
            Dim item As New CImpiegato
            If (azienda Is Nothing) Then Throw New ArgumentNullException("azienda")
            With item
                .Azienda = azienda
                .Ufficio = ufficio
                .Posizione = posizione
                .Stato = ObjectStatus.OBJECT_VALID
            End With
            MyBase.Add(item)
            Return item
        End Function

        Protected Friend Function Initialize(ByVal persona As CPersonaFisica) As Boolean
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")

            MyBase.Clear()
            Me.m_Persona = persona

            If GetID(persona) = 0 Then Return True

            Dim conn As CDBConnection = persona.GetConnection
            If (conn.IsRemote) Then
                Dim ret As String = RPC.InvokeMethod("/websvc/?_m=Persone&_a=getImpieghi", "aid", RPC.int2n(GetID(persona)))
                If (ret <> "") Then Me.AddRange(XML.Utils.Serializer.Deserialize(ret))
            Else
                Dim cursor As CImpiegatiCursor = Nothing
                Try
                    cursor = New CImpiegatiCursor
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                    cursor.PersonaID.Value = Databases.GetID(persona, 0)
                    cursor.ID.SortOrder = SortEnum.SORT_DESC
                    cursor.IgnoreRights = True
                    While Not cursor.EOF
                        MyBase.Add(cursor.Item)
                        cursor.MoveNext()
                    End While
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
                    If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
                End Try
            End If


            Return True
        End Function


    End Class






End Class