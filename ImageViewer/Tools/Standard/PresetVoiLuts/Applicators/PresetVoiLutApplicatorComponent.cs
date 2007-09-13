using System.Collections.Generic;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using ClearCanvas.Desktop;
using ClearCanvas.Desktop.Actions;
using System;

namespace ClearCanvas.ImageViewer.Tools.Standard.PresetVoiLuts.Applicators
{
	[Serializable]
	public sealed class PresetVoiLutApplicatorValidationException : Exception
	{
		internal PresetVoiLutApplicatorValidationException(string message)
			: base(message)
		{
		}
	}

	public abstract class PresetVoiLutApplicatorComponent : ApplicationComponent, IPresetVoiLutApplicator, IPresetVoiLutApplicatorComponent
	{
		private IPresetVoiLutApplicatorFactory _sourceFactory;
		private EditContext _editContext;
		private bool _valid;

		protected PresetVoiLutApplicatorComponent()
		{
			_valid = false;
		}

		#region Sealed Off Application Component functionality

		public sealed override IActionSet ExportedActions
		{
			get
			{
				return base.ExportedActions;
			}
		}

		#endregion

		#region IPresetVoiLutApplicator Members

		public abstract string Name { get; }
		public abstract string Description { get; }

		public IPresetVoiLutApplicatorFactory SourceFactory
		{
			get { return _sourceFactory; }
			internal set
			{
				Platform.CheckForNullReference(value, "SourceFactory");
				_sourceFactory = value;
			}
		}

		public abstract bool AppliesTo(IPresentationImage presentationImage);

		public abstract void Apply(IPresentationImage image);

		public PresetVoiLutConfiguration GetConfiguration()
		{
			Validate();

			PresetVoiLutConfiguration configuration = PresetVoiLutConfiguration.FromFactory(_sourceFactory);
			foreach (KeyValuePair<string, string> pair in SimpleSerializer.Deserialize(this))
				configuration[pair.Key] = pair.Value;

			return configuration;
		}

		#endregion

		#region IEditPresetVoiLutApplicationComponent Members

		public IPresetVoiLutApplicator GetApplicator()
		{
			Validate();
			return this;
		}

		public EditContext EditContext
		{
			get { return _editContext; }
			set { _editContext = value; }
		}

		public bool Valid
		{
			get { return _valid; }
			protected set
			{
				if (_valid == value)
					return;

				_valid = value;
				NotifyPropertyChanged("Valid");
			}
		}

		#endregion

		public abstract void Validate();

		protected virtual void UpdateValid()
		{
		}

		protected void OnPropertyChanged(string propertyName)
		{
			UpdateValid();
			base.Modified = true;
			NotifyPropertyChanged(propertyName);
		}

		protected static PresetVoiLutApplicatorValidationException CreateValidationException(string message)
		{
			return new PresetVoiLutApplicatorValidationException(message);
		}
	}
}