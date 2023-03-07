﻿Imports DMD.Databases
Imports DMD.Sistema

Partial Public Class Anagrafica

    Public Class CContattiPerPersonaCollection
        Inherits CCollection(Of CContatto)

        Private m_Persona As CPersona

        Public Sub New()
            Me.m_Persona = Nothing
        End Sub

        Public Sub New(ByVal persona As CPersona)
            Me.New()
            Me.Load(persona)
        End Sub

        Protected Friend Overridable Sub SetPersona(ByVal value As CPersona)
            Me.m_Persona = value
            If (value IsNot Nothing) Then
                For Each c As CContatto In Me
                    c.SetPersona(value)
                Next
            End If
        End Sub

        Protected Overrides Sub OnInsert(index As Integer, value As Object)
            If (Me.m_Persona IsNot Nothing) Then
                With DirectCast(value, CContatto)
                    .SetPersona(Me.m_Persona)
                    '.SetOrdine(index)
                End With
            End If
            MyBase.OnInsert(index, value)
        End Sub

        Protected Overrides Sub OnSet(index As Integer, oldValue As Object, newValue As Object)
            If (Me.m_Persona IsNot Nothing) Then
                With DirectCast(newValue, CContatto)
                    .SetPersona(Me.m_Persona)
                    '.SetOrdine(index)
                End With
            End If
            MyBase.OnSet(index, oldValue, newValue)
        End Sub

        Public Shadows Function Add(ByVal contatto As CContatto) As CContatto
            For Each c As CContatto In Me
                If (c.Tipo = contatto.Tipo AndAlso c.Valore = contatto.Valore) Then
                    c.MergeWith(contatto)
                    Return c
                End If
            Next
            contatto.Ordine = Me.Count
            MyBase.Add(contatto)
            Return contatto
        End Function

        ''' <summary>
        ''' Crea un nuovo oggetto con nome, lo salva, e lo restituisce
        ''' </summary>
        ''' <param name="tipo"></param>
        ''' <param name="nome"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function Add(ByVal tipo As String, ByVal nome As String, ByVal valore As String) As CContatto
            Dim c As New CContatto
            c.Tipo = tipo
            c.Nome = nome
            c.Valore = valore
            c.Stato = ObjectStatus.OBJECT_VALID
            'If (c.Save()
            Return Me.Add(c)
        End Function

         
        Public Function GetContattoPredefinito(ByVal tipo As String) As CContatto
            tipo = LCase(Trim(tipo))

            For Each c As CContatto In Me
                If (LCase(c.Tipo) = tipo AndAlso c.Valore <> "" AndAlso c.Predefinito) Then
                    Return c
                End If
            Next
            For Each c As CContatto In Me
                If (LCase(c.Tipo) = tipo AndAlso c.Valore <> "") Then
                    Return c
                End If
            Next

            Return Nothing
        End Function

        Public Sub SetContattoPredefinito(ByVal tipo As String, ByVal valore As String)
            Dim c As CContatto
            Dim c1 As New CContatto
            c1.Tipo = tipo
            c1.Valore = valore

            For Each c In Me
                If (Strings.Compare(c.Tipo, c1.Tipo, CompareMethod.Text) = 0) Then
                    If (c.Valore = c1.Valore) Then
                        c.Predefinito = True
                        c.Save()
                        Return
                    Else
                        c.Predefinito = False
                        c.Save()
                        Return
                    End If
                End If
            Next
            c = Me.Add(tipo, tipo, valore)
            c.Predefinito = True
            c.Save()
        End Sub

        Protected Friend Sub Load(ByVal persona As CPersona)
            If (persona Is Nothing) Then Throw New ArgumentNullException("persona")

            Me.Clear()
            Me.m_Persona = persona

            If (GetID(persona) = 0) Then Return

            Dim conn As CDBConnection = APPConn
            If (conn.IsRemote) Then
                Dim tmp As String = RPC.InvokeMethod("/widgets/websvc/?_m=Anagrafica&_a=LoadContattiPersona", "pid", RPC.int2n(GetID(Me.m_Persona)))
                If (tmp <> "") Then Me.AddRange(XML.Utils.Serializer.Deserialize(tmp))
            Else
                Dim dbSQL As String
                Dim reader As DBReader = Nothing


                Try
                    dbSQL = "SELECT * FROM [tbl_Contatti] WHERE [Persona]=" & GetID(persona) & " AND [Stato]=" & ObjectStatus.OBJECT_VALID & ";"
                    reader = New DBReader(APPConn.Tables("tbl_Contatti"), dbSQL)
                    While reader.Read
                        Dim Item As New CContatto
                        If APPConn.Load(Item, reader) Then MyBase.Add(Item)
                    End While
                Catch ex As exception
                    Throw
                Finally
                    If (reader IsNot Nothing) Then reader.Dispose() : reader = Nothing
                End Try
            End If

            Me.Sort()
        End Sub

    End Class


End Class
