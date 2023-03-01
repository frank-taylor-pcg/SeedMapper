using Caliburn.Micro;

namespace SeedMapper.Models;

public class Seed : PropertyChangedBase
{
	#region Properties

	private string _LimsId;

	public string LimsId
	{
		get => _LimsId;
		set => Set(ref _LimsId, value);
	}

	private int _GroupNumber = -1;

	public int GroupNumber
	{
		get => _GroupNumber;
		set => Set(ref _GroupNumber, value);
	}

	private int _GroupSequence = -1;

	public int GroupSequence
	{
		get => _GroupSequence;
		set => Set(ref _GroupSequence, value);
	}

	private string? _DestinationContainer;

	public string? DestinationContainer
	{
		get => _DestinationContainer;
		set => Set(ref _DestinationContainer, value);
	}

	private string? _DestinationWell;

	public string? DestinationWell
	{
		get => _DestinationWell;
		set => Set(ref _DestinationWell, value);
	}

	#endregion

	public override string ToString()
	{
		return
			$"LimsID: {LimsId} | GroupNumber: {GroupNumber} | GroupSequence: {GroupSequence} "
			+ $"| DestinationContainer: {DestinationContainer} | DestinationWell: {DestinationWell}";
	}
}