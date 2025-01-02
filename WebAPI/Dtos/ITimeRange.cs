namespace WebAPI.Dtos
{
    public interface ITimeRange
    {
        DateTime StartDateTime { get; set; }
        DateTime EndDateTime { get; set; }
    }
}
