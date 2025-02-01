using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using DigitalProduction.Maui.Controls;
using DigitalProduction.Maui.ViewModels;
using DigitalProduction.Maui.Views;
using XSLTProcessorMaui.ViewModels;

namespace XSLTProcessorMaui;

public partial class MainPage : DigitalProductionMainPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Override to set the height to something usable.
	/// </summary>
	protected override void OnAppearing()
	{
		base.OnAppearing();
		App.Window!.MinimumWidth = 600;
		App.Window.MinimumHeight = 810;
		App.Window.MaximumHeight = 810;
	}

	async void OnAbout(object sender, EventArgs eventArgs)
	{
		AboutView1 view = new(new AboutViewModel(true));
		_ = await Shell.Current.ShowPopupAsync(view);
	}

	public async void OnBrowseForXmlInputFile(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new()
		{
			PickerTitle = "Select an XML File",
			FileTypes   = DigitalProduction.Maui.IO.FileTypes.Xml
		};
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			XmlInputFileEntry.Text = result.FullPath;
		}
	}

	public async void OnBrowseForXsltFile(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new()
		{
			PickerTitle = "Select an XML File",
			FileTypes   = DigitalProduction.Maui.IO.FileTypes.Xslt
		};
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			XsltFileEntry.Text = result.FullPath;
		}
	}

	private async void OnBrowseForOutputDirectory(object sender, EventArgs eventArgs)
	{
		CancellationToken cancellationToken = new();
		FolderPickerResult folderResult = await FolderPicker.PickAsync(OutputDirectoryEntry.Text, cancellationToken);
		if (folderResult.IsSuccessful)
		{
			OutputDirectoryEntry.Text = folderResult.Folder.Path;
		}
	}

	public async void OnBrowseForPostProcessor(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new() { PickerTitle = "Select a Postprocessor" };
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			PostprocessorEntry.Text = result.FullPath;
		}
	}

	public static async Task<FileResult?> BrowseForFile(PickOptions options)
	{
		try
		{
			return await FilePicker.PickAsync(options);
		}
		catch
		{
			//(Exception exception)
			//string message = exception.Message;
			// The user canceled or something went wrong.
		}
		return null;
	}

	protected virtual async void OnProcessButtonClicked(object? sender, EventArgs eventArgs)
	{
		MainViewModel? viewModel = BindingContext as MainViewModel;
		System.Diagnostics.Debug.Assert(viewModel != null);

		if (!DigitalProduction.IO.Path.PathIsWritable(viewModel.OutputFileFullPath))
		{
			await DisplayAlert("Error", "The output file is not writable.  The file may be open by another application.  Please resolve the situation or choose another file name.", "Ok");
			return;
		}

		ProcessingResult processingResult = viewModel.Process();

		await DisplayAlert("Processing Result", processingResult.Message, "Ok");
	}
}