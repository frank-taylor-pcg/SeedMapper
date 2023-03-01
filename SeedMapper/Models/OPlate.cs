using System.Collections.ObjectModel;

namespace SeedMapper.Models;

public class OPlate
{
	public string Barcode { get; set; } = "Unknown";
	public ObservableCollection<Seed> Seeds { get; set; } = new();
	public bool Unloaded { get; set; } = false;

	public OPlate()
	{
		for (int i = 0; i < 24; i++)
		{
			Seeds.Add(new Seed());
		}
	}
}