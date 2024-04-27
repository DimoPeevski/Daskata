using Daskata.Core.ViewModels;

public class ExamListViewModel
{
    public PaginatedList<FullExamViewModel> Exams { get; set; } = null!;
        
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public string? CategoryFilter { get; set; }

    public DateTime? DateFromFilter { get; set; }

    public DateTime? DateToFilter { get; set; }
}