'/*
'  Copyright 2007-2010 Stefano Chizzolini. http://www.dmdpdf.org

'  Contributors:
'    * Stefano Chizzolini (original code developer, http://www.stefanochizzolini.it)

'  This file should be part of the source code distribution of "PDF Clown library" (the
'  Program): see the accompanying README files for more info.

'  This Program is free software; you can redistribute it and/or modify it under the terms
'  of the GNU Lesser General Public License as published by the Free Software Foundation;
'  either version 3 of the License, or (at your option) any later version.

'  This Program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
'  either expressed or implied; without even the implied warranty of MERCHANTABILITY or
'  FITNESS FOR A PARTICULAR PURPOSE. See the License for more details.

'  You should have received a copy of the GNU Lesser General Public License along with this
'  Program (see README files); if not, go to the GNU website (http://www.gnu.org/licenses/).

'  Redistribution and use, with or without modification, are permitted provided that such
'  redistributions retain the above copyright notice, license and disclaimer, along with
'  this list of conditions.
'*/

Imports DMD.org.dmdpdf.bytes
Imports DMD.org.dmdpdf.objects

Imports System
Imports System.Collections
Imports System.Collections.Generic

Namespace DMD.org.dmdpdf.documents.contents.objects

    '/**
    '  <summary>Inline image entries (anonymous) operation [PDF:1.6:4.8.6].</summary>
    '  <remarks>This is a figurative operation necessary to constrain the inline image entries section
    '  within the content stream model.</remarks>
    '*/
    <PDF(VersionEnum.PDF10)>
    Public NotInheritable  class InlineImageHeader
        Inherits Operation
        Implements IDictionary(Of PdfName, PdfDirectObject)

#Region "dynamic"
#Region "constructors"

        ' [FIX0.0.4:2] Null operator.
        Public Sub New(ByVal operands As IList(Of PdfDirectObject))
            MyBase.new(String.Empty, operands)
        End Sub

#End Region

