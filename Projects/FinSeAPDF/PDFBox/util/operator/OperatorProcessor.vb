Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.util

Namespace org.apache.pdfbox.util.operator

    '/**
    ' * @author Huault : huault@free.fr
    ' * @version $Revision: 1.3 $
    ' */
    Public MustInherit Class OperatorProcessor


        ''' <summary>
        ''' The stream engine processing context.
        ''' </summary>
        ''' <remarks></remarks>
        Protected context As PDFStreamEngine = Nothing

        '/**
        ' * Constructor.
        ' *
        ' */
        Protected Sub New()
        End Sub

        '/**
        ' * Get the context for processing.
        ' *
        ' * @return The processing context.
        ' */
        Protected Function getContext() As PDFStreamEngine
            Return context
        End Function

        '/**
        ' * Set the processing context.
        ' *
        ' * @param ctx The context for processing.
        ' */
        Public Sub setContext(ByVal ctx As PDFStreamEngine)
            context = ctx
        End Sub

        '/**
        ' * process the operator.
        ' * @param operator The operator that is being processed.
        ' * @param arguments arguments needed by this operator.
        ' *
        ' * @throws IOException If there is an error processing the operator.
        ' */
        Public MustOverride Sub process(ByVal [operator] As PDFOperator, ByVal arguments As List(Of COSBase))

    End Class

End Namespace


