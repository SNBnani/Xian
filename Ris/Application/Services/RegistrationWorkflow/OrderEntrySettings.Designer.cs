﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Application.Services.RegistrationWorkflow {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class OrderEntrySettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static OrderEntrySettings defaultInstance = ((OrderEntrySettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new OrderEntrySettings())));
        
        public static OrderEntrySettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AutoGenerateVisit {
            get {
                return ((bool)(this["AutoGenerateVisit"]));
            }
        }
    }
}
