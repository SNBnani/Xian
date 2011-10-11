﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3623
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.Configuration {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class PublishingSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static PublishingSettings defaultInstance = ((PublishingSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new PublishingSettings())));
        
        public static PublishingSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// Specifies that created SOP instances are to be published to Default Servers
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Specifies that created SOP instances are to be published to Default Servers")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool PublishToDefaultServers {
            get {
                return ((bool)(this["PublishToDefaultServers"]));
            }
            set {
                this["PublishToDefaultServers"] = value;
            }
        }
        
        /// <summary>
        /// Specifies that created SOP instances are to be published to the Source AE as specified in the study&apos;s headers
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Specifies that created SOP instances are to be published to the Source AE as spec" +
            "ified in the study\'s headers")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PublishLocalToSourceAE {
            get {
                return ((bool)(this["PublishLocalToSourceAE"]));
            }
            set {
                this["PublishLocalToSourceAE"] = value;
            }
        }
    }
}
