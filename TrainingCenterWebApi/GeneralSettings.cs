namespace TrainingCenterWebApi
{

    public class JwtSettings
    {
        public required String Key { get; set; }
        public required String Issuer { get; set; }
        public required String Audience { get; set; }
        public Int32 ExpiryInMinutes { get; set; } = 20;
    }

    public class GeneralSettings
    {
        public const String sectionName = "GeneralSettings";
        public required String ConnectionString { get; set; }

        public required String CorsAllowedOrigins { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }
}
