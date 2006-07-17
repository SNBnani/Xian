using System;
using System.Collections;
using System.Text;

using ClearCanvas.Enterprise;

namespace ClearCanvas.Healthcare {

    /// <summary>
    /// HL7 Sex enumeration
    /// </summary>
	public enum Sex
	{
        /// <summary>
        /// Female
        /// </summary>
        [EnumValue("Female")]
        F,

        /// <summary>
        /// Male
        /// </summary>
        [EnumValue("Male")]
        M,

        /// <summary>
        /// Other
        /// </summary>
        [EnumValue("Other")]
        O,

        /// <summary>
        /// Unknown
        /// </summary>
        [EnumValue("Unknown")]
        U,

        /// <summary>
        /// Ambiguous
        /// </summary>
        [EnumValue("Ambiguous")]
        A,

        /// <summary>
        /// Not applicable
        /// </summary>
        [EnumValue("Not Applicable")]
        N
	}
}