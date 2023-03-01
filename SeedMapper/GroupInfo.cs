using System;

namespace SeedMapper;

public class GroupInfo
{
	public int Number { get; set; }
	public int Sequence { get; set; }
	public int Start { get; set; }
	public int End { get; set; }

	public GroupInfo(int number, int sequence)
	{
		Number = number;
		Sequence = sequence;
		CalculateRange();
	}

	private void CalculateRange()
	{
		int start = Convert.ToInt32(Sequence) - 1;
		while (start % 24 != 0)
			start--;
		Start = start + 1;
		End = Start + 24;
	}
}