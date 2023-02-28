using System.Collections.Generic;
using System.Linq;
using SeedMapper.Models;

namespace SeedMapper;

public class SeedAssigner
{
	public bool ChangePlateOnLims { get; set; } = true;

	private List<OPlate> _loadedOPlates = new();

	public void ScanOPlates(IEnumerable<OPlate> oplates)
	{
		_loadedOPlates = oplates.ToList();
	}

	private Seed? FindFirstUnassignedSeed(IEnumerable<Seed> seeds)
	{
		return seeds.FirstOrDefault(x => x.DestinationContainer is null);
	}

	private IEnumerable<Seed>? GetSeedsToAssign(Seed first, IEnumerable<Seed> seeds)
	{
		if (!ChangePlateOnLims)
			return seeds
				.Where(x =>
					x.GroupSequence >= first.GroupSequence
					&& x.GroupSequence < first.GroupSequence + 24);

		return seeds
			.Where(x =>
				string.Equals(x.LimsId, first.LimsId)
				&& x.GroupSequence >= first.GroupSequence
				&& x.GroupSequence < first.GroupSequence + 24);
	}

	// This is incorrect
	private OPlate? GetNextValidOPlate(Seed seed, IEnumerable<Seed> seeds)
	{
		OPlate? result = null;
		foreach (OPlate plate in _loadedOPlates)
		{
			if (seeds.Any(x => string.Equals(x.DestinationContainer, plate.Barcode))) continue;
			result = plate;
			break;
		}

		return result;
	}

	public void AssignOPlates(IEnumerable<Seed> seeds)
	{
		Seed? first = FindFirstUnassignedSeed(seeds);
		Log(first.ToString());

		while (first is not null)
		{
			OPlate? oplate = GetNextValidOPlate(first, seeds);
			if (oplate is null) return;

			IEnumerable<Seed> toAssign = GetSeedsToAssign(first, seeds);

			// Get the offset within the o-plate to start assigning the new seed
			int offset = 0;

			foreach (Seed seed in toAssign)
			{
				seed.DestinationContainer = oplate.Barcode;
				oplate.Seeds[offset++].GroupSequence = seed.GroupSequence;
				if (offset >= 24) break;
			}
			
			first = FindFirstUnassignedSeed(seeds);
			Log(first.ToString());
		}
	}

	private void Log(string msg)
	{
		System.Diagnostics.Debug.WriteLine(msg);
	}
}