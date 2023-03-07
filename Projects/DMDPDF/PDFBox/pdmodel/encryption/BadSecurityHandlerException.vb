Namespace org.apache.pdfbox.pdmodel.encryption

    '/**
    ' * This exception can be thrown by the SecurityHandlersManager class when
    ' * a document required an unimplemented security handler to be opened.
    ' *
    ' * @author Benoit Guillon (benoit.guillon@snv.jussieu.fr)
    ' * @version $Revision: 1.2 $
    ' */
    Public Class BadSecurityHandlerException
        Inherits System.Exception


        ''' <summary>
        ''' Default Constructor.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="e">A sub exception.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal e As System.Exception)
            MyBase.New(e.Message, e)
        End Sub

        '/**
        ' * Constructor.
        ' *
        ' * @param msg Message describing exception.
        ' */
        Public Sub New(ByVal msg As String)
            MyBase.New(msg)
        End Sub

    End Class

End Namespace
