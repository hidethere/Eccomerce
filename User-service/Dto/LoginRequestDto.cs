namespace User_service.Dto
{
    public record LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
