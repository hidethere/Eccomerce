namespace User_service.Dto
{
    public record ServiceResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
