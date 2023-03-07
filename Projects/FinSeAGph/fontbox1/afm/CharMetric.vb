Imports FinSeA.org.fontbox.util

Namespace org.fontbox.afm

    '/**
    ' * This class represents a single character metric.
    ' *
    ' * @author Ben Litchfield (ben@benlitchfield.com)
    ' * @version $Revision: 1.1 $
    ' */
    Public Class CharMetric

        Private characterCode As Integer

        Private wx As Single
        Private w0x As Single
        Private w1x As Single

        Private wy As Single
        Private w0y As Single
        Private w1y As Single

        Private w() As Single
        Private w0() As Single
        Private w1() As Single
        Private vv() As Single

        Private name As String
        Private boundingBox As BoundingBox
        Private ligatures As FinSeA.List = New FinSeA.ArrayList()

        '/** Getter for property boundingBox.
        ' * @return Value of property boundingBox.
        ' */
        Public Function getBoundingBox() As BoundingBox
            Return boundingBox
        End Function

        '/** Setter for property boundingBox.
        ' * @param bBox New value of property boundingBox.
        ' */
        Public Sub setBoundingBox(ByVal bBox As BoundingBox)
            boundingBox = bBox
        End Sub

        '/** Getter for property characterCode.
        ' * @return Value of property characterCode.
        ' */
        Public Function getCharacterCode() As Integer
            Return characterCode
        End Function

        '/** Setter for property characterCode.
        ' * @param cCode New value of property characterCode.
        ' */
        Public Sub setCharacterCode(ByVal cCode As Integer)
            characterCode = cCode
        End Sub

        '/**
        ' * This will add an entry to the list of ligatures.
        ' *
        ' * @param ligature The ligature to add.
        ' */
        Public Sub addLigature(ByVal ligature As Ligature)
            ligatures.add(ligature)
        End Sub

        '/** Getter for property ligatures.
        ' * @return Value of property ligatures.
        ' */
        Public Function getLigatures() As FinSeA.List
            Return ligatures
        End Function

        '/** Setter for property ligatures.
        ' * @param lig New value of property ligatures.
        ' */
        Public Sub setLigatures(ByVal lig As FinSeA.List)
            Me.ligatures = lig
        End Sub

        '/** Getter for property name.
        ' * @return Value of property name.
        ' */
        Public Function getName() As String
            Return name
        End Function

        '/** Setter for property name.
        ' * @param n New value of property name.
        ' */
        Public Sub setName(ByVal n As String)
            Me.name = n
        End Sub

        '/** Getter for property vv.
        ' * @return Value of property vv.
        ' */
        Public Function getVv() As Single()
            Return Me.vv
        End Function

        '/** Setter for property vv.
        ' * @param vvValue New value of property vv.
        ' */
        Public Sub setVv(ByVal vvValue() As Single)
            Me.vv = vvValue
        End Sub

        '/** Getter for property w.
        ' * @return Value of property w.
        ' */
        Public Function getW() As Single()
            Return Me.w
        End Function

        '/** Setter for property w.
        ' * @param wValue New value of property w.
        ' */
        Public Sub setW(ByVal wValue() As Single)
            Me.w = wValue
        End Sub

        '/** Getter for property w0.
        ' * @return Value of property w0.
        ' */
        Public Function getW0() As Single()
            Return Me.w0
        End Function

        '/** Setter for property w0.
        ' * @param w0Value New value of property w0.
        ' */
        Public Sub setW0(ByVal w0Value() As Single)
            w0 = w0Value
        End Sub

        '/** Getter for property w0x.
        ' * @return Value of property w0x.
        ' */
        Public Function getW0x() As Single
            Return w0x
        End Function

        '/** Setter for property w0x.
        ' * @param w0xValue New value of property w0x.
        ' */
        Public Sub setW0x(ByVal w0xValue As Single)
            w0x = w0xValue
        End Sub

        '/** Getter for property w0y.
        ' * @return Value of property w0y.
        ' */
        Public Function getW0y() As Single
            Return w0y
        End Function

        '/** Setter for property w0y.
        ' * @param w0yValue New value of property w0y.
        ' */
        Public Sub setW0y(ByVal w0yValue As Single)
            w0y = w0yValue
        End Sub

        '/** Getter for property w1.
        ' * @return Value of property w1.
        ' */
        Public Function getW1() As Single()
            Return Me.w1
        End Function

        '/** Setter for property w1.
        ' * @param w1Value New value of property w1.
        ' */
        Public Sub setW1(ByVal w1Value() As Single)
            w1 = w1Value
        End Sub

        '/** Getter for property w1x.
        ' * @return Value of property w1x.
        ' */
        Public Function getW1x() As Single
            Return w1x
        End Function

        '/** Setter for property w1x.
        ' * @param w1xValue New value of property w1x.
        ' */
        Public Sub setW1x(ByVal w1xValue As Single)
            w1x = w1xValue
        End Sub

        '/** Getter for property w1y.
        ' * @return Value of property w1y.
        ' */
        Public Function getW1y() As Single
            Return w1y
        End Function

        '/** Setter for property w1y.
        ' * @param w1yValue New value of property w1y.
        ' */
        Public Sub setW1y(ByVal w1yValue As Single)
            w1y = w1yValue
        End Sub

        '/** Getter for property wx.
        ' * @return Value of property wx.
        ' */
        Public Function getWx() As Single
            Return wx
        End Function

        '/** Setter for property wx.
        ' * @param wxValue New value of property wx.
        ' */
        Public Sub setWx(ByVal wxValue As Single)
            wx = wxValue
        End Sub

        '/** Getter for property wy.
        ' * @return Value of property wy.
        ' */
        Public Function getWy() As Single
            Return wy
        End Function

        '/** Setter for property wy.
        ' * @param wyValue New value of property wy.
        ' */
        Public Sub setWy(ByVal wyValue As Single)
            Me.wy = wyValue
        End Sub

    End Class

End Namespace