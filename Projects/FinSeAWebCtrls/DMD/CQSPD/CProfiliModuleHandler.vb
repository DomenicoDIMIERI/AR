Imports DMD.Databases
Imports DMD.CQSPD
Imports DMD.Anagrafica
Imports DMD.Sistema
Imports DMD.XML

Namespace Forms

    Public Class CProfiliModuleHandler
        Inherits CBaseModuleHandler

        Public Sub New()
            MyBase.New(ModuleSupportFlags.SAnnotations Or ModuleSupportFlags.SCreate Or ModuleSupportFlags.SDelete Or ModuleSupportFlags.SDuplicate Or ModuleSupportFlags.SEdit)
            Me.UseLocal = True
        End Sub

        Public Overrides Function CreateCursor() As DBObjectCursorBase
            Return New CProfiliCursor
        End Function


        Public Function saveprods(ByVal renderer As Object) As String
            If Not Me.Module.UserCanDoAction("edit") Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If

            Dim idProfilo As Integer = Formats.ToInteger(Me.GetParameter(renderer, "pid", ""))
            Dim profilo As CProfilo = DMD.CQSPD.Profili.GetItemById(idProfilo)
            Dim pids() As String = Split(Me.GetParameter(renderer, "ids", ""), "|")
            Dim actions() As String = Split(GetParameter(renderer, "act", ""), "|")
            Dim values() As String = Split(GetParameter(renderer, "val", ""), "|")
            Dim ret As CProdottoProfilo() = Nothing

            ReDim ret(UBound(actions))
            For i = 0 To UBound(actions)
                ret(i) = profilo.ProdottiXProfiloRelations.SetRelationship(pids(i), actions(i), Replace(values(i), ".", ","))
            Next

            Return XML.Utils.Serializer.Serialize(ret)
        End Function

        Public Overrides Function GetInternalItemById(id As Integer) As Object
            Return CQSPD.Profili.GetItemById(id)
        End Function

        Public Function SetGrpAuth(ByVal renderer As Object) As String
            Dim pid, gid As Integer
            Dim preventivatore As CProfilo
            Dim allow, negate As Boolean

            pid = Formats.ToInteger(Me.GetParameter(renderer, "pid", ""))
            preventivatore = Me.GetInternalItemById(pid)
            If Not Me.CanEdit(preventivatore) Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If
            gid = Formats.ToInteger(Me.GetParameter(renderer, "gid", ""))
            allow = Formats.ToBool(Me.GetParameter(renderer, "allow", ""))
            negate = Formats.ToBool(Me.GetParameter(renderer, "negate", ""))

            Dim a As CProfiloXGroupAllowNegate = preventivatore.SetGroupAllowNegate(Sistema.Groups.GetItemById(gid), allow, negate)

            Return XML.Utils.Serializer.Serialize(a)
        End Function

        Public Function SetUserAuth(ByVal renderer As Object) As String
            Dim pid, uid As Integer
            Dim preventivatore As CProfilo
            Dim allow, negate As Boolean

            pid = Formats.ToInteger(Me.GetParameter(renderer, "pid", ""))
            preventivatore = Me.GetInternalItemById(pid)

            If Not Me.CanEdit(preventivatore) Then
                Throw New PermissionDeniedException
                Return vbNullString
            End If

            uid = Formats.ToInteger(Me.GetParameter(renderer, "uid", ""))
            allow = Formats.ToBool(Me.GetParameter(renderer, "allow", ""))
            negate = Formats.ToBool(Me.GetParameter(renderer, "negate", ""))

            Dim a As CProfiloXUserAllowNegate = preventivatore.SetUserAllowNegate(Sistema.Users.GetItemById(uid), allow, negate)

            Return XML.Utils.Serializer.Serialize(a)
        End Function

        Public Function GetPreventivatoriUtente(ByVal renderer As Object) As String
            Dim items As CCollection(Of CProfilo) = DMD.CQSPD.Profili.GetPreventivatoriUtente
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        ''' <summary>
        ''' Restituisce al sistema remoto l'elenco dei tipi contratto disponibili per il profilo
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function gettclist(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim profilo As CProfilo = DMD.CQSPD.Profili.GetItemById(pid)
            Dim items As CCollection(Of CTipoContratto) = profilo.GetTipiContrattoDisponibili
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

        Public Function gettrlist(ByVal renderer As Object) As String
            Dim pid As Integer = RPC.n2int(Me.GetParameter(renderer, "pid", ""))
            Dim profilo As CProfilo = DMD.CQSPD.Profili.GetItemById(pid)
            Dim tc As String = RPC.n2str(Me.GetParameter(renderer, "tc", ""))
            Dim items As CKeyCollection(Of CTipoRapporto) = profilo.GetTipiRapportoDisponibili(tc)
            If (items.Count > 0) Then
                Return XML.Utils.Serializer.Serialize(items.ToArray, XMLSerializeMethod.Document)
            Else
                Return vbNullString
            End If
        End Function

    End Class

End Namespace