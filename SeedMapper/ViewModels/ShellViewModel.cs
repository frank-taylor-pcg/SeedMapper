using System.Collections.ObjectModel;
using Caliburn.Micro;
using SeedMapper.Models;

namespace SeedMapper.ViewModels;

public class ShellViewModel : Conductor<object>
{
	private string _Test = "Testing";

	public string Test
	{
		get => _Test;
		set => Set(ref _Test, value);
	}

	private ObservableCollection<OPlate> _OPlates = new();

	public ObservableCollection<OPlate> OPlates
	{
		get => _OPlates;
		set => Set(ref _OPlates, value);
	}

	private ObservableCollection<Seed> _Seeds = new();

	public ObservableCollection<Seed> Seeds
	{
		get => _Seeds;
		set => Set(ref _Seeds, value);
	}

	private SeedAssigner mapper = new();

	public ShellViewModel()
	{
		AddOPlates(6);
		Add5LimsWith20SeedsEach();

		mapper.ChangePlateOnLims = false;
		mapper.ScanOPlates(OPlates);
		mapper.AssignOPlates(Seeds);
	}
	
	private void AddOPlates(int qty)
	{
		for (int i = 1; i <= qty; i++)
		{
			OPlate oplate = new()
			{
				Barcode = $"T1{i:0000}",
			};
			OPlates.Add(oplate);
		}
	}

	private void Add5LimsWith20SeedsEach()
	{
		for (int i = 0; i < 100; i++)
		{
			Seed seed = new()
			{
				LimsId = $"Lims {i / 20 + 1}",
				GroupNumber = 1,
				GroupSequence = (i % 20) + 1,
			};
			Seeds.Add(seed);
		}
	}
}