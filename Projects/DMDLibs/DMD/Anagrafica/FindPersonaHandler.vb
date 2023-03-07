Imports DMD
Imports DMD.Sistema
Imports DMD.Databases



Partial Public Class Anagrafica

    Public MustInherit Class FindPersonaHandler
        Implements IComparable

        Public Event Changed(ByVal sender As Object, ByVal e As System.EventArgs)


        Private m_Priority As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Priority = 0
        End Sub



        Public Property Prioriy As Integer
            Get
                Return Me.m_Priority
            End Get
            Set(value As Integer)
                If (Me.m_Priority = value) Then Exit Property
                Me.m_Priority = value
                Me.OnChanged(New System.EventArgs)
            End Set
        End Property

        ''' <summary>
        ''' Restituisce il nome del comando elaborato da questo gestore
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetHandledCommand() As String

        ''' <summary>
        ''' Se vero indica al sistema che il gestore è in grado di elaborare il filtro
        ''' </summary>
        ''' <param name="param"></param>
        ''' <param name="filter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function CanHandle(ByVal param As String, ByVal filter As CRMFinlterI) As Boolean

        ''' <summary>
        ''' Elabora il filtro
        ''' </summary>
        ''' <param name="param"></param>
        ''' <param name="filter"></param>
        ''' <param name="ret"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub Find(ByVal param As String, ByVal filter As CRMFinlterI, ByVal ret As CCollection(Of CPersonaInfo))


        Protected Overridable Sub OnChanged(ByVal e As System.EventArgs)
            RaiseEvent Changed(Me, e)
        End Sub

        Public Function CompareTo(ByVal obj As FindPersonaHandler) As Integer
            Return Arrays.Compare(Me.m_Priority, obj.m_Priority)
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