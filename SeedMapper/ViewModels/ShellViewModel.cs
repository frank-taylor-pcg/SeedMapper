using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Caliburn.Micro;
using SeedMapper.Models;

namespace SeedMapper.ViewModels;

public class ShellViewModel : Conductor<object>
{
	#region Properties

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

	private string _Title;

	public string Title
	{
		get => _Title;
		set => Set(ref _Title, value);
	}

	private ObservableCollection<string> _Lims = new();

	public ObservableCollection<string> Lims
	{
		get => _Lims;
		set => Set(ref _Lims, value);
	}

	private string _Description;

	public string Description
	{
		get => _Description;
		set => Set(ref _Description, value);
	}

	private readonly SeedAssigner _mapper = new();
	private readonly Random _rnd = new();

	#endregion
	
	private void Reset()
	{
		OPlates.Clear();
		Lims.Clear();
		Seeds.Clear();
	}

	private void BuildDescription()
	{
		StringBuilder sb = new();
		sb.AppendLine("O-Plates:");
		sb.AppendLine($"  {string.Join(", ", OPlates.Select(x => x.Barcode))}");
		sb.AppendLine();
		sb.AppendLine("Lims:");
		sb.AppendLine(GetLimsString());
		Description = sb.ToString();
	}

	private string GetLimsString()
	{
		StringBuilder sb = new();
		int i = 0;
		int limsPerLine = 4;
		while (i < Lims.Count)
		{
			sb.AppendLine($"  {string.Join(", ", Lims.Skip(i).Take(limsPerLine))}");
			i += limsPerLine;
		}

		return sb.ToString();
	}

	private void RunTest(bool changeLimsOnOplateChange, bool leaveGaps)
	{
		BuildDescription();

		_mapper.ChangePlateOnLims = changeLimsOnOplateChange;
		_mapper.ScanOPlates(OPlates);
		_mapper.AssignOPlates(Seeds, leaveGaps);
	}

	public void NonCCNoGaps()
	{
		Reset();
		CreateOPlates(4);
		CreateLims(2);

		foreach (string lims in Lims)
		{
			CreateSeeds(lims, _rnd.Next(25, 49));
		}

		RunTest(true, false);
	}

	public void NonCCGaps()
	{
		Reset();
		CreateOPlates(4);
		CreateLims(2);

		foreach (string lims in Lims)
		{
			CreateSeeds(lims, _rnd.Next(25, 49), true);
		}

		RunTest(true, true);
	}
	
	public void Single()
	{
		Reset();
		CreateOPlates(8);
		CreateLims(12);

		foreach (string lims in Lims)
		{
			CreateSeeds(lims, 12);
		}
		
		// Reorder the seeds for CC since it's one group number and sequence is steady incrementing
		int sequence = 1;
		foreach (Seed seed in Seeds)
		{
			int index = _rnd.Next(Lims.Count);
			seed.LimsId = Lims[index];
			seed.GroupSequence = sequence++;
		}

		RunTest(false, false);
	}
	
	
	// This test is incomplete
	public void Multi()
	{
		Reset();
		CreateOPlates(8);
		CreateLims(12);

		foreach (string lims in Lims)
		{
			CreateSeeds(lims, 12);
		}
		
		// Reorder the seeds for CC since it's one group number and sequence is steady incrementing
		int sequence = 1;
		foreach (Seed seed in Seeds)
		{
			int index = _rnd.Next(Lims.Count);
			seed.LimsId = Lims[index];
			seed.GroupSequence = sequence++;
		}

		BuildDescription();

		_mapper.ChangePlateOnLims = false;
		
		// Dock first cart
		_mapper.ScanOPlates(OPlates.Take(4));
		_mapper.AssignOPlates(Seeds);
	}

	private void CreateOPlates(int qty)
	{
		for (int i = 1; i <= qty; i++)
		{
			OPlate oplate = new()
			{
				Barcode = $"T{i:00}",
			};
			OPlates.Add(oplate);
		}
	}

	private void CreateLims(int qty)
	{
		for (int i = 0; i < qty; i++)
		{
			char c = (char)('A' + i);
			Lims.Add($"Lims {c}");
		}
	}

	private void CreateSeeds(string lims, int qty, bool useGaps = false)
	{
		for (int i = 0; i < qty; i++)
		{
			if (useGaps && _rnd.Next(10) == 0)
			{
				continue;
			}
			
			Seed seed = new()
			{
				LimsId = lims,
				GroupNumber = 1,
				GroupSequence = i + 1
			};
			Seeds.Add(seed);
		}
	}
}