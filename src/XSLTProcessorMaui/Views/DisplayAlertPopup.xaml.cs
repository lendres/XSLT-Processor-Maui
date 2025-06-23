using DigitalProduction.Maui.Views;
using System.Threading;

namespace Data.Translation.Pages;

public partial class DisplayAlertPopup : PopupView
{

	#region Construction

	public DisplayAlertPopup(bool automaticallyClose = false)
	{
		InitializeComponent();
		TitleLabel.Text		=  "Information";
		Opened				+= OnOpen;
		AutomaticallyClose	= automaticallyClose;
	}

	#endregion

	public string	Title				{ set { TitleLabel.Text = value; } }
	public string	Message				{ set { MessageLabel.Text = value; } }
	public bool		AutomaticallyClose	{ get; set; }							= false;
	public int		DelayTime			{ get; set; }							= 5000;

	#region Events

	private void OnOpen(object? sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs eventArgs)
	{
		if (AutomaticallyClose)
		{
			StartAutoCloseTimer();
		}
	}

	protected virtual void OnClose(object? sender, EventArgs eventArgs)
	{
		Close();
	}

	#endregion

    private async void StartAutoCloseTimer()
    {
		// 5000 milliseconds = 5 seconds.
        await Task.Delay(5000);
        Close();
    }

	async private void Close()
	{
		CancellationTokenSource cancelationTokenSource = new(TimeSpan.FromSeconds(5));
		await CloseAsync(true, cancelationTokenSource.Token);
	}
}