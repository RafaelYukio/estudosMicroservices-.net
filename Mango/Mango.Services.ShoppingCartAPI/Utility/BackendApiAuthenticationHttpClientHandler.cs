using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Mango.Services.ShoppingCartAPI.Utility
{
    // Classe para passar o token na requesição do ShoppingCartAPI para o CouponAPI
    // Chamadas diretas do front vem com o Token
    // Chamadas entre serviços não vem com o Token (resolvemos aqui)
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Pega o token que vem da requisição do front
            var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

            // Coloca o token no header das requisições que vem daqui para outro serviço
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
