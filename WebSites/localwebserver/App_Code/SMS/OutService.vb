Imports FinSeA

Namespace FinSeA


    Partial Class SMSGateway

         

        Public Class OutService
            Inherits DBObjectService

            Private m_Nome As String                                'Nome del servizio
            Private m_Credito As Decimal                            'Credito residuo
            Private m_SogliaCredito As Decimal                      'Soglia credito
            Private m_NotificaA As String                           'Notifica (sotto la soglia)
            Private m_CostoSMS As Decimal                           'Costo per ogni SMS inviato
            Private m_ScadenzaCredito As Nullable(Of Date)          'Data di scadenza dell'intero credito
            Private m_Note As String                                'Note 
            Private m_Flags As Integer                              'Flags

            Public Sub New()
                Me.m_Nome = ""
                Me.m_Credito = 0.0
                Me.m_SogliaCredito = 0.0
                Me.m_NotificaA = ""
                Me.m_CostoSMS = 0.0
                Me.m_ScadenzaCredito = Nothing
                Me.m_Note = ""
                Me.m_Flags = 0
            End Sub


            ''' <summary>
            ''' Restituisce o imposta il nome del servizio
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Nome As String
                Get
                    Return Me.m_Nome
                End Get
                Set(value As String)
                    value = Trim(value)
                    Dim oldValue As String = Me.m_Nome
                    If (oldValue = value) Then Exit Property
                    Me.m_Nome = value
                    Me.DoChanged("Nome", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta il credito residuo
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property CreditoResiduo As Decimal
                Get
                    Return Me.m_Credito
                End Get
                Set(value As Decimal)
                    Me.m_Credito = value
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta il costo per il singolo SMS inviato
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property CostoSMS As Decimal 
                Get
                    Return Me.m_CostoSMS
                End Get
                Set(value As Decimal)
                    Dim oldValue As Decimal = Me.m_CostoSMS
                    If (oldValue = value) Then Exit Property
                    Me.m_CostoSMS = value
                    Me.DoChanged("CostoSMS", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta la data di scadenza del credito
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property ScadenzaCredito As Nullable (of Date)
                Get
                    Return Me.m_ScadenzaCredito
                End Get
                Set(value As Nullable(Of Date))
                    Dim oldValue As Nullable(Of Date) = Me.m_ScadenzaCredito
                    If (oldValue.Equals(value)) Then Exit Property
                    Me.m_ScadenzaCredito = value
                    Me.DoChanged("ScadenzaCredito", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta delle note aggiuntive
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Note As String 
                Get
                    Return Me.m_Note
                End Get
                Set(value As String)
                    Dim oldValue As String = Me.m_Note
                    If (oldValue = value) Then Exit Property
                    Me.m_Note = value
                    Me.DoChanged("Note", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta la soglia del credito al di sotto della quale viene inviato un messaggio email all'indirizzo specificato da NotificaA
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property SogliaCredito As Decimal 
                Get
                    Return Me.m_SogliaCredito
                End Get
                Set(value As Decimal)
                    Dim oldValue As Decimal = Me.m_SogliaCredito
                    If (oldValue = value) Then Exit Property
                    Me.m_SogliaCredito = value
                    Me.DoChanged("SogliaCredito", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta l'indirizzo e-mail a cui viene inviata la mail di notifica sotto la soglia credito
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property NotificaA As String 
                Get
                    Return Me.m_NotificaA
                End Get
                Set(value As String)
                    Dim oldValue As String = Me.m_NotificaA
                    If (oldValue = value) Then Exit Property
                    Me.m_NotificaA = value
                    Me.DoChanged("NotificaA", value, oldValue)
                End Set
            End Property

            ''' <summary>
            ''' Restituisce o imposta dei flags aggiuntivi
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Flags As Integer
                Get
                    Return Me.m_Flags
                End Get
                Set(value As Integer)
                    Dim oldValue As Integer = Me.m_Flags
                    If (oldValue = value) Then Exit Property
                    Me.m_Flags = value
                    Me.DoChanged("Flags", value, oldValue)
                End Set
            End Property

            Protected Overrides Sub Load(dbRis As Data.IDataReader)
                MyBase.Load(dbRis)
                Me.m_Nome = ToStr(dbRis("Nome"))
                Me.m_Credito = ToValuta(dbRis("Credito"))
                Me.m_CostoSMS = ToValuta(dbRis("CostoSMS"))
                Me.m_ScadenzaCredito = ParseDate(dbRis("ScadenzaCredito"))
                Me.m_Note = ToStr(dbRis("Note"))
                Me.m_SogliaCredito = ToValuta(dbRis("SogliaCredito"))
                Me.m_NotificaA = ToStr(dbRis("NotificaA"))
                Me.m_Flags = ToInt(dbRis("Flags"))
            End Sub

             

            Protected Overrides Function GetConnection() As DBConnection
                Return SMSGateway.Database
            End Function

            Protected Overrides Function GetInsertCommand() As String
                Dim dbSQL As String = ""
                dbSQL &= "INSERT INTO tbl_OutServices "
                dbSQL &= "(Nome, Credito, CostoSMS, ScadenzaCredito, [Note], SogliaCredito, NotificaA, Flags) "
                dbSQL &= " VALUES "
                dbSQL &= "(" & DBStr(Me.Nome) & ", " & DBNumber(Me.CreditoResiduo) & ", " & DBNumber(Me.CostoSMS) & ", " & DBDate(Me.ScadenzaCredito) & ", " & DBStr(Me.Note) & ", " & DBNumber(Me.SogliaCredito) & ", " & DBStr(Me.NotificaA) & ", " & DBNumber(Me.Flags) & ")"
                Return dbSQL
            End Function

            Protected Overrides Function GetUpdateCommand() As String
                Dim dbSQL As String = ""
                dbSQL = "UPDATE tbl_OutServices SET "
                dbSQL &= "Nome = " & DBStr(Me.Nome) & ", "
                dbSQL &= "Credito= " & DBNumber(Me.CreditoResiduo) & ", "
                dbSQL &= "CostoSMS= " & DBNumber(Me.CostoSMS) & ", "
                dbSQL &= "ScadenzaCredito= " & DBDate(Me.ScadenzaCredito) & ", "
                dbSQL &= "[Note]= " & DBStr(Me.Note) & ", "
                dbSQL &= "[SogliaCredito]= " & DBNumber(Me.SogliaCredito) & ", "
                dbSQL &= "[NotificaA]= " & DBStr(Me.NotificaA) & ", "
                dbSQL &= "[Flags]= " & DBNumber(Me.Flags)
                dbSQL &= " WHERE [ID]=" & Me.m_ID
                Return dbSQL
            End Function
        End Class

    End Class

End Namespace