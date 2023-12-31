﻿namespace Mango.Web.Utility
{
    public class StaticDetails
    {
        // URLs das API são salvas no appsettings
        // Para inserir os dados das URLs do appsettings nestas variáveis, é preciso configurar no Program.cs
        public static string CouponAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string OrderAPIBase { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JwtToken";

        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
