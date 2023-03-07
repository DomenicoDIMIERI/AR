Imports DMD.S300
Imports DMD.S300.CKT_DLL
Imports System.Runtime.InteropServices


Namespace Internals

    Public Class S300FingerPrintsCollection
        Inherits System.Collections.ReadOnlyCollectionBase

        Private m_User As S300PersonInfo

        Public Sub New()
            Me.m_User = Nothing
        End Sub

        Friend Sub New(ByVal user As S300PersonInfo)
            Me.New
            Me.Load(user)
        End Sub

        Default Public Property Item(ByVal fpid As Integer) As S300FingerPrint
            Get
                Return MyBase.InnerList.Item(fpid)
            End Get
            Set(value As S300FingerPrint)
                If (fpid < 0) Then Throw New ArgumentOutOfRangeException("L'indice dell'impronta non può essere < 0")
                If (value Is Nothing) Then Throw New ArgumentNullException("Impossibile assegnare il valore NULL")
                If (Me.m_User Is Nothing) Then Throw New ArgumentNullException("Utente non impostato")

                Dim devID As Integer = Me.m_User.Device.DeviceID
                Dim ret As Integer = CKT_DLL.CKT_PutFPTemplate(devID, Me.m_User.PersonID, fpid, value.m_Data, value.m_Data.Length)
                If (ret = 1) Then
                    value.m_FPID = fpid
                    value.m_User = Me.m_User

                    If (fpid < Me.Count) Then
                        Me.InnerList(fpid) = value
                    Else
                        Me.InnerList.Add(value)
                    End If
                Else
                    Throw New S300Exception(ret)
                End If
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="user"></param>
        Friend Sub Load(ByVal user As S300PersonInfo)
            Me.InnerList.Clear()
            If (user Is Nothing) Then Throw New ArgumentNullException("user")
            Me.m_User = user

            Dim devID As Integer = user.Device.DeviceID
            For fpid As Integer = 0 To 10
                Dim fpDataLen As Integer = 0
                Dim fpDataPtr As IntPtr = IntPtr.Zero
                Dim ret As Integer = CKT_DLL.CKT_GetFPTemplate(devID, user.PersonID, fpid, fpDataPtr, fpDataLen)
                If (ret = 1 AndAlso fpDataLen > 0) Then
                    Dim fp As New S300FingerPrint
                    fp.m_User = user
                    fp.m_FPID = fpid
                    fp.m_Data = Array.CreateInstance(GetType(Byte), fpDataLen)
                    Marshal.Copy(fpDataPtr, fp.m_Data, 0, fpDataLen)
                    'ret = CKT_DLL.CKT_GetFPTemplate(devID, user.PersonID, fpid, fp.m_Data, fpDataLen)
                    ret = CKT_DLL.CKT_FreeMemory(fpDataPtr)
                    Me.InnerList.Add(fp)
                End If
            Next


        End Sub

        Friend Sub NotifyDelete(ByVal fp As S300FingerPrint)
            Me.InnerList.Remove(fp)
        End Sub

        Friend Sub NotifyAdd(ByVal fp As S300FingerPrint)
            Me.InnerList.Add(fp)
        End Sub

    End Class


End Namespace
