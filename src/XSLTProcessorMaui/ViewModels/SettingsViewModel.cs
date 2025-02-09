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
		// Preferences.RunPostprocessor;
	}

	private void AddValidations()
	{
		//XmlInputFile.Validations.Add(new IsNotNullOrEmptyRule		{ ValidationMessage = "A file name is required." });
		//XmlInputFile.Validations.Add(new FileExistsRule				{ ValidationMessage = "The file does not exist." });
		//XmlInputFile.Validate();

		//OutputFile.Validations.Add(new IsNotNullOrEmptyRule			{ ValidationMessage = "A file name is required." });
		//OutputFile.Validate();

		//OnRunPostprocessingChanged(RunPostprocessing);
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial ValidatableObject<string>			XmlInputFile { get; set; }					= new();

	[ObservableProperty]
	public partial bool									IsSubmittable { get; set; }

	#endregion

	#region Validation

	[RelayCommand]
	private void ValidateXmlInputFile()
	{
		XmlInputFile.Validate();
		ValidateSubmittable();
	}

	public bool ValidateSubmittable() => IsSubmittable = true;
	//	OutputDirectory.IsValid &&
	//	Postprocessor.IsValid;

	#endregion

	#region On Properties Changed

	//partial void OnRunPostprocessingChanged(bool value)
	//{
	//	if (value)
	//	{ 
	//		Postprocessor.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = "A file name is required." });
	//		Postprocessor.Validations.Add(new FileExistsRule { ValidationMessage = "The file does not exist." });
	//		ValidatePostprocessor();
	//	}
	//	else
	//	{
	//		Postprocessor.Validations.Clear();
	//		ValidatePostprocessor();
	//	}
	//}

	#endregion

	#region Methods and Commands


	private void SaveSettings()
	{
		//Preferences.XmlInputFile		= XmlInputFile.Value!.Trim();
	}

	#endregion

} // End class.