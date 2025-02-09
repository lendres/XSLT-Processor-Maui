using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;

namespace XSLTProcessorMaui;

public partial class SettingsView : PopupView
{
	#region Construction

	public SettingsView(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	#endregion

	#region Events

	protected override void OnSaveButtonClicked(object? sender, EventArgs eventArgs)
	{
		SettingsViewModel? viewModel = BindingContext as SettingsViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);

		viewModel.Save();
		base.OnSaveButtonClicked(sender, eventArgs);
	}

	#endregion
}