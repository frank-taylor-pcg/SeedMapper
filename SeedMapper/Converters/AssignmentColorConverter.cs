using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SeedMapper.Models;

namespace SeedMapper.Converters;

public class AssignmentColorConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not Seed seed) return new SolidColorBrush(Colors.Fuchsia);
			
		return (seed.GroupSequence == -1)
			? new SolidColorBrush(Colors.Brown)
			: new SolidColorBrush(Colors.MediumSeaGreen);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}