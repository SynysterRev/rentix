using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rentix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactoEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Charge_Property_PropertyId",
                table: "Charge");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Property_PropertyId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Lease_Property_PropertyId",
                table: "Lease");

            migrationBuilder.DropForeignKey(
                name: "FK_lease_tenant_Lease_LeasesId",
                table: "lease_tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_lease_tenant_Tenant_TenantsId",
                table: "lease_tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Address_AddressId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_AspNetUsers_UserId",
                table: "Property");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenant",
                table: "Tenant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Property",
                table: "Property");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lease",
                table: "Lease");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                table: "Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Charge",
                table: "Charge");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.RenameTable(
                name: "Tenant",
                newName: "Tenants");

            migrationBuilder.RenameTable(
                name: "Property",
                newName: "Properties");

            migrationBuilder.RenameTable(
                name: "Lease",
                newName: "Leases");

            migrationBuilder.RenameTable(
                name: "Document",
                newName: "Documents");

            migrationBuilder.RenameTable(
                name: "Charge",
                newName: "Charges");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Properties",
                newName: "LandlordId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_UserId",
                table: "Properties",
                newName: "IX_Properties_LandlordId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_AddressId",
                table: "Properties",
                newName: "IX_Properties_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Lease_PropertyId",
                table: "Leases",
                newName: "IX_Leases_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Document_PropertyId",
                table: "Documents",
                newName: "IX_Documents_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Charge_PropertyId",
                table: "Charges",
                newName: "IX_Charges_PropertyId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadAt",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leases",
                table: "Leases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                table: "Documents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Charges",
                table: "Charges",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Charges_Properties_PropertyId",
                table: "Charges",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Properties_PropertyId",
                table: "Documents",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_lease_tenant_Leases_LeasesId",
                table: "lease_tenant",
                column: "LeasesId",
                principalTable: "Leases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lease_tenant_Tenants_TenantsId",
                table: "lease_tenant",
                column: "TenantsId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Properties_PropertyId",
                table: "Leases",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_LandlordId",
                table: "Properties",
                column: "LandlordId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Charges_Properties_PropertyId",
                table: "Charges");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Properties_PropertyId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_lease_tenant_Leases_LeasesId",
                table: "lease_tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_lease_tenant_Tenants_TenantsId",
                table: "lease_tenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Properties_PropertyId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Addresses_AddressId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_LandlordId",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leases",
                table: "Leases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Charges",
                table: "Charges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Tenants",
                newName: "Tenant");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "Property");

            migrationBuilder.RenameTable(
                name: "Leases",
                newName: "Lease");

            migrationBuilder.RenameTable(
                name: "Documents",
                newName: "Document");

            migrationBuilder.RenameTable(
                name: "Charges",
                newName: "Charge");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "LandlordId",
                table: "Property",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_LandlordId",
                table: "Property",
                newName: "IX_Property_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_AddressId",
                table: "Property",
                newName: "IX_Property_AddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_PropertyId",
                table: "Lease",
                newName: "IX_Lease_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_PropertyId",
                table: "Document",
                newName: "IX_Document_PropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_Charges_PropertyId",
                table: "Charge",
                newName: "IX_Charge_PropertyId");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UploadAt",
                table: "Document",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenant",
                table: "Tenant",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Property",
                table: "Property",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lease",
                table: "Lease",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                table: "Document",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Charge",
                table: "Charge",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Address_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Charge_Property_PropertyId",
                table: "Charge",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Property_PropertyId",
                table: "Document",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lease_Property_PropertyId",
                table: "Lease",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_lease_tenant_Lease_LeasesId",
                table: "lease_tenant",
                column: "LeasesId",
                principalTable: "Lease",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lease_tenant_Tenant_TenantsId",
                table: "lease_tenant",
                column: "TenantsId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Address_AddressId",
                table: "Property",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_AspNetUsers_UserId",
                table: "Property",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
