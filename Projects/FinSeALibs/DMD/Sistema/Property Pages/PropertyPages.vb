Imports System.Net.Mail 'importo il Namespace
Imports System.Net.Sockets
Imports DMD
Imports DMD.Databases
Imports DMD.Anagrafica

Partial Public Class Sistema

    Public NotInheritable Class CPropertyPagesClass
        Inherits CGeneralClass(Of CRegisteredPropertyPage)

        
        Friend Sub New()
            MyBase.New("modPropertyPages", GetType(CRegisteredPropertyPageCursor), -1)
        End Sub

        Public Function GetPropertyID(ByVal typeName As String, ByVal pageName As String) As Integer
            Return GetID(GetPropertyPage(typeName, pageName))
        End Function

        Public Function GetPropertyPage(ByVal typeName As String, ByVal pageName As String) As CRegisteredPropertyPage
            Dim items As CCollection(Of CRegisteredPropertyPage) = Me.LoadAll
            For Each p As CRegisteredPropertyPage In items
                If p.ClassName = typeName AndAlso p.TabPageClass = pageName Then Return p
            Next
            Return Nothing
        End Function

        Public Function RegisterPropertyPage(ByVal typeName As String, ByVal pageName As String, ByVal priority As Integer) As CRegisteredPropertyPage
            Dim item As CRegisteredPropertyPage
            item = GetPropertyPage(typeName, pageName)
            If (item Is Nothing) Then
                item = New CRegisteredPropertyPage
                item.ClassName = typeName
                item.TabPageClass = pageName

                Me.AddToCache(item)
            Else
                item.Priority = priority
            End If
            item.Save()
            Return item
        End Function

        Private Class RPPriorityComparer
            Implements IComparer

            Public Function Compare(ByVal a As CRegisteredPropertyPage, ByVal b As CRegisteredPropertyPage) As Integer
                Return a.Priority - b.Priority
            End Function

            Private Function Compare1(x As Object, y As Object) As Integer Implements IComparer.Compare
                Return Me.Compare(x, y)
            End Function
        End Class

        ''' <summary>
        ''' Restituisce un array contenente i tipi delle classi di tipo PropertyPage registrate per il tipo specificato
        ''' </summary>
        ''' <param name="typeName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRegisteredPropertyPages(ByVal typeName As String) As System.Type()
            Dim ret As New System.Collections.ArrayList
            typeName = Trim(typeName)
            For Each rp As CRegisteredPropertyPage In Me.LoadAll
                If Strings.Compare(rp.ClassName, typeName, CompareMethod.Text) = 0 Then
                    If rp.TabPageType IsNot Nothing Then
                        ret.Add(rp.TabPageType)
                    Else
                        Throw New ArgumentException("La pagina di proprietà [" & rp.TabPageClass & "] definita per il tipo [" & rp.ClassName & "] non è definita")
                    End If
                End If
            Next
            Return ret.ToArray(GetType(System.Type))
        End Function

        ''' <summary>
        ''' Restituisce un array contenente i nomi delle classi di tipo PropertyPage registrate per il tipo specificato
        ''' </summary>
        ''' <param name="typeName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRegisteredPropertyPageNames(ByVal typeName As String) As String()
            Dim ret As New System.Collections.ArrayList
            typeName = Trim(typeName)
            For Each rp As CRegisteredPropertyPage In Me.LoadAll
                If Strings.Compare(rp.ClassName, typeName, CompareMethod.Text) = 0 Then
                    If rp.TabPageClass <> "" Then ret.Add(rp.TabPageClass)
                End If
            Next
            Return ret.ToArray(GetType(String))
        End Function

        ''' <summary>
        ''' Restituisce un array contenente i nomi delle classi di tipo PropertyPage registrate per il tipo specificato
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRegisteredPropertyPages(ByVal type As System.Type) As System.Type()
            Return GetRegisteredPropertyPages(type.Name)
        End Function


    End Class

    Public Shared ReadOnly PropertyPages As New CPropertyPagesClass


End Class