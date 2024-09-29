using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JavaHateBE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameMode = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<uint>(type: "INTEGER", nullable: false),
                    startTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    endTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<float>(type: "REAL", nullable: false),
                    Difficulty = table.Column<byte>(type: "INTEGER", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "Answer", "Difficulty", "GameId", "Text" },
                values: new object[,]
                {
                    { new Guid("011bbaeb-5b64-4ac4-a10f-06f3c0aaa877"), 11f, (byte)1, null, "11 + 0" },
                    { new Guid("021f7f23-b575-4237-a37b-6518173932d5"), 18f, (byte)1, null, "15 + 3" },
                    { new Guid("0315ad97-3215-4ba6-b7d2-4fa501dd7942"), 18f, (byte)1, null, "4 + 14" },
                    { new Guid("04ea0cf2-551d-47c5-bf06-12396270bdb7"), 17f, (byte)1, null, "15 + 2" },
                    { new Guid("0830dc1e-e47b-485f-a6ea-acc245827dd7"), 19f, (byte)1, null, "13 + 6" },
                    { new Guid("0985d3c7-d227-4bbf-ad11-845ff6840530"), 16f, (byte)1, null, "13 + 3" },
                    { new Guid("0fb0deae-6973-40a0-8e14-f681ab287da2"), 16f, (byte)1, null, "10 + 6" },
                    { new Guid("11cb8584-5291-4bfb-8d48-008e12c2331b"), 28f, (byte)1, null, "14 + 14" },
                    { new Guid("1278e732-65b1-4782-bc3d-d4d73c4c013f"), 14f, (byte)1, null, "8 + 6" },
                    { new Guid("12a6f1ed-dd3a-4ca9-8cd8-be8f987d6f5b"), 5f, (byte)1, null, "1 + 4" },
                    { new Guid("136b9622-3c1e-49f1-b7ff-751dca58263b"), 24f, (byte)1, null, "13 + 11" },
                    { new Guid("141d38a3-0b5c-45b3-b3ee-637a09f390ea"), 15f, (byte)1, null, "1 + 14" },
                    { new Guid("144cad9e-a2da-48bf-8146-af65c641f44a"), 16f, (byte)1, null, "4 + 12" },
                    { new Guid("154c24c1-5ac7-43f7-a638-d7e02e60ae0c"), 19f, (byte)1, null, "11 + 8" },
                    { new Guid("16ecd8af-6614-4670-83a9-f12b9fa1b125"), 13f, (byte)1, null, "2 + 11" },
                    { new Guid("18644b19-5bfa-40b5-9394-6984c9c4da1a"), 11f, (byte)1, null, "4 + 7" },
                    { new Guid("19360b01-4af3-43d7-8266-e1cc1d0719db"), 7f, (byte)1, null, "2 + 5" },
                    { new Guid("1bf42073-ed27-4cbe-b676-c1ea547f2cde"), 28f, (byte)1, null, "15 + 13" },
                    { new Guid("1c049424-a4e6-49bf-a3d1-016851362563"), 13f, (byte)1, null, "3 + 10" },
                    { new Guid("1c59b022-fc6b-4551-9b0d-cc4e180ef986"), 16f, (byte)1, null, "11 + 5" },
                    { new Guid("1dfae402-8866-46cb-895d-ba48555947fa"), 5f, (byte)1, null, "5 + 0" },
                    { new Guid("1ef5f07a-1392-478a-bcad-cf9fcc010118"), 21f, (byte)1, null, "11 + 10" },
                    { new Guid("2079f50e-508e-4e5b-85b3-42cb1cb30a92"), 6f, (byte)1, null, "5 + 1" },
                    { new Guid("20df3a02-3a72-45e7-b4ac-58727daef0cf"), 20f, (byte)1, null, "11 + 9" },
                    { new Guid("20e06049-ac11-4707-8479-453a1e68258e"), 17f, (byte)1, null, "6 + 11" },
                    { new Guid("217482c6-8bc9-48d5-af0c-e416e05ed838"), 25f, (byte)1, null, "14 + 11" },
                    { new Guid("23c2db07-b421-4c82-8d8d-70d463658215"), 25f, (byte)1, null, "12 + 13" },
                    { new Guid("24d562bb-c89a-4d39-8bfc-1b4943de9b6d"), 23f, (byte)1, null, "15 + 8" },
                    { new Guid("26a36816-077b-467b-98b4-3a552ec545e8"), 25f, (byte)1, null, "13 + 12" },
                    { new Guid("2971e466-a8b4-4d10-b39b-b8f476f5065a"), 10f, (byte)1, null, "2 + 8" },
                    { new Guid("29934a15-2c0e-488b-8920-59c799fbfbb3"), 18f, (byte)1, null, "7 + 11" },
                    { new Guid("29ec8618-9310-4ddd-a63c-baad5006e814"), 6f, (byte)1, null, "6 + 0" },
                    { new Guid("2be501a3-9269-410c-94c3-9bf95ec993dd"), 14f, (byte)1, null, "7 + 7" },
                    { new Guid("2e3950fc-ef00-47c7-b663-44609b740056"), 20f, (byte)1, null, "14 + 6" },
                    { new Guid("2f35c43a-f4ec-4ac7-9e9e-16b70518825b"), 22f, (byte)1, null, "12 + 10" },
                    { new Guid("2f73753e-5eea-4f7b-a0ae-2243bf696823"), 13f, (byte)1, null, "12 + 1" },
                    { new Guid("2f9a101e-25f4-4d65-959d-99c692930067"), 26f, (byte)1, null, "15 + 11" },
                    { new Guid("3081bd52-2094-4fa9-9eb0-571d3ff1d375"), 18f, (byte)1, null, "12 + 6" },
                    { new Guid("3139cfe5-ca6f-4c16-ab08-777e69be00a3"), 14f, (byte)1, null, "4 + 10" },
                    { new Guid("330b197e-0f20-44e1-941e-4734e963c018"), 17f, (byte)1, null, "3 + 14" },
                    { new Guid("3561c725-46fa-433e-9176-1a5543da915b"), 10f, (byte)1, null, "4 + 6" },
                    { new Guid("3931fec2-addb-41cb-bc70-7f0dc11d971b"), 16f, (byte)1, null, "5 + 11" },
                    { new Guid("39427a49-4bfc-4fe0-ac55-a00ef8ccff39"), 11f, (byte)1, null, "10 + 1" },
                    { new Guid("3bca1897-3ba4-463d-a520-99b69983d1e2"), 14f, (byte)1, null, "9 + 5" },
                    { new Guid("3cb2ea57-5510-46f6-8d01-a926d4fecca8"), 21f, (byte)1, null, "14 + 7" },
                    { new Guid("3d8ffb20-b4ca-4e9a-a33d-914b04e8885c"), 11f, (byte)1, null, "2 + 9" },
                    { new Guid("4147515c-17b3-4cf1-8ac4-460bfaba9a2d"), 1f, (byte)1, null, "1 + 0" },
                    { new Guid("41d04a71-8241-4fe6-a93a-92a53f674dee"), 16f, (byte)1, null, "9 + 7" },
                    { new Guid("4329603d-4d24-4fb8-8c34-5435bf09ee92"), 6f, (byte)1, null, "1 + 5" },
                    { new Guid("433b6b14-2481-4976-9e69-9d9a591c8d50"), 22f, (byte)1, null, "15 + 7" },
                    { new Guid("4482d0dc-8b35-40d5-8550-bde8f40733ed"), 17f, (byte)1, null, "12 + 5" },
                    { new Guid("4526df6d-1c3b-49aa-a256-fc38a0392118"), 4f, (byte)1, null, "2 + 2" },
                    { new Guid("458b732c-9362-42ed-b781-764a425df2cc"), 9f, (byte)1, null, "9 + 0" },
                    { new Guid("45f7abd1-6ac1-4e83-b198-8981a6dab6a4"), 8f, (byte)1, null, "8 + 0" },
                    { new Guid("45ff3123-3bbf-4563-881b-40d9b2e85819"), 22f, (byte)1, null, "13 + 9" },
                    { new Guid("46ad150c-df25-46d6-ba20-416d7f6ca560"), 21f, (byte)1, null, "10 + 11" },
                    { new Guid("46bea4bc-0b4e-4b14-a03f-64ee95542cbd"), 9f, (byte)1, null, "1 + 8" },
                    { new Guid("47bbae4c-76d8-4283-973a-b3e63e78cfb4"), 12f, (byte)1, null, "7 + 5" },
                    { new Guid("4aebc357-861e-45b1-95cb-6988ca0f6fe5"), 9f, (byte)1, null, "8 + 1" },
                    { new Guid("4b5b8930-ede0-4dbc-94c9-ddcf7e799987"), 19f, (byte)1, null, "12 + 7" },
                    { new Guid("4bf124b5-61e1-4b9a-a58c-ae466ae8c032"), 13f, (byte)1, null, "5 + 8" },
                    { new Guid("4f6511b9-f11b-4ffd-a413-5196f2ba151c"), 7f, (byte)1, null, "4 + 3" },
                    { new Guid("50e693d3-9236-44b0-92fa-efb0465148ec"), 24f, (byte)1, null, "14 + 10" },
                    { new Guid("5193775c-8303-49d9-8e0a-158470773178"), 10f, (byte)1, null, "7 + 3" },
                    { new Guid("5222e0b5-653f-4da8-8b80-821661460dd6"), 20f, (byte)1, null, "9 + 11" },
                    { new Guid("52e1be02-4de9-4d2e-b884-cb0ae54cdc42"), 19f, (byte)1, null, "14 + 5" },
                    { new Guid("52ecc2a6-7183-4aa6-bfa3-e28010d4c7c6"), 16f, (byte)1, null, "12 + 4" },
                    { new Guid("542c8876-7da1-4d34-b21e-2cff71136a7a"), 13f, (byte)1, null, "1 + 12" },
                    { new Guid("5463343b-6603-49b4-bd5c-e72a0b18c97e"), 11f, (byte)1, null, "1 + 10" },
                    { new Guid("549b5ecf-09cf-46eb-9a7f-df5c8a49176c"), 12f, (byte)1, null, "8 + 4" },
                    { new Guid("55c46172-59ba-4557-9f5c-1dbe691c840d"), 5f, (byte)1, null, "3 + 2" },
                    { new Guid("56b817eb-a313-4a04-979b-94e20b6eb327"), 9f, (byte)1, null, "2 + 7" },
                    { new Guid("56b8744c-45ac-471c-8ada-b2d1d18521f6"), 13f, (byte)1, null, "13 + 0" },
                    { new Guid("56cbb2be-860d-44ff-9c30-00ef22c1392c"), 11f, (byte)1, null, "3 + 8" },
                    { new Guid("5906595d-d9c5-43a2-ae81-ba9b4affa88e"), 11f, (byte)1, null, "8 + 3" },
                    { new Guid("59a14c83-8deb-44bf-b91c-f5c80227924f"), 20f, (byte)1, null, "13 + 7" },
                    { new Guid("5a5864f4-7ae0-4cc3-9fad-caa83ab16207"), 21f, (byte)1, null, "15 + 6" },
                    { new Guid("5ab79744-0786-432f-8ca6-7d0cbfb35421"), 29f, (byte)1, null, "15 + 14" },
                    { new Guid("5c69161e-a1dd-45a7-bd1b-b1cc6dbe6e25"), 14f, (byte)1, null, "3 + 11" },
                    { new Guid("5cb1ad6a-1e18-4786-8b14-e2a2e4ad791c"), 14f, (byte)1, null, "14 + 0" },
                    { new Guid("5d888602-c7c8-4aed-9930-65b5138970cb"), 10f, (byte)1, null, "8 + 2" },
                    { new Guid("5de8a92e-5df0-49d9-84ec-38b7a9359c2b"), 2f, (byte)1, null, "2 + 0" },
                    { new Guid("5e41039a-5e5d-41f6-91eb-e93a3100f74d"), 19f, (byte)1, null, "8 + 11" },
                    { new Guid("5f937cb5-8579-40ed-9b86-4e48fd4b328f"), 4f, (byte)1, null, "3 + 1" },
                    { new Guid("5faa314a-6b76-4dcc-9acb-e39deace7b70"), 14f, (byte)1, null, "11 + 3" },
                    { new Guid("60302a2e-53b3-45ee-8937-d15f4426d160"), 18f, (byte)1, null, "13 + 5" },
                    { new Guid("60a1cf46-6655-4651-a2e2-ad8eb97bbf67"), 10f, (byte)1, null, "1 + 9" },
                    { new Guid("6122a062-a434-48f9-8e55-32914df78221"), 15f, (byte)1, null, "8 + 7" },
                    { new Guid("613d8bf6-9080-4527-948c-7973dad63f70"), 15f, (byte)1, null, "5 + 10" },
                    { new Guid("61f3a9a8-ca6b-443a-926f-2f8e0a625066"), 12f, (byte)1, null, "5 + 7" },
                    { new Guid("626de15a-339b-41c3-a4da-93eee12941e2"), 27f, (byte)1, null, "14 + 13" },
                    { new Guid("647e468b-33fd-429b-8992-b078ca505e75"), 16f, (byte)1, null, "3 + 13" },
                    { new Guid("65152cdf-8478-479d-83da-f69c10cb671f"), 2f, (byte)1, null, "1 + 1" },
                    { new Guid("65652316-515d-4f62-adc3-68cab828d9ca"), 22f, (byte)1, null, "14 + 8" },
                    { new Guid("6624a104-da37-46fc-8b15-09102d5ef5d0"), 6f, (byte)1, null, "2 + 4" },
                    { new Guid("67338c1c-029b-4131-9432-62d41488e16b"), 22f, (byte)1, null, "11 + 11" },
                    { new Guid("68a9ca64-5771-4d74-81a5-b0023dadbb69"), 8f, (byte)1, null, "5 + 3" },
                    { new Guid("6af36c9a-bfc0-4f47-95e9-f8f7af775c18"), 18f, (byte)1, null, "6 + 12" },
                    { new Guid("6ba8e87e-c9ed-458f-ac1d-40800197d764"), 15f, (byte)1, null, "15 + 0" },
                    { new Guid("6f40497b-393d-486e-9adc-623940c92f6f"), 13f, (byte)1, null, "11 + 2" },
                    { new Guid("705c8d9d-c05e-414b-a2ae-af742d722f70"), 12f, (byte)1, null, "6 + 6" },
                    { new Guid("70c89ed5-f0ff-4aea-9d29-761cab29bb88"), 24f, (byte)1, null, "12 + 12" },
                    { new Guid("71426c32-92a9-4458-8abf-ef99d5b6db16"), 23f, (byte)1, null, "13 + 10" },
                    { new Guid("7386391a-c781-4529-afe1-a16e4d0e77fd"), 16f, (byte)1, null, "2 + 14" },
                    { new Guid("75a895f9-a65f-41e0-9fd3-b14535804f1d"), 15f, (byte)1, null, "13 + 2" },
                    { new Guid("796cfe00-9431-4f44-ab11-cadbfeb95a94"), 27f, (byte)1, null, "15 + 12" },
                    { new Guid("7bf048a3-bf01-4520-9514-ab1ab90bf787"), 21f, (byte)1, null, "7 + 14" },
                    { new Guid("7dbd6b2c-efdb-4a42-aafd-ccd3fd8ee422"), 8f, (byte)1, null, "2 + 6" },
                    { new Guid("7e1bef1f-0b38-4e4f-b4f6-02e51ef09d40"), 16f, (byte)1, null, "6 + 10" },
                    { new Guid("7e6a2bd9-2a2e-4b7f-8182-92a73b376117"), 11f, (byte)1, null, "5 + 6" },
                    { new Guid("7e8fbd28-8eba-4cee-973d-dce2701581fd"), 15f, (byte)1, null, "7 + 8" },
                    { new Guid("7ef9c795-7951-404d-a80a-ffd6ab38375c"), 5f, (byte)1, null, "2 + 3" },
                    { new Guid("803ccbe4-4b1d-4416-ab60-c1b2c4898e7a"), 17f, (byte)1, null, "7 + 10" },
                    { new Guid("8171086b-2d9f-4c1e-904f-4b6c011cd2f2"), 14f, (byte)1, null, "1 + 13" },
                    { new Guid("821eecbd-6b9b-4f0e-a081-0dd4bb8c84b4"), 26f, (byte)1, null, "13 + 13" },
                    { new Guid("858bfa14-73de-4238-b4e8-1461fc174d5e"), 15f, (byte)1, null, "10 + 5" },
                    { new Guid("875810aa-0d2f-4d82-9bd6-834937f775b3"), 18f, (byte)1, null, "9 + 9" },
                    { new Guid("877397dc-edc3-4dc9-a5e4-fd8161f4d40f"), 17f, (byte)1, null, "14 + 3" },
                    { new Guid("87bb4ff0-5fdc-47ea-8f8c-671772b65c50"), 3f, (byte)1, null, "1 + 2" },
                    { new Guid("88d3811e-b19a-4d98-a2c9-0d5c78a76c53"), 6f, (byte)1, null, "4 + 2" },
                    { new Guid("8a34f6ba-408c-4ca2-8424-c53e1638f83b"), 22f, (byte)1, null, "10 + 12" },
                    { new Guid("8badd258-c674-4631-a625-54348fc0a823"), 8f, (byte)1, null, "1 + 7" },
                    { new Guid("8c172e64-be00-4641-9836-a5c9e6e88843"), 20f, (byte)1, null, "10 + 10" },
                    { new Guid("8c894268-876d-4248-b5e8-4cb5e9c1be51"), 8f, (byte)1, null, "6 + 2" },
                    { new Guid("8d616bd5-a704-43dd-8d41-3016474f2a18"), 11f, (byte)1, null, "9 + 2" },
                    { new Guid("8d64d4e2-4ec9-4e06-878e-387e31e49474"), 7f, (byte)1, null, "7 + 0" },
                    { new Guid("8ddcc778-83cc-4d22-9fb1-8298659f21c6"), 20f, (byte)1, null, "12 + 8" },
                    { new Guid("8df802b3-6c55-4c12-bfb5-1f66eaa7f30e"), 24f, (byte)1, null, "11 + 13" },
                    { new Guid("8e984bbe-ac39-4969-a753-76009c1fb4af"), 15f, (byte)1, null, "11 + 4" },
                    { new Guid("8f84ec6a-321e-4e2a-9722-c58091801a48"), 24f, (byte)1, null, "15 + 9" },
                    { new Guid("9032b542-af56-4294-b275-6a8185dace4b"), 13f, (byte)1, null, "10 + 3" },
                    { new Guid("90c0b619-e562-46bf-bad9-55d1981e282c"), 23f, (byte)1, null, "14 + 9" },
                    { new Guid("932582af-c651-4d77-b730-f7e14fb6a798"), 17f, (byte)1, null, "8 + 9" },
                    { new Guid("94d1dcb3-4ed8-4df1-aca7-870343b3427c"), 16f, (byte)1, null, "15 + 1" },
                    { new Guid("96cbf69a-2975-496b-9b6e-43c03f913063"), 3f, (byte)1, null, "3 + 0" },
                    { new Guid("9849a143-ca26-4366-b443-a43360ad7351"), 22f, (byte)1, null, "8 + 14" },
                    { new Guid("99027f1e-827a-4394-bdd1-8f0b5d4a3d7b"), 10f, (byte)1, null, "5 + 5" },
                    { new Guid("9ae9b551-4a98-467b-932a-1c023cefa487"), 19f, (byte)1, null, "5 + 14" },
                    { new Guid("9b974246-3e50-42f0-9810-1fdc277128c2"), 14f, (byte)1, null, "2 + 12" },
                    { new Guid("9ba342c5-9676-4e5c-85b1-31d43555043f"), 4f, (byte)1, null, "1 + 3" },
                    { new Guid("9bcfcef0-eafb-4750-a6ea-0b39648691e9"), 15f, (byte)1, null, "2 + 13" },
                    { new Guid("9bf4b61d-60b2-484e-9ea8-9c8f914b037a"), 7f, (byte)1, null, "1 + 6" },
                    { new Guid("9c1fdcd3-a43d-4407-99f2-23eac74af576"), 13f, (byte)1, null, "4 + 9" },
                    { new Guid("9c314b50-b8bc-48bd-a41b-46fde4df577f"), 15f, (byte)1, null, "3 + 12" },
                    { new Guid("9c95ba14-5bef-47e8-902f-005cfa0b522a"), 3f, (byte)1, null, "2 + 1" },
                    { new Guid("9f6e091a-1762-4f7a-a69e-fadfe6012ebf"), 17f, (byte)1, null, "9 + 8" },
                    { new Guid("a000ba43-49bc-432b-b562-3b8010cabd89"), 18f, (byte)1, null, "10 + 8" },
                    { new Guid("a1ad4adf-b9fe-451b-b171-23ab31c5731d"), 14f, (byte)1, null, "12 + 2" },
                    { new Guid("a220449e-d251-448e-9c6a-5f32c53c8b11"), 5f, (byte)1, null, "4 + 1" },
                    { new Guid("a2452253-006c-4ccb-848c-0200bc40278e"), 23f, (byte)1, null, "10 + 13" },
                    { new Guid("a2dfdca1-4b5b-4a9d-8654-dd21e1930c49"), 11f, (byte)1, null, "7 + 4" },
                    { new Guid("a6f1d5a8-2938-423c-b7ca-781c990012c4"), 21f, (byte)1, null, "13 + 8" },
                    { new Guid("a6f3d295-e64a-4968-9145-aa14b7e44adc"), 9f, (byte)1, null, "7 + 2" },
                    { new Guid("a7373afc-4041-4d40-8a92-0fe636ad7125"), 23f, (byte)1, null, "12 + 11" },
                    { new Guid("a97f2827-5f0d-4684-8ecb-101bed93615d"), 11f, (byte)1, null, "6 + 5" },
                    { new Guid("ab23c0b9-c819-4ad1-8fea-5be5eaaea418"), 6f, (byte)1, null, "3 + 3" },
                    { new Guid("abb89eac-3c1d-4230-a124-1b1615c0b6c0"), 13f, (byte)1, null, "7 + 6" },
                    { new Guid("abf3c8ad-70cb-48a5-9a55-b35d7e2da12a"), 18f, (byte)1, null, "14 + 4" },
                    { new Guid("ad214681-226d-404e-8638-72c239566104"), 18f, (byte)1, null, "11 + 7" },
                    { new Guid("ad56f05b-8c69-4450-bba5-b888ec15f638"), 15f, (byte)1, null, "4 + 11" },
                    { new Guid("aff138e8-abfe-4844-8ab5-1ba43d82f25d"), 15f, (byte)1, null, "6 + 9" },
                    { new Guid("b0c83071-3bde-4cb1-bbdf-c37448c804b0"), 12f, (byte)1, null, "10 + 2" },
                    { new Guid("b210facb-a5da-4c3e-8e77-9ca187994747"), 15f, (byte)1, null, "9 + 6" },
                    { new Guid("b3ce327a-94bf-4fd8-9f4d-560a0102683a"), 14f, (byte)1, null, "6 + 8" },
                    { new Guid("b54aa4b8-1fce-4a99-92e0-b1ba412d7c6d"), 16f, (byte)1, null, "8 + 8" },
                    { new Guid("b596b0a4-a706-4ca6-ab34-9e793d898f86"), 27f, (byte)1, null, "13 + 14" },
                    { new Guid("b5c7a790-0b85-4318-9482-981df9db017f"), 13f, (byte)1, null, "9 + 4" },
                    { new Guid("b647f8e5-623d-4db7-aea6-3c366c3da6e8"), 19f, (byte)1, null, "10 + 9" },
                    { new Guid("be4e21a1-02fa-48f5-8c12-ae4e0a5264a6"), 20f, (byte)1, null, "15 + 5" },
                    { new Guid("c00f9aeb-13d1-4766-a3ce-d6c8b4464343"), 12f, (byte)1, null, "9 + 3" },
                    { new Guid("c1262a3f-18f3-441b-88ba-39b28cf02edc"), 19f, (byte)1, null, "9 + 10" },
                    { new Guid("c19cbd73-511f-4fea-bad7-58f92957ab95"), 25f, (byte)1, null, "11 + 14" },
                    { new Guid("c1bda796-ce9e-4ec2-8ba0-c9a8c8436ddf"), 8f, (byte)1, null, "4 + 4" },
                    { new Guid("c3651a3e-f9a8-4d0a-8342-da0b601c9df1"), 17f, (byte)1, null, "4 + 13" },
                    { new Guid("c700ceb3-7bfd-488c-87f1-a96a4cc95b8d"), 20f, (byte)1, null, "8 + 12" },
                    { new Guid("c795619e-8ac6-4744-b7fa-2ed1dfb4cbbe"), 16f, (byte)1, null, "14 + 2" },
                    { new Guid("c7ee4cf2-4ca2-4e27-bb60-22736143d5e1"), 20f, (byte)1, null, "6 + 14" },
                    { new Guid("c7f6b861-0fd4-438c-94a9-55693f08afb9"), 19f, (byte)1, null, "6 + 13" },
                    { new Guid("cae63f60-e489-42b7-8d15-3afad69e20b5"), 12f, (byte)1, null, "4 + 8" },
                    { new Guid("cb6eb4f7-e044-43b0-9d4c-376f573c62e1"), 15f, (byte)1, null, "14 + 1" },
                    { new Guid("cb73eb84-47d7-422e-bcf0-7b2b1f364183"), 26f, (byte)1, null, "12 + 14" },
                    { new Guid("cbd3d830-9bc9-4f2a-937d-4b0bfa9154a3"), 21f, (byte)1, null, "12 + 9" },
                    { new Guid("cbd424a5-0484-4911-ae1c-b41c3e2b8d79"), 13f, (byte)1, null, "8 + 5" },
                    { new Guid("cc254121-e2f8-4ffd-83bf-5b7178e684f1"), 19f, (byte)1, null, "15 + 4" },
                    { new Guid("cd21b27d-0bdf-4fd2-a505-ab7985780185"), 17f, (byte)1, null, "10 + 7" },
                    { new Guid("ce77cdb1-f224-4cbc-965d-c29dd7772630"), 14f, (byte)1, null, "5 + 9" },
                    { new Guid("cf36f11d-b7cc-4045-bfb4-caa7ecbddaf6"), 10f, (byte)1, null, "9 + 1" },
                    { new Guid("d00207a8-ed91-4c87-b06b-71fa18e1240c"), 12f, (byte)1, null, "2 + 10" },
                    { new Guid("d3466c23-a754-4e32-8b94-97a3cfd64228"), 14f, (byte)1, null, "10 + 4" },
                    { new Guid("d415f04a-1c35-44f3-b7d1-f5800c8ac963"), 19f, (byte)1, null, "7 + 12" },
                    { new Guid("d48aeddf-5090-40a9-ac6a-d3fa668b618f"), 18f, (byte)1, null, "5 + 13" },
                    { new Guid("d756dfdf-90b0-4c4a-8ef8-d8692e8c4891"), 15f, (byte)1, null, "12 + 3" },
                    { new Guid("da292caf-4d38-43c5-a41a-08d16909e518"), 12f, (byte)1, null, "1 + 11" },
                    { new Guid("dc141469-f899-4930-a669-d443160e0ceb"), 22f, (byte)1, null, "9 + 13" },
                    { new Guid("dc284571-7d0d-4ac0-8b86-3d496369191f"), 9f, (byte)1, null, "4 + 5" },
                    { new Guid("dca5c3ef-f834-4f97-86c1-22e2e12a0ad1"), 23f, (byte)1, null, "9 + 14" },
                    { new Guid("dd71ca68-e47b-423a-ad4b-ed15d3555ee8"), 13f, (byte)1, null, "6 + 7" },
                    { new Guid("de185fc8-4795-4d33-93c7-1da4db669975"), 12f, (byte)1, null, "11 + 1" },
                    { new Guid("df3ad41e-49bd-4fd6-8706-19ac46169a4f"), 7f, (byte)1, null, "3 + 4" },
                    { new Guid("e245e2fd-e1c9-4277-9129-dc0b3649f75c"), 7f, (byte)1, null, "6 + 1" },
                    { new Guid("e26b0747-0aac-4995-83af-bbfc43dccfdd"), 26f, (byte)1, null, "14 + 12" },
                    { new Guid("e2b44890-b25c-4b38-b42a-49472dc97db0"), 10f, (byte)1, null, "6 + 4" },
                    { new Guid("e349feca-ca6d-41e1-ad85-68ee3ad22591"), 23f, (byte)1, null, "11 + 12" },
                    { new Guid("e3999c72-d1ca-4421-89d9-29e4bfcb7e8a"), 8f, (byte)1, null, "7 + 1" },
                    { new Guid("e4990ad6-6e7e-4d49-958b-24b8a618ad07"), 12f, (byte)1, null, "3 + 9" },
                    { new Guid("e511368c-5109-4fa8-acfe-6bd287672510"), 14f, (byte)1, null, "13 + 1" },
                    { new Guid("e5bc9f1b-31ce-443a-aa67-348ab93bdae0"), 17f, (byte)1, null, "11 + 6" },
                    { new Guid("e664c3dc-ec4e-40ea-b901-a613f76a8116"), 4f, (byte)1, null, "4 + 0" },
                    { new Guid("e7ab120c-a8bc-42ad-afce-ba8fee3454b5"), 24f, (byte)1, null, "10 + 14" },
                    { new Guid("e95403e0-a21e-4eb6-a0e8-c0f998abce77"), 9f, (byte)1, null, "3 + 6" },
                    { new Guid("e970b918-7a09-4398-a95e-d810a9804218"), 7f, (byte)1, null, "5 + 2" },
                    { new Guid("eabc115a-658d-4775-bcae-8c251df04616"), 18f, (byte)1, null, "8 + 10" },
                    { new Guid("ec1b152f-2ba3-463d-ae22-335e219332d9"), 21f, (byte)1, null, "9 + 12" },
                    { new Guid("ec420d16-3e1c-4ea1-9cdb-7c2b3df700b8"), 9f, (byte)1, null, "5 + 4" },
                    { new Guid("ecfb091c-fc5f-450f-8544-f6923412c3b3"), 10f, (byte)1, null, "10 + 0" },
                    { new Guid("efeb539d-e5d3-4fcb-a362-ca1440671a2a"), 25f, (byte)1, null, "15 + 10" },
                    { new Guid("f0374d96-e9e3-4802-9495-018281e7da8f"), 16f, (byte)1, null, "7 + 9" },
                    { new Guid("f1ffcfcb-0aa8-4301-a9b5-0501421c7bab"), 12f, (byte)1, null, "12 + 0" },
                    { new Guid("f3d17e55-5e27-4bd2-8b3a-7e027a965c64"), 10f, (byte)1, null, "3 + 7" },
                    { new Guid("f62c2946-1892-40ba-bc7e-3f50ec19efdb"), 21f, (byte)1, null, "8 + 13" },
                    { new Guid("f72428df-7526-46ff-b51d-f101d5e86317"), 17f, (byte)1, null, "5 + 12" },
                    { new Guid("facbc299-f4ef-48a8-a79b-5dbdce3aa5d7"), 9f, (byte)1, null, "6 + 3" },
                    { new Guid("fd952aa2-b8d9-4e17-814e-4e8f85e16a92"), 20f, (byte)1, null, "7 + 13" },
                    { new Guid("fe624533-073a-46a0-aa69-0faade3425cc"), 8f, (byte)1, null, "3 + 5" },
                    { new Guid("ff3fabb2-c3b4-4b41-8c94-102c82b15ea7"), 17f, (byte)1, null, "13 + 4" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastLogin", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("1d352f3c-f683-4d8f-80fd-58e22274e450"), "matas@example.com", new DateTime(2024, 9, 29, 16, 45, 47, 609, DateTimeKind.Local).AddTicks(6460), "7/tcPZ0ilHja0bm9c0d4cUTzJrqLv5exZxBuceBjQmB7RokvFDwSTMzpamSgnbPv", "Matas" },
                    { new Guid("b2b25912-86eb-439f-93ce-c6fb7171caa9"), "robbe@example.com", new DateTime(2024, 9, 29, 16, 45, 47, 576, DateTimeKind.Local).AddTicks(460), "1A1qLT9m0ODKsyYkqWChw/OJlAucTQLhcvXGobw41vDwoXMt6jTOeGsPmKw9NYKt", "Robbe" },
                    { new Guid("ef118dba-27b0-4113-9145-c1ed7da4fdc1"), "valdemar@example.com", new DateTime(2024, 9, 29, 16, 45, 47, 592, DateTimeKind.Local).AddTicks(9810), "z3i6oB6C0WHzqQ42fwEhlZgcOKI0cJZr2G4P3kZyX2AV3e68Ktu6QMZQZo1Wxdry", "Valdemar" },
                    { new Guid("fe689ef8-8df3-4d33-94ca-f3b1a1fa23ad"), "david@example.com", new DateTime(2024, 9, 29, 16, 45, 47, 559, DateTimeKind.Local).AddTicks(6300), "lky7elm96TArktWcrsc/BnahSZ8Rn2WMV5tYbjmSLLzyxuJYmxJEyTDp9UtHo0A8", "David" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_GameId",
                table: "Questions",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
