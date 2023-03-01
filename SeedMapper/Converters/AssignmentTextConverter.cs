using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SeedMapper.Models;

namespace SeedMapper.Converters;

public class AssignmentTextConverter : IValueConverter
{
	public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not Seed seed) return string.Empty;

		return (seed.GroupSequence == -1)
			? string.Empty
			: $"{seed.LimsId}{Environment.NewLine}{seed.GroupNumber} - {seed.GroupSequence}";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}