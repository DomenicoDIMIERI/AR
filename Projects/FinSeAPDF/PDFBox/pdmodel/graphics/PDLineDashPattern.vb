Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common
Imports FinSeA

Namespace org.apache.pdfbox.pdmodel.graphics

    '   /**
    '* This class represents the line dash pattern for a graphics state.  See PDF
    '* Reference 1.5 section 4.3.2
    '*
    '* @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    '* @version $Revision: 1.7 $
    '*/
    Public Class PDLineDashPattern
        Implements COSObjectable, ICloneable

        Private lineDashPattern As COSArray = Nothing

        '/**
        ' * Creates a blank line dash pattern.  With no dashes and a phase of 0.
        ' */
        Public Sub New()
            lineDashPattern = New COSArray()
            lineDashPattern.add(New COSArray())
            lineDashPattern.add(COSInteger.ZERO)
        End Sub

        '/**
        ' * Constructs a line dash pattern from an existing array.
        ' *
        ' * @param ldp The existing line dash pattern.
        ' */
        Public Sub New(ByVal ldp As COSArray)
            lineDashPattern = ldp
        End Sub

        '/**
        ' * Constructs a line dash pattern from an existing array.
        ' *
        ' * @param ldp The existing line dash pattern.
        ' * @param phase The phase for the line dash pattern.
        ' */
        Public Sub New(ByVal ldp As COSArray, ByVal phase As Integer)
            lineDashPattern = New COSArray()
            lineDashPattern.add(ldp)
            lineDashPattern.add(COSInteger.get(phase))
        End Sub

        Public Function clone() As Object Implements ICloneable.Clone
            Dim pattern As PDLineDashPattern = Nothing
            pattern = Me.MemberwiseClone
            pattern.setDashPattern(getDashPattern())
            pattern.setPhaseStart(getPhaseStart())
            Return pattern
        End Function

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return lineDashPattern
        End Function

        '/**
        ' * This will get the line dash pattern phase.  The dash phase specifies the
        ' * distance into the dash pattern at which to start the dash.
        ' *
        ' * @return The line dash pattern phase.
        ' */
        Public Function getPhaseStart() As Integer
            Dim phase As COSNumber = lineDashPattern.get(1)
            Return phase.intValue()
        End Function

        '/**
        ' * This will set the line dash pattern phase.
        ' *
        ' * @param phase The new line dash patter phase.
        ' */
        Public Sub setPhaseStart(ByVal phase As Integer)
            lineDashPattern.set(1, phase)
        End Sub

        '/**
        ' * This will return a list of java.lang.Integer objects that represent the line
        ' * dash pattern appearance.
        ' *
        ' * @return The line dash pattern.
        ' */
        Public Function getDashPattern() As List
            Dim dashPatterns As COSArray = lineDashPattern.get(0)
            Return COSArrayList.convertIntegerCOSArrayToList(dashPatterns)
        End Function

        '/**
        ' * Get the line dash pattern as a COS object.
        ' *
        ' * @return The cos array line dash pattern.
        ' */
        Public Function getCOSDashPattern() As COSArray
            Return lineDashPattern.get(0)
        End Function

        '/**
        ' * This will replace the existing line dash pattern.
        ' *
        ' * @param dashPattern A list of java.lang.Integer objects.
        ' */
        Public Sub setDashPattern(ByVal dashPattern As List)
            lineDashPattern.set(0, COSArrayList.converterToCOSArray(dashPattern))
        End Sub

        '/**
        ' * Checks if the dashPattern is empty or all values equals 0.
        ' * 
        ' * @return true if the dashPattern is empty or all values equals 0  
        ' */
        Public Function isDashPatternEmpty() As Boolean
            Dim dashPattern As Single() = getCOSDashPattern().toFloatArray()
            Dim dashPatternEmpty As Boolean = True
            If (dashPattern IsNot Nothing) Then
                Dim arraySize As Integer = dashPattern.Length
                For i As Integer = 0 To arraySize - 1
                    If (dashPattern(i) > 0) Then
                        dashPatternEmpty = False
                        Exit For
                    End If
                Next
            End If
            Return dashPatternEmpty
        End Function

    End Class

End Namespace
