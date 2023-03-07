Imports System.IO

Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA.org.apache.pdfbox.pdmodel.graphics.xobject
Imports FinSeA.org.apache.pdfbox.pdmodel.interactive.annotation

Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure


    '/**
    ' * An object reference.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDObjectReference
        Implements COSObjectable

        ''' <summary>
        ''' TYPE of this object.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TYPE As String = "OBJR"

        Private dictionary As COSDictionary

        '/**
        ' * Returns the underlying dictionary.
        ' * 
        ' * @return the dictionary
        ' */
        Protected Function getCOSDictionary() As COSDictionary
            Return Me.dictionary
        End Function

        '/**
        ' * Default Constructor.
        ' *
        ' */
        Public Sub New()
            Me.dictionary = New COSDictionary()
            Me.dictionary.setName(COSName.TYPE, TYPE)
        End Sub

        '/**
        ' * Constructor for an existing object reference.
        ' *
        ' * @param theDictionary The existing dictionary.
        ' */
        Public Sub New(ByVal theDictionary As COSDictionary)
            dictionary = theDictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.dictionary
        End Function

        '/**
        ' * Gets a higher-level object for the referenced object.
        ' * Currently this method may return a {@link PDAnnotation},
        ' * a {@link PDXObject} or <code>null</code>.
        ' * 
        ' * @return a higher-level object for the referenced object
        ' */
        Public Function getReferencedObject() As COSObjectable
            Dim obj As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.OBJ)
            If (Not (TypeOf (obj) Is COSDictionary)) Then
                Return Nothing
            End If
            Try
                Dim xobject As PDXObject = PDXObject.createXObject(obj)
                If (xobject IsNot Nothing) Then
                    Return xobject
                End If
                Dim objDictionary As COSDictionary = obj
                Dim annotation As PDAnnotation = PDAnnotation.createAnnotation(obj)
                '/*
                ' * COSName.TYPE is optional, so if annotation is of type unknown and
                ' * COSName.TYPE is not COSName.ANNOT it still may be an annotation.
                ' * TODO shall we return the annotation object instead of null?
                ' * what else can be the target of the object reference?
                ' */
                If (Not (TypeOf (annotation) Is PDAnnotationUnknown) OrElse COSName.ANNOT.equals(objDictionary.getDictionaryObject(COSName.TYPE))) Then
                    Return annotation
                End If
            Catch exception As IOException
                ' this can only happen if the target is an XObject.
            End Try
            Return Nothing
        End Function

        '/**
        ' * Sets the referenced annotation.
        ' * 
        ' * @param annotation the referenced annotation
        ' */
        Public Sub setReferencedObject(ByVal annotation As PDAnnotation)
            Me.getCOSDictionary().setItem(COSName.OBJ, annotation)
        End Sub

        '/**
        ' * Sets the referenced XObject.
        ' * 
        ' * @param xobject the referenced XObject
        ' */
        Public Sub setReferencedObject(ByVal xobject As PDXObject)
            Me.getCOSDictionary().setItem(COSName.OBJ, xobject)
        End Sub

    End Class

End Namespace
