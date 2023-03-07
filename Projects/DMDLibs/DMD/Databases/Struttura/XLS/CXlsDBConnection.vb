Imports DMD
Imports DMD.Sistema
Imports System.Xml.Serialization

Public partial class Databases

    Public Class CXlsDBConnection
        Inherits COleDBConnection

        ''' <summary>
        ''' Restituisce o imposta la versione predefinita
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property DefauleVersion As String = "" 'Auto

        Private m_Version As String = ""
        Private m_UseHeaders As Boolean

        Public Sub New()
            Me.m_UseHeaders = True
        End Sub

        Public Sub New(ByVal fileName As String)
            Me.New(fileName, DefauleVersion, True)
        End Sub

        Public Sub New(ByVal fileName As String, ByVal version As String)
            Me.New(fileName, version, True)
        End Sub

        Public Sub New(ByVal fileName As String, ByVal useHaders As Boolean)
            Me.New(fileName, DefauleVersion, useHaders)
        End Sub

        Public Sub New(ByVal fileName As String, ByVal version As String, ByVal useHeaders As Boolean)
            Me.New()
            Me.m_Version = Strings.Trim(version)
            If (Me.m_Version = "") Then Me.m_Version = DefauleVersion
            Me.m_UseHeaders = useHeaders
            Me.Path = fileName
        End Sub

        Public ReadOnly Property DriverVerion As String
            Get
                Return Me.m_Version
            End Get
        End Property


        Public ReadOnly Property UseHaders As Boolean
            Get
                Return Me.m_UseHeaders
            End Get
        End Property

        Public Overrides Function GetSqlDataType(field As CDBEntityField) As String
            If field.DataType Is GetType(String) Then
                Return IIf(field.MaxLength > 0, "TEXT(255)", "MEMO")
            Else
                Return MyBase.GetSqlDataType(field)
            End If
        End Function

        Public Overrides Function GetFriendlyName(name As String) As String
            name = MyBase.GetFriendlyName(name)
            If Right(name, 1) = "$" Or Right(name, 1) = "_" Then
                Return Left(name, Len(name) - 1)
            Else
                Return name
            End If
        End Function

        Public Overrides Function GetInternalTableName(table As CDBEntity) As String
            If (table.IsHidden) Then Return "[" & table.Name & "_]"
            Return "[" & table.Name & "$]"
        End Function

        Public Overrides Function GetConnectionString() As String
            Const excelObject As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""{3};HDR=YES;{4}"""

            If (Me.m_Version = "") Then
                'Autoidentify
                Select Case LCase(Trim(DMD.Sistema.FileSystem.GetExtensionName(Me.Path)))
                    Case "xls"  ' For Excel Below 2007 Format
                        'Return String.Format(excelObject, "Jet", "4.0", Me.Path, "Excel 8.0", "")
                        Return String.Format(excelObject, "ACE", "12.0", Me.Path, "Excel 8.0", "")
                    Case "xlsx" ' For Excel 2007 File  Format
                        Return String.Format(excelObject, "ACE", "12.0", Me.Path, "Excel 12.0", "")
                    Case Else
                        Throw New ArgumentOutOfRangeException
                End Select
            Else
                If (Me.m_Version = "12.0") Then
                    Return String.Format(excelObject, "ACE", Me.m_Version, Me.Path, "Excel 12.0", "")
                Else
                    Return String.Format(excelObject, "Jet", "4.0", Me.Path, "Excel 8.0", "")
                End If
            End If
        End Function

        Protected Overrides Function GetTablesArray() As CDBTable()
            Dim ret As CDBTable() = MyBase.GetTablesArray()
            For Each table In ret
                If (table.Name.StartsWith("'") AndAlso table.Name.EndsWith("'")) Then
                    table.Name = table.Name.Substring(1, table.Name.Length - 2)
                End If
                table.IsHidden = Right(table.Name, 1) = "_"
                If (table.Name.EndsWith("$")) Then
                    If (table.Name.StartsWith("'")) Then
                        table.Name = table.Name.Substring(1, table.Name.Length - 3)
                    Else
                        table.Name = table.Name.Substring(0, table.Name.Length - 1)
                    End If
                Else
                    table.Name = table.Name.Substring(0, table.Name.Length - 1)
                End If
            Next
            Return ret
        End Function

        'Protected Friend Overrides Sub CreateTable(table As CDBTable)
        '    Dim sql As String
        '    Dim t As Boolean = False
        '    sql = vbNullString
        '    sql &= "CREATE TABLE [" & table.Name & "] ("
        '    For Each field As CDBEntityField In table.Fields
        '        If (t) Then sql &= ","
        '        sql &= "[" & field.Name & "] " & Me.GetSqlDataType(field)
        '        t = True
        '    Next
        '    sql &= ")"
        '    Me.ExecuteCommand(sql)
        'End Sub

    End Class


End Class


