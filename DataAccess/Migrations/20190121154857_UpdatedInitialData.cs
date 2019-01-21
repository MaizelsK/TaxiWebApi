using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class UpdatedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2b$10$iHS0q4ZWNzu9Y1qdY9oEBe1A4LDUEdpfBvGeAVWWYcHDOc8lFuYUW");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Role", "Username" },
                values: new object[] { 2, "driver@mail.ru", "$2b$10$W/FLgneRHKB7dkcxNmfTC.0iglAMUqk4.2MkPWlXgooU5pObx55AC", "driver", "driver" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2b$10$gMtTAmHiXGcugIWxOy37p.OBvwhgsqtIyZ4bZQXQHGdY3syPOkequ");
        }
    }
}
