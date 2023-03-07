Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common

Namespace org.apache.pdfbox.pdmodel.interactive.measurement

    '/**
    ' * This class represents a number format dictionary.
    ' * 
    ' * @version $Revision: 1.0$
    ' *
    ' */
    Public Class PDNumberFormatDictionary
        Implements COSObjectable

        ''' <summary>
        ''' The type of the dictionary.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const TYPE = "NumberFormat"

        ''' <summary>
        ''' Constant indicating that the label specified by U is a suffix to the value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LABEL_SUFFIX_TO_VALUE = "S"

        ''' <summary>
        ''' Constant indicating that the label specified by U is a postfix to the value.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const LABEL_PREFIX_TO_VALUE = "P"

        ''' <summary>
        ''' Constant for showing a fractional value as decimal to the precision specified by the D entry.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FRACTIONAL_DISPLAY_DECIMAL = "D"

        ''' <summary>
        ''' Constant for showing a fractional value as a fraction with denominator specified by the D entry.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FRACTIONAL_DISPLAY_FRACTION = "F"

        ''' <summary>
        ''' Constant for showing a fractional value without fractional part; round to the nearest whole unit.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FRACTIONAL_DISPLAY_ROUND = "R"

        ''' <summary>
        ''' Constant for showing a fractional value without fractional part; truncate to achieve whole units.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const FRACTIONAL_DISPLAY_TRUNCATE = "T"

        Private numberFormatDictionary As COSDictionary

        '/**
        ' * Constructor.
        ' */
        Public Sub New()
            Me.numberFormatDictionary = New COSDictionary()
            Me.numberFormatDictionary.setName(COSName.TYPE, TYPE)
        End Sub

        '/**
        ' * Constructor.
        ' * 
        ' * @param dictionary the corresponding dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            Me.numberFormatDictionary = dictionary
        End Sub

        Public Function getCOSObject() As COSBase Implements COSObjectable.getCOSObject
            Return Me.numberFormatDictionary
        End Function

        '/**
        ' * This will return the dictionary.
        ' * 
        ' * @return the number format dictionary
        ' */
        Public Function getDictionary() As COSDictionary
            Return Me.numberFormatDictionary
        End Function

        '/**
        ' * This will return the type of the number format dictionary.
        ' * It must be "NumberFormat"
        ' * 
        ' * @return the type
        ' */
        Public Function getNumFormatType() As String
            Return TYPE
        End Function

        '/**
        ' * This will return the label for the units.
        ' * 
        ' * @return the label for the units
        ' */
        Public Function getUnits() As String
            Return Me.getDictionary().getString("U")
        End Function

        '/**
        ' * This will set the label for the units.
        ' * 
        ' * @param units the label for the units
        ' */
        Public Sub setUnits(ByVal units As String)
            Me.getDictionary().setString("U", units)
        End Sub

        '/**
        ' * This will return the conversion factor.
        ' * 
        ' * @return the conversion factor
        ' */
        Public Function getConversionFactor() As Single
            Return Me.getDictionary().getFloat("C")
        End Function

        '/**
        ' * This will set the conversion factor.
        ' * 
        ' * @param conversionFactor the conversion factor
        ' */
        Public Sub setConversionFactor(ByVal conversionFactor As Single)
            Me.getDictionary().setFloat("C", conversionFactor)
        End Sub

        '/** 
        ' * This will return the value for the manner to display a fractional value.
        ' *  
        ' * @return the manner to display a fractional value
        ' */
        Public Function getFractionalDisplay() As String
            Return Me.getDictionary().getString("F", FRACTIONAL_DISPLAY_DECIMAL)
        End Function

        '/** 
        ' * This will set the value for the manner to display a fractional value.
        ' * Allowed values are "D", "F", "R" and "T"
        ' * @param fractionalDisplay the manner to display a fractional value
        ' */
        Public Sub setFractionalDisplay(ByVal fractionalDisplay As String)
            If ((fractionalDisplay Is Nothing) OrElse FRACTIONAL_DISPLAY_DECIMAL.Equals(fractionalDisplay) OrElse FRACTIONAL_DISPLAY_FRACTION.Equals(fractionalDisplay) OrElse FRACTIONAL_DISPLAY_ROUND.Equals(fractionalDisplay) OrElse FRACTIONAL_DISPLAY_TRUNCATE.Equals(fractionalDisplay)) Then
                Me.getDictionary().setString("F", fractionalDisplay)
            Else
                Throw New ArgumentOutOfRangeException("Value must be ""D"", ""F"", ""R"", or ""T"", (or null).")
            End If
        End Sub

        '/**
        ' * This will return the precision or denominator of a fractional amount.
        ' * 
        ' * @return the precision or denominator
        ' */
        Public Function getDenominator() As Integer
            Return Me.getDictionary().getInt("D")
        End Function

        '/**
        ' * This will set the precision or denominator of a fractional amount.
        ' * 
        ' * @param denominator the precision or denominator
        ' */
        Public Sub setDenominator(ByVal denominator As Integer)
            Me.getDictionary().setInt("D", denominator)
        End Sub

        '/**
        ' * This will return the value indication if the denominator of the fractional value is reduced/truncated .
        ' * 
        ' * @return fd
        ' */
        Public Function isFD() As Boolean
            Return Me.getDictionary().getBoolean("FD", False)
        End Function

        '/**
        ' * This will set the value indication if the denominator of the fractional value is reduced/truncated .
        ' * The denominator may not be reduced/truncated if true
        ' * @param fd fd
        ' */
        Public Sub setFD(ByVal fd As Boolean)
            Me.getDictionary().setBoolean("FD", fd)
        End Sub

        '/**
        ' * This will return the text to be used between orders of thousands in display of numerical values.
        ' * 
        ' * @return thousands separator
        ' */
        Public Function getThousandsSeparator() As String
            Return Me.getDictionary().getString("RT", ",")
        End Function

        '/**
        ' * This will set the text to be used between orders of thousands in display of numerical values.
        ' * 
        ' * @param thousandsSeparator thousands separator
        ' */
        Public Sub setThousandsSeparator(ByVal thousandsSeparator As String)
            Me.getDictionary().setString("RT", thousandsSeparator)
        End Sub

        '/**
        ' * This will return the text to be used as the decimal point in displaying numerical values.
        ' * 
        ' * @return decimal separator
        ' */
        Public Function getDecimalSeparator() As String
            Return Me.getDictionary().getString("RD", ".")
        End Function

        '/**
        ' * This will set the text to be used as the decimal point in displaying numerical values.
        ' * 
        ' * @param decimalSeparator decimal separator
        ' */
        Public Sub setDecimalSeparator(ByVal decimalSeparator As String)
            Me.getDictionary().setString("RD", decimalSeparator)
        End Sub

        '/**
        ' * This will return the text to be concatenated to the left of the label specified by U.
        ' * @return label prefix
        ' */
        Public Function getLabelPrefixString() As String
            Return Me.getDictionary().getString("PS", " ")
        End Function

        '/**
        ' * This will set the text to be concatenated to the left of the label specified by U.
        ' * @param labelPrefixString label prefix
        ' */
        Public Sub setLabelPrefixString(ByVal labelPrefixString As String)
            Me.getDictionary().setString("PS", labelPrefixString)
        End Sub

        '/**
        ' * This will return the text to be concatenated after the label specified by U.
        ' * 
        ' * @return label suffix
        ' */
        Public Function getLabelSuffixString() As String
            Return Me.getDictionary().getString("SS", " ")
        End Function

        '/**
        ' * This will set the text to be concatenated after the label specified by U.
        ' * 
        ' * @param labelSuffixString label suffix
        ' */
        Public Sub setLabelSuffixString(ByVal labelSuffixString As String)
            Me.getDictionary().setString("SS", labelSuffixString)
        End Sub

        '/**
        ' * This will return a value indicating the ordering of the label specified by U to the calculated unit value.
        ' * 
        ' * @return label position 
        ' */
        Public Function getLabelPositionToValue() As String
            Return Me.getDictionary().getString("O", LABEL_SUFFIX_TO_VALUE)
        End Function

        '/**
        ' * This will set the value indicating the ordering of the label specified by U to the calculated unit value.
        ' * Possible values are "S" and "P"
        ' * 
        ' * @param labelPositionToValue label position 
        ' */
        Public Sub setLabelPositionToValue(ByVal labelPositionToValue As String)
            If ((labelPositionToValue Is Nothing) OrElse LABEL_PREFIX_TO_VALUE.Equals(labelPositionToValue) OrElse LABEL_SUFFIX_TO_VALUE.Equals(labelPositionToValue)) Then
                Me.getDictionary().setString("O", labelPositionToValue)
            Else
                Throw New ArgumentOutOfRangeException("Value must be ""S"", or ""P"" (or null).")
            End If
        End Sub

    End Class

End Namespace
