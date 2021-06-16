namespace IesWebPortal.Services.Interfaces
{
    public interface IMLLabelConfig
    {
        int ColCount { get; set; }
        string Description { get; set; }
        string ReportName { get; set; }
        int RowCount { get; set; }
        string Settings { get; set; }
    }
}