#region License

// Copyright (c) 2011, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using ClearCanvas.Common.Utilities;
using ClearCanvas.Ris.Application.Common;

namespace ClearCanvas.Ris.Client.Formatting
{
    public static class TelephoneFormat
    {
        /// <summary>
        /// Formats the telephone number according to the default format as specified in <see cref="FormatSettings"/>
        /// </summary>
        /// <param name="tn"></param>
        /// <returns></returns>
        public static string Format(TelephoneDetail tn)
        {
            return Format(tn, FormatSettings.Default.TelephoneNumberDefaultFormat);
        }

        /// <summary>
        /// Formats the address according to the specified format string.
        /// </summary>
        /// <remarks>
        /// Valid format specifiers are as follows:
        ///     %C - country code, preceded by +
        ///     %c - country code if different from default country code specified in <see cref="FormatSettings"/>
        ///     %A - area code
        ///     %N - phone number in form XXX-XXXX 
        ///     %X - extension, preceded by x
        /// </remarks>
        /// <param name="tn"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(TelephoneDetail tn, string format)
        {
            string result = format;
            result = result.Replace("%C", tn.CountryCode == null ? "" : string.Format("+{0}", tn.CountryCode));

            result = result.Replace("%c",
                (tn.CountryCode == null || tn.CountryCode == FormatSettings.Default.TelephoneNumberSuppressCountryCode) ? ""
                : string.Format("+{0}", tn.CountryCode));

            result = result.Replace("%A", tn.AreaCode == null ? "" : tn.AreaCode);
            result = result.Replace("%N", StringMask.Apply(tn.Number, FormatSettings.Default.TelephoneNumberLocalMask) ?? "");
            result = result.Replace("%X", string.IsNullOrEmpty(tn.Extension) ? "" : string.Format("x{0}", tn.Extension));

            return result.Trim();
        }

		/// <summary>
		/// Formats a string telephone number to the default format string.
		/// </summary>
		public static string Format(string number)
		{
			return Format(number, TextFieldMasks.TelephoneNumberFullMask);
		}


        /// <summary>
        /// Formats a string telephone number to the specified format string.
        /// </summary>
        /// <remarks>
        /// The format should be specified similar to (000)000-0000 or 000-0000 where the '0's will be replaced by 
        /// digits in the provided number.  If the length of the provided number is not the same as the number of 
        /// '0's in the format string, the number will be returned unformatted.
        /// </remarks>
        /// <param name="number"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(string number, string format)
        {
            // do nothing if the number of digits in the number doesn't match the number of placeholders in the format string
            if (number.Length != CollectionUtils.Select(format, delegate(char c) { return c.Equals('0'); }).Count)
            {
                return number;
            }

            // replace placeholders with the actual digits one-by-one
            char[] result = format.ToCharArray();
            int j = 0;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] != '0')
                    continue;

                result[i] = number[j++];
            }

            return new string(result).Trim();
        }
    }
}
