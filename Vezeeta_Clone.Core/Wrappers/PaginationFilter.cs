namespace Vezeeta_Clone.Core.Wrappers
{
    public class PaginationFilter
    {
        private const int MaxPageSize = 50;

        public int PageNumber
        {
            get => field;
            set => field = Math.Max(1, value);
        } = 1;

        public int PageSize
        {
            get => field;
            set => field = Math.Min(value, MaxPageSize);
        } = 10;
    }
}
