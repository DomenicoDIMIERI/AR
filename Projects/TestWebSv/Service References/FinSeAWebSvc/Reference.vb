﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18408
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace FinSeAWebSvc
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="FinSeAWebSvc.WebSvcSoap")>  _
    Public Interface WebSvcSoap
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/HelloWorld", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function HelloWorld() As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Prova1", ReplyAction:="*"), _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=True)> _
        Function Prova1() As FinSeA.Databases.DBObjectBase
    End Interface
    
    ' '''<remarks/>
    '<System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408"),  _
    ' System.SerializableAttribute(),  _
    ' System.Diagnostics.DebuggerStepThroughAttribute(),  _
    ' System.ComponentModel.DesignerCategoryAttribute("code"),  _
    ' System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://tempuri.org/")>  _
    'Partial Public MustInherit Class DBObjectBase
    '    Inherits Object
    '    Implements System.ComponentModel.INotifyPropertyChanged

    '    Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    '    Protected Sub RaisePropertyChanged(ByVal propertyName As String)
    '        Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
    '        If (Not (propertyChanged) Is Nothing) Then
    '            propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
    '        End If
    '    End Sub
    'End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface WebSvcSoapChannel
        Inherits FinSeAWebSvc.WebSvcSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class WebSvcSoapClient
        Inherits System.ServiceModel.ClientBase(Of FinSeAWebSvc.WebSvcSoap)
        Implements FinSeAWebSvc.WebSvcSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function HelloWorld() As String Implements FinSeAWebSvc.WebSvcSoap.HelloWorld
            Return MyBase.Channel.HelloWorld
        End Function
        
        Public Function Prova1() As FinSeA.Databases.DBObjectBase Implements FinSeAWebSvc.WebSvcSoap.Prova1
            Return MyBase.Channel.Prova1
        End Function
    End Class
End Namespace