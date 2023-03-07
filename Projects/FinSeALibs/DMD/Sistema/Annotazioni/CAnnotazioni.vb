Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization
Imports DMD.Anagrafica
Imports DMD.Databases

Partial Public Class Sistema

    <Serializable> _
    Public Class CAnnotazioni
        Inherits CCollection(Of CAnnotazione)

        Private m_Contesto As Object
        Private m_Owner As Object

        Public Sub New()
            Me.m_Owner = Nothing
            Me.m_Contesto = Nothing
        End Sub

        Public Sub New(ByVal owner As Object)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetOwner(owner)
            Me.Initialize()
        End Sub

        Public Sub New(ByVal owner As Object, ByVal contesto As Object)
            Me.New()
            If (owner Is Nothing) Then Throw New ArgumentNullException("owner")
            Me.SetOwner(owner)
            Me.SetContesto(contesto)
            Me.Initialize()
        End Sub

        Public Sub SetOwner(ByVal value As Object)
            Me.m_Owner = value
            For Each a As CAnnotazione In Me
                a.SetOwner(value)
            Next
        End Sub

        Protected Friend Sub SetContesto(ByVal value As Object)
            Me.m_Contesto = value
            For Each a As CAnnotazione In Me
                a.SetContesto(value)
            Next
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            Dim item As CAnnotazione = value
            If Not (Me.m_Owner Is Nothing) Then item.SetOwner(Me.m_Owner)
            If (Me.m_Contesto IsNot Nothing) Then item.SetContesto(Me.m_Contesto)
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            Dim item As CAnnotazione = newValue
            If Not (Me.m_Owner Is Nothing) Then item.SetOwner(Me.m_Owner)
            If (Me.m_Contesto IsNot Nothing) Then item.SetContesto(Me.m_Contesto)
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Sub AddItem(ByVal item As CAnnotazione)
            MyBase.Add(item)
        End Sub

        Public Overloads Function Add(ByVal nota As String) As CAnnotazione
            Dim item As New CAnnotazione
            'item.Produttore = Anagrafica.Aziende.AziendaPrincipale
            item.Valore = nota
            item.SetContesto(Me.m_Contesto)
            item.SetOwner(Me.m_Owner)
            item.Stato = ObjectStatus.OBJECT_VALID
            Me.AddItem(item)
            Return item
        End Function

        Public Function GetCompactString() As String
            Dim i As Integer
            Dim html As String
            Dim nomeCreatoDa, nomeModificatoDa As String
            html = ""
            For i = 0 To MyBase.Count - 1
                If (html <> "") Then html = html & vbCrLf
                nomeCreatoDa = ""
                nomeModificatoDa = ""
                If Not (MyBase.Item(i) Is Nothing) Then
                    If Not (Me(i).CreatoDa Is Nothing) Then nomeCreatoDa = Me(i).CreatoDa.Nominativo
                    If Not (Me(i).ModificatoDa Is Nothing) Then nomeModificatoDa = Me(i).ModificatoDa.Nominativo
                End If
                html = html & Formats.FormatUserDateTime(Me(i).ModificatoIl) & " " & nomeModificatoDa & " " & Me(i).Valore
            Next
            Return html
        End Function

        Public Function GetFullString() As String
            Dim i As Integer
            Dim html, nomeCreatoDa, nomeModificatoDa As String
            html = ""
            For i = 0 To MyBase.Count - 1
                If (html <> "") Then html = html & vbCrLf
                nomeCreatoDa = ""
                nomeModificatoDa = ""
                If Not (MyBase.Item(i) Is Nothing) Then
                    If Not (MyBase.Item(i).CreatoDa Is Nothing) Then nomeCreatoDa = MyBase.Item(i).CreatoDa.Nominativo
                    If Not (MyBase.Item(i).ModificatoDa Is Nothing) Then nomeModificatoDa = MyBase.Item(i).ModificatoDa.Nominativo
                End If
                html = html & Formats.FormatUserDateTime(MyBase.Item(i).ModificatoIl) & " " & nomeModificatoDa & " " & MyBase.Item(i).Valore
            Next
            Return html
        End Function

        Protected Function Initialize() As Boolean
            MyBase.Clear()
            If (GetID(Me.m_Owner) = 0) Then Return True

            Dim cursor As New CAnnotazioniCursor
            cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            cursor.OwnerID.Value = GetID(Me.m_Owner)
            cursor.OwnerType.Value = TypeName(Me.m_Owner)
            cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC
            If (Me.m_Contesto IsNot Nothing) Then
                cursor.IDContesto.Value = GetID(Me.m_Contesto)
                cursor.TipoContesto.Value = TypeName(Me.m_Contesto)
            End If
            While Not cursor.EOF
                MyBase.Add(cursor.Item)
                cursor.MoveNext()
            End While
            If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing

            Return True
        End Function


    End Class

End Class

