using NDivert.Filter;
using System;
using System.Linq.Expressions;

namespace NDivert.Filter
{
	/// <summary>
	/// WinDivert Filter
	/// </summary>
	public sealed class FilterDefinition
	{
		internal string _stringValue;
		internal Expression<Func<IFilter, bool>> _filterExpression;

		private FilterDefinition()
		{

		}

		public static implicit operator FilterDefinition(string value)
		{
			return new FilterDefinition()
			{
				_stringValue = value
			};
		}

		public static FilterDefinition FromExpression(Expression<Func<IFilter, bool>> filter)
		{
			return new FilterDefinition()
			{
				_filterExpression = filter
			};
		}

		public override string ToString()
		{
			return _stringValue ?? DivertFilterStringBuilder.MakeFilter(_filterExpression);
		}
	}

}
