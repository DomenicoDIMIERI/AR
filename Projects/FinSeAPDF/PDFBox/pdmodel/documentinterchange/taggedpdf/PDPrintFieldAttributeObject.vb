Imports System.Text
Imports FinSeA.org.apache.pdfbox.cos

Namespace org.apache.pdfbox.pdmodel.documentinterchange.taggedpdf

    '/**
    ' * A PrintField attribute object.
    ' * 
    ' * @author <a href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * @version $Revision: $
    ' */
    Public Class PDPrintFieldAttributeObject
        Inherits PDStandardAttributeObject

        ''' <summary>
        ''' standard attribute owner: PrintField
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OWNER_PRINT_FIELD As String = "PrintField"

        Private Const ROLE As String = "Role"
        Private Const CHECKED As String = "checked"
        Private Const DESC As String = "Desc"

        ''' <summary>
        ''' role: rb: Radio button
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ROLE_RB As String = "rb"

        ''' <summary>
        ''' role: cb: Check box
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ROLE_CB As String = "cb"

        ''' <summary>
        ''' role: pb: Push button
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ROLE_PB As String = "pb"

        ''' <summary>
        ''' role: tv: Text-value field
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ROLE_TV As String = "tv"

        ''' <summary>
        ''' checked state: on
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHECKED_STATE_ON As String = "on"

        ''' <summary>
        ''' checked state: off
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHECKED_STATE_OFF As String = "off"

        ''' <summary>
        ''' checked state: neutral
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CHECKED_STATE_NEUTRAL As String = "neutral"


        '/**
        ' * Default constructor.
        ' */
        Public Sub New()
            Me.setOwner(OWNER_PRINT_FIELD)
        End Sub

        '/**
        ' * Creates a new PrintField attribute object with a given dictionary.
        ' * 
        ' * @param dictionary the dictionary
        ' */
        Public Sub New(ByVal dictionary As COSDictionary)
            MyBase.New(dictionary)
        End Sub


        '/**
        ' * Gets the role.
        ' * 
        ' * @return the role
        ' */
        Public Function getRole() As String
            Return Me.getName(ROLE)
        End Function

        '/**
        ' * Sets the role. The value of Role shall be one of the following:
        ' * <ul>
        ' *   <li>{@link #ROLE_RB},</li>
        ' *   <li>{@link #ROLE_CB},</li>
        ' *   <li>{@link #ROLE_PB},</li>
        ' *   <li>{@link #ROLE_TV}.</li>
        ' * </ul>
        ' * 
        ' * @param role the role
        ' */
        Public Sub setRole(ByVal role As String)
            Me.setName(role, role)
        End Sub

        '/**
        ' * Gets the checked state. The default value is {@link #CHECKED_STATE_OFF}.
        ' * 
        ' * @return the checked state
        ' */
        Public Function getCheckedState() As String
            Return Me.getName(CHECKED, CHECKED_STATE_OFF)
        End Function

        '/**
        ' * Sets the checked state. The value shall be one of:
        ' * <ul>
        ' *   <li>{@link #CHECKED_STATE_ON},</li>
        ' *   <li>{@link #CHECKED_STATE_OFF} (default), or</li>
        ' *   <li>{@link #CHECKED_STATE_NEUTRAL}.</li>
        ' * </ul>
        ' * 
        ' * @param checkedState the checked state
        ' */
        Public Sub setCheckedState(ByVal checkedState As String)
            Me.setName(CHECKED, checkedState)
        End Sub

        '/**
        ' * Gets the alternate name of the field (Desc).
        ' * 
        ' * @return the alternate name of the field
        ' */
        Public Function getAlternateName() As String
            Return Me.getString(DESC)
        End Function

        '/**
        ' * Sets the alternate name of the field (Desc).
        ' * 
        ' * @param alternateName the alternate name of the field
        ' */
        Public Sub setAlternateName(ByVal alternateName As String)
            Me.setString(DESC, alternateName)
        End Sub

        Public Overrides Function toString() As String
            Dim sb As New StringBuilder()
            sb.Append(MyBase.toString())
            If (Me.isSpecified(ROLE)) Then
                sb.Append(", Role=").Append(Me.getRole())
            End If
            If (Me.isSpecified(CHECKED)) Then
                sb.Append(", Checked=").Append(Me.getCheckedState())
            End If
            If (Me.isSpecified(DESC)) Then
                sb.Append(", Desc=").Append(Me.getAlternateName())
            End If
            Return sb.ToString()
        End Function

    End Class

End Namespace
