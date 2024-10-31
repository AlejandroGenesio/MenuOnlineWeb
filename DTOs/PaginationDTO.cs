namespace MenuOnlineUdemy.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int recordsByPage = 10;
        private readonly int maxTotalByPage = 50;

        public int RecordsByPage
        {
            get
            {
                return recordsByPage;
            }
            set
            {
                recordsByPage = (value > maxTotalByPage) ? maxTotalByPage : value;
            }
        }
    }
}
