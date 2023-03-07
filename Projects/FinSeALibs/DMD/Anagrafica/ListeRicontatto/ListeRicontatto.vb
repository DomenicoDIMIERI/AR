Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Internals
Imports DMD.Anagrafica

Namespace Internals

    Public NotInheritable Class CListeRicontattoClass
        Inherits CGeneralClass(Of CListaRicontatti)

        Private m_Database As CDBConnection
        Private m_Items As CListeRicontattoItemsClass

        Friend Sub New()
            MyBase.New("modListeRicontatto", GetType(CListaRicontattiCursor), -1)
            Me.m_Items = Nothing
            Me.m_Database = Nothing
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

        Public Function GetItemByName(ByVal nome As String) As CListaRicontatti
            nome = Trim(nome)
            If (nome = "") Then Return Nothing
            For Each item As CListaRicontatti In Me.LoadAll
                If Strings.Compare(item.Name, nome) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetListeRicontatto() As CCollection(Of CListaRicontatti)
            Return Me.LoadAll
        End Function

        Public ReadOnly Property Items As CListeRicontattoItemsClass
            Get
                SyncLock Me
                    If (Me.m_Items Is Nothing) Then Me.m_Items = New CListeRicontattoItemsClass
                    Return Me.m_Items
                End SyncLock
            End Get
        End Property


    End Class


End Namespace

Partial Public Class Anagrafica


   
    Private Shared m_ListeRicontatto As CListeRicontattoClass = Nothing

    Public Shared ReadOnly Property ListeRicontatto As CListeRicontattoClass
        Get
            If (m_ListeRicontatto Is Nothing) Then m_ListeRicontatto = New CListeRicontattoClass
            Return m_ListeRicontatto
        End Get
    End Property

End Class