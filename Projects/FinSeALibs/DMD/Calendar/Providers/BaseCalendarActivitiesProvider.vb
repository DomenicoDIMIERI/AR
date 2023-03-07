Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica


' Classe:		BaseCalendarActivitiesProvider
' Descrizione:	Classe base che implementa i metodi dell'interfaccia ICalendarProvider
' Versione:	    1.0.0.0
' Data:		    20/12/2017
' Autore:		DMD
' info:		    domenico.dimieri@hotmail.it
' 
' Note:		

Partial Class Sistema

    ''' <summary>
    ''' Classe base del provider di attività del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseCalendarActivitiesProvider
        Implements ICalendarProvider

        Private m_Module As CModule

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            Me.m_Module = Nothing
        End Sub

        Public ReadOnly Property [Module] As CModule
            Get
                If (m_Module Is Nothing) Then m_Module = Me.GetModule
                Return m_Module
            End Get
        End Property

        Protected Overridable Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overridable Function InstantiateNewItem() As Object Implements ICalendarProvider.InstantiateNewItem
            Return New CCalendarActivity
        End Function

        Public Overridable Function GetCreateCommand() As String Implements ICalendarProvider.GetCreateCommand
            GetCreateCommand = "/calendar/activities/?_a=create"
        End Function

        Public Overridable Function GetShortDescription() As String Implements ICalendarProvider.GetShortDescription
            Return "Attività"
        End Function

        Public Overridable Function GetSupportedTypes() As System.Type() Implements ICalendarProvider.GetSupportedTypes
            Return {GetType(CCalendarActivity)}
        End Function

        ''' <summary>
        ''' Restituisce una collezione di oggetti CActivePerson contenente tutte le persone con attività in sospes o nelle date indicate. La collezione è ordinata in funzione della priorità o del ritardo delle operazioni
        ''' </summary>
        ''' <param name="fromDate"></param>
        ''' <param name="toDate"></param>
        ''' <param name="ufficio"></param>
        ''' <param name="operatore"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetActivePersons(ByVal nomeLista As String, ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date), Optional ByVal ufficio As Integer = 0, Optional ByVal operatore As Integer = 0) As CCollection(Of CActivePerson) Implements ICalendarProvider.GetActivePersons

        Public MustOverride Function GetScadenze(ByVal fromDate As Nullable(Of Date), ByVal toDate As Nullable(Of Date)) As CCollection(Of ICalendarActivity) Implements ICalendarProvider.GetScadenze

        Public MustOverride Function GetPendingActivities() As CCollection(Of ICalendarActivity) Implements ICalendarProvider.GetPendingActivities

        ''' <summary>
        ''' Restituisce l'elenco delle cose da fare per l'utente specificato
        ''' </summary>
        ''' <param name="user"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function GetToDoList(ByVal user As CUser) As CCollection(Of ICalendarActivity) Implements ICalendarProvider.GetToDoList

        ''' <summary>
        ''' Restituisce il nome che identifica univocamente il provider all'interno del sistema
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride ReadOnly Property UniqueName As String Implements ICalendarProvider.UniqueName

        Public MustOverride Sub DeleteActivity(item As ICalendarActivity, Optional ByVal force As Boolean = False) Implements ICalendarProvider.DeleteActivity

        Public MustOverride Sub SaveActivity(item As ICalendarActivity, Optional ByVal force As Boolean = False) Implements ICalendarProvider.SaveActivity

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class