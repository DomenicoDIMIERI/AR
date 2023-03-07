
Namespace Renderers

    'Public Class PDFDocumentRenderer
    '    Inherits DocumentRendererBase

    '    Private m_PDF As New FinSeA.PDF.PDFWriter

    '    Public Sub New()
    '    End Sub

    '    Public Sub New(ByVal document As GDE.CDocumento, ByVal template As GDE.DocumentTemplate, ByVal context As Object)
    '        MyBase.New(document, template, context)
    '    End Sub

    '    Public Overrides Sub Render()
    '        If (LCase(Me.DocumentTemplate.PageFormatName) = "custom") Then
    '            Me.m_PDF = New PDF.PDFWriter("P", "cm", Me.DocumentTemplate.PageFormat)
    '        Else
    '            Me.m_PDF = New PDF.PDFWriter("P", "cm", Me.DocumentTemplate.PageFormatName)
    '        End If
    '        For Each item As TemplateItem In Me.DocumentTemplate.Items
    '            Select Case item.ItemType
    '                Case GDE.TemplateItemTypes.DRAWELLIPSE : Me.m_PDF.DrawEllipse(item.Color, item.Bounds)
    '                Case GDE.TemplateItemTypes.FILLELLIPSE : Me.m_PDF.FillEllipse(item.Color, item.Bounds)
    '                Case GDE.TemplateItemTypes.DRAWRECT : Me.m_PDF.DrawRectangle(item.Color, item.Bounds)
    '                Case GDE.TemplateItemTypes.FILLRECT : Me.m_PDF.DrawRectangle(item.Color, item.Bounds)
    '                Case GDE.TemplateItemTypes.DRAWIMAGE : Me.m_PDF.DrawImage(Me.LoadImage(item.Text), item.Bounds)
    '                Case GDE.TemplateItemTypes.NEWPAGE : Me.m_PDF.AddPage()
    '                Case GDE.TemplateItemTypes.TEXTOUT : Me.m_PDF.TextOut(item.Color, item.Bounds, item.Text)
    '                Case GDE.TemplateItemTypes.DATAFIELD : Me.m_PDF.TextOut(item.Color, item.Bounds, Me.GetDataField(item.Text))
    '                Case GDE.TemplateItemTypes.EXPRESSION : Me.m_PDF.TextOut(item.Color, item.Bounds, Me.EvaluateExpression(item.Text))
    '                Case Else
    '                    Throw New NotSupportedException
    '            End Select
    '        Next
    '    End Sub

    '    Public Overrides Sub SaveToFile(fileName As String)
    '        Me.m_PDF.Save(fileName)
    '    End Sub
    'End Class

End Namespace
