Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Imports DMD.Anagrafica



Partial Class CustomerCalls

    Public Class CRMStatsCursor
        Inherits DBObjectCursorBase(Of CRMStats)

        Private m_IDPuntoOperativo As New CCursorField(Of Integer)("IDPuntoOperativo")
        Private m_IDOperatore As New CCursorField(Of Integer)("IDOperatore")
        Private m_Data As New CCursorField(Of Date)("Data")

        Private m_InCallNum As New CCursorField(Of Integer)("InCallNum")
        Private m_InCallMinLen As New CCursorField(Of Double)("InCallMinLen")
        Private m_InCallMaxLen As New CCursorField(Of Double)("InCallMaxLen")
        Private m_InCallTotLen As New CCursorField(Of Double)("InCallTotLen")
        Private m_InCallMinWait As New CCursorField(Of Double)("InCallMinWait")
        Private m_InCallMaxWait As New CCursorField(Of Double)("InCallMaxWait")
        Private m_InCallTotWait As New CCursorField(Of Double)("InCallTotWait")
        Private m_InCallCost As New CCursorField(Of Decimal)("InCallCost")

        Private m_OutCallNum As New CCursorField(Of Integer)("OutCallNum")
        Private m_OutCallMinLen As New CCursorField(Of Double)("OutCallMinLen")
        Private m_OutCallMaxLen As New CCursorField(Of Double)("OutCallMaxLen")
        Private m_OutCallTotLen As New CCursorField(Of Double)("OutCallTotLen")
        Private m_OutCallMinWait As New CCursorField(Of Double)("OutCallMinWait")
        Private m_OutCallMaxWait As New CCursorField(Of Double)("OutCallMaxWait")
        Private m_OutCallTotWait As New CCursorField(Of Double)("OutCallTotWait")
        Private m_OutCallCost As New CCursorField(Of Decimal)("OutCallCost")

        Private m_InDateNum As New CCursorField(Of Integer)("InDateNum")
        Private m_InDateMinLen As New CCursorField(Of Double)("InDateMinLen")
        Private m_InDateMaxLen As New CCursorField(Of Double)("InDateMaxLen")
        Private m_InDateTotLen As New CCursorField(Of Double)("InDateTotLen")
        Private m_InDateMinWait As New CCursorField(Of Double)("InDateMinWait")
        Private m_InDateMaxWait As New CCursorField(Of Double)("InDateMaxWait")
        Private m_InDateTotWait As New CCursorField(Of Double)("InDateTotWait")
        Private m_InDateCost As New CCursorField(Of Decimal)("InDateCost")

        Private m_OutDateNum As New CCursorField(Of Integer)("OutDateNum")
        Private m_OutDateMinLen As New CCursorField(Of Double)("OutDateMinLen")
        Private m_OutDateMaxLen As New CCursorField(Of Double)("OutDateMaxLen")
        Private m_OutDateTotLen As New CCursorField(Of Double)("OutDateTotLen")
        Private m_OutDateMinWait As New CCursorField(Of Double)("OutDateMinWait")
        Private m_OutDateMaxWait As New CCursorField(Of Double)("OutDateMaxWait")
        Private m_OutDateTotWait As New CCursorField(Of Double)("OutDateTotWait")
        Private m_OutDateCost As New CCursorField(Of Decimal)("OutDateCost")

        Private m_InSMSNum As New CCursorField(Of Integer)("InSMSNum")
        Private m_InSMSMinLen As New CCursorField(Of Double)("InSMSMinLen")
        Private m_InSMSMaxLen As New CCursorField(Of Double)("InSMSMaxLen")
        Private m_InSMSTotLen As New CCursorField(Of Double)("InSMSTotLen")
        Private m_InSMSCost As New CCursorField(Of Decimal)("InSMSCost")

        Private m_OutSMSNum As New CCursorField(Of Integer)("OutSMSNum")
        Private m_OutSMSMinLen As New CCursorField(Of Double)("OutSMSMinLen")
        Private m_OutSMSMaxLen As New CCursorField(Of Double)("OutSMSMaxLen")
        Private m_OutSMSTotLen As New CCursorField(Of Double)("OutSMSTotLen")
        Private m_OutSMSCost As New CCursorField(Of Decimal)("OutSMSCost")

        Private m_InFAXNum As New CCursorField(Of Integer)("InFAXNum")
        Private m_InFAXMinLen As New CCursorField(Of Double)("InFAXMinLen")
        Private m_InFAXMaxLen As New CCursorField(Of Double)("InFAXMaxLen")
        Private m_InFAXTotLen As New CCursorField(Of Double)("InFAXTotLen")
        Private m_InFAXCost As New CCursorField(Of Decimal)("InFAXCost")

        Private m_OutFAXNum As New CCursorField(Of Integer)("OutFAXNum")
        Private m_OutFAXMinLen As New CCursorField(Of Double)("OutFAXMinLen")
        Private m_OutFAXMaxLen As New CCursorField(Of Double)("OutFAXMaxLen")
        Private m_OutFAXTotLen As New CCursorField(Of Double)("OutFAXTotLen")
        Private m_OutFAXCost As New CCursorField(Of Decimal)("OutFAXCost")

        Private m_InEMAILNum As New CCursorField(Of Integer)("InEMAILNum")
        Private m_InEMAILMinLen As New CCursorField(Of Double)("InEMAILMinLen")
        Private m_InEMAILMaxLen As New CCursorField(Of Double)("InEMAILMaxLen")
        Private m_InEMAILTotLen As New CCursorField(Of Double)("InEMAILTotLen")
        Private m_InEMAILCost As New CCursorField(Of Decimal)("InEMAILCost")

        Private m_OutEMAILNum As New CCursorField(Of Integer)("OutEMAILNum")
        Private m_OutEMAILMinLen As New CCursorField(Of Double)("OutEMAILMinLen")
        Private m_OutEMAILMaxLen As New CCursorField(Of Double)("OutEMAILMaxLen")
        Private m_OutEMAILTotLen As New CCursorField(Of Double)("OutEMAILTotLen")
        Private m_OutEMAILCost As New CCursorField(Of Decimal)("OutEMAILCost")

        Private m_InTelegramNum As New CCursorField(Of Integer)("InTelegramNum")
        Private m_InTelegramMinLen As New CCursorField(Of Double)("InTelegramMinLen")
        Private m_InTelegramMaxLen As New CCursorField(Of Double)("InTelegramMaxLen")
        Private m_InTelegramTotLen As New CCursorField(Of Double)("InTelegramTotLen")
        Private m_InTelegramCost As New CCursorField(Of Decimal)("InTelegramCost")

        Private m_OutTelegramNum As New CCursorField(Of Integer)("OutTelegramNum")
        Private m_OutTelegramMinLen As New CCursorField(Of Double)("OutTelegramMinLen")
        Private m_OutTelegramMaxLen As New CCursorField(Of Double)("OutTelegramMaxLen")
        Private m_OutTelegramTotLen As New CCursorField(Of Double)("OutTelegramTotLen")
        Private m_OutTelegramCost As New CCursorField(Of Decimal)("OutTelegramCost")

        Private m_Ricalcola As New CCursorField(Of Boolean)("Ricalcola")

        Public Sub New()

        End Sub

        Public ReadOnly Property IDPuntoOperativo As CCursorField(Of Integer)
            Get
                Return Me.m_IDPuntoOperativo
            End Get
        End Property

        Public ReadOnly Property IDOperatore As CCursorField(Of Integer)
            Get
                Return Me.m_IDOperatore
            End Get
        End Property

        Public ReadOnly Property Data As CCursorField(Of Date)
            Get
                Return Me.m_Data
            End Get
        End Property

        Public ReadOnly Property InCallNum As CCursorField(Of Integer)
            Get
                Return Me.m_InCallNum
            End Get
        End Property

        Public ReadOnly Property InCallMinLen As CCursorField(Of Double)
            Get
                Return Me.m_InCallMinLen
            End Get
        End Property

        Public ReadOnly Property InCallMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_InCallMaxLen
            End Get
        End Property

        Public ReadOnly Property InCallTotLen As CCursorField(Of Double)
            Get
                Return Me.m_InCallTotLen
            End Get
        End Property

        Public ReadOnly Property InCallMinWait As CCursorField(Of Double)
            Get
                Return Me.m_InCallMinWait
            End Get
        End Property

        Public ReadOnly Property InCallMaxWait As CCursorField(Of Double)
            Get
                Return Me.m_InCallMaxWait
            End Get
        End Property

        Public ReadOnly Property InCallTotWait As CCursorField(Of Double)
            Get
                Return Me.m_InCallTotWait
            End Get
        End Property

        Public ReadOnly Property InCallCost As CCursorField(Of Decimal)
            Get
                Return Me.m_InCallCost
            End Get
        End Property

        Public ReadOnly Property OutCallNum As CCursorField(Of Integer)
            Get
                Return Me.m_OutCallNum
            End Get
        End Property

        Public ReadOnly Property OutCallMinLen As CCursorField(Of Double)
            Get
                Return Me.m_OutCallMinLen
            End Get
        End Property

        Public ReadOnly Property OutCallMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_OutCallMaxLen
            End Get
        End Property

        Public ReadOnly Property OutCallTotLen As CCursorField(Of Double)
            Get
                Return Me.m_OutCallTotLen
            End Get
        End Property

        Public ReadOnly Property OutCallMinWait As CCursorField(Of Double)
            Get
                Return Me.m_OutCallMinWait
            End Get
        End Property

        Public ReadOnly Property OutCallMaxWait As CCursorField(Of Double)
            Get
                Return Me.m_OutCallMaxWait
            End Get
        End Property

        Public ReadOnly Property OutCallTotWait As CCursorField(Of Double)
            Get
                Return Me.m_OutCallTotWait
            End Get
        End Property

        Public ReadOnly Property OutCallCost As CCursorField(Of Decimal)
            Get
                Return Me.m_OutCallCost
            End Get
        End Property

        Public ReadOnly Property InDateNum As CCursorField(Of Integer)
            Get
                Return Me.m_InDateNum
            End Get
        End Property

        Public ReadOnly Property InDateMinLen As CCursorField(Of Double)
            Get
                Return Me.m_InDateMinLen
            End Get
        End Property

        Public ReadOnly Property InDateMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_InDateMaxLen
            End Get
        End Property

        Public ReadOnly Property InDateTotLen As CCursorField(Of Double)
            Get
                Return Me.m_InDateTotLen
            End Get
        End Property

        Public ReadOnly Property InDateMinWait As CCursorField(Of Double)
            Get
                Return Me.m_InDateMinWait
            End Get
        End Property

        Public ReadOnly Property InDateMaxWait As CCursorField(Of Double)
            Get
                Return Me.m_InDateMaxWait
            End Get
        End Property

        Public ReadOnly Property InDateTotWait As CCursorField(Of Double)
            Get
                Return Me.m_InDateTotWait
            End Get
        End Property

        Public ReadOnly Property InDateCost As CCursorField(Of Decimal)
            Get
                Return Me.m_InDateCost
            End Get
        End Property

        Public ReadOnly Property OutDateNum As CCursorField(Of Integer)
            Get
                Return Me.m_OutDateNum
            End Get
        End Property

        Public ReadOnly Property OutDateMinLen As CCursorField(Of Double)
            Get
                Return Me.m_OutDateMinLen
            End Get
        End Property

        Public ReadOnly Property OutDateMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_OutDateMaxLen
            End Get
        End Property

        Public ReadOnly Property OutDateTotLen As CCursorField(Of Double)
            Get
                Return Me.m_OutDateTotLen
            End Get
        End Property

        Public ReadOnly Property OutDateMinWait As CCursorField(Of Double)
            Get
                Return Me.m_OutDateMinWait
            End Get
        End Property

        Public ReadOnly Property OutDateMaxWait As CCursorField(Of Double)
            Get
                Return Me.m_OutDateMaxWait
            End Get
        End Property

        Public ReadOnly Property OutDateTotWait As CCursorField(Of Double)
            Get
                Return Me.m_OutDateTotWait
            End Get
        End Property

        Public ReadOnly Property OutDateCost As CCursorField(Of Decimal)
            Get
                Return Me.m_OutDateCost
            End Get
        End Property

        Public ReadOnly Property InSMSNum As CCursorField(Of Integer)
            Get
                Return Me.m_InSMSNum
            End Get
        End Property

        Public ReadOnly Property InSMSMinLen As CCursorField(Of Double)
            Get
                Return Me.m_InSMSMinLen
            End Get
        End Property

        Public ReadOnly Property InSMSMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_InSMSMaxLen
            End Get
        End Property

        Public ReadOnly Property InSMSTotLen As CCursorField(Of Double)
            Get
                Return Me.m_InSMSTotLen
            End Get
        End Property

        Public ReadOnly Property InSMSCost As CCursorField(Of Decimal)
            Get
                Return Me.m_InSMSCost
            End Get
        End Property

        Public ReadOnly Property OutSMSNum As CCursorField(Of Integer)
            Get
                Return Me.m_OutSMSNum
            End Get
        End Property

        Public ReadOnly Property OutSMSMinLen As CCursorField(Of Double)
            Get
                Return Me.m_OutSMSMinLen
            End Get
        End Property

        Public ReadOnly Property OutSMSMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_OutSMSMaxLen
            End Get
        End Property

        Public ReadOnly Property OutSMSTotLen As CCursorField(Of Double)
            Get
                Return Me.m_OutSMSTotLen
            End Get
        End Property

        Public ReadOnly Property OutSMSCost As CCursorField(Of Decimal)
            Get
                Return Me.m_OutSMSCost
            End Get
        End Property

        Public ReadOnly Property InFAXNum As CCursorField(Of Integer)
            Get
                Return Me.m_InFAXNum
            End Get
        End Property

        Public ReadOnly Property InFAXMinLen As CCursorField(Of Double)
            Get
                Return Me.m_InFAXMinLen
            End Get
        End Property

        Public ReadOnly Property InFAXMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_InFAXMaxLen
            End Get
        End Property

        Public ReadOnly Property InFAXTotLen As CCursorField(Of Double)
            Get
                Return Me.m_InFAXTotLen
            End Get
        End Property

        Public ReadOnly Property InFAXCost As CCursorField(Of Decimal)
            Get
                Return Me.m_InFAXCost
            End Get
        End Property

        Public ReadOnly Property OutFAXNum As CCursorField(Of Integer)
            Get
                Return Me.m_OutFAXNum
            End Get
        End Property

        Public ReadOnly Property OutFAXMinLen As CCursorField(Of Double)
            Get
                Return Me.m_OutFAXMinLen
            End Get
        End Property

        Public ReadOnly Property OutFAXMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_OutFAXMaxLen
            End Get
        End Property

        Public ReadOnly Property OutFAXTotLen As CCursorField(Of Double)
            Get
                Return Me.m_OutFAXTotLen
            End Get
        End Property

        Public ReadOnly Property OutFAXCost As CCursorField(Of Decimal)
            Get
                Return Me.m_OutFAXCost
            End Get
        End Property

        Public ReadOnly Property InEMAILNum As CCursorField(Of Integer)
            Get
                Return Me.m_InEMAILNum
            End Get
        End Property

        Public ReadOnly Property InEMAILMinLen As CCursorField(Of Double)
            Get
                Return Me.m_InEMAILMinLen
            End Get
        End Property

        Public ReadOnly Property InEMAILMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_InEMAILMaxLen
            End Get
        End Property

        Public ReadOnly Property InEMAILTotLen As CCursorField(Of Double)
            Get
                Return Me.m_InEMAILTotLen
            End Get
        End Property

        Public ReadOnly Property InEMAILCost As CCursorField(Of Decimal)
            Get
                Return Me.m_InEMAILCost
            End Get
        End Property

        Public ReadOnly Property OutEMAILNum As CCursorField(Of Integer)
            Get
                Return Me.m_OutEMAILNum
            End Get
        End Property

        Public ReadOnly Property OutEMAILMinLen As CCursorField(Of Double)
            Get
                Return Me.m_OutEMAILMinLen
            End Get
        End Property

        Public ReadOnly Property OutEMAILMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_OutEMAILMaxLen
            End Get
        End Property

        Public ReadOnly Property OutEMAILTotLen As CCursorField(Of Double)
            Get
                Return Me.m_OutEMAILTotLen
            End Get
        End Property

        Public ReadOnly Property OutEMAILCost As CCursorField(Of Decimal)
            Get
                Return Me.m_OutEMAILCost
            End Get
        End Property

        Public ReadOnly Property InTelegramNum As CCursorField(Of Integer)
            Get
                Return Me.m_InTelegramNum
            End Get
        End Property

        Public ReadOnly Property InTelegramMinLen As CCursorField(Of Double)
            Get
                Return Me.m_InTelegramMinLen
            End Get
        End Property

        Public ReadOnly Property InTelegramMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_InTelegramMaxLen
            End Get
        End Property

        Public ReadOnly Property InTelegramTotLen As CCursorField(Of Double)
            Get
                Return Me.m_InTelegramTotLen
            End Get
        End Property

        Public ReadOnly Property InTelegramCost As CCursorField(Of Decimal)
            Get
                Return Me.m_InTelegramCost
            End Get
        End Property

        Public ReadOnly Property OutTelegramNum As CCursorField(Of Integer)
            Get
                Return Me.m_OutTelegramNum
            End Get
        End Property

        Public ReadOnly Property OutTelegramMinLen As CCursorField(Of Double)
            Get
                Return Me.m_OutTelegramMinLen
            End Get
        End Property

        Public ReadOnly Property OutTelegramMaxLen As CCursorField(Of Double)
            Get
                Return Me.m_OutTelegramMaxLen
            End Get
        End Property

        Public ReadOnly Property OutTelegramTotLen As CCursorField(Of Double)
            Get
                Return Me.m_OutTelegramTotLen
            End Get
        End Property

        Public ReadOnly Property OutTelegramCost As CCursorField(Of Decimal)
            Get
                Return Me.m_OutTelegramCost
            End Get
        End Property

        Public ReadOnly Property Ricalcola As CCursorField(Of Boolean)
            Get
                Return Me.m_Ricalcola
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return CRM.StatsDB
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Nothing
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_CRMStats"
        End Function


    End Class



End Class