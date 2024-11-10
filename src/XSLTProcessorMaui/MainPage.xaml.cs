﻿using CommunityToolkit.Maui.Views;
using DigitalProduction.Controls;
using DigitalProduction.ViewModels;
using DigitalProduction.Views;

namespace XSLTProcessorMaui;

public partial class MainPage : DigitalProductionMainPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	async void OnAbout(object sender, EventArgs eventArgs)
	{
		AboutView1 view = new(new AboutViewModel(true));
		_ = await Shell.Current.ShowPopupAsync(view);
	}

	public async void OnBrowseForXmlInputFile(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions	= new() { PickerTitle = "Select an XML File" };
		pickOptions.FileTypes	= DigitalProduction.IO.FileTypes.Xml;
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			XmlInputFileEntry.Text = result.FullPath;
		}
	}

	public async void OnBrowseForXsltFile(object sender, EventArgs eventArgs)
	{
		PickOptions pickOptions = new() { PickerTitle = "Select an XML File" };
		pickOptions.FileTypes   = DigitalProduction.IO.FileTypes.Xslt;
		FileResult? result = await BrowseForFile(pickOptions);
		if (result != null)
		{
			XsltFileEntry.Text = result.FullPath;
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
	}
}
