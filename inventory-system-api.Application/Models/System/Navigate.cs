namespace inventory_system_api.Application.Models.System
{
    public class Navigate
    {
        public int PageNo { get; set; } = 0;
        public int RowPerPage { get; set; } = 10;

        public int PageCount {  get; set; }

        public object? Entity {  get; set; }
    }
}
