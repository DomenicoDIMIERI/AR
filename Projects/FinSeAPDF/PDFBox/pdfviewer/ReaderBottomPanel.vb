'/*
' * Licensed to the Apache Software Foundation (ASF) under one or more
' * contributor license agreements.  See the NOTICE file distributed with
' * this work for additional information regarding copyright ownership.
' * The ASF licenses this file to You under the Apache License, Version 2.0
' * (the "License"); you may not use this file except in compliance with
' * the License.  You may obtain a copy of the License at
' *
' *      http://www.apache.org/licenses/LICENSE-2.0
' *
' * Unless required by applicable law or agreed to in writing, software
' * distributed under the License is distributed on an "AS IS" BASIS,
' * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' * See the License for the specific language governing permissions and
' * limitations under the License.
' */

'import java.awt.Dimension;

'import javax.swing.JPanel;

'import javax.swing.JLabel;
'import java.awt.FlowLayout;

Imports System.Drawing
Imports System.Windows.Forms

Namespace org.apache.pdfbox.pdfviewer

    '/**
    ' * A panel to display at the bottom of the window for status and other stuff.
    ' *
    ' * @author <a href="mailto:ben@benlitchfield.com">Ben Litchfield</a>
    ' * @version $Revision: 1.2 $
    ' */
    Public Class ReaderBottomPanel
        Inherits Panel ' JPanel

        Private statusLabel As Label = Nothing 'JLabel

        '/**
        ' * This is the default constructor.
        ' */
        Public Sub New()
            MyBase.New()
            initialize()
        End Sub

        '/**
        ' * This method initializes Me.
        ' */
        Private Sub initialize()
            'Dim flowLayout1 As New FlowLayout()
            'Me.setLayout(flowLayout1)
            'Me.setComponentOrientation(java.awt.ComponentOrientation.LEFT_TO_RIGHT)
            'flowLayout1.setAlignment(java.awt.FlowLayout.LEFT)
            Me.Controls.Add(getStatusLabel())
        End Sub

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Return New Size(1000, 20)
        End Function

        '/**
        ' * This method initializes status label.
        ' *
        ' * @return javax.swing.JLabel
        ' */
        Public Function getStatusLabel() As Label 'JLabel 
            If (statusLabel Is Nothing) Then
                statusLabel = New Label ' JLabel()
                statusLabel.Text = "Ready"
            End If
            Return statusLabel
        End Function


    End Class

End Namespace