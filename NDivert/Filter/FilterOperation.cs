namespace NDivert.Filter
{
	/// <summary>
	///	WINDIVERT_FILTER_TEST_*
	/// </summary>
	public enum FilterOperation
		:byte
	{
		Equals = 0,
		NotEquals = 1,
		Less=2,
		LessOrEquals=3,
		Greater=4,
		GreaterOrEqials=5
	}

}
