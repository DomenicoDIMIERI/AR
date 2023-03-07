Imports FinSeA.org.apache.pdfbox.cos
Imports FinSeA.org.apache.pdfbox.pdmodel.common


Namespace org.apache.pdfbox.pdmodel.documentinterchange.logicalstructure


    '/**
    ' * A root of a structure tree.
    ' * 
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>, <a
    ' * href="mailto:Johannes%20Koch%20%3Ckoch@apache.org%3E">Johannes Koch</a>
    ' * 
    ' */
    Public Class PDStructureTreeRoot
        Inherits PDStructureNode

        Private Const TYPE As String = "StructTreeRoot"

        '/**
        ' * Default Constructor.
        ' * 
        ' */
        Public Sub New()
            MyBase.New(TYPE)
        End Sub

        '/**
        ' * Constructor for an existing structure element.
        ' * 
        ' * @param dic The existing dictionary.
        ' */
        Public Sub New(ByVal dic As COSDictionary)
            MyBase.New(dic)
        End Sub

        '/**
        ' * Returns the K array entry.
        ' * 
        ' * @return the K array entry
        ' */
        Public Function getKArray() As COSArray
            Dim k As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.K)
            If (k IsNot Nothing) Then
                If (TypeOf (k) Is COSDictionary) Then
                    Dim kdict As COSDictionary = k
                    k = kdict.getDictionaryObject(COSName.K)
                    If (TypeOf (k) Is COSArray) Then
                        Return k
                    End If
                Else
                    Return k
                End If
            End If
            Return Nothing
        End Function

        '/**
        ' * Returns the K entry.
        ' * 
        ' * @return the K entry
        ' */
        Public Function getK() As COSBase
            Return Me.getCOSDictionary().getDictionaryObject(COSName.K)
        End Function

        '/**
        ' * Sets the K entry.
        ' * 
        ' * @param k the K value
        ' */
        Public Sub setK(ByVal k As COSBase)
            Me.getCOSDictionary().setItem(COSName.K, k)
        End Sub

        '/**
        ' * Returns the ID tree.
        ' * 
        ' * @return the ID tree
        ' */
        Public Function getIDTree() As PDNameTreeNode
            Dim idTreeDic As COSDictionary = Me.getCOSDictionary().getDictionaryObject(COSName.ID_TREE)
            If (idTreeDic IsNot Nothing) Then
                Return New PDNameTreeNode(idTreeDic, GetType(PDStructureElement))
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets the ID tree.
        ' * 
        ' * @param idTree the ID tree
        ' */
        Public Sub setIDTree(ByVal idTree As PDNameTreeNode)
            Me.getCOSDictionary().setItem(COSName.ID_TREE, idTree)
        End Sub

        '/**
        ' * Returns the parent tree.
        ' * 
        ' * @return the parent tree
        ' */
        Public Function getParentTree() As PDNumberTreeNode
            Dim parentTreeDic As COSDictionary = Me.getCOSDictionary().getDictionaryObject(COSName.PARENT_TREE)
            If (parentTreeDic IsNot Nothing) Then
                Return New PDNumberTreeNode(parentTreeDic, GetType(COSBase))
            End If
            Return Nothing
        End Function

        '/**
        ' * Sets the parent tree.
        ' * 
        ' * @param parentTree the parent tree
        ' */
        Public Sub setParentTree(ByVal parentTree As PDNumberTreeNode)
            Me.getCOSDictionary().setItem(COSName.PARENT_TREE, parentTree)
        End Sub

        '/**
        ' * Returns the next key in the parent tree.
        ' * 
        ' * @return the next key in the parent tree
        ' */
        Public Function getParentTreeNextKey() As Integer
            Return Me.getCOSDictionary().getInt(COSName.PARENT_TREE_NEXT_KEY)
        End Function

        '/**
        ' * Sets the next key in the parent tree.
        ' * 
        ' * @param parentTreeNextkey the next key in the parent tree.
        ' */
        Public Sub setParentTreeNextKey(ByVal parentTreeNextkey As Integer)
            Me.getCOSDictionary().setInt(COSName.PARENT_TREE_NEXT_KEY, parentTreeNextkey)
        End Sub

        '/**
        ' * Returns the role map.
        ' * 
        ' * @return the role map
        ' */
        Public Function getRoleMap() As Map(Of String, Object)
            Dim rm As COSBase = Me.getCOSDictionary().getDictionaryObject(COSName.ROLE_MAP)
            If (TypeOf (rm) Is COSDictionary) Then
                Try
                    Return COSDictionaryMap.convertBasicTypesToMap(rm)
                Catch e As System.IO.IOException
                    Debug.Print(e.ToString)
                End Try
            End If
            Return New Hashtable(Of String, Object)
        End Function

        '/**
        ' * Sets the role map.
        ' * 
        ' * @param roleMap the role map
        ' */
        Public Sub setRoleMap(ByVal roleMap As Map(Of String, String))
            Dim rmDic As COSDictionary = New COSDictionary()
            For Each key As String In roleMap.keySet()
                rmDic.setName(key, roleMap.get(key))
            Next
            Me.getCOSDictionary().setItem(COSName.ROLE_MAP, rmDic)
        End Sub

    End Class

End Namespace
