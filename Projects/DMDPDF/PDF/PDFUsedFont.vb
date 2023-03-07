Namespace PDF

    Public Class PDFUsedFont
        Private m_Font As PDFFont
        Public i As Integer
        Public xtype As String = "TrueType"
        Public xname As String '='Calligrapher-Regular';
        Public xup As Single '-200;
        Public xut As Single '=20;
        Public xcw As New CKeyCollection(Of Single) 'char widths
        Public n As Integer
        Public xfile As String
        Public xenc As String
        Public diff As String
        Public xdesc As PDFFontDescriptor

        Public Sub New()
        End Sub

        Public Sub New( _
                      ByVal i As Integer, _
                      ByVal xtype As String, _
                      ByVal xname As String, _
                      ByVal xup As Single, _
                      ByVal xut As Single, _
                      ByVal xcw As CKeyCollection(Of Single) _
                      )
            Me.i = i
            Me.xtype = xtype
            Me.xname = xname
            Me.xup = xup
            Me.xut = xut
            Me.xcw = xcw
        End Sub

    End Class

End Namespace