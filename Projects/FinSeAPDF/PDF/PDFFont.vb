Namespace PDF

    Public Class PDFFont
        '  Public i As Integer
        ' Public cw As Single
        Public xtype As String = "TrueType"
        Public xname As String '='Calligrapher-Regular';
        Public xdesc As PDFFontDescriptor '=lib.newArray('Ascent',899,'Descent',-234,'CapHeight',731,'Flags',32,'FontBBox','[-50 -234 1328 899]','ItalicAngle',0.0,'StemV',70,'MissingWidth',800);
        Public xup As Single '-200;
        Public xut As Single '=20;
        Public xcw As New CKeyCollection(Of Single) 'char widths
        Public xenc As String '='cp1252';
        Public xdiff As String '='';
        Public xfile As String '='calligra.ttf';
        Public xoriginalsize As Integer '=40120;
        'Public n As Integer

        Public Sub New()
        End Sub

        Public Sub New( _
                      ByVal xtype As String, _
                      ByVal xname As String, _
                      ByVal xdesc As PDFFontDescriptor, _
                      ByVal xup As Single, _
                      ByVal xut As Single, _
                      ByVal xcw As CKeyCollection(Of Single), _
                      ByVal xenc As String, _
                      ByVal xdiff As String, _
                      ByVal xfile As String, _
                      ByVal xoriginalsize As Integer _
                      )
            Me.xtype = xtype
            Me.xname = xname
            Me.xdesc = xdesc
            Me.xup = xup
            Me.xut = xut
            Me.xcw = xcw
            Me.xenc = xenc
            Me.xdiff = xdiff
            Me.xfile = xfile
            Me.xoriginalsize = xoriginalsize
        End Sub

        Public Sub New(ByVal xtype As String, ByVal xname As String, ByVal xdesc As PDFFontDescriptor, ByVal xup As Single, ByVal xut As Single, ByVal xcw As Object(), ByVal xenc As String, ByVal xdiff As String, ByVal xfile As String, ByVal xoriginalsize As Integer)
            Me.xtype = xtype
            Me.xname = xname
            Me.xdesc = xdesc
            Me.xup = xup
            Me.xut = xut
            For i As Integer = 0 To UBound(xcw) Step 2
                Me.xcw.Add(CStr(xcw(i)), CSng(xcw(i + 1)))
            Next
            Me.xenc = xenc
            Me.xdiff = xdiff
            Me.xfile = xfile
            Me.xoriginalsize = xoriginalsize
        End Sub

        Protected Sub SetWidths(ByVal values As Object())
            Me.xcw.Clear()
            For i As Integer = 0 To UBound(values) Step 2
                Me.xcw.Add(CStr(values(i)), CSng(values(i + 1)))
            Next
        End Sub
    End Class

End Namespace
