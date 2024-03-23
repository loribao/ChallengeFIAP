namespace AppSec.Domain.Commands;

public record SastStartCommand(int ProjectId,string Sonartoken) ;
