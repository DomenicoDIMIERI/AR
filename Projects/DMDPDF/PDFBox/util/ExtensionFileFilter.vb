Imports FinSeA.Io
'imports javax.swing.filechooser.FileFilter;

Namespace org.apache.pdfbox.util

    '/**
    ' * A FileFilter that will only accept files of a certain extension.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class ExtensionFileFilter
        'Inherits FileFilter

        Private extensions() As String = Nothing
        Private desc As String

        '/**
        ' * Constructor.
        ' *
        ' * @param ext A list of filename extensions, ie new String[] { "PDF"}.
        ' * @param description A description of the files.
        ' */
        Public Sub New(ByVal ext() As String, ByVal description As String)
            Me.extensions = ext
            Me.desc = description
        End Sub

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function accept(ByVal pathname As String) As Boolean
            If (FinSeA.Sistema.FileSystem.FolderExists(pathname)) Then Return True
            Dim acceptable As Boolean = False
            Dim name As String = FinSeA.Sistema.FileSystem.GetFileName(pathname).ToUpper()
            Dim i As Integer = 0
            While (Not acceptable AndAlso i < extensions.Length)
                If (name.EndsWith(extensions(i).ToUpper())) Then
                    acceptable = True
                End If
                i += 1
            End While
            Return acceptable
        End Function

        '/**
        ' * {@inheritDoc}
        ' */
        Public Function getDescription() As String
            Return desc
        End Function

    End Class

End Namespace
