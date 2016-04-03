namespace StudyTravel.Common.QueryFilter
{
    public class DataQueryModel
    {
        public string SortBy { get; set; }
        public int Top { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool Desc { get; set; }

        public string Filter { get; set; }
    }
}