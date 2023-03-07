Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Partial Class Sistema

    ''' <summary>
    ''' Racchiude alcune info sui contatti
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CCalendarActivityInfo
        Implements XML.IDMDXMLSerializable

        Public IDRicontatto As Integer
        Public TipoRicontatto As String
        Public IDContattoPrecedente As Integer
        Public DataProssimoRicontatto As Nullable(Of Date)
        Public DataProssimoAppuntamento As Nullable(Of Date)
        Public DataUltimoRicontatto As Nullable(Of Date)
        Public DataUltimoAppuntamento As Nullable(Of Date)
        Public NumeroPraticheInCorso As Integer
        Public NumeroPraticheArchiviate As Integer

        Public Sub New()
            DMD.DMDObject.IncreaseCounter(Me)
            IDRicontatto = 0
            TipoRicontatto = ""
            IDContattoPrecedente = 0
            DataProssimoRicontatto = Nothing
            DataProssimoAppuntamento = Nothing
            DataUltimoRicontatto = Nothing
            DataUltimoAppuntamento = Nothing
            NumeroPraticheInCorso = 0
            NumeroPraticheArchiviate = 0
        End Sub

        Public Sub Initialize(ByVal personID As Integer)
            'Dim cursor As New CCustomerCallsCursor
            'cursor.IDPersona.Value = personID
            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            ''cursor.Ricontattare.Value = True
            ''cursor.DataRicontatto.SortOrder = SortEnum.SORT_ASC
            'Me.IDContattoPrecedente = GetID(cursor.Item)
            '' If Not (cursor.Item Is Nothing) Then Me.DataProssimoRicontatto = cursor.Item.DataRicontatto
            'cursor.Reset()

            'cursor = New CCustomerCallsCursor
            'cursor.IDPersona.Value = personID
            'cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            'cursor.Data.SortOrder = SortEnum.SORT_DESC

            'If Not (cursor.Item Is Nothing) Then Me.DataUltimoRicontatto = cursor.Item.Data
            'cursor.Reset()

            'Me.IDRicontatto = GetID(Ricontatti.GetProssimoRicontatto(personID))
        End Sub

        Protected Sub XMLSerialize(ByVal writer As DMD.XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("IDRicontatto", Me.IDRicontatto)
            writer.WriteAttribute("TipoRicontatto", Me.TipoRicontatto)
            writer.WriteAttribute("IDContattoPrecedente", Me.IDContattoPrecedente)
            writer.WriteAttribute("DataProssimoRicontatto", Me.DataProssimoRicontatto)
            writer.WriteAttribute("DataProssimoAppuntamento", Me.DataProssimoAppuntamento)
            writer.WriteAttribute("DataUltimoRicontatto", Me.DataUltimoRicontatto)
            writer.WriteAttribute("DataUltimoAppuntamento", Me.DataUltimoAppuntamento)
            writer.WriteAttribute("NumeroPraticheInCorso", Me.NumeroPraticheInCorso)
            writer.WriteAttribute("NumeroPraticheArchiviate", Me.NumeroPraticheArchiviate)
        End Sub

        Protected Sub SetFieldInternal(ByVal fieldName As String, ByVal fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "IDRicontatto" : Me.IDRicontatto = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "TipoRicontatto" : Me.TipoRicontatto = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "IDContattoPrecedente" : Me.IDContattoPrecedente = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "DataProssimoRicontatto" : Me.DataProssimoRicontatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataProssimoAppuntamento" : Me.DataProssimoAppuntamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataUltimoRicontatto" : Me.DataUltimoRicontatto = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "DataUltimoAppuntamento" : Me.DataUltimoAppuntamento = XML.Utils.Serializer.DeserializeDate(fieldValue)
                Case "NumeroPraticheInCorso" : Me.NumeroPraticheInCorso = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "NumeroPraticheArchiviate" : Me.NumeroPraticheArchiviate = XML.Utils.Serializer.DeserializeInteger(fieldValue)
            End Select
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            DMD.DMDObject.DecreaseCounter(Me)
        End Sub
    End Class


End Class