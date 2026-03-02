namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers
{
    public class GetQuestionAnswersIn
    {
        public string? SearchValue { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsViewAll { get; set; }
        public bool? IsShowActiveOnly { get; set; }
    }
}
