Imports System
Imports System.Runtime.CompilerServices
Imports DMD.org.dmdpdf.documents.contents

Namespace org.dmdpdf.samples.cli

    Module Utils

        '/**
        '  <summary>Gets the value of the given property.</summary>
        '  <param name="propertyName">Property name whose value has to be retrieved.</param>
        '*/
        <Extension>
        Public Function [Get](ByVal obj As Object, ByVal propertyName As String) As Object
            Return obj.GetType().GetProperty(propertyName).GetValue(obj, Nothing)
        End Function

        '/**
        '  <summary> Gets whether the Object's definition is compatible with the given type's.</summary>
        '  <remarks> This extension method represents a workaround To the lack Of type covariance support In C#.
        '  You may consider it equivalent To a (forbidden) overloading Of the 'is' operator.</remarks>
        '  <param name = "type" > Type To verify against the Object's definition.</param>
        '*/
        <Extension>
        Public Function [Is](ByVal obj As Object, ByVal Type As System.Type) As Boolean
            Dim objType As Type = obj.GetType()
            Dim typeDefinition As Type = GetDefinition(Type)
            While (objType IsNot Nothing)
                If (typeDefinition Is GetDefinition(objType)) Then
                    Return True
                End If
                objType = objType.BaseType
            End While
            Return False
        End Function

        Public Sub Prompt(ByVal Message As String)
            Console.WriteLine(vbLf & Message)
            Console.WriteLine("Press ENTER to continue")
            Console.ReadLine()
        End Sub

        Private Function GetDefinition(ByVal type As Type) As Type
            If (type.IsGenericType) Then
                Return type.GetGenericTypeDefinition()
            Else
                Return type
            End If
        End Function


    End Module

End Namespace