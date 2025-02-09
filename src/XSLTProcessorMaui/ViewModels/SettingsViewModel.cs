using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Validation;

namespace XSLTProcessorMaui;

public partial class SettingsViewModel : ObservableObject
{
	#region Fields
	#endregion

	#region Construction

	public SettingsViewModel()
    {
		InitializeValues();
		AddValidations();
		ValidateSubmittable();
	}

	private void InitializeValues()
	{
		RestoreLastValuesAtStartup = Preferences.RestoreLastValuesAtStartup;
	}

	private void AddValidations()
	{
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial bool									RestoreLastValuesAtStartup { get; set; }

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	#endregion

	#region Validation

	public bool ValidateSubmittable() => IsSubmittable = true;
	//	OutputDirectory.IsValid &&
	//	Postprocessor.IsValid;

	#endregion

	#region On Properties Changed
	#endregion

	#region Methods and Commands

	public void Save()
	{
		Preferences.RestoreLastValuesAtStartup = RestoreLastValuesAtStartup;
	}

	#endregion

} // End class.