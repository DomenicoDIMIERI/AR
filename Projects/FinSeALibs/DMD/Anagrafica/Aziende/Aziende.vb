Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals


Namespace Internals

    Public NotInheritable Class CAziendeClass
        Inherits CGeneralClass(Of CAzienda)

        Public Event AziendaPrincipaleChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)

        Private m_AziendaPrincipale As CAziendaPrincipale = Nothing

        Friend Sub New()
            MyBase.New("Aziende", GetType(CAziendeCursor), 0)
        End Sub

        Public Overrides Function GetItemById(id As Integer) As CAzienda
            Return MyBase.GetItemById(id)
        End Function

        Public Function GetAziendaByPIVA(ByVal value As String) As CAzienda
            value = Formats.ParsePartitaIVA(value)
            If (value = vbNullString) Then Return Nothing

            SyncLock Me
                For Each item As CacheItem In Me.CachedItems
                    If item.Item.PartitaIVA = value Then Return item.Item
                Next
            End SyncLock

            Dim cursor As New CAziendeCursor
            Try
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.IgnoreRights = True
                cursor.PageSize = 1
                cursor.PartitaIVA.Value = value
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function



        Public Function GetItemByName(ByVal value As String) As CAzienda
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing

            SyncLock Me
                For Each item As CacheItem In Me.CachedItems
                    If item.Item.RagioneSociale = value Then Return item.Item
                Next
            End SyncLock

            Dim cursor As New CAziendeCursor
            Try
                cursor.Cognome.Value = value
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        Public Function GetItemByCF(ByVal value As String) As CAzienda
            value = Formats.ParseCodiceFiscale(value)
            If (value = vbNullString) Then Return Nothing

            SyncLock Me
                For Each item As CacheItem In Me.CachedItems
                    If item.Item.CodiceFiscale = value Then Return item.Item
                Next
            End SyncLock

            Dim cursor As New CAziendeCursor
            Try
                cursor.CodiceFiscale.Value = value
                cursor.IgnoreRights = True
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                Return cursor.Item
            Catch ex As Exception
                Throw
            Finally
                cursor.Dispose()
            End Try
        End Function

        ''' <summary>
        ''' Restituisce o imposta l'azienda principale
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AziendaPrincipale() As CAziendaPrincipale
            Get
                If (Me.m_AziendaPrincipale Is Nothing) Then
                    Dim idAzienda As Integer = Sistema.ApplicationContext.IDAziendaPrincipale
                    Dim a As CAzienda = Anagrafica.Aziende.GetItemById(idAzienda)
                    If (a Is Nothing) Then
                        a = New CAziendaPrincipale
                        a.RagioneSociale = "Azienda principale"
                        a.Stato = ObjectStatus.OBJECT_VALID
                        a.Save()
                        Sistema.ApplicationContext.IDAziendaPrincipale = GetID(a)
                    Else
                        m_AziendaPrincipale = New CAziendaPrincipale(a)
                    End If
                    Sistema.ApplicationContext.IDAziendaPrincipale = GetID(a)
                End If

                Return m_AziendaPrincipale
            End Get
            Set(value As CAziendaPrincipale)
                Dim oldValue As CAziendaPrincipale = Me.AziendaPrincipale
                If (oldValue Is value) Then Exit Property
                Me.m_AziendaPrincipale = value
                Sistema.ApplicationContext.IDAziendaPrincipale = GetID(value)
                Me.OnAziendaPrincipaleChanged(New PropertyChangedEventArgs("AziendaPrincipale", value, oldValue))
            End Set
        End Property

        Protected Sub OnAziendaPrincipaleChanged(ByVal e As PropertyChangedEventArgs)
            RaiseEvent AziendaPrincipaleChanged(Me, e)
        End Sub



    End Class

End Namespace

Partial Public Class Anagrafica


    Private Shared m_Aziende As CAziendeClass = Nothing

    Public Shared ReadOnly Property Aziende As CAziendeClass
        Get
            If (m_Aziende Is Nothing) Then m_Aziende = New CAziendeClass
            Return m_Aziende
        End Get
    End Property

End Class