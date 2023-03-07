Imports DMD
Imports DMD.Sistema
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Internals


Namespace Internals


    Public NotInheritable Class CFontiClass
        Inherits CGeneralClass(Of CFonte)

        Private m_Providers As New System.Collections.ArrayList

        'Public Shared Event FonteMerged(ByVal e As FonteMergedEventArgs)


        Friend Sub New()
            MyBase.New("modFonti", GetType(CFontiCursor), -1)
        End Sub

        Public Overloads Function GetItemByName(ByVal value As String) As IFonte
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each item As IFonte In Me.LoadAll
                If Strings.Compare(item.Nome, value) = 0 Then Return item
            Next
            Return Nothing
        End Function

        Public Function GetItemByIDAnnuncio(ByVal value As String) As IFonte
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            For Each item As CFonte In Me.LoadAll
                If Strings.Compare(item.IDAnnuncio, value) = 0 Then Return item
            Next
            Return Nothing
            'End SyncLock
        End Function

        Public Overloads Function GetItemById(ByVal providerName As String, ByVal tipo As String, ByVal id As Integer) As IFonte
            ' SyncLock Me.lockObject
            If (id = 0) Then Return Nothing
            Dim p As IFonteProvider = Me.GetProviderByName(providerName)
            If (p Is Nothing) Then Return Nothing
            Return p.GetItemById(tipo, id)
            'End SyncLock
        End Function



        Public Overloads Function GetItemByName(ByVal providerName As String, ByVal tipo As String, ByVal value As String) As IFonte
            'SyncLock Me.lockObject
            value = Trim(value)
            If (value = vbNullString) Then Return Nothing
            Dim p As IFonteProvider = Me.GetProviderByName(providerName)
            If (p Is Nothing) Then Return Nothing
            Return p.GetItemByName(tipo, value)
            'End SyncLock
        End Function

        Public Function GetItemsAsArray(ByVal providerName As String, ByVal tipo As String, Optional ByVal onlyValid As Boolean = True) As IFonte()
            'SyncLock Me.lockObject
            Dim provider As IFonteProvider = Me.GetProviderByName(providerName)
            If provider Is Nothing Then Return Nothing
            Return provider.GetItemsAsArray(tipo, onlyValid)
            'End SyncLock
        End Function

#Region "Statistiche"


        Public Function GetStatisticheEsterne(ByVal entryPage As String, ByVal di As Nullable(Of Date), ByVal df As Nullable(Of Date)) As CKeyCollection(Of CFonteExternalStats)
            'SyncLock Me.lockObject
            Dim ret As New CKeyCollection(Of CFonteExternalStats)
            entryPage = Trim(entryPage)
            If (entryPage = "") Then Throw New ArgumentOutOfRangeException("entryPage")
            Dim text As String = RPC.InvokeMethod(entryPage, "di", RPC.date2n(di), "df", RPC.date2n(df))
            If (text <> "") Then
                Dim items As CCollection = XML.Utils.Serializer.Deserialize(text)
                For i As Integer = 0 To items.Count - 1
                    Dim item As CFonteExternalStats = items(i)
                    ret.Add(item.IDAnnuncio, item)
                Next
            End If
            Return ret
            'End SyncLock
        End Function

#End Region

        Public Sub RegisterProvider(ByVal provider As IFonteProvider)
            SyncLock Me
                m_Providers.Add(provider)
            End SyncLock
        End Sub

        Public Sub UnregisterProvider(ByVal provider As IFonteProvider)
            SyncLock Me
                m_Providers.Remove(provider)
            End SyncLock
        End Sub

        Public ReadOnly Property Providers() As CCollection(Of IFonteProvider)
            Get
                SyncLock Me
                    Dim ret As New CCollection(Of IFonteProvider)
                    For i As Integer = 0 To Me.m_Providers.Count - 1
                        ret.Add(Me.m_Providers(i))
                    Next
                    Return ret
                End SyncLock
            End Get
        End Property

        Public Function GetProviderByName(ByVal name As String) As IFonteProvider
            name = Trim(name)
            For i As Integer = 0 To Me.m_Providers.Count - 1
                Dim p As IFonteProvider = Me.m_Providers(i)
                Dim items() As String = p.GetSupportedNames
                For j As Integer = 0 To UBound(items)
                    If items(j) = name Then Return p
                Next
            Next
            Return Nothing
        End Function

    End Class


End Namespace

Partial Public Class Anagrafica


    Private Shared m_Fonti As CFontiClass = Nothing

    Public Shared ReadOnly Property Fonti As CFontiClass
        Get
            If (m_Fonti Is Nothing) Then m_Fonti = New CFontiClass
            Return m_Fonti
        End Get
    End Property

End Class