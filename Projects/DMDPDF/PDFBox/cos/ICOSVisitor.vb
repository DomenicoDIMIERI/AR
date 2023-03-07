Imports FinSeA.org.apache.pdfbox.exceptions

Namespace org.apache.pdfbox.cos


    '/**
    ' * An interface for visiting a PDF document at the type (COS) level.
    ' *
    ' * @author Michael Traut
    ' * @version $Revision: 1.6 $
    ' */
    Public Interface ICOSVisitor
        '/**
        ' * Notification of visit to Array object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromArray(ByVal obj As COSArray) As Object  'throws COSVisitorException;

        '/**
        ' * Notification of visit to boolean object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromBoolean(ByVal obj As COSBoolean) As Object  'throws COSVisitorException;

        '/**
        ' * Notification of visit to dictionary object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromDictionary(ByVal obj As COSDictionary) As Object  'throws COSVisitorException;

        '/**
        ' * Notification of visit to document object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromDocument(ByVal obj As COSDocument) As Object 'throws COSVisitorException;

        '/**
        ' * Notification of visit to Single object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromFloat(ByVal obj As COSFloat) As Object 'throws COSVisitorException;

        '/**
        ' * Notification of visit to integer object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromInt(ByVal obj As COSInteger) As Object 'throws COSVisitorException;

        '/**
        ' * Notification of visit to name object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromName(ByVal obj As COSName) As Object 'throws COSVisitorException;

        '/**
        ' * Notification of visit to null object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromNull(ByVal obj As COSNull) As Object 'throws COSVisitorException;

        '/**
        ' * Notification of visit to stream object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromStream(ByVal obj As COSStream) As Object 'throws COSVisitorException;

        '/**
        ' * Notification of visit to string object.
        ' *
        ' * @param obj The Object that is being visited.
        ' * @return any Object depending on the visitor implementation, or null
        ' * @throws COSVisitorException If there is an error while visiting this object.
        ' */
        Function visitFromString(ByVal obj As COSString) As Object 'throws COSVisitorException;

    End Interface

End Namespace
