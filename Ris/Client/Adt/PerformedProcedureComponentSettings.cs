using System;
using System.Configuration;
using ClearCanvas.Common.Configuration;

namespace ClearCanvas.Ris.Client.Adt
{

    // TODO add a description of the purpose of the settings group here
    [SettingsGroupDescriptionAttribute("")]
    [SettingsProvider(typeof(ClearCanvas.Common.Configuration.StandardSettingsProvider))]
    internal sealed partial class PerformedProcedureComponentSettings
    {

        private PerformedProcedureComponentSettings()
        {
            ApplicationSettingsRegister.Instance.RegisterInstance(this);
        }
    }
}
