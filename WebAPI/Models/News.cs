namespace WebAPI.Models;

public partial class News
{
    public Guid NewsId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public DateTime InsertDateTime { get; set; }

    public DateTime UpdateDateTime { get; set; }

    public Guid InsertEmployeeId { get; set; }

    public Guid UpdateEmployeeId { get; set; }

    public int Click { get; set; }

    public bool Enable { get; set; }

    public int Order { get; set; }

    public virtual Employee InsertEmployee { get; set; }
    public virtual Employee UpdateEmployee { get; set; }
    public virtual ICollection<NewsFiles> NewsFiles { get; set; }
}
