using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SeedMapper.Models;

namespace SeedMapper.UserControls;

public partial class UCOplate : UserControl
{
	public static readonly DependencyProperty BarcodeProperty = DependencyProperty.Register(
		nameof(Barcode), typeof(string), typeof(UCOplate), new PropertyMetadata(default(string)));
	public string Barcode
	{
		get => (string)GetValue(BarcodeProperty);
		set => SetValue(BarcodeProperty, value);
	}

	public static readonly DependencyProperty SeedsProperty = DependencyProperty.Register(
		nameof(Seeds), typeof(ObservableCollection<Seed>), typeof(UCOplate), new PropertyMetadata(new ObservableCollection<Seed>()));
	public ObservableCollection<Seed> Seeds
	{
		get => (ObservableCollection<Seed>)GetValue(SeedsProperty);
		set => SetValue(SeedsProperty, value);
	}
	
	public UCOplate()
	{
		InitializeComponent();
	}
}