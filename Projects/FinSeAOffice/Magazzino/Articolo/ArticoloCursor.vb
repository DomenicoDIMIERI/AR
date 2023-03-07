Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica



Partial Public Class Office

    <Serializable> _
    Public Class AttributoArticoloItem
        Implements DMD.XML.IDMDXMLSerializable

        Public Nome As String
        Public Tipo As Nullable(Of System.TypeCode)
        Public ValoreFormattato As String
        Public UnitaDiMisura As String

        Public Sub New()
            Me.Nome = ""
            Me.Tipo = Nothing
            Me.ValoreFormattato = ""
            Me.UnitaDiMisura = ""
        End Sub

        Public Function IsSet() As Boolean
            Return (Me.ValoreFormattato <> "")
        End Function



        Protected Overridable Sub SetFieldInternal(fieldName As String, fieldValue As Object) Implements XML.IDMDXMLSerializable.SetFieldInternal
            Select Case fieldName
                Case "Nome" : Me.Nome = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "Tipo" : Me.Tipo = XML.Utils.Serializer.DeserializeInteger(fieldValue)
                Case "ValoreFormattato" : Me.ValoreFormattato = XML.Utils.Serializer.DeserializeString(fieldValue)
                Case "UnitaDiMisura" : Me.UnitaDiMisura = XML.Utils.Serializer.DeserializeString(fieldValue)
            End Select
        End Sub

        Protected Overridable Sub XMLSerialize(writer As XML.XMLWriter) Implements XML.IDMDXMLSerializable.XMLSerialize
            writer.WriteAttribute("Nome", Me.Nome)
            writer.WriteAttribute("Tipo", Me.Tipo)
            writer.WriteAttribute("ValoreFormattato", Me.ValoreFormattato)
            writer.WriteAttribute("UnitaDiMisura", Me.UnitaDiMisura)
        End Sub
    End Class

    Public Class ArticoloCursor
        Inherits DBObjectCursor(Of Articolo)

        Private m_Nome As New CCursorFieldObj(Of String)("Nome")
        Private m_Flags As New CCursorField(Of ArticoloFlags)("Flags")
        Private m_Marca As New CCursorFieldObj(Of String)("Marca")
        Private m_Modello As New CCursorFieldObj(Of String)("Modello")
        Private m_Descrizione As New CCursorFieldObj(Of String)("Descrizione")
        Private m_IconURL As New CCursorFieldObj(Of String)("IconURL")
        Private m_CategoriaPrincipale As New CCursorFieldObj(Of String)("CategoriaPrincipale")
        Private m_UnitaBase As New CCursorFieldObj(Of String)("UnitaBase")
        Private m_DecimaliValuta As New CCursorField(Of Integer)("DecimaliValuta")
        Private m_DecimaliQuantita As New CCursorField(Of Integer)("DecimaliQuantita")
        Private m_TipoCodice As New CCursorFieldObj(Of String)("TipoCodice")
        Private m_ValoreCodice As New CCursorFieldObj(Of String)("ValoreCodice")
        Private m_ProductPage As New CCursorFieldObj(Of String)("ProductPage")
        Private m_SupportPage As New CCursorFieldObj(Of String)("SupportPage")

        Private m_Attributi As New CCollection(Of AttributoArticoloItem)

        Public Sub New()
        End Sub

        Public ReadOnly Property ProductPage As CCursorFieldObj(Of String)
            Get
                Return Me.m_ProductPage
            End Get
        End Property

        Public ReadOnly Property SupportPage As CCursorFieldObj(Of String)
            Get
                Return Me.m_SupportPage
            End Get
        End Property

        Public ReadOnly Property Attributi As CCollection(Of AttributoArticoloItem)
            Get
                Return Me.m_Attributi
            End Get
        End Property

        Public ReadOnly Property TipoCodice As CCursorFieldObj(Of String)
            Get
                Return Me.m_TipoCodice
            End Get
        End Property

        Public ReadOnly Property ValoreCodice As CCursorFieldObj(Of String)
            Get
                Return Me.m_ValoreCodice
            End Get
        End Property


        Public ReadOnly Property UnitaBase As CCursorFieldObj(Of String)
            Get
                Return Me.m_UnitaBase
            End Get
        End Property

        Public ReadOnly Property DecimaliValuta As CCursorField(Of Integer)
            Get
                Return Me.m_DecimaliValuta
            End Get
        End Property

        Public ReadOnly Property DecimaliQuantita As CCursorField(Of Integer)
            Get
                Return Me.m_DecimaliQuantita
            End Get
        End Property

        Public ReadOnly Property Nome As CCursorFieldObj(Of String)
            Get
                Return Me.m_Nome
            End Get
        End Property

        Public ReadOnly Property CategoriaPrincipale As CCursorFieldObj(Of String)
            Get
                Return Me.m_CategoriaPrincipale
            End Get
        End Property

        Public ReadOnly Property Marca As CCursorFieldObj(Of String)
            Get
                Return Me.m_Marca
            End Get
        End Property

        Public ReadOnly Property Modello As CCursorFieldObj(Of String)
            Get
                Return Me.m_Modello
            End Get
        End Property

        Public ReadOnly Property Descrizione As CCursorFieldObj(Of String)
            Get
                Return Me.m_Descrizione
            End Get
        End Property

        Public ReadOnly Property IconURL As CCursorFieldObj(Of String)
            Get
                Return Me.m_IconURL
            End Get
        End Property


        Public ReadOnly Property Flags As CCursorField(Of ArticoloFlags)
            Get
                Return Me.m_Flags
            End Get
        End Property

        Protected Overrides Function GetConnection() As CDBConnection
            Return Office.Database
        End Function

        Protected Overrides Function GetModule() As CModule
            Return Office.Articoli.Module
        End Function

        Public Overrides Function GetTableName() As String
            Return "tbl_OfficeArticoli"
        End Function

        Protected Overrides Function GetWhereFields() As CKeyCollection(Of CCursorField)
            Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetWhereFields()
            ret.RemoveByKey("TipoCodice")
            ret.RemoveByKey("ValoreCodice")
            Return ret
        End Function

        Protected Overrides Function GetSortFields() As CKeyCollection(Of CCursorField)
            Dim ret As CKeyCollection(Of CCursorField) = MyBase.GetSortFields()
            ret.RemoveByKey("TipoCodice")
            ret.RemoveByKey("ValoreCodice")
            Return ret
        End Function

        Public Overrides Function GetWherePart() As String
            Dim ret As String = MyBase.GetWherePart()
            Dim tmpSQL As String
            If (Me.m_ValoreCodice.IsSet OrElse Me.m_TipoCodice.IsSet) Then
                tmpSQL = "[ID] In ("
                tmpSQL &= "SELECT IDArticolo FROM [tbl_OfficeCodiciArticolo] WHERE ([Stato]=" & ObjectStatus.OBJECT_VALID & ")"
                If Me.m_ValoreCodice.IsSet Then tmpSQL &= " AND " & Me.m_ValoreCodice.GetSQL("Valore")
                If (Me.m_TipoCodice.IsSet) Then tmpSQL &= " AND " & Me.m_TipoCodice.GetSQL("Tipo")
                tmpSQL &= ")"
                ret = Strings.Combine(ret, tmpSQL, " AND ")
            End If
            
            Return ret
        End Function

        Public Overrides Function GetSQL() As String
            Dim ret As String = MyBase.GetSQL()
            Dim tmpSQL As String
            If (Me.IsAttributoSet) Then
                Dim t As Integer = 0
                Dim totalids() As Integer = Nothing

                For Each attr As AttributoArticoloItem In Me.m_Attributi
                    If attr.IsSet Then
                        tmpSQL = "[Stato]=" & ObjectStatus.OBJECT_VALID
                        If (Strings.Trim(attr.Nome) <> "") Then tmpSQL = Strings.Combine(tmpSQL, "[NomeAttributo]=" & DBUtils.DBString(Strings.Trim(attr.Nome)), " AND ")
                        If (attr.Tipo.HasValue) Then tmpSQL = Strings.Combine(tmpSQL, "[TipoAttributo]=" & DBUtils.DBNumber(attr.Tipo), " AND ")
                        If (Strings.Trim(attr.ValoreFormattato) <> "") Then tmpSQL = Strings.Combine(tmpSQL, "[ValoreAttributo]=" & DBUtils.DBString(Strings.Trim(attr.ValoreFormattato)), " AND ")
                        If (Strings.Trim(attr.UnitaDiMisura) <> "") Then tmpSQL = Strings.Combine(tmpSQL, "[UnitaDiMisura]=" & DBUtils.DBString(Strings.Trim(attr.UnitaDiMisura)), " AND ")

                        ret = "SELECT [T" & t & "].* FROM (" & ret & ") AS [T" & t & "] INNER JOIN ("
                        ret &= "SELECT [IDArticolo] AS [ID] FROM [tbl_OfficeArticoliAttributi] WHERE " & tmpSQL
                        ret &= " GROUP BY [IDArticolo]) AS [T" & (t + 1) & "] "
                        ret &= " ON [T" & t & "].[ID]=[T" & (t + 1) & "].[ID]"
                        t += 2
                    End If
                Next
            End If
            Return ret
        End Function

        Private Function IsAttributoSet() As Boolean
            For Each attr As AttributoArticoloItem In Me.m_Attributi
                If attr.IsSet Then Return True
            Next
            Return False
        End Function

        Protected Overrides Sub XMLSerialize(writer As XML.XMLWriter)
            MyBase.XMLSerialize(writer)
            writer.WriteTag("Attributi", Me.m_Attributi)
        End Sub

        Protected Overrides Sub SetFieldInternal(fieldName As String, fieldValue As Object)
            Select Case fieldName
                Case "Attributi"
                    Me.m_Attributi.Clear()
                    Me.m_Attributi.AddRange(fieldValue)
                Case Else
                    MyBase.SetFieldInternal(fieldName, fieldValue)
            End Select
        End Sub
    End Class

End Class


