﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Ris.Application.Services {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class LogicalHL7EventSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static LogicalHL7EventSettings defaultInstance = ((LogicalHL7EventSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new LogicalHL7EventSettings())));
        
        public static LogicalHL7EventSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// Specifies whether the creation of logical HL7 outbound events is enabled.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Specifies whether the creation of logical HL7 outbound events is enabled.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool LogicalHL7EventsEnabled {
            get {
                return ((bool)(this["LogicalHL7EventsEnabled"]));
            }
        }
    }
}
