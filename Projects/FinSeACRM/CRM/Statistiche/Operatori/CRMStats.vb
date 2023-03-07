Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Public Class CustomerCalls

    Public Class CRMStats
        Inherits DBObjectBase

        Public IDPuntoOperativo As Integer = 0
        Public IDOperatore As Integer = 0
        Public Data As Date

        Public InCallNum As Integer = 0
        Public InCallMinLen As Double = 0
        Public InCallMaxLen As Double = 0
        Public InCallTotLen As Double = 0
        Public InCallMinWait As Double = 0
        Public InCallMaxWait As Double = 0
        Public InCallTotWait As Double = 0
        Public InCallCost As Decimal = 0

        Public OutCallNum As Integer = 0
        Public OutCallMinLen As Double = 0
        Public OutCallMaxLen As Double = 0
        Public OutCallTotLen As Double = 0
        Public OutCallMinWait As Double = 0
        Public OutCallMaxWait As Double = 0
        Public OutCallTotWait As Double = 0
        Public OutCallCost As Decimal = 0

        Public InDateNum As Integer = 0
        Public InDateMinLen As Double = 0
        Public InDateMaxLen As Double = 0
        Public InDateTotLen As Double = 0
        Public InDateMinWait As Double = 0
        Public InDateMaxWait As Double = 0
        Public InDateTotWait As Double = 0
        Public InDateCost As Decimal = 0

        Public OutDateNum As Integer = 0
        Public OutDateMinLen As Double = 0
        Public OutDateMaxLen As Double = 0
        Public OutDateTotLen As Double = 0
        Public OutDateMinWait As Double = 0
        Public OutDateMaxWait As Double = 0
        Public OutDateTotWait As Double = 0
        Public OutDateCost As Decimal = 0

        Public InSMSNum As Integer = 0
        Public InSMSMinLen As Double = 0
        Public InSMSMaxLen As Double = 0
        Public InSMSTotLen As Double = 0
        Public InSMSCost As Decimal = 0

        Public OutSMSNum As Integer = 0
        Public OutSMSMinLen As Double = 0
        Public OutSMSMaxLen As Double = 0
        Public OutSMSTotLen As Double = 0
        Public OutSMSCost As Decimal = 0

        Public InFAXNum As Integer = 0
        Public InFAXMinLen As Double = 0
        Public InFAXMaxLen As Double = 0
        Public InFAXTotLen As Double = 0
        Public InFAXCost As Decimal = 0

        Public OutFAXNum As Integer = 0
        Public OutFAXMinLen As Double = 0
        Public OutFAXMaxLen As Double = 0
        Public OutFAXTotLen As Double = 0
        Public OutFAXCost As Decimal = 0

        Public InEMAILNum As Integer = 0
        Public InEMAILMinLen As Double = 0
        Public InEMAILMaxLen As Double = 0
        Public InEMAILTotLen As Double = 0
        Public InEMAILCost As Decimal = 0

        Public OutEMAILNum As Integer = 0
        Public OutEMAILMinLen As Double = 0
        Public OutEMAILMaxLen As Double = 0
        Public OutEMAILTotLen As Double = 0
        Public OutEMAILCost As Decimal = 0

        Public InTelegramNum As Integer = 0
        Public InTelegramMinLen As Double = 0
        Public InTelegramMaxLen As Double = 0
        Public InTelegramTotLen As Double = 0
        Public InTelegramCost As Decimal = 0

        Public OutTelegramNum As Integer = 0
        Public OutTelegramMinLen As Double = 0
        Public OutTelegramMaxLen As Double = 0
        Public OutTelegramTotLen As Double = 0
        Public OutTelegramCost As Decimal = 0

        Public NumAppuntamentiPrevisti As Integer = 0
        Public NumTelefonatePreviste As Integer = 0

        Public Ricalcola As Boolean = False

        Protected Overrides Function GetConnection() As CDBConnection
            Return CustomerCalls.CRM.StatsDB
        End Function

        Public Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMStats"
        End Function

        Protected Overrides Function LoadFromRecordset(reader As DBReader) As Boolean
            Me.IDPuntoOperativo = reader.Read("IDPuntoOperativo", Me.IDPuntoOperativo)
            Me.IDOperatore = reader.Read("IDOperatore", Me.IDOperatore)
            Me.Data = reader.Read("Data", Me.Data)

            Me.InCallNum = reader.Read("InCallNum", Me.InCallNum)
            Me.InCallMinLen = reader.Read("InCallMinLen", Me.InCallMinLen)
            Me.InCallMaxLen = reader.Read("InCallMaxLen", Me.InCallMaxLen)
            Me.InCallTotLen = reader.Read("InCallTotLen", Me.InCallTotLen)
            Me.InCallMinWait = reader.Read("InCallMinWait", Me.InCallMinWait)
            Me.InCallMaxWait = reader.Read("InCallMaxWait", Me.InCallMaxWait)
            Me.InCallTotWait = reader.Read("InCallTotWait", Me.InCallTotWait)
            Me.InCallCost = reader.Read("InCallCost", Me.InCallCost)

            Me.OutCallNum = reader.Read("OutCallNum", Me.OutCallNum)
            Me.OutCallMinLen = reader.Read("OutCallMinLen", Me.OutCallMinLen)
            Me.OutCallMaxLen = reader.Read("OutCallMaxLen", Me.OutCallMaxLen)
            Me.OutCallTotLen = reader.Read("OutCallTotLen", Me.OutCallTotLen)
            Me.OutCallMinWait = reader.Read("OutCallMinWait", Me.OutCallMinWait)
            Me.OutCallMaxWait = reader.Read("OutCallMaxWait", Me.OutCallMaxWait)
            Me.OutCallTotWait = reader.Read("OutCallTotWait", Me.OutCallTotWait)
            Me.OutCallCost = reader.Read("OutCallCost", Me.OutCallCost)

            Me.InDateNum = reader.Read("InDateNum", Me.InDateNum)
            Me.InDateMinLen = reader.Read("InDateMinLen", Me.InDateMinLen)
            Me.InDateMaxLen = reader.Read("InDateMaxLen", Me.InDateMaxLen)
            Me.InDateTotLen = reader.Read("InDateTotLen", Me.InDateTotLen)
            Me.InDateMinWait = reader.Read("InDateMinWait", Me.InDateMinWait)
            Me.InDateMaxWait = reader.Read("InDateMaxWait", Me.InDateMaxWait)
            Me.InDateTotWait = reader.Read("InDateTotWait", Me.InDateTotWait)
            Me.InDateCost = reader.Read("InDateCost", Me.InDateCost)

            Me.OutDateNum = reader.Read("OutDateNum", Me.OutDateNum)
            Me.OutDateMinLen = reader.Read("OutDateMinLen", Me.OutDateMinLen)
            Me.OutDateMaxLen = reader.Read("OutDateMaxLen", Me.OutDateMaxLen)
            Me.OutDateTotLen = reader.Read("OutDateTotLen", Me.OutDateTotLen)
            Me.OutDateMinWait = reader.Read("OutDateMinWait", Me.OutDateMinWait)
            Me.OutDateMaxWait = reader.Read("OutDateMaxWait", Me.OutDateMaxWait)
            Me.OutDateTotWait = reader.Read("OutDateTotWait", Me.OutDateTotWait)
            Me.OutDateCost = reader.Read("OutDateCost", Me.OutDateCost)


            Me.InSMSNum = reader.Read("InSMSNum", Me.InSMSNum)
            Me.InSMSMinLen = reader.Read("InSMSMinLen", Me.InSMSMinLen)
            Me.InSMSMaxLen = reader.Read("InSMSMaxLen", Me.InSMSMaxLen)
            Me.InSMSTotLen = reader.Read("InSMSTotLen", Me.InSMSTotLen)
            Me.InSMSCost = reader.Read("InSMSCost", Me.InSMSCost)

            Me.OutSMSNum = reader.Read("OutSMSNum", Me.OutSMSNum)
            Me.OutSMSMinLen = reader.Read("OutSMSMinLen", Me.OutSMSMinLen)
            Me.OutSMSMaxLen = reader.Read("OutSMSMaxLen", Me.OutSMSMaxLen)
            Me.OutSMSTotLen = reader.Read("OutSMSTotLen", Me.OutSMSTotLen)
            Me.OutSMSCost = reader.Read("OutSMSCost", Me.OutSMSCost)

            Me.InFAXNum = reader.Read("InFAXNum", Me.InFAXNum)
            Me.InFAXMinLen = reader.Read("InFAXMinLen", Me.InFAXMinLen)
            Me.InFAXMaxLen = reader.Read("InFAXMaxLen", Me.InFAXMaxLen)
            Me.InFAXTotLen = reader.Read("InFAXTotLen", Me.InFAXTotLen)
            Me.InFAXCost = reader.Read("INFAXCost", Me.InFAXCost)

            Me.OutFAXNum = reader.Read("OutFAXNum", Me.OutFAXNum)
            Me.OutFAXMinLen = reader.Read("OutFAXMinLen", Me.OutFAXMinLen)
            Me.OutFAXMaxLen = reader.Read("OutFAXMaxLen", Me.OutFAXMaxLen)
            Me.OutFAXTotLen = reader.Read("OutFAXTotLen", Me.OutFAXTotLen)
            Me.OutFAXCost = reader.Read("OutFAXCost", Me.OutFAXCost)

            Me.InEMAILNum = reader.Read("InEMAILNum", Me.InEMAILNum)
            Me.InEMAILMinLen = reader.Read("InEMAILMinLen", Me.InEMAILMinLen)
            Me.InEMAILMaxLen = reader.Read("InEMAILMinLen", Me.InEMAILMinLen)
            Me.InEMAILTotLen = reader.Read("InEMAILTotLen", Me.InEMAILTotLen)

            Me.OutEMAILNum = reader.Read("OutEMAILNum", Me.OutEMAILNum)
            Me.OutEMAILMinLen = reader.Read("OutEMAILMinLen", Me.OutEMAILMinLen)
            Me.OutEMAILMaxLen = reader.Read("OutEMAILMaxLen", Me.OutEMAILMaxLen)
            Me.OutEMAILTotLen = reader.Read("OutEMAILTotLen", Me.OutEMAILTotLen)

            Me.InTelegramNum = reader.Read("InTelegramNum", Me.InTelegramNum)
            Me.InTelegramMinLen = reader.Read("InTelegramMinLen", Me.InTelegramMinLen)
            Me.InTelegramMaxLen = reader.Read("InTelegramMinLen", Me.InTelegramMinLen)
            Me.InTelegramTotLen = reader.Read("InTelegramTotLen", Me.InTelegramTotLen)
            Me.InTelegramCost = reader.Read("InTelegramCost", Me.InTelegramCost)

            Me.OutTelegramNum = reader.Read("OutTelegramNum", Me.OutTelegramNum)
            Me.OutTelegramMinLen = reader.Read("OutTelegramMinLen", Me.OutTelegramMinLen)
            Me.OutTelegramMaxLen = reader.Read("OutTelegramMaxLen", Me.OutTelegramMaxLen)
            Me.OutTelegramTotLen = reader.Read("OutTelegramTotLen", Me.OutTelegramTotLen)
            Me.OutTelegramCost = reader.Read("OutTelegramCost", Me.OutTelegramCost)

            Me.NumAppuntamentiPrevisti = reader.Read("NumAppuntamentiPrevisti", Me.NumAppuntamentiPrevisti)
            Me.NumTelefonatePreviste = reader.Read("NumTelefonatePreviste", Me.NumTelefonatePreviste)


            Me.Ricalcola = reader.Read("Ricalcola", Me.Ricalcola)

            Return MyBase.LoadFromRecordset(reader)
        End Function

        Protected Overrides Function SaveToRecordset(writer As DBWriter) As Boolean
            writer.Write("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.Write("IDOperatore", Me.IDOperatore)
            writer.Write("Data", Me.Data)

            writer.Write("InCallNum", Me.InCallNum)
            writer.Write("InCallMinLen", Me.InCallMinLen)
            writer.Write("InCallMaxLen", Me.InCallMaxLen)
            writer.Write("InCallTotLen", Me.InCallTotLen)
            writer.Write("InCallMinWait", Me.InCallMinWait)
            writer.Write("InCallMaxWait", Me.InCallMaxWait)
            writer.Write("InCallTotWait", Me.InCallTotWait)
            writer.Write("InCallCost", Me.InCallCost)

            writer.Write("OutCallNum", Me.OutCallNum)
            writer.Write("OutCallMinLen", Me.OutCallMinLen)
            writer.Write("OutCallMaxLen", Me.OutCallMaxLen)
            writer.Write("OutCallTotLen", Me.OutCallTotLen)
            writer.Write("OutCallMinWait", Me.OutCallMinWait)
            writer.Write("OutCallMaxWait", Me.OutCallMaxWait)
            writer.Write("OutCallTotWait", Me.OutCallTotWait)
            writer.Write("OutCallCost", Me.OutCallCost)

            writer.Write("InDateNum", Me.InDateNum)
            writer.Write("InDateMinLen", Me.InDateMinLen)
            writer.Write("InDateMaxLen", Me.InDateMaxLen)
            writer.Write("InDateTotLen", Me.InDateTotLen)
            writer.Write("InDateMinWait", Me.InDateMinWait)
            writer.Write("InDateMaxWait", Me.InDateMaxWait)
            writer.Write("InDateTotWait", Me.InDateTotWait)
            writer.Write("InDateCost", Me.InDateCost)

            writer.Write("OutDateNum", Me.OutDateNum)
            writer.Write("OutDateMinLen", Me.OutDateMinLen)
            writer.Write("OutDateMaxLen", Me.OutDateMaxLen)
            writer.Write("OutDateTotLen", Me.OutDateTotLen)
            writer.Write("OutDateMinWait", Me.OutDateMinWait)
            writer.Write("OutDateMaxWait", Me.OutDateMaxWait)
            writer.Write("OutDateTotWait", Me.OutDateTotWait)
            writer.Write("OutDateCost", Me.OutDateCost)

            writer.Write("InSMSNum", Me.InSMSNum)
            writer.Write("InSMSMinLen", Me.InSMSMinLen)
            writer.Write("InSMSMaxLen", Me.InSMSMaxLen)
            writer.Write("InSMSTotLen", Me.InSMSTotLen)
            writer.Write("InSMSCost", Me.InSMSCost)

            writer.Write("OutSMSNum", Me.OutSMSNum)
            writer.Write("OutSMSMinLen", Me.OutSMSMinLen)
            writer.Write("OutSMSMaxLen", Me.OutSMSMaxLen)
            writer.Write("OutSMSTotLen", Me.OutSMSTotLen)
            writer.Write("OutSMSCost", Me.OutSMSCost)

            writer.Write("InFAXNum", Me.InFAXNum)
            writer.Write("InFAXMinLen", Me.InFAXMinLen)
            writer.Write("InFAXMaxLen", Me.InFAXMaxLen)
            writer.Write("InFAXTotLen", Me.InFAXTotLen)
            writer.Write("InFAXCost", Me.InFAXCost)

            writer.Write("OutFAXNum", Me.OutFAXNum)
            writer.Write("OutFAXMinLen", Me.OutFAXMinLen)
            writer.Write("OutFAXMaxLen", Me.OutFAXMaxLen)
            writer.Write("OutFAXTotLen", Me.OutFAXTotLen)
            writer.Write("OutFAXCost", Me.OutFAXCost)

            writer.Write("InEMAILNum", Me.InEMAILNum)
            writer.Write("InEMAILMinLen", Me.InEMAILMinLen)
            writer.Write("InEMAILMinLen", Me.InEMAILMinLen)
            writer.Write("InEMAILTotLen", Me.InEMAILTotLen)

            writer.Write("OutEMAILNum", Me.OutEMAILNum)
            writer.Write("OutEMAILMinLen", Me.OutEMAILMinLen)
            writer.Write("OutEMAILMaxLen", Me.OutEMAILMaxLen)
            writer.Write("OutEMAILTotLen", Me.OutEMAILTotLen)

            writer.Write("InTelegramNum", Me.InTelegramNum)
            writer.Write("InTelegramMinLen", Me.InTelegramMinLen)
            writer.Write("InTelegramMinLen", Me.InTelegramMinLen)
            writer.Write("InTelegramTotLen", Me.InTelegramTotLen)
            writer.Write("InTelegramCost", Me.InTelegramCost)

            writer.Write("OutTelegramNum", Me.OutTelegramNum)
            writer.Write("OutTelegramMinLen", Me.OutTelegramMinLen)
            writer.Write("OutTelegramMaxLen", Me.OutTelegramMaxLen)
            writer.Write("OutTelegramTotLen", Me.OutTelegramTotLen)
            writer.Write("OutTelegramCost", Me.OutTelegramCost)

            writer.Write("NumAppuntamentiPrevisti", Me.NumAppuntamentiPrevisti)
            writer.Write("NumTelefonatePreviste", Me.NumTelefonatePreviste)

            writer.Write("Ricalcola", Me.Ricalcola)

            Return MyBase.SaveToRecordset(writer)
        End Function

        Private Sub NotifyVisitaIn(ByVal c As CVisita)
            If (Me.InDateNum = 0) Then
                Me.InDateMinLen = c.Durata
                Me.InDateMaxLen = c.Durata
                Me.InDateMinWait = c.Attesa
                Me.InDateMaxWait = c.Attesa
                Me.InDateCost = Formats.ToValuta(c.Costo)
            Else
                Me.InDateMinLen = Math.Min(c.Durata, Me.InDateMinLen)
                Me.InDateMaxLen = Math.Max(c.Durata, Me.InDateMaxLen)
                Me.InDateMinWait = Math.Min(c.Attesa, Me.InDateMinWait)
                Me.InDateMaxWait = Math.Max(c.Attesa, Me.InDateMaxWait)
                Me.InDateCost += Formats.ToValuta(c.Costo)
            End If
            Me.InDateNum += 1
            Me.InDateTotLen += c.Durata
            Me.InDateTotWait += c.Attesa
        End Sub

        Private Sub NotifyVisitaOut(ByVal c As CVisita)
            If (Me.OutDateNum = 0) Then
                Me.OutDateMinLen = c.Durata
                Me.OutDateMaxLen = c.Durata
                Me.OutDateMinWait = c.Attesa
                Me.OutDateMaxWait = c.Attesa
                Me.OutDateCost = Formats.ToValuta(c.Costo)
            Else
                Me.OutDateMinLen = Math.Min(c.Durata, Me.OutDateMinLen)
                Me.OutDateMaxLen = Math.Max(c.Durata, Me.OutDateMaxLen)
                Me.OutDateMinWait = Math.Min(c.Attesa, Me.OutDateMinWait)
                Me.OutDateMaxWait = Math.Max(c.Attesa, Me.OutDateMaxWait)
                Me.OutDateCost += Formats.ToValuta(c.Costo)
            End If
            Me.OutDateNum += 1
            Me.OutDateTotLen += c.Durata
            Me.OutDateTotWait += c.Attesa
        End Sub

        Private Sub NotifyTelefonataIn(ByVal c As CTelefonata)
            If (Me.InCallNum = 0) Then
                Me.InCallMinLen = c.Durata
                Me.InCallMaxLen = c.Durata
                Me.InCallMinWait = c.Attesa
                Me.InCallMaxWait = c.Attesa
                Me.InCallCost = Formats.ToValuta(c.Costo)
            Else
                Me.InCallMinLen = Math.Min(c.Durata, Me.InCallMinLen)
                Me.InCallMaxLen = Math.Max(c.Durata, Me.InCallMaxLen)
                Me.InCallMinWait = Math.Min(c.Attesa, Me.InCallMinWait)
                Me.InCallMaxWait = Math.Max(c.Attesa, Me.InCallMaxWait)
                Me.InCallCost += Formats.ToValuta(c.Costo)
            End If
            Me.InCallNum += 1
            Me.InCallTotLen += c.Durata
            Me.InCallTotWait += c.Attesa
        End Sub

        Private Sub NotifyTelefonataOut(ByVal c As CTelefonata)
            If (Me.OutCallNum = 0) Then
                Me.OutCallMinLen = c.Durata
                Me.OutCallMaxLen = c.Durata
                Me.OutCallMinWait = c.Attesa
                Me.OutCallMaxWait = c.Attesa
                Me.OutCallCost = Formats.ToValuta(c.Costo)
            Else
                Me.OutCallMinLen = Math.Min(c.Durata, Me.OutCallMinLen)
                Me.OutCallMaxLen = Math.Max(c.Durata, Me.OutCallMaxLen)
                Me.OutCallMinWait = Math.Min(c.Attesa, Me.OutCallMinWait)
                Me.OutCallMaxWait = Math.Min(c.Attesa, Me.OutCallMaxWait)
                Me.OutCallCost += Formats.ToValuta(c.Costo)
            End If
            Me.OutCallNum += 1
            Me.OutCallTotLen += c.Durata
            Me.OutCallTotWait += c.Attesa
        End Sub

        Private Sub NotifySMSIn(ByVal c As SMSMessage)
            If (Me.InSMSNum = 0) Then
                Me.InSMSMinLen = Len(c.Note)
                Me.InSMSMaxLen = Len(c.Note)
                Me.InSMSCost = Formats.ToValuta(c.Costo)
            Else
                Me.InSMSMinLen = Math.Min(Len(c.Note), Me.InSMSMinLen)
                Me.InSMSMaxLen = Math.Max(Len(c.Note), Me.InSMSMaxLen)
                Me.InSMSCost += Formats.ToValuta(c.Costo)
            End If
            Me.InSMSNum += 1
            Me.InSMSTotLen += Len(c.Note)
        End Sub

        Private Sub NotifySMSOut(ByVal c As SMSMessage)
            If (Me.OutSMSNum = 0) Then
                Me.OutSMSMinLen = Len(c.Note)
                Me.OutSMSMaxLen = Len(c.Note)
                Me.OutSMSCost = Formats.ToValuta(c.Costo)
            Else
                Me.OutSMSMinLen = Math.Min(Len(c.Note), Me.OutSMSMinLen)
                Me.OutSMSMaxLen = Math.Max(Len(c.Note), Me.OutSMSMaxLen)
                Me.OutSMSCost += Formats.ToValuta(c.Costo)
            End If
            Me.OutSMSNum += 1
            Me.OutSMSTotLen += Len(c.Note)
        End Sub

        Private Sub NotifyFAXIn(ByVal c As FaxDocument)
            If (Me.InFAXNum = 0) Then
                Me.InFAXMinLen = Len(c.Note)
                Me.InFAXMaxLen = Len(c.Note)
                Me.InFAXCost = Formats.ToValuta(c.Costo)
            Else
                Me.InFAXMinLen = Math.Min(Len(c.Note), Me.InFAXMinLen)
                Me.InFAXMaxLen = Math.Max(Len(c.Note), Me.InFAXMaxLen)
                Me.InFAXCost += Formats.ToValuta(c.Costo)
            End If
            Me.InFAXNum += 1
            Me.InFAXTotLen += Len(c.Note)
        End Sub

        Private Sub NotifyFAXOut(ByVal c As FaxDocument)
            If (Me.OutFAXNum = 0) Then
                Me.OutFAXMinLen = Len(c.Note)
                Me.OutFAXMaxLen = Len(c.Note)
                Me.OutFAXCost = Formats.ToValuta(c.Costo)
            Else
                Me.OutFAXMinLen = Math.Min(Len(c.Note), Me.OutFAXMinLen)
                Me.OutFAXMaxLen = Math.Max(Len(c.Note), Me.OutFAXMaxLen)
                Me.OutFAXCost += Formats.ToValuta(c.Costo)
            End If
            Me.OutFAXNum += 1
            Me.OutFAXTotLen += Len(c.Note)
        End Sub

        Private Sub NotifyTelegramIn(ByVal c As CTelegramma)
            If (Me.InTelegramNum = 0) Then
                Me.InTelegramMinLen = Len(c.Note)
                Me.InTelegramMaxLen = Len(c.Note)
                Me.InTelegramCost = Formats.ToValuta(c.Costo)
            Else
                Me.InTelegramMinLen = Math.Min(Len(c.Note), Me.InTelegramMinLen)
                Me.InTelegramMaxLen = Math.Max(Len(c.Note), Me.InTelegramMaxLen)
                Me.InTelegramCost += Formats.ToValuta(c.Costo)
            End If
            Me.InTelegramNum += 1
            Me.InTelegramTotLen += Len(c.Note)
        End Sub

        Private Sub NotifyTelegramOut(ByVal c As CTelegramma)
            If (Me.OutTelegramNum = 0) Then
                Me.OutTelegramMinLen = Len(c.Note)
                Me.OutTelegramMaxLen = Len(c.Note)
                Me.OutTelegramCost = Formats.ToValuta(c.Costo)
            Else
                Me.OutTelegramMinLen = Math.Min(Len(c.Note), Me.OutTelegramMinLen)
                Me.OutTelegramMaxLen = Math.Max(Len(c.Note), Me.OutTelegramMaxLen)
                Me.OutTelegramCost += Formats.ToValuta(c.Costo)
            End If
            Me.OutTelegramNum += 1
            Me.OutTelegramTotLen += Len(c.Note)
        End Sub

        Public Sub NotifyNew(ByVal c As CContattoUtente)
            If (TypeOf (c) Is CVisita) Then
                If (c.Ricevuta) Then
                    Me.NotifyVisitaIn(c)
                Else
                    Me.NotifyVisitaOut(c)
                End If
            ElseIf (TypeOf (c) Is CTelefonata) Then
                If (c.Ricevuta) Then
                    Me.NotifyTelefonataIn(c)
                Else
                    Me.NotifyTelefonataOut(c)
                End If
            ElseIf (TypeOf (c) Is SMSMessage) Then
                If (c.Ricevuta) Then
                    Me.NotifySMSIn(c)
                Else
                    Me.NotifySMSOut(c)
                End If
            ElseIf (TypeOf (c) Is FaxDocument) Then
                If (c.Ricevuta) Then
                    Me.NotifyFAXIn(c)
                Else
                    Me.NotifyFAXOut(c)
                End If
            ElseIf (TypeOf (c) Is CTelegramma) Then
                If (c.Ricevuta) Then
                    Me.NotifyTelegramIn(c)
                Else
                    Me.NotifyTelegramOut(c)
                End If
                'ElseIf (TypeOf (c) Is email) Then
                '    If (c.Ricevuta) Then
                '        Me.NotifySMSIn(c)
                '    Else
                '        Me.NotifySMSOut(c)
                '    End If
            End If

        End Sub

        ''' <summary>
        ''' Ricalcola le statistiche sulla base dell'effettivo contenuto delle tabelle
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Aggiorna()
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                Dim toDate As Date = Calendar.DateAdd(DateInterval.Day, 1, Me.Data)
                Dim dbSQL As String = "SELECT [ClassName], [Ricevuta], Count(*) As [Num],  Min([Durata]) As [MinLen], Max([Durata]) As [MaxLen], Sum([Durata]) As [TotLen], Min([Attesa]) As [MinWait], Max([Attesa]) As [MaxWait], Sum([Attesa]) As [TotWait], Sum([Costo]) As [TotCost] FROM [tbl_Telefonate] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND [IDPuntoOperativo]=" & DBUtils.DBNumber(Me.IDPuntoOperativo) & " AND [IDOperatore]=" & Me.IDOperatore & " AND [DataStr]>='" & DBUtils.ToDBDateStr(Data) & "' AND [DataStr]<'" & DBUtils.ToDBDateStr(toDate) & "' GROUP BY [ClassName], [Ricevuta]"
                dbRis = CRM.TelDB.ExecuteReader(dbSQL)
                While (dbRis.Read)
                    Dim className As String = Formats.ToString(dbRis("ClassName"))
                    Dim ricevuta As Boolean = Formats.ToBool(dbRis("Ricevuta"))

                    Dim num As Integer = Formats.ToInteger(dbRis("Num"))
                    Dim MinLen As Double = Formats.ToDouble(dbRis("MinLen"))
                    Dim MaxLen As Double = Formats.ToDouble(dbRis("MaxLen"))
                    Dim TotLen As Double = Formats.ToDouble(dbRis("TotLen"))
                    Dim MinWait As Double = Formats.ToDouble(dbRis("MinWait"))
                    Dim MaxWait As Double = Formats.ToDouble(dbRis("MaxWait"))
                    Dim TotWait As Double = Formats.ToDouble(dbRis("TotWait"))
                    Dim TotCost As Double = Formats.ToDouble(dbRis("TotCost"))

                    'Dim dbRis1 As System.Data.IDataReader = Me.StatsDB.ExecuteReader("SELECT * FROM [tbl_CRMStats] WHERE [idOperatore]=" & DBUtils.DBNumber(idOperatore) & " AND [Data]=" & DBUtils.DBDate(Data))
                    'Dim item As New CRMStats
                    'If (dbRis1.Read) Then Me.StatsDB.Load(item, dbRis1)
                    'dbRis1.Dispose()

                    'item.IDOperatore = idOperatore
                    'item.Data = Data
                    If (ricevuta) Then
                        Select Case className
                            Case "CTelefonata"
                                Me.InCallMaxLen = MaxLen
                                Me.InCallMaxWait = MaxWait
                                Me.InCallMinLen = MinLen
                                Me.InCallMinWait = MinWait
                                Me.InCallNum = num
                                Me.InCallCost = TotCost
                            Case "CVisita"
                                Me.InDateMaxLen = MaxLen
                                Me.InDateMaxWait = MaxWait
                                Me.InDateMinLen = MinLen
                                Me.InDateMinWait = MinWait
                                Me.InDateNum = num
                                Me.InDateCost = TotCost
                            Case "SMSMessage"
                                Me.InSMSNum = num
                                Me.InSMSCost = TotCost
                            Case "FaxDocument"
                                Me.InFAXNum = num
                                Me.InFAXCost = TotCost
                            Case "CTelegramma"
                                Me.InTelegramNum = num
                                Me.InTelegramCost = TotCost
                        End Select
                    Else
                        Select Case className
                            Case "CTelefonata"
                                Me.OutCallMaxLen = MaxLen
                                Me.OutCallMaxWait = MaxWait
                                Me.OutCallMinLen = MinLen
                                Me.OutCallMinWait = MinWait
                                Me.OutCallNum = num
                                Me.OutCallCost = TotCost
                            Case "CVisita"
                                Me.OutDateMaxLen = MaxLen
                                Me.OutDateMaxWait = MaxWait
                                Me.OutDateMinLen = MinLen
                                Me.OutDateMinWait = MinWait
                                Me.OutDateNum = num
                                Me.OutDateCost = TotCost
                            Case "SMSMessage"
                                Me.OutSMSNum = num
                                Me.OutSMSCost = TotCost
                            Case "FaxDocument"
                                Me.OutFAXNum = num
                                Me.OutFAXCost = TotCost
                            Case "CTelegramma"
                                Me.OutTelegramNum = num
                                Me.OutTelegramCost = TotCost
                        End Select
                    End If
                End While
                dbRis.Dispose()
                dbRis = Nothing

                Me.Ricalcola = False
                Me.Save(True)
            Catch ex As Exception
                Throw
            Finally
                If (dbRis IsNot Nothing) Then dbRis.Dispose() : dbRis = Nothing
            End Try
        End Sub

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            writer.WriteAttribute("IDPuntoOperativo", Me.IDPuntoOperativo)
            writer.WriteAttribute("IDOperatore", Me.IDOperatore)
            writer.WriteAttribute("Data", Me.Data)

            writer.WriteAttribute("InCallNum", Me.InCallNum)
            writer.WriteAttribute("InCallMinLen", Me.InCallMinLen)
            writer.WriteAttribute("InCallMaxLen", Me.InCallMaxLen)
            writer.WriteAttribute("InCallTotLen", Me.InCallTotLen)
            writer.WriteAttribute("InCallMinWait", Me.InCallMinWait)
            writer.WriteAttribute("InCallMaxWait", Me.InCallMaxWait)
            writer.WriteAttribute("InCallTotWait", Me.InCallTotWait)
            writer.WriteAttribute("InCallCost", Me.InCallCost)

            writer.WriteAttribute("OutCallNum", Me.OutCallNum)
            writer.WriteAttribute("OutCallMinLen", Me.OutCallMinLen)
            writer.WriteAttribute("OutCallMaxLen", Me.OutCallMaxLen)
            writer.WriteAttribute("OutCallTotLen", Me.OutCallTotLen)
            writer.WriteAttribute("OutCallMinWait", Me.OutCallMinWait)
            writer.WriteAttribute("OutCallMaxWait", Me.OutCallMaxWait)
            writer.WriteAttribute("OutCallTotWait", Me.OutCallTotWait)
            writer.WriteAttribute("OutCallCost", Me.InCallCost)

            writer.WriteAttribute("InDateNum", Me.InDateNum)
            writer.WriteAttribute("InDateMinLen", Me.InDateMinLen)
            writer.WriteAttribute("InDateMaxLen", Me.InDateMaxLen)
            writer.WriteAttribute("InDateTotLen", Me.InDateTotLen)
            writer.WriteAttribute("InDateMinWait", Me.InDateMinWait)
            writer.WriteAttribute("InDateMaxWait", Me.InDateMaxWait)
            writer.WriteAttribute("InDateTotWait", Me.InDateTotWait)
            writer.WriteAttribute("InDateCost", Me.InCallCost)

            writer.WriteAttribute("OutDateNum", Me.OutDateNum)
            writer.WriteAttribute("OutDateMinLen", Me.OutDateMinLen)
            writer.WriteAttribute("OutDateMaxLen", Me.OutDateMaxLen)
            writer.WriteAttribute("OutDateTotLen", Me.OutDateTotLen)
            writer.WriteAttribute("OutDateMinWait", Me.OutDateMinWait)
            writer.WriteAttribute("OutDateMaxWait", Me.OutDateMaxWait)
            writer.WriteAttribute("OutDateTotWait", Me.OutDateTotWait)
            writer.WriteAttribute("OutDateCost", Me.InCallCost)

            writer.WriteAttribute("InSMSNum", Me.InSMSNum)
            writer.WriteAttribute("InSMSMinLen", Me.InSMSMinLen)
            writer.WriteAttribute("InSMSMaxLen", Me.InSMSMaxLen)
            writer.WriteAttribute("InSMSTotLen", Me.InSMSTotLen)
            writer.WriteAttribute("InSMSCost", Me.InCallCost)

            writer.WriteAttribute("OutSMSNum", Me.OutSMSNum)
            writer.WriteAttribute("OutSMSMinLen", Me.OutSMSMinLen)
            writer.WriteAttribute("OutSMSMaxLen", Me.OutSMSMaxLen)
            writer.WriteAttribute("OutSMSTotLen", Me.OutSMSTotLen)
            writer.WriteAttribute("OutSMSCost", Me.InCallCost)

            writer.WriteAttribute("InFAXNum", Me.InFAXNum)
            writer.WriteAttribute("InFAXMinLen", Me.InFAXMinLen)
            writer.WriteAttribute("InFAXMaxLen", Me.InFAXMaxLen)
            writer.WriteAttribute("InFAXTotLen", Me.InFAXTotLen)
            writer.WriteAttribute("InFAXCost", Me.InCallCost)

            writer.WriteAttribute("OutFAXNum", Me.OutFAXNum)
            writer.WriteAttribute("OutFAXMinLen", Me.OutFAXMinLen)
            writer.WriteAttribute("OutFAXMaxLen", Me.OutFAXMaxLen)
            writer.WriteAttribute("OutFAXTotLen", Me.OutFAXTotLen)
            writer.WriteAttribute("OutFAXCost", Me.InCallCost)

            writer.WriteAttribute("InEMAILNum", Me.InEMAILNum)
            writer.WriteAttribute("InEMAILMinLen", Me.InEMAILMinLen)
            writer.WriteAttribute("InEMAILMinLen", Me.InEMAILMinLen)
            writer.WriteAttribute("InEMAILTotLen", Me.InEMAILTotLen)

            writer.WriteAttribute("OutEMAILNum", Me.OutEMAILNum)
            writer.WriteAttribute("OutEMAILMinLen", Me.OutEMAILMinLen)
            writer.WriteAttribute("OutEMAILMaxLen", Me.OutEMAILMaxLen)
            writer.WriteAttribute("OutEMAILTotLen", Me.OutEMAILTotLen)

            writer.WriteAttribute("InTelegramNum", Me.InTelegramNum)
            writer.WriteAttribute("InTelegramMinLen", Me.InTelegramMinLen)
            writer.WriteAttribute("InTelegramMinLen", Me.InTelegramMinLen)
            writer.WriteAttribute("InTelegramTotLen", Me.InTelegramTotLen)
            writer.WriteAttribute("InTelegramCost", Me.InCallCost)

            writer.WriteAttribute("OutTelegramNum", Me.OutTelegramNum)
            writer.WriteAttribute("OutTelegramMinLen", Me.OutTelegramMinLen)
            writer.WriteAttribute("OutTelegramMaxLen", Me.OutTelegramMaxLen)
            writer.WriteAttribute("OutTelegramTotLen", Me.OutTelegramTotLen)
            writer.WriteAttribute("OutTelegramCost", Me.InCallCost)

            writer.WriteAttribute("NumAppuntamentiPrevisti", Me.NumAppuntamentiPrevisti)
            writer.WriteAttribute("NumTelefonatePreviste", Me.NumTelefonatePreviste)

            writer.WriteAttribute("Ricalcola", Me.Ricalcola)

            MyBase.XMLSerialize(writer)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "IDPuntoOperativo" : Me.IDPuntoOperativo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "IDOperatore" : Me.IDOperatore = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "Data" : Me.Data = XML.Utils.Serializer.DeserializeDate(fieldValue)

                Case "InCallNum" : Me.InCallNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InCallMinLen" : Me.InCallMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InCallMaxLen" : Me.InCallMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InCallTotLen" : Me.InCallTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InCallMinWait" : Me.InCallMinWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InCallMaxWait" : Me.InCallMaxWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InCallTotWait" : Me.InCallTotWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InCallCost" : Me.InCallCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "OutCallNum" : Me.OutCallNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OutCallMinLen" : Me.OutCallMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutCallMaxLen" : Me.OutCallMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutCallTotLen" : Me.OutCallTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutCallMinWait" : Me.OutCallMinWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutCallMaxWait" : Me.OutCallMaxWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutCallTotWait" : Me.OutCallTotWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutCallCost" : Me.OutCallCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "InDateNum" : Me.InDateNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InDateMinLen" : Me.InDateMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InDateMaxLen" : Me.InDateMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InDateTotLen" : Me.InDateTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InDateMinWait" : Me.InDateMinWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InDateMaxWait" : Me.InDateMaxWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InDateTotWait" : Me.InDateTotWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InDateCost" : Me.InDateCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "OutDateNum" : Me.OutDateNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OutDateMinLen" : Me.OutDateMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutDateMaxLen" : Me.OutDateMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutDateTotLen" : Me.OutDateTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutDateMinWait" : Me.OutDateMinWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutDateMaxWait" : Me.OutDateMaxWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutDateTotWait" : Me.OutDateTotWait = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutDateCost" : Me.OutDateCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "InSMSNum" : Me.InSMSNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InSMSMinLen" : Me.InSMSMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InSMSMaxLen" : Me.InSMSMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InSMSTotLen" : Me.InSMSTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InSMSCost" : Me.InSMSCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "OutSMSNum" : Me.OutSMSNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OutSMSMinLen" : Me.OutSMSMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutSMSMaxLen" : Me.OutSMSMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutSMSTotLen" : Me.OutSMSTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutSMSCost" : Me.OutSMSCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "InFAXNum" : Me.InFAXNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InFAXMinLen" : Me.InFAXMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InFAXMaxLen" : Me.InFAXMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InFAXTotLen" : Me.InFAXTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InFAXCost" : Me.InFAXCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "OutFAXNum" : Me.OutFAXNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OutFAXMinLen" : Me.OutFAXMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutFAXMaxLen" : Me.OutFAXMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutFAXTotLen" : Me.OutFAXTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutFAXCost" : Me.OutFAXCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "InEMAILNum" : Me.InEMAILNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InEMAILMinLen" : Me.InEMAILMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InEMAILMinLen" : Me.InEMAILMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InEMAILTotLen" : Me.InEMAILTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "OutEMAILNum" : Me.OutEMAILNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OutEMAILMinLen" : Me.OutEMAILMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutEMAILMaxLen" : Me.OutEMAILMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutEMAILTotLen" : Me.OutEMAILTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "InTelegramNum" : Me.InTelegramNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "InTelegramMinLen" : Me.InTelegramMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InTelegramMinLen" : Me.InTelegramMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InTelegramTotLen" : Me.InTelegramTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "InTelegramCost" : Me.InTelegramCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "OutTelegramNum" : Me.OutTelegramNum = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "OutTelegramMinLen" : Me.OutTelegramMinLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutTelegramMaxLen" : Me.OutTelegramMaxLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutTelegramTotLen" : Me.OutTelegramTotLen = XML.Utils.Serializer.DeserializeDouble(fieldValue)
                Case "OutTelegramCost" : Me.OutTelegramCost = XML.Utils.Serializer.DeserializeDouble(fieldValue)

                Case "NumAppuntamentiPrevisti" : Me.NumAppuntamentiPrevisti = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumTelefonatePreviste" : Me.NumTelefonatePreviste = XML.Utils.Serializer.DeserializeInteger(fieldValue)

                Case "Ricalcola", Me.Ricalcola = XML.Utils.Serializer.DeserializeBoolean(fieldValue)

                Case Else : MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select

        End Sub

        Public Shared Function GetStats(ByVal idPO As Integer, ByVal IDOperatore As Integer, ByVal data As Date) As CRMStats
            Dim dbRis As System.Data.IDataReader = Nothing
            Try
                data = Calendar.GetDatePart(data)
                Dim item As New CRMStats
                dbRis = CustomerCalls.CRM.StatsDB.ExecuteReader("SELECT * FROM [tbl_CRMStats] WHERE [IDPuntoOperativo]=" & DBUtils.DBNumber(idPO) & " AND [idOperatore]=" & DBUtils.DBNumber(IDOperatore) & " AND [Data]=" & DBUtils.DBDate(data))
                If (dbRis.Read) Then CustomerCalls.CRM.StatsDB.Load(item, dbRis)
                item.IDPuntoOperativo = idPO
                item.IDOperatore = IDOperatore
                item.Data = data
                Return item
            Catch ex As Exception
                Throw
            Finally
                If dbRis IsNot Nothing Then dbRis.Dispose()
                dbRis = Nothing
            End Try
        End Function
    End Class



End Class