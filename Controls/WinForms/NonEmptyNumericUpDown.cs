using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ClearCanvas.Controls.WinForms
{
	public class NonEmptyNumericUpDown: NumericUpDown
	{
		// To keep in line with the control's default behaviour,
		// verify the value when the control loses focus and change
		// it if necessary.
		protected override void UpdateEditText()
		{
			base.UpdateEditText();

			if (String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(Minimum.ToString()))
				Text = Minimum.ToString();
		}
	}
}
