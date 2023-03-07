Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.Internals
Imports DMD.ADV

Namespace Internals


    Public NotInheritable Class CCampagneClass
        Inherits CGeneralClass(Of CCampagnaPubblicitaria)



        Private m_Checking As Boolean = False
        Public ReadOnly exeLock As New Object
        Private m_Initialized As Boolean = False

        Friend Sub New()
            MyBase.New("modADV", GetType(CCampagnaPubblicitariaCursor))
        End Sub

        Public Overrides Sub Initialize()
            If (Me.m_Initialized) Then Return

            MyBase.Initialize()
            'Me.m_Checking = False
            ' Me.m_Timer = New System.Timers.Timer(6000) 'Le campagne vengono controllate ogni minuto
            ' Me.m_Timer.Enabled = True
            'AddHandler m_Timer.Elapsed, AddressOf TimerHandler
            Sistema.Types.RegisteredTypeProviders.Add("CCampagnaPubblicitaria", AddressOf ADV.Campagne.GetItemById)
            Me.m_Initialized = True
        End Sub

        Public Overrides Sub Terminate()
            If (Not Me.m_Initialized) Then Return

            'RemoveHandler m_Timer.Elapsed, AddressOf TimerHandler
            'Me.m_Timer.Enabled = False
            'Me.m_Timer = Nothing
            MyBase.Terminate()

            Me.m_Initialized = False
        End Sub

        'Public Property EnableTimer As Boolean
        '    Get
        '        Return m_Timer.Enabled
        '    End Get
        '    Set(value As Boolean)
        '        m_Timer.Enabled = value
        '        If (value) Then
        '            m_Timer.Start()
        '        Else
        '            m_Timer.Stop()
        '        End If
        '    End Set
        'End Property

        Public Function GetHandler(ByVal tipoCampagna As TipoCampagnaPubblicitaria) As HandlerTipoCampagna
            Select Case tipoCampagna
                Case TipoCampagnaPubblicitaria.eMail : Return HandlerTipoCampagnaEMail.Instance
                Case TipoCampagnaPubblicitaria.NonImpostato : Return New NullCampagnaADVHandler
                Case TipoCampagnaPubblicitaria.Quotidiani : Return New NullCampagnaADVHandler
                Case TipoCampagnaPubblicitaria.Web : Return New NullCampagnaADVHandler
                Case TipoCampagnaPubblicitaria.Fax : Return HandlerTipoCampagnaFax.Instance
                Case TipoCampagnaPubblicitaria.SMS : Return HandlerTipoCampagnaSMS.Instance
                Case Else : Return Nothing
            End Select
        End Function

        ''' <summary>
        ''' Effettuia il controllo e l'invio sincrono delle campagne
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Check()
            SyncLock Me.exeLock
                If (Me.m_Checking) Then Return
                Me.m_Checking = True
                Dim items As New CCollection(Of CCampagnaPubblicitaria)    'Campagne che sono in conda per essere attivate
                Dim c As CCampagnaPubblicitaria
                Dim s As CalendarSchedule
                Dim cursor As CCampagnaPubblicitariaCursor = Nothing
#If Not DEBUG Then
                Try
#End If
                cursor = New CCampagnaPubblicitariaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.Attiva.Value = True
                cursor.StatoCampagna.Value = StatoCampagnaPubblicitaria.Programmata
                cursor.StatoCampagna.Operator = OP.OP_NE
                cursor.IgnoreRights = True
                While Not cursor.EOF
                    c = cursor.Item
                    s = c.Programmazione.GetNextSchedule
                    If (s IsNot Nothing) Then
                        Dim d As Nullable(Of Date) = s.CalcolaProssimaEsecuzione
                        If (d.HasValue) AndAlso (d.Value <= Now()) Then
                            Sistema.ApplicationContext.Log("Accodo la campagna: " & c.ToString)
                            'c.StatoCampagna = StatoCampagnaPubblicitaria.Programmata
                            'c.Save()
                            items.Add(c)
                        End If
                    End If
                    cursor.MoveNext()
                End While
                cursor.Dispose() : cursor = Nothing

                For Each c In items
                    s = c.Programmazione.GetNextSchedule
                    s.UltimaEsecuzione = DateUtils.Now()
                    s.ConteggioEsecuzioni += 1
                    s.Save()

                    c.StatoCampagna = StatoCampagnaPubblicitaria.Programmata
                    c.Save()

                    Dim lista As CCollection(Of CRisultatoCampagna) = c.GetListaDiInvio
                    c.Invia(lista)

                    c.StatoCampagna = StatoCampagnaPubblicitaria.Eseguita
                    c.Save()
                Next
#If Not DEBUG Then
                Catch ex As Exception
                    Sistema.Events.NotifyUnhandledException(ex)
                    Throw
                Finally
#End If
                Me.m_Checking = False
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
#If Not DEBUG Then
                End Try
#End If

            End SyncLock

        End Sub


        '        Private Sub DequeueADVs()
        '            Dim cursor As New CCampagnaPubblicitariaCursor
        '            Dim c As CCampagnaPubblicitaria = Nothing
        '#If Not Debug Then
        '            Try
        '#End If
        '                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
        '                cursor.Attiva.Value = True
        '                cursor.StatoCampagna.Value = StatoCampagnaPubblicitaria.Programmata
        '                cursor.IgnoreRights = True
        '                c = cursor.Item
        '#If Not Debug Then
        '            Catch ex As Exception
        '                Throw
        '            Finally
        '#End If
        '                cursor.Dispose()
        '#If Not Debug Then
        '            End Try
        '#End If

        '            If (c IsNot Nothing) Then
        '                Dim s As CalendarSchedule = c.Programmazione.GetNextSchedule
        '                If (s IsNot Nothing) Then
        '                    s.UltimaEsecuzione = Now()
        '                    s.ConteggioEsecuzioni += 1
        '                    s.Save()
        '                    c.StatoCampagna = StatoCampagnaPubblicitaria.Eseguita
        '                    c.Save()
        '                    Dim lista As CCollection(Of CRisultatoCampagna) = c.GetListaDiInvio
        '                    Debug.Print("Eseguo la campagna: " & c.ToString)
        '                    c.Invia(lista)
        '                End If
        '            End If

        '        End Sub

        'Private Sub TimerHandler(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        '    If m_Checking Then Return
        '    Me.m_Checking = True

        '    Me.Check()
        '    Me.m_Checking = False
        'End Sub



        Public Function GetItemByName(ByVal nomeCampagna As String) As CCampagnaPubblicitaria
            nomeCampagna = Strings.Trim(nomeCampagna)
            If (nomeCampagna = "") Then Return Nothing

            Dim cursor As CCampagnaPubblicitariaCursor = Nothing
            Try
                cursor = New CCampagnaPubblicitariaCursor
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID
                cursor.NomeCampagna.Value = nomeCampagna
                Return cursor.Item
            Catch ex As Exception
                Sistema.Events.NotifyUnhandledException(ex)
                Throw
            Finally
                If (cursor IsNot Nothing) Then cursor.Dispose() : cursor = Nothing
            End Try
        End Function

    End Class

End Namespace

Partial Public Class ADV




    Private Shared m_Campagne As CCampagneClass = Nothing

    Public Shared ReadOnly Property Campagne As CCampagneClass
        Get
            If (m_Campagne Is Nothing) Then m_Campagne = New CCampagneClass
            Return m_Campagne
        End Get
    End Property

End Class
