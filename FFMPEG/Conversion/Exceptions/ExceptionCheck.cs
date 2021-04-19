
namespace FFMPEG.Conversion.Exceptions
{
	internal class ExceptionCheck
	{
		private string _searchPhrase;

		private bool _ContainsFileIsEmptyMessage;

		public ExceptionCheck(string searchPhrase, bool ContainsFileIsEmptyMessage = false)
		{
			_searchPhrase = searchPhrase;
			_ContainsFileIsEmptyMessage = ContainsFileIsEmptyMessage;
		}

		internal bool CheckLog(string log)
		{
			if (log.Contains(_searchPhrase) && (!_ContainsFileIsEmptyMessage || log.Contains("Output file is empty")))
			{
				return true;
			}
			return false;
		}
	}
}
