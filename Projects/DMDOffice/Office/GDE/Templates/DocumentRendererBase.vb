Imports DMD
Imports DMD.Databases
Imports DMD.Sistema
Imports DMD.Anagrafica

Imports System.Drawing

Partial Class Office




    Public MustInherit Class DocumentRendererBase
        Private m_Document As CDocumento
        Private m_Template As DocumentTemplate
        Private m_Context As Object

        Public Sub New()
        End Sub

        Public Sub New(ByVal document As CDocumento, ByVal template As DocumentTemplate, ByVal context As Object)
            Me.New()
            Me.m_Document = document
            Me.m_Template = template
            Me.m_Context = context
        End Sub

        Public ReadOnly Property Document As CDocumento
            Get
                Return Me.m_Document
            End Get
        End Property

        Public ReadOnly Property DocumentTemplate As DocumentTemplate
            Get
                Return Me.m_Template
            End Get
        End Property

        Public ReadOnly Property Context As Object
            Get
                Return Me.m_Context
            End Get
        End Property

        Public MustOverride Sub Render()

        Public MustOverride Sub SaveToFile(ByVal fileName As String)


        Public Overridable Function LoadImage(ByVal path As String) As System.Drawing.Image
            path = Trim(path)
            If (path = vbNullString) Then Throw New ArgumentNullException(path)
            Return New System.Drawing.Bitmap(path)
        End Function

        Public Overridable Function GetDataField(ByVal fieldName As String) As Object
            fieldName = Trim(fieldName)
            If (fieldName = vbNullString) Then Throw New ArgumentNullException(fieldName)
            Dim t As System.Type = Me.Context.GetType
            Dim m As System.Reflection.PropertyInfo = t.GetProperty(fieldName, Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            Return m.GetValue(Me.Context, New Object() {})
        End Function

        Public Overridable Function EvaluateExpression(ByVal expression As String) As Object
            Throw New NotImplementedException
        End Function


    End Class



End Class