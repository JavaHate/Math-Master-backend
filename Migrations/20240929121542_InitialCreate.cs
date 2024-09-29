﻿using System;
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
                    GamerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_GamerId",
                        column: x => x.GamerId,
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
                    { new Guid("0021beb8-c4e7-41f1-be58-313d458cff7c"), 23f, (byte)1, null, "13 + 10" },
                    { new Guid("0037ace5-17c6-4370-ad23-cb5e9c87fd9c"), 17f, (byte)1, null, "4 + 13" },
                    { new Guid("0059f165-3577-4484-a5aa-2d85eeec80a0"), 17f, (byte)1, null, "7 + 10" },
                    { new Guid("01349749-f850-44b2-a45c-608a902c2db2"), 9f, (byte)1, null, "7 + 2" },
                    { new Guid("042dcb97-02f7-4aef-af6b-6710ab102295"), 25f, (byte)1, null, "14 + 11" },
                    { new Guid("0491a1b8-0e6e-467b-a378-6624a72a6567"), 13f, (byte)1, null, "8 + 5" },
                    { new Guid("058d7cfe-a078-4af3-9a7c-64b9d560b145"), 21f, (byte)1, null, "10 + 11" },
                    { new Guid("06e02e8b-f5ab-4c5c-a479-2b76ca3262ae"), 15f, (byte)1, null, "3 + 12" },
                    { new Guid("07a01b9d-73e2-4874-8307-4e919925a021"), 23f, (byte)1, null, "15 + 8" },
                    { new Guid("087040a8-a5a2-4b2a-8945-e640d6063105"), 14f, (byte)1, null, "4 + 10" },
                    { new Guid("087cd2bc-c9ed-4477-8a9f-30a978217dc9"), 16f, (byte)1, null, "12 + 4" },
                    { new Guid("0a6c6b21-da6a-4408-8790-ef3093174343"), 23f, (byte)1, null, "11 + 12" },
                    { new Guid("0a9d95c8-95a5-4745-83fa-b8f87ce96c63"), 20f, (byte)1, null, "7 + 13" },
                    { new Guid("0b0beb0c-8376-44e9-ae3e-3cac39e9d54a"), 17f, (byte)1, null, "6 + 11" },
                    { new Guid("0d2ba0a7-ca2e-4284-b6e5-73356e638e6e"), 11f, (byte)1, null, "6 + 5" },
                    { new Guid("0d42663a-9025-426c-bb7a-0719e8ae8725"), 15f, (byte)1, null, "14 + 1" },
                    { new Guid("0d435ecd-781a-4a62-b17e-060aa9e89d52"), 18f, (byte)1, null, "15 + 3" },
                    { new Guid("0f700204-79b2-4bc3-b5c6-238809f34508"), 16f, (byte)1, null, "13 + 3" },
                    { new Guid("14a08b93-5dd4-4638-a9b0-3dd6b44f4280"), 26f, (byte)1, null, "15 + 11" },
                    { new Guid("15c2b7c7-6b0f-4d85-989e-2d9e50cf727d"), 10f, (byte)1, null, "3 + 7" },
                    { new Guid("16b4cab9-ad1a-4237-aa4c-2609d07b63cb"), 6f, (byte)1, null, "5 + 1" },
                    { new Guid("17da0cd7-a9ec-4bf4-aea0-9a221336cd59"), 24f, (byte)1, null, "10 + 14" },
                    { new Guid("17fa71b2-90b5-414d-ae3a-a6cfc9a010e6"), 28f, (byte)1, null, "15 + 13" },
                    { new Guid("19273e26-4493-44b5-997c-5f912caca662"), 19f, (byte)1, null, "7 + 12" },
                    { new Guid("193f1357-a0aa-4f3f-b39d-c94376118758"), 7f, (byte)1, null, "3 + 4" },
                    { new Guid("19539f0d-f1d7-4e27-9986-6b03062f5dc0"), 19f, (byte)1, null, "6 + 13" },
                    { new Guid("1c30107f-aa1b-4392-91d5-7fffb5dd36d8"), 22f, (byte)1, null, "8 + 14" },
                    { new Guid("1ce52f43-1cbf-49b2-82e7-ee7f0d296a56"), 7f, (byte)1, null, "7 + 0" },
                    { new Guid("1db1e2ab-3da9-49ec-aa48-be653baa5a3a"), 28f, (byte)1, null, "14 + 14" },
                    { new Guid("20c1dad8-670d-4920-9872-94944c07320e"), 7f, (byte)1, null, "6 + 1" },
                    { new Guid("2158299c-feb3-4383-9937-413d6bc596c7"), 12f, (byte)1, null, "1 + 11" },
                    { new Guid("2396c93d-900f-46dc-9482-7f77a7481b2c"), 19f, (byte)1, null, "13 + 6" },
                    { new Guid("2436bcff-5075-4bcc-89dd-b53491dc9dc4"), 12f, (byte)1, null, "6 + 6" },
                    { new Guid("2531e970-608a-4b62-99f1-f453f03802c9"), 16f, (byte)1, null, "6 + 10" },
                    { new Guid("25dd1827-06d4-4614-850a-bbdfedb4c8e3"), 22f, (byte)1, null, "14 + 8" },
                    { new Guid("25ea781d-96bb-4cfd-8597-f193e4ed9b67"), 8f, (byte)1, null, "6 + 2" },
                    { new Guid("267e64eb-732d-4544-9567-02ad294fb70a"), 25f, (byte)1, null, "12 + 13" },
                    { new Guid("28cb868d-4d29-4754-8354-cd267ad09955"), 17f, (byte)1, null, "11 + 6" },
                    { new Guid("29db99e4-0200-432f-a9a2-cf518f55ba14"), 18f, (byte)1, null, "14 + 4" },
                    { new Guid("2ad2e26c-ee73-446c-ae8f-7aebfb9d48c5"), 16f, (byte)1, null, "4 + 12" },
                    { new Guid("2bf2e2b4-64a9-4c77-ba25-23017e6f5eb4"), 12f, (byte)1, null, "8 + 4" },
                    { new Guid("2c4baf05-08b9-414f-9c8c-2a37deab9234"), 18f, (byte)1, null, "8 + 10" },
                    { new Guid("2d1fd7ea-095f-4dc7-b424-f19e64904f27"), 8f, (byte)1, null, "3 + 5" },
                    { new Guid("2d60e7ab-139e-4952-a660-c85812de0037"), 20f, (byte)1, null, "11 + 9" },
                    { new Guid("2d9c2396-053f-41b8-89e2-94cea7f88138"), 23f, (byte)1, null, "14 + 9" },
                    { new Guid("2e239d24-eb23-473a-b5c6-4b54be5a36d7"), 13f, (byte)1, null, "2 + 11" },
                    { new Guid("2e7ba478-6739-41a0-8cc0-76628d3c55c1"), 14f, (byte)1, null, "1 + 13" },
                    { new Guid("2f9fa183-7fa9-4252-a48d-19d923e79914"), 17f, (byte)1, null, "10 + 7" },
                    { new Guid("306b5048-e53a-4a5a-87d1-afa215c163ca"), 16f, (byte)1, null, "10 + 6" },
                    { new Guid("33a27247-8691-4988-8584-61a493daf17a"), 19f, (byte)1, null, "9 + 10" },
                    { new Guid("3620f831-6214-4dd3-84be-1eeace1def8d"), 18f, (byte)1, null, "12 + 6" },
                    { new Guid("3645d300-86b9-4c45-9308-3434b6940840"), 25f, (byte)1, null, "11 + 14" },
                    { new Guid("37074c90-7234-4edd-a5cf-e0aad4ade6b8"), 15f, (byte)1, null, "1 + 14" },
                    { new Guid("3bc3c542-c15b-4adf-a8e9-5cc91003a377"), 13f, (byte)1, null, "3 + 10" },
                    { new Guid("3c9a0bae-9839-46b9-84f4-1bc8f250df9b"), 22f, (byte)1, null, "11 + 11" },
                    { new Guid("3e1218a9-df87-4b14-9583-77e305b8d6d8"), 5f, (byte)1, null, "1 + 4" },
                    { new Guid("3f1dd2cf-db48-47fb-b983-4790cf904953"), 14f, (byte)1, null, "3 + 11" },
                    { new Guid("3fd47041-fbc7-4dce-ae08-be5ea90f0b78"), 20f, (byte)1, null, "10 + 10" },
                    { new Guid("401da26e-20c0-4efa-b32b-36e1255c9bd8"), 15f, (byte)1, null, "12 + 3" },
                    { new Guid("420a2b70-6146-450c-9478-dd0cb2dc55c5"), 7f, (byte)1, null, "4 + 3" },
                    { new Guid("4355949e-c8df-47ee-86d9-c2d5911f2e0a"), 20f, (byte)1, null, "8 + 12" },
                    { new Guid("447e1b28-97ad-4dc9-a5aa-688644a50eb2"), 19f, (byte)1, null, "10 + 9" },
                    { new Guid("4595734f-06d6-4788-9e2d-7e6ce1c4a3cf"), 14f, (byte)1, null, "6 + 8" },
                    { new Guid("4707f15f-f49f-4e21-8a1a-3881f8a2a788"), 17f, (byte)1, null, "14 + 3" },
                    { new Guid("472c0c80-44de-4eed-8ee5-adab30f60c78"), 10f, (byte)1, null, "8 + 2" },
                    { new Guid("47ab8fab-393e-4a59-9872-87884e1478c0"), 8f, (byte)1, null, "4 + 4" },
                    { new Guid("48d2fb07-9a21-4045-8851-8475c5042d57"), 10f, (byte)1, null, "1 + 9" },
                    { new Guid("4987de1e-07f6-47b4-a69c-ff712ab13095"), 16f, (byte)1, null, "11 + 5" },
                    { new Guid("4a41ba29-c385-4f5a-81cc-067fa85b9e32"), 15f, (byte)1, null, "13 + 2" },
                    { new Guid("4bf75310-6b14-4236-a889-fe7c1100bfd3"), 15f, (byte)1, null, "8 + 7" },
                    { new Guid("4d73d9c6-c99d-4b3b-b745-66decc2d6bc0"), 8f, (byte)1, null, "2 + 6" },
                    { new Guid("4e00c96b-6702-4a47-bca1-73240e9028e6"), 2f, (byte)1, null, "2 + 0" },
                    { new Guid("4e1aa73e-c008-42e1-bf02-cfb8e86473dd"), 4f, (byte)1, null, "3 + 1" },
                    { new Guid("50f50fcc-293c-4ef4-adc2-d8dc810efd5f"), 21f, (byte)1, null, "9 + 12" },
                    { new Guid("50f57356-9f88-4efd-bd47-21577d69c172"), 10f, (byte)1, null, "2 + 8" },
                    { new Guid("541017f8-2290-4b6f-b0fd-e283694b85bd"), 22f, (byte)1, null, "9 + 13" },
                    { new Guid("54263f95-4176-4c2d-a02b-2bb9ed3c8c07"), 19f, (byte)1, null, "12 + 7" },
                    { new Guid("564d2246-7422-4df7-91b3-23c5dc8385f2"), 13f, (byte)1, null, "10 + 3" },
                    { new Guid("566ab7e0-e417-4b46-8e22-8ed42f20ce7c"), 18f, (byte)1, null, "7 + 11" },
                    { new Guid("5d2f9712-6ad5-4e39-918a-01f8301d075c"), 10f, (byte)1, null, "6 + 4" },
                    { new Guid("5e03a606-426a-469c-8989-e1b06a357ee5"), 17f, (byte)1, null, "8 + 9" },
                    { new Guid("5eb4b6bf-8c08-4887-bf95-6e362e4e6742"), 15f, (byte)1, null, "15 + 0" },
                    { new Guid("5edbad34-a9bd-46e2-ab69-034928a88bbf"), 10f, (byte)1, null, "7 + 3" },
                    { new Guid("5f089b9a-ce5c-4ac0-a79b-f403f283ebbf"), 15f, (byte)1, null, "7 + 8" },
                    { new Guid("5f59d4b7-ae3b-4ae1-aee8-e7f223bffde2"), 12f, (byte)1, null, "3 + 9" },
                    { new Guid("601b105a-5305-4e2b-adf0-ba674ee59f22"), 21f, (byte)1, null, "15 + 6" },
                    { new Guid("632e90ca-ff86-495c-86a8-532218786a85"), 24f, (byte)1, null, "11 + 13" },
                    { new Guid("65a088f5-97af-4dbe-85da-61fde96a7505"), 11f, (byte)1, null, "7 + 4" },
                    { new Guid("6638699e-12ce-433b-a37b-be3cea54ea52"), 27f, (byte)1, null, "14 + 13" },
                    { new Guid("6724c657-38d2-483f-bcc5-aead8b84f72c"), 22f, (byte)1, null, "10 + 12" },
                    { new Guid("68023a36-582a-4b61-ac7e-c8866904b35f"), 16f, (byte)1, null, "8 + 8" },
                    { new Guid("6affb8de-7809-4b84-a851-120f071c413a"), 21f, (byte)1, null, "12 + 9" },
                    { new Guid("6b27a3c8-e635-43ee-871a-a5c849b13912"), 6f, (byte)1, null, "3 + 3" },
                    { new Guid("6ceed409-7789-4e20-a70e-a47005c084b0"), 9f, (byte)1, null, "4 + 5" },
                    { new Guid("6defa922-78b9-4544-8888-a6c89c11a5e5"), 17f, (byte)1, null, "5 + 12" },
                    { new Guid("6e722dad-8d36-42fc-b1cf-473be9200601"), 12f, (byte)1, null, "4 + 8" },
                    { new Guid("6e87e437-35b3-4c66-9937-56080fd93662"), 12f, (byte)1, null, "5 + 7" },
                    { new Guid("70ab0c12-7623-468d-9eca-6615f52bcc0e"), 21f, (byte)1, null, "11 + 10" },
                    { new Guid("729edc49-9999-4ff4-94dc-90b396ff8317"), 4f, (byte)1, null, "2 + 2" },
                    { new Guid("72b458b2-8105-432f-aebf-f601e0dbd883"), 23f, (byte)1, null, "12 + 11" },
                    { new Guid("74dda721-a63d-4e92-a079-5e3040e096a7"), 14f, (byte)1, null, "14 + 0" },
                    { new Guid("74e7d6c6-7b5e-49bd-b6de-6d9d3d604bfe"), 3f, (byte)1, null, "2 + 1" },
                    { new Guid("74f439bf-805d-473e-96a5-adfa87776c51"), 8f, (byte)1, null, "5 + 3" },
                    { new Guid("7641098a-71fe-46e9-b2dc-c0cd05b8fdf5"), 18f, (byte)1, null, "5 + 13" },
                    { new Guid("76946252-ca24-452f-9d92-c00c3a819b4e"), 21f, (byte)1, null, "14 + 7" },
                    { new Guid("77136477-f091-4d1b-9f24-2514fa97b9cd"), 15f, (byte)1, null, "2 + 13" },
                    { new Guid("77144a7b-89d4-414c-9c35-a0630d5e3cdf"), 11f, (byte)1, null, "8 + 3" },
                    { new Guid("7bfbae98-b91c-489c-bdd5-b217846032ce"), 9f, (byte)1, null, "3 + 6" },
                    { new Guid("7c109874-62e0-499a-886f-e319318e93ca"), 10f, (byte)1, null, "4 + 6" },
                    { new Guid("7c57bc85-f903-4c6b-b434-7c60182c9ffc"), 15f, (byte)1, null, "6 + 9" },
                    { new Guid("7ee7f199-0b71-4506-8f1d-6f6017e2618d"), 10f, (byte)1, null, "9 + 1" },
                    { new Guid("7f10596f-9796-4fac-b011-025241c5d4d3"), 5f, (byte)1, null, "4 + 1" },
                    { new Guid("80317c39-08e0-4c61-a399-2b6b56d2d62e"), 16f, (byte)1, null, "7 + 9" },
                    { new Guid("8489cb1f-8d2b-42d4-a69c-e096ad398ddf"), 11f, (byte)1, null, "3 + 8" },
                    { new Guid("8804ac63-ed0b-4c22-af40-781e00d09162"), 22f, (byte)1, null, "13 + 9" },
                    { new Guid("8a1c0162-7371-428b-9771-fca9baf0ca0b"), 3f, (byte)1, null, "3 + 0" },
                    { new Guid("8b5cdbfc-c19b-4ed9-b280-ce92ef3ffbf4"), 15f, (byte)1, null, "5 + 10" },
                    { new Guid("8b75116b-6c10-437b-9cdb-066f5560175d"), 1f, (byte)1, null, "1 + 0" },
                    { new Guid("8c8ae444-a57b-4439-a947-a3c8ec55be41"), 6f, (byte)1, null, "1 + 5" },
                    { new Guid("8ccd71a9-d818-4b45-9457-05fe02231b1e"), 12f, (byte)1, null, "9 + 3" },
                    { new Guid("8e2a8767-cb38-4207-a36d-0eda5f3f5b85"), 12f, (byte)1, null, "2 + 10" },
                    { new Guid("8eefd17a-d2e4-4a11-9b8b-14c0191ad4bb"), 24f, (byte)1, null, "14 + 10" },
                    { new Guid("8f543f95-018d-4b36-85d7-e1a933a74b36"), 7f, (byte)1, null, "5 + 2" },
                    { new Guid("8f89201e-ed9b-4e8e-8764-a080c4bd8618"), 17f, (byte)1, null, "15 + 2" },
                    { new Guid("906a9da9-3808-40fa-8dde-3ebcfc66aa24"), 16f, (byte)1, null, "3 + 13" },
                    { new Guid("90f69b80-744d-4021-a15d-125c2dd92baa"), 17f, (byte)1, null, "12 + 5" },
                    { new Guid("91b63222-e0c5-482b-957b-b49a556e50f3"), 23f, (byte)1, null, "10 + 13" },
                    { new Guid("92351484-0f10-4b75-8d9d-852664f7ecc1"), 8f, (byte)1, null, "1 + 7" },
                    { new Guid("9268052c-f4bc-47a5-9abf-5b7146ec50ff"), 13f, (byte)1, null, "7 + 6" },
                    { new Guid("92c319e1-dae5-4429-91b6-ffea5b8d5b81"), 13f, (byte)1, null, "1 + 12" },
                    { new Guid("93e73140-daf0-47b5-94a6-afab55464b89"), 13f, (byte)1, null, "6 + 7" },
                    { new Guid("95feb5cf-e8c6-4caf-b3a3-c56e0890a0bb"), 9f, (byte)1, null, "1 + 8" },
                    { new Guid("9742cfac-7bb4-45de-8658-6df58cab0e16"), 15f, (byte)1, null, "4 + 11" },
                    { new Guid("9765ae07-4b5a-437d-bb03-9983abecd135"), 11f, (byte)1, null, "10 + 1" },
                    { new Guid("98a15c99-a864-4fb8-8730-8aeecf28341e"), 4f, (byte)1, null, "4 + 0" },
                    { new Guid("9a396456-c1b4-4a1f-b582-272e974b2d01"), 20f, (byte)1, null, "15 + 5" },
                    { new Guid("9c2d7d45-4dff-4f28-9819-9f8486fd8b69"), 17f, (byte)1, null, "13 + 4" },
                    { new Guid("9c519769-b6a5-49a5-aad8-c95acbbf5cb4"), 13f, (byte)1, null, "5 + 8" },
                    { new Guid("9dec7b32-a30b-47d1-99ff-b02afb9d2443"), 8f, (byte)1, null, "8 + 0" },
                    { new Guid("9f3a9956-67b5-482b-8926-8bebfd20ff08"), 21f, (byte)1, null, "13 + 8" },
                    { new Guid("a04f45ea-6a34-499b-97d8-9b878d1c1206"), 5f, (byte)1, null, "5 + 0" },
                    { new Guid("a1022083-fc89-4d6b-8fbc-345e59925be7"), 11f, (byte)1, null, "9 + 2" },
                    { new Guid("a1602f50-4bf3-4ee2-87a2-4cfc020b066f"), 7f, (byte)1, null, "2 + 5" },
                    { new Guid("a161510b-d528-4a07-81f7-56928c34df6f"), 20f, (byte)1, null, "12 + 8" },
                    { new Guid("a19c509f-6f84-4640-9980-b1bfcef4c257"), 18f, (byte)1, null, "10 + 8" },
                    { new Guid("a20a115c-ab86-42cb-a1cd-41122bcceb53"), 25f, (byte)1, null, "13 + 12" },
                    { new Guid("a31d0b69-162f-4578-863e-ade474145350"), 7f, (byte)1, null, "1 + 6" },
                    { new Guid("a5235bd1-2f89-46f4-af2c-56fd17cdb8c8"), 15f, (byte)1, null, "10 + 5" },
                    { new Guid("a5763413-3202-4654-9861-2ae34ee10027"), 16f, (byte)1, null, "15 + 1" },
                    { new Guid("a59ccd4d-634f-4ae4-a90f-4a4995905bfc"), 20f, (byte)1, null, "6 + 14" },
                    { new Guid("a6c3a693-6391-43f8-b116-b038a5c8b4b1"), 16f, (byte)1, null, "2 + 14" },
                    { new Guid("a7ba5998-c1be-44f4-a9b7-55d38d1a50c3"), 24f, (byte)1, null, "15 + 9" },
                    { new Guid("a7c5b01a-3e66-41f3-8d28-56d7ef1fe7c2"), 27f, (byte)1, null, "13 + 14" },
                    { new Guid("a85042a0-3bd7-4941-9caa-8afe76ddbe1c"), 16f, (byte)1, null, "14 + 2" },
                    { new Guid("acd3ced1-7943-48d5-851c-128a2c0b2c93"), 9f, (byte)1, null, "6 + 3" },
                    { new Guid("acf9944a-3b67-450d-983c-f753fa362944"), 10f, (byte)1, null, "10 + 0" },
                    { new Guid("ad3f542b-9286-4eea-9a70-658438eadd31"), 12f, (byte)1, null, "11 + 1" },
                    { new Guid("addf8924-f179-4872-82e1-2de05f0194ab"), 12f, (byte)1, null, "7 + 5" },
                    { new Guid("af2376b6-be99-4768-b563-e7b146956e1e"), 19f, (byte)1, null, "15 + 4" },
                    { new Guid("afa665e3-5a71-42b0-b66c-8c3059885be7"), 19f, (byte)1, null, "11 + 8" },
                    { new Guid("b0563f25-58e1-4748-9bb8-5faeb08b5bd2"), 10f, (byte)1, null, "5 + 5" },
                    { new Guid("b0e4018e-cad6-4d55-8975-34d4ed824241"), 2f, (byte)1, null, "1 + 1" },
                    { new Guid("b1119314-e838-489b-b062-101cdf74c5d8"), 5f, (byte)1, null, "2 + 3" },
                    { new Guid("b45b1c16-940b-4645-a2a6-a6718d9d6579"), 18f, (byte)1, null, "9 + 9" },
                    { new Guid("b8634df0-f30d-41ff-92eb-a21fefeb50bf"), 18f, (byte)1, null, "6 + 12" },
                    { new Guid("ba158bfd-7f1f-41bc-8389-6bd83dbce631"), 8f, (byte)1, null, "7 + 1" },
                    { new Guid("bbfdd738-6e48-4286-990f-d62f124ed9ec"), 13f, (byte)1, null, "12 + 1" },
                    { new Guid("bc1bbc50-6e94-4113-ac83-873426c54cd5"), 14f, (byte)1, null, "10 + 4" },
                    { new Guid("bc5e72e9-6b1d-4610-a97b-6cda7fcfc8af"), 20f, (byte)1, null, "9 + 11" },
                    { new Guid("bff8533a-112f-4b3a-9c53-0374241d2775"), 19f, (byte)1, null, "14 + 5" },
                    { new Guid("c0378efd-fb69-4b89-9f81-fe36dc49ca2b"), 13f, (byte)1, null, "13 + 0" },
                    { new Guid("c04ee8c3-b4e6-4a3a-99a4-0f7ed2decc01"), 5f, (byte)1, null, "3 + 2" },
                    { new Guid("c0f6acc6-704c-4d9c-88ac-2933b1b784fb"), 24f, (byte)1, null, "13 + 11" },
                    { new Guid("c1fb905b-f4dc-4518-a13b-45e180f0d278"), 16f, (byte)1, null, "5 + 11" },
                    { new Guid("c319d845-55c7-4241-b871-cbeb2fad02b7"), 9f, (byte)1, null, "5 + 4" },
                    { new Guid("c31b1888-c044-4eb1-957c-61548eeaf3c9"), 6f, (byte)1, null, "6 + 0" },
                    { new Guid("c324cc6d-1686-41f5-a552-1f68830189d3"), 21f, (byte)1, null, "7 + 14" },
                    { new Guid("c369bcaa-6479-44eb-a0d5-59a745e7c64a"), 22f, (byte)1, null, "12 + 10" },
                    { new Guid("c44e6dd1-1e30-412a-bc4c-9efe31633961"), 22f, (byte)1, null, "15 + 7" },
                    { new Guid("c4752c88-cfcc-496a-ae0a-0418940b7a5a"), 11f, (byte)1, null, "5 + 6" },
                    { new Guid("c71ff243-5eb7-4239-842b-0c319124b739"), 6f, (byte)1, null, "2 + 4" },
                    { new Guid("ce3bd767-4686-478f-a87d-f22be36b294c"), 18f, (byte)1, null, "13 + 5" },
                    { new Guid("cfd13b14-fd8a-46e6-9c1c-709d7ba561a1"), 13f, (byte)1, null, "4 + 9" },
                    { new Guid("d0da7235-f77f-48cb-8a81-26c569dab8f8"), 13f, (byte)1, null, "11 + 2" },
                    { new Guid("d169aaff-34a4-4f10-80b7-c3aa16193b3d"), 26f, (byte)1, null, "13 + 13" },
                    { new Guid("d530a539-077c-4009-a9b7-fed6d4849259"), 21f, (byte)1, null, "8 + 13" },
                    { new Guid("d7552674-6d7d-4959-8ff9-e188ea269ed8"), 14f, (byte)1, null, "7 + 7" },
                    { new Guid("d8f40887-a923-45d0-8325-aaedd9145ce2"), 14f, (byte)1, null, "12 + 2" },
                    { new Guid("d98f5919-788e-4722-95db-9a3294c6ffa7"), 16f, (byte)1, null, "9 + 7" },
                    { new Guid("db2c2f28-2633-4467-949a-0be2d5f55fb6"), 13f, (byte)1, null, "9 + 4" },
                    { new Guid("dc3bfba6-f965-4253-b54d-0cb20ca5dce2"), 11f, (byte)1, null, "2 + 9" },
                    { new Guid("de5e9b50-2113-4a2e-b66f-47623795c7b4"), 20f, (byte)1, null, "13 + 7" },
                    { new Guid("df7fb328-b82d-4cb2-a0f0-d315285cfe54"), 19f, (byte)1, null, "5 + 14" },
                    { new Guid("e165f2ab-2b8e-4ae0-9ea3-a021ce7de936"), 15f, (byte)1, null, "9 + 6" },
                    { new Guid("e339e41a-6691-4f0b-aff1-9a5d2655aa65"), 12f, (byte)1, null, "10 + 2" },
                    { new Guid("e4189b9d-28eb-42da-81fd-736936f61b49"), 14f, (byte)1, null, "8 + 6" },
                    { new Guid("e6e7dd42-90d8-4a05-8bbd-c75a7e3184d5"), 18f, (byte)1, null, "4 + 14" },
                    { new Guid("e7acd0d8-45fc-4b88-ac5b-4420214f435d"), 11f, (byte)1, null, "11 + 0" },
                    { new Guid("e9263a81-3c75-4887-a092-ad9393b95de9"), 4f, (byte)1, null, "1 + 3" },
                    { new Guid("ea4274ff-e4f2-494f-95de-33f3c10e01e1"), 18f, (byte)1, null, "11 + 7" },
                    { new Guid("eb7e4478-6d63-45fa-9501-8c7d1bab0dab"), 26f, (byte)1, null, "14 + 12" },
                    { new Guid("ee5e7b3d-bb22-4c43-91ff-6f79d02903e1"), 24f, (byte)1, null, "12 + 12" },
                    { new Guid("ef0e398e-7c1d-4c41-be6f-cb8c63f783c2"), 20f, (byte)1, null, "14 + 6" },
                    { new Guid("effac817-0d95-4b8a-8db0-d64aed6bd794"), 14f, (byte)1, null, "11 + 3" },
                    { new Guid("f05fd6a0-669d-4fcc-877b-b3d020149bb7"), 11f, (byte)1, null, "4 + 7" },
                    { new Guid("f394a431-d4a4-4d4c-846d-1245e6fdc947"), 23f, (byte)1, null, "9 + 14" },
                    { new Guid("f3d2e63c-82a3-4443-a400-3389bb6013ba"), 27f, (byte)1, null, "15 + 12" },
                    { new Guid("f3fc19c3-6c4a-441f-9870-71a244d912c6"), 14f, (byte)1, null, "5 + 9" },
                    { new Guid("f4bc026c-3a37-4618-9630-ec4385dd7227"), 11f, (byte)1, null, "1 + 10" },
                    { new Guid("f4e95a7d-0baa-4f16-8956-a5a03845c188"), 25f, (byte)1, null, "15 + 10" },
                    { new Guid("f6d804c3-c637-4ec5-bc40-90abd71141a7"), 15f, (byte)1, null, "11 + 4" },
                    { new Guid("f73a1309-6dd5-45c8-997e-a9551307fce2"), 26f, (byte)1, null, "12 + 14" },
                    { new Guid("f75cd25d-f671-4110-9184-d2a46b98dbc4"), 6f, (byte)1, null, "4 + 2" },
                    { new Guid("f7e2fa54-bef2-4e94-b545-eb889e579ded"), 9f, (byte)1, null, "9 + 0" },
                    { new Guid("f8792046-d2ff-4e8a-a99c-778595877a76"), 19f, (byte)1, null, "8 + 11" },
                    { new Guid("f88531a1-d8f9-4604-8069-cc15554e5136"), 29f, (byte)1, null, "15 + 14" },
                    { new Guid("fac944f5-ca13-481f-8792-8cdc6689d022"), 17f, (byte)1, null, "3 + 14" },
                    { new Guid("fb25ca05-e3ea-4836-b703-eb632300ddfa"), 14f, (byte)1, null, "9 + 5" },
                    { new Guid("fb52580f-0389-4f4c-bdcd-b70d1f0ff9d6"), 3f, (byte)1, null, "1 + 2" },
                    { new Guid("fcf6a5c5-4579-4026-8308-9a54c09a19fc"), 9f, (byte)1, null, "2 + 7" },
                    { new Guid("fd079104-426b-4fa9-9f1f-e0caae940cb9"), 12f, (byte)1, null, "12 + 0" },
                    { new Guid("fd63dfde-4278-4246-8890-729368fc0234"), 17f, (byte)1, null, "9 + 8" },
                    { new Guid("fd703835-4333-42df-bc12-fdddfefd899d"), 14f, (byte)1, null, "2 + 12" },
                    { new Guid("fe74be95-714c-4929-9c25-2d3d7d9f2d30"), 14f, (byte)1, null, "13 + 1" },
                    { new Guid("ff6be3e3-c87a-487f-8c63-d25c33aec844"), 9f, (byte)1, null, "8 + 1" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LastLogin", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("074e31b0-034c-4152-b4f8-d87561fccd27"), "matas@example.com", new DateTime(2024, 9, 29, 15, 15, 41, 575, DateTimeKind.Local).AddTicks(9780), "yEzEocUc0nSqIwt38hAx8x4PffSEIAhyUNpivHuM7RYN0you3SfpsdDe6kSLOiiE", "Matas" },
                    { new Guid("4fdb1c13-cb50-4f5b-817d-bfa1585dfb3a"), "robbe@example.com", new DateTime(2024, 9, 29, 15, 15, 41, 543, DateTimeKind.Local).AddTicks(3210), "FaH4gMkcI9AMxXvA9fvk0RMWw+DgQFd/TusEC1fFCGJttAs/LW38LmSlJvjEQlZ3", "Robbe" },
                    { new Guid("ad58bdfa-08ef-4d5f-9586-db38aa760b29"), "david@example.com", new DateTime(2024, 9, 29, 15, 15, 41, 526, DateTimeKind.Local).AddTicks(9330), "hDsfwOAXof+bpLnjwmUOmlrv+S/Pyj2sNBqw/A8MigqL8vDegwzFYAQ1XwaDIysn", "David" },
                    { new Guid("caaf7590-48f7-4f28-8946-93523d75b4b3"), "valdemar@example.com", new DateTime(2024, 9, 29, 15, 15, 41, 559, DateTimeKind.Local).AddTicks(5120), "4avSMEsq5v8nfYUOXZAH+r3pOc5m0tm4KXMOpKGTFN3xTNP/8muv4NsW5YFKdPTz", "Valdemar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GamerId",
                table: "Games",
                column: "GamerId");

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
