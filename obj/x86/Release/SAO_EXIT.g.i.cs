﻿#pragma checksum "..\..\..\SAO_EXIT.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "19838FEE1F2C021ACD1D6063F09498D8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SAP_WPF {
    
    
    /// <summary>
    /// SAO_EXIT
    /// </summary>
    public partial class SAO_EXIT : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\..\SAO_EXIT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SAP_WPF.SAO_EXIT EXT_MGS;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\SAO_EXIT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\SAO_EXIT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label MSG_TITLE;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\SAO_EXIT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SAO_BUCANCEL;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\SAO_EXIT.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SAO_BUOK;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SAP WPF;component/sao_exit.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\SAO_EXIT.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.EXT_MGS = ((SAP_WPF.SAO_EXIT)(target));
            
            #line 5 "..\..\..\SAO_EXIT.xaml"
            this.EXT_MGS.Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.MSG_TITLE = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.SAO_BUCANCEL = ((System.Windows.Controls.Button)(target));
            
            #line 109 "..\..\..\SAO_EXIT.xaml"
            this.SAO_BUCANCEL.Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            
            #line 109 "..\..\..\SAO_EXIT.xaml"
            this.SAO_BUCANCEL.MouseMove += new System.Windows.Input.MouseEventHandler(this.Button_MouseMove);
            
            #line default
            #line hidden
            
            #line 109 "..\..\..\SAO_EXIT.xaml"
            this.SAO_BUCANCEL.MouseLeave += new System.Windows.Input.MouseEventHandler(this.SAO_BUCANCEL_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SAO_BUOK = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\..\SAO_EXIT.xaml"
            this.SAO_BUOK.Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            
            #line 112 "..\..\..\SAO_EXIT.xaml"
            this.SAO_BUOK.MouseMove += new System.Windows.Input.MouseEventHandler(this.SAO_BUOK_MouseMove);
            
            #line default
            #line hidden
            
            #line 112 "..\..\..\SAO_EXIT.xaml"
            this.SAO_BUOK.MouseLeave += new System.Windows.Input.MouseEventHandler(this.SAO_BUOK_MouseLeave);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

