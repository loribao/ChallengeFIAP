namespace AppSec.Domain.Commands;

public record DastStartCommand
{
    public int ProjectId { get; set; }
    public string TargetUrl { get; set; }
    public string DastToken { get; set; }
    public string DastUrlApi { get; set; }
}
