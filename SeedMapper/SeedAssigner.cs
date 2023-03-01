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
				.Where(x => x.DestinationContainer is null)
				.OrderBy(x => x.GroupNumber)
				.ThenBy(x => x.GroupSequence);

		return seeds
			.Where(x => Equals(x.LimsId, first.LimsId) && x.DestinationContainer is null)
			.OrderBy(x => x.LimsId)
			.ThenBy(x => x.GroupNumber)
			.ThenBy(x => x.GroupSequence);
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

	private int GetOffsetOfFirstEmptyWell(string oplate, IEnumerable<Seed> seeds)
	{
		return seeds.Count(x => Equals(x.DestinationContainer, oplate));
	}

	private void GappedAssignment(OPlate oplate, IEnumerable<Seed> seeds)
	{
		Seed first = seeds.FirstOrDefault();
		if (first is null) return;

		GroupInfo info = new(first.GroupNumber, first.GroupSequence);

		IEnumerable<Seed> range = seeds
			.Where(x =>
				x.GroupSequence >= info.Start
				&& x.GroupSequence < info.End);

		int offset = 0;
		foreach (Seed seed in range)
		{
			int seedOffset = (seed.GroupSequence - 1) % 24; 
			while (seedOffset != offset)
			{
				oplate.Seeds[offset++] = new Seed();
			}

			seed.DestinationContainer = oplate.Barcode;
			oplate.Seeds[offset++] = seed;
		}
	}
	
	private void GaplessAssignment(OPlate oplate, IEnumerable<Seed> seeds)
	{
		// Get the offset within the o-plate to start assigning the new seed
		int offset = GetOffsetOfFirstEmptyWell(oplate.Barcode, seeds);

		foreach (Seed seed in seeds)
		{
			seed.DestinationContainer = oplate.Barcode;
			oplate.Seeds[offset++] = seed;
			if (offset >= 24) break;
		}
	}

	public void AssignOPlates(IEnumerable<Seed> seeds, bool leaveGaps = false)
	{
		Seed? first = FindFirstUnassignedSeed(seeds);

		while (first is not null)
		{
			Log(first.ToString());
			OPlate? oplate = GetNextValidOPlate(first, seeds);
			if (oplate is null) return;

			IEnumerable<Seed> toAssign = GetSeedsToAssign(first, seeds);

			if (leaveGaps)
			{
				GappedAssignment(oplate, toAssign);
			}
			else
			{
				GaplessAssignment(oplate, toAssign);
			}
			
			first = FindFirstUnassignedSeed(seeds);
		}
	}

	private void Log(string msg) => System.Diagnostics.Debug.WriteLine(msg);
}