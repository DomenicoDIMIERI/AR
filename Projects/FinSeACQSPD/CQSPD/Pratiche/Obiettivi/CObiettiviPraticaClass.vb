Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Namespace CQSPDInternals

    Public Class CObiettivoPraticaClass
        Inherits CGeneralClass(Of CQSPD.CObiettivoPratica)

        Public Sub New()
            MyBase.New("CQSPDObiettiviPratica", GetType(CQSPD.CObiettivoPraticaCursor), -1)
        End Sub

        ''' <summary>
        ''' Restituisce tutti gli obiettivi attivi e validi alla data indicata
        ''' </summary>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetObiettiviAl(ByVal d As Date) As CCollection(Of CQSPD.CObiettivoPratica)
            Dim ret As New CCollection(Of CQSPD.CObiettivoPratica)
            SyncLock Me
                For Each o As CQSPD.CObiettivoPratica In Me.LoadAll
                    If (o.Stato = ObjectStatus.OBJECT_VALID AndAlso o.IsValid(d)) Then
                        ret.Add(o)
                    End If
                Next
            End SyncLock
            ret.Sort()
            Return ret
        End Function

        ''' <summary>
        ''' Restituisce gli obiettivi validi alla data per l'ufficio specificato
        ''' </summary>
        ''' <param name="po"></param>
        ''' <param name="d"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetObiettiviAl(ByVal po As CUfficio, ByVal d As Date) As CCollection(Of CQSPD.CObiettivoPratica)
            Dim ret As New CCollection(Of CQSPD.CObiettivoPratica)
            If (po Is Nothing) Then Throw New ArgumentNullException("po")
            SyncLock Me
                For Each o As CQSPD.CObiettivoPratica In Me.LoadAll
                    If (o.Stato = ObjectStatus.OBJECT_VALID AndAlso (o.IDPuntoOperativo = 0 OrElse o.IDPuntoOperativo = GetID(po)) AndAlso o.IsValid(d)) Then
                        ret.Add(o)
                    End If
                Next
            End SyncLock
            ret.Sort()
            Return ret
        End Function

    End Class

End Namespace


Partial Public Class CQSPD


    Private Shared m_Obiettivi As CQSPDInternals.CObiettivoPraticaClass

    Public Shared ReadOnly Property Obiettivi As CQSPDInternals.CObiettivoPraticaClass
        Get
            If (m_Obiettivi Is Nothing) Then m_Obiettivi = New CQSPDInternals.CObiettivoPraticaClass
            Return m_Obiettivi
        End Get
    End Property



    


End Class
