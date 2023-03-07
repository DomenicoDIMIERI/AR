'/*=============================================================================
'*
'*	(C) Copyright 2007, Michael Carlisle (mike.carlisle@thecodeking.co.uk)
'*
'*   http://www.TheCodeKing.co.uk
'*  
'*	All rights reserved.
'*	The code and information is provided "as-is" without waranty of any kind,
'*	either expresed or implied.
'*
'*-----------------------------------------------------------------------------
'*	History:
'*		11/02/2007	Michael Carlisle				Version 1.0
'*       08/09/2007  Michael Carlisle                Version 1.1
'*=============================================================================
'*/
Imports System

Namespace Net.Messaging

    ''' <summary>
    ''' The event args used by the message handler. This enables DataGram data 
    ''' to be passed to the handler.
    ''' </summary>
    Public NotInheritable Class XDMessageEventArgs
        Inherits EventArgs

        Private _dataGram As DataGram

        ''' <summary>
        ''' Gets the DataGram associated with this instance.
        ''' </summary>
        Public ReadOnly Property DataGram As DataGram
            Get
                Return Me._dataGram
            End Get
        End Property

        ''' <summary>
        ''' Constructor used to create a new instance from a DataGram struct.
        ''' </summary>
        ''' <param name="dataGram">The DataGram instance.</param>
        Public Sub New(ByVal dataGram As DataGram)
            Me._dataGram = dataGram
        End Sub

    End Class

End Namespace