#Region "Public"
#Region "IDictionary"

        Public sub Add( byval key as PdfName , byval value as PdfDirectObject  ) Implements IDictionary(Of PdfName, PdfDirectObject).Add 
            If (ContainsKey(key)) Then Throw New ArgumentException("Key '" & key.toString  & "' already in use.", "key")
            Me(key) = value
        End Sub 

        Public function ContainsKey( byval key as PdfName  ) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject).ContainsKey 
            return GetKeyIndex(key) IsNot Nothing 
        End Function 

        Public ReadOnly Property  Keys As ICollection(of PdfName) Implements IDictionary(Of PdfName, PdfDirectObject).Keys 
          Get
            Dim _keys As ICollection(of PdfName) = New List(of PdfName)
                Dim length As Integer = Me._operands.Count - 1
                For index As Integer = 0 to length - 1  Step 2
                    _keys.Add(CType(Me._operands(index), PdfName))
                next   
            Return _keys
          End Get
        End Property 

    Public function Remove( byval key as PdfName  ) As Boolean  implements IDictionary(Of PdfName, PdfDirectObject).Remove
      Dim index As Integer ? = GetKeyIndex(key)
      If (Not index.HasValue) Then Return False
      Me._operands.RemoveAt(index.Value)
      Me._operands.RemoveAt(index.Value)
      Return True
    End Function 

    Public default property Item(byval key as PdfName       ) As PdfDirectObject implements IDictionary(Of PdfName, PdfDirectObject).Item  
      Get
        '/*
        '  NOTE: This Is an intentional violation of the official .NET Framework Class
        '  Library prescription: no exception thrown anytime a key Is Not found.
        '*/
        Dim index As Integer? = GetKeyIndex(key)
        If (index .HasValue  = False ) Then
            Return  Nothing 
        Else 
            Return Me._operands(index.Value+1)
        end if  
      End get
      Set(BYVal value As PdfDirectObject)
        Dim index As Integer? = GetKeyIndex(key)
        If (index is nothing ) Then
          Me._operands.Add(key)
          Me._operands.Add(value)
        Else
          Me._operands(index.Value)= key
          Me._operands(index.Value+1) = value
        end if  
      End Set
    End Property 

    Public function TryGetValue( byval key as PdfName , byref value as PdfDirectObject       ) As Boolean Implements IDictionary(Of PdfName, PdfDirectObject) .TryGetValue 
        throw New NotImplementedException
    End Function 


    Public ReadOnly Property  Values As ICollection(Of PdfDirectObject) Implements IDictionary(Of PdfName, PdfDirectObject).Values 
      Get
        Dim _values As ICollection(Of PdfDirectObject) = New List(Of PdfDirectObject)
        Dim length As Integer = Me._operands.Count - 1
        For index As Integer = 1 To length - 1 Step 2
            _values.Add(Me._operands(index))
        Next
        Return _values
      End get
    End Property 

    #Region "ICollection"

        private sub Add( byval  keyValuePair As KeyValuePair(of PdfName,PdfDirectObject)) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).add
            Me.Add(keyValuePair.Key,keyValuePair.Value)
        End Sub

        Public sub Clear( ) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Clear 
            Me._operands.Clear()
        End Sub

        private function Contains( byval keyValuePair As KeyValuePair(of PdfName,PdfDirectObject) ) As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Contains 
            return (Me(keyValuePair.Key) is keyValuePair.Value)
        End Function

        Public Sub CopyTo(ByVal keyValuePairs As KeyValuePair(Of PdfName, PdfDirectObject)(), ByVal index As Integer) Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).CopyTo
            Throw New NotImplementedException
        End Sub

        Public ReadOnly Property Count As Integer Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Count
            Get
                Return CInt(Me._operands.Count / 2)
            End Get
        End Property

        Public ReadOnly Property  IsReadOnly As Boolean Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)) .IsReadOnly 
            Get
                return false
            End get
        End Property 

        Public Function Remove( ByVal keyValuePair As KeyValuePair(Of PdfName,PdfDirectObject)) As Boolean  Implements ICollection(Of KeyValuePair(Of PdfName, PdfDirectObject)).Remove 
            Throw New NotImplementedException
        End Function


    #Region "IEnumerable<KeyValuePair(of PdfName,PdfDirectObject)>"

        
    '  For (
    '    int index = 0,
    '      length = operands.Count - 1;
    '    index < length;
    '    index += 2
    '    )
    '  {
    '    yield return New KeyValuePair(of PdfName,PdfDirectObject)(
    '      (PdfName)operands[index],
    '      operands[index+1]
    '      );
    '  }
    '}

        Private class mEnumerator
            Implements IEnumerator(Of KeyValuePair(of PdfName,PdfDirectObject))

            Public o As InlineImageHeader
            Private index As Integer 
            Private length As Integer 

            Public sub New(BYVal o As InlineImageHeader )
                Me.o = o
                Me.Reset
            End sub

            Public ReadOnly Property Current As KeyValuePair(Of PdfName, PdfDirectObject) Implements IEnumerator(Of KeyValuePair(Of PdfName, PdfDirectObject)).Current
                Get
                    Return New KeyValuePair(Of PdfName, PdfDirectObject)(CType(Me.o._operands(Me.index), PdfName), Me.o._operands(Me.index + 1))
                End Get
            End Property

            Private ReadOnly Property IEnumerator_Current As Object Implements IEnumerator.Current
                Get
                    Return Me.Current
                End Get
            End Property

            Public Sub Reset() Implements IEnumerator.Reset
                Me.index = -1
                Me.length = Me.o._operands.Count
            End Sub

            Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
                If (Me.index + 2 < Me.length) Then
                    Me.index += 2
                    Return True
                Else
                    Return False
                End If
            End Function
 

            ' Questo codice viene aggiunto da Visual Basic per implementare in modo corretto il criterio Disposable.
            Public Sub Dispose() Implements IDisposable.Dispose
                Me.o = Nothing
                ' TODO: rimuovere il commento dalla riga seguente se è stato eseguito l'override di Finalize().
                ' GC.SuppressFinalize(Me)
            End Sub
 

        End Class

        Public function GetEnumerator() As IEnumerator(Of KeyValuePair(of PdfName,PdfDirectObject)) Implements IEnumerable(Of KeyValuePair(Of PdfName, PdfDirectObject)) .GetEnumerator 
            Return New mEnumerator (Me)
        End Function


    #Region "IEnumerable"
        private function _GetEnumerator() As IEnumerator   implements IEnumerable.GetEnumerator
            Return Me.GetEnumerator()
    End Function 

    #End Region
    #End Region
    #End Region
    #End Region
    #End Region

    #Region "Private"
    Private function GetKeyIndex( byval key  As Object ) As Integer?
            Dim length As Integer = Me._operands.Count - 1
            For  index As Integer = 0 To length - 1 Step 2
                If (Me._operands(index).Equals(key)) Then
                    Return index
                end if
            next
            Return  Nothing  
    End Function 

    #end region
    #end region
  End Class


End Namespace 