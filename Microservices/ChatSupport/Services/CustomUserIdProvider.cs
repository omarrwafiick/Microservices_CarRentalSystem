using Microsoft.AspNetCore.SignalR;

namespace ChatSupportApi.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            //example on how it is sent using JS
            //const connection = new signalR.HubConnectionBuilder()
            //    .withUrl("/chathub?userId=USER123")
            //    .build();
            var httpContext = connection.GetHttpContext();
            var userId = httpContext?.Request.Query["userId"].ToString();
            return string.IsNullOrEmpty(userId) ? null : userId;
        }
    }
}
