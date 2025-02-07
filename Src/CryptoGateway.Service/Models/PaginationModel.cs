namespace CryptoGateway.Service.Models;

public class PaginationModel
{
    private static int OffsetMaker(int page, int pageSize) => (page - 1) * pageSize;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int Offset => OffsetMaker(Page, PageSize);
}