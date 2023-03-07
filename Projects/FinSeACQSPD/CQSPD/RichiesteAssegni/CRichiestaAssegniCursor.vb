Imports DMD
Imports DMD.Sistema
Imports DMD.Databases

Partial Class CQSPD

  
  
      Public Class CRichiestaAssegniCursor
            Inherits DBObjectCursor(Of CRichiestaAssegni)

            Private m_Banca As CCursorFieldObj(Of String)
            Private m_NomeRichiedente As CCursorFieldObj(Of String)
            Private m_CognomeRichiedente As CCursorFieldObj(Of String)
            Private m_IndirizzoRichiedente As CCursorFieldObj(Of String)
            Private m_Dipendenza As CCursorFieldObj(Of String)
            Private m_Data As CCursorField(Of Date)
            Private m_PerCassa As CCursorField(Of Date)
            Private m_ConAddebitoSuCC As CCursorField(Of Date)
            Private m_NumeroContoCorrente As CCursorFieldObj(Of String)
            Private m_IntestazioneContoCorrente As CCursorFieldObj(Of String)

            Public Sub New()
                Me.m_Banca = New CCursorFieldObj(Of String)("Banca")
                Me.m_NomeRichiedente = New CCursorFieldObj(Of String)("NomeRichiedente")
                Me.m_CognomeRichiedente = New CCursorFieldObj(Of String)("CognomeRichiedente")
                Me.m_IndirizzoRichiedente = New CCursorFieldObj(Of String)("IndirizzoRichiedente")
                Me.m_Dipendenza = New CCursorFieldObj(Of String)("Dipendenza")
                Me.m_Data = New CCursorField(Of Date)("Data")
                Me.m_PerCassa = New CCursorField(Of Date)("PerCassa")
                Me.m_ConAddebitoSuCC = New CCursorField(Of Date)("ConAddebitoSuCC")
                Me.m_NumeroContoCorrente = New CCursorFieldObj(Of String)("NumeroCCBancario")
                Me.m_IntestazioneContoCorrente = New CCursorFieldObj(Of String)("IntestazioneCC")
            End Sub

            Protected Overrides Function GetConnection() As CDBConnection
                Return CQSPD.Database
            End Function

            Protected Overrides Function GetModule() As CModule
                Return RichiesteAssegni.Module
            End Function

            Public Overrides Function GetTableName() As String
                Return "tbl_RichiesteAssegniCircolari"
            End Function
        End Class


End Class