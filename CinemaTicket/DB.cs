using Microsoft.Data.SqlClient;

namespace CinemaTicket
{
    public static class DB
    {
        // Nên viết chuỗi kết nối trên 1 dòng hoặc nối chuỗi để tránh dính khoảng trắng thừa
        private static readonly string _conn =
            "Server=DESKTOP-AV9ONFH\\SQLEXPRESS;" +
            "Database=CinemaDB;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            // Quan trọng: Luôn trả về một đối tượng NEW để mỗi Form có kết nối riêng
            return new SqlConnection(_conn);
        }
    }
}