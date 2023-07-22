namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        // URLs das API são salvas no appsettings
        // Para inserir os dados das URLs do appsettings nestas variáveis, é preciso configurar no Program.cs
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
