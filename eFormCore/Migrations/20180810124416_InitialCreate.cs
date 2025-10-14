/*
The MIT License (MIT)

Copyright (c) 2007 - 2025 Microting A/S

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.eForm.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Setup for SQL Server Provider

            string autoIDGenStrategy = "SqlServer:ValueGenerationStrategy";
            object autoIDGenStrategyValue= MySqlValueGenerationStrategy.IdentityColumn;

            // Setup for MySQL Provider
            if (migrationBuilder.ActiveProvider=="Pomelo.EntityFrameworkCore.MySql")
            {
                DbConfig.IsMySQL = true;
                autoIDGenStrategy = "MySql:ValueGenerationStrategy";
                autoIDGenStrategyValue = MySqlValueGenerationStrategy.IdentityColumn;
            }

            migrationBuilder.CreateTable(
                name: "case_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    case_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    status = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    done_at = table.Column<DateTime>(nullable: true),
                    site_id = table.Column<int>(nullable: true),
                    unit_id = table.Column<int>(nullable: true),
                    done_by_user_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    type = table.Column<string>(maxLength: 255, nullable: true),
                    microting_uid = table.Column<string>(maxLength: 255, nullable: true),
                    microting_check_uid = table.Column<string>(maxLength: 255, nullable: true),
                    case_uid = table.Column<string>(maxLength: 255, nullable: true),
                    custom = table.Column<string>(nullable: true),
                    field_value_1 = table.Column<string>(nullable: true),
                    field_value_2 = table.Column<string>(nullable: true),
                    field_value_3 = table.Column<string>(nullable: true),
                    field_value_4 = table.Column<string>(nullable: true),
                    field_value_5 = table.Column<string>(nullable: true),
                    field_value_6 = table.Column<string>(nullable: true),
                    field_value_7 = table.Column<string>(nullable: true),
                    field_value_8 = table.Column<string>(nullable: true),
                    field_value_9 = table.Column<string>(nullable: true),
                    field_value_10 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_case_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_site_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    check_list_site_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    site_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    microting_uid = table.Column<string>(maxLength: 255, nullable: true),
                    last_check_id = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_site_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_value_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    check_list_value_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    status = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    user_id = table.Column<int>(nullable: true),
                    case_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    check_list_duplicate_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_value_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_values",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    status = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    user_id = table.Column<int>(nullable: true),
                    case_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    check_list_duplicate_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_values", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "check_list_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    check_list_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    custom = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: true),
                    repeated = table.Column<int>(nullable: true),
                    display_index = table.Column<int>(nullable: true),
                    case_type = table.Column<string>(maxLength: 255, nullable: true),
                    folder_name = table.Column<string>(maxLength: 255, nullable: true),
                    review_enabled = table.Column<short>(nullable: true),
                    manual_sync = table.Column<short>(nullable: true),
                    extra_fields_enabled = table.Column<short>(nullable: true),
                    done_button_enabled = table.Column<short>(nullable: true),
                    approval_enabled = table.Column<short>(nullable: true),
                    multi_approval = table.Column<short>(nullable: true),
                    fast_navigation = table.Column<short>(nullable: true),
                    download_entities = table.Column<short>(nullable: true),
                    field_1 = table.Column<int>(nullable: true),
                    field_2 = table.Column<int>(nullable: true),
                    field_3 = table.Column<int>(nullable: true),
                    field_4 = table.Column<int>(nullable: true),
                    field_5 = table.Column<int>(nullable: true),
                    field_6 = table.Column<int>(nullable: true),
                    field_7 = table.Column<int>(nullable: true),
                    field_8 = table.Column<int>(nullable: true),
                    field_9 = table.Column<int>(nullable: true),
                    field_10 = table.Column<int>(nullable: true),
                    quick_sync_enabled = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entity_group_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    entity_group_id = table.Column<int>(nullable: false),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    microting_uid = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_group_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entity_groups",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    microting_uid = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    type = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entity_item_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    entity_items_id = table.Column<int>(nullable: false),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    entity_group_id = table.Column<int>(nullable: true),
                    entity_item_uid = table.Column<string>(maxLength: 50, nullable: true),
                    microting_uid = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    synced = table.Column<short>(nullable: true),
                    //migrated_entity_group_id = table.Column<short>(nullable: true),
                    display_index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_item_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "entity_items",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    entity_group_id = table.Column<int>(nullable: true),
                    entity_item_uid = table.Column<string>(maxLength: 50, nullable: true),
                    microting_uid = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    synced = table.Column<short>(nullable: true),
                    //migrated_entity_group_id = table.Column<short>(nullable: true),
                    display_index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entity_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "field_types",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    field_type = table.Column<string>(maxLength: 255, nullable: true),
                    description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "field_value_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    field_value_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    done_at = table.Column<DateTime>(nullable: true),
                    date = table.Column<DateTime>(nullable: true),
                    user_id = table.Column<int>(nullable: true),
                    case_id = table.Column<int>(nullable: true),
                    field_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    check_list_duplicate_id = table.Column<int>(nullable: true),
                    uploaded_data_id = table.Column<int>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    latitude = table.Column<string>(maxLength: 255, nullable: true),
                    longitude = table.Column<string>(maxLength: 255, nullable: true),
                    altitude = table.Column<string>(maxLength: 255, nullable: true),
                    heading = table.Column<string>(maxLength: 255, nullable: true),
                    accuracy = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_value_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "field_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    field_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    parent_field_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    field_type_id = table.Column<int>(nullable: true),
                    mandatory = table.Column<short>(nullable: true),
                    read_only = table.Column<short>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    color = table.Column<string>(maxLength: 255, nullable: true),
                    display_index = table.Column<int>(nullable: true),
                    dummy = table.Column<short>(nullable: true),
                    default_value = table.Column<string>(nullable: true),
                    unit_name = table.Column<string>(maxLength: 255, nullable: true),
                    min_value = table.Column<string>(maxLength: 255, nullable: true),
                    max_value = table.Column<string>(maxLength: 255, nullable: true),
                    max_length = table.Column<int>(nullable: true),
                    decimal_count = table.Column<int>(nullable: true),
                    multi = table.Column<int>(nullable: true),
                    optional = table.Column<short>(nullable: true),
                    selected = table.Column<short>(nullable: true),
                    split_screen = table.Column<short>(nullable: true),
                    geolocation_enabled = table.Column<short>(nullable: true),
                    geolocation_forced = table.Column<short>(nullable: true),
                    geolocation_hidden = table.Column<short>(nullable: true),
                    stop_on_save = table.Column<short>(nullable: true),
                    is_num = table.Column<short>(nullable: true),
                    barcode_enabled = table.Column<short>(nullable: true),
                    barcode_type = table.Column<string>(maxLength: 255, nullable: true),
                    query_type = table.Column<string>(maxLength: 255, nullable: true),
                    key_value_pair_list = table.Column<string>(nullable: true),
                    custom = table.Column<string>(nullable: true),
                    entity_group_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "log_exceptions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: false),
                    level = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_exceptions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: false),
                    level = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    microting_uid = table.Column<string>(maxLength: 255, nullable: true),
                    transmission = table.Column<string>(nullable: true),
                    notification_uid = table.Column<string>(maxLength: 255, nullable: true),
                    activity = table.Column<string>(nullable: true),
                    exception = table.Column<string>(nullable: true),
                    stacktrace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    name = table.Column<string>(maxLength: 50, nullable: false),
                    value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true),
                    microting_uid = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    site_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_worker_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    site_id = table.Column<int>(nullable: true),
                    worker_id = table.Column<int>(nullable: true),
                    microting_uid = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    site_worker_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_worker_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sites",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true),
                    microting_uid = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sites", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tag_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true),
                    taggings_count = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    tag_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tagging_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    tag_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    tagger_id = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    tagging_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tagging_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    name = table.Column<string>(maxLength: 255, nullable: true),
                    taggings_count = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unit_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    microting_uid = table.Column<int>(nullable: true),
                    otp_code = table.Column<int>(nullable: true),
                    customer_no = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    unit_id = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    site_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_unit_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_data",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    uploader_id = table.Column<int>(nullable: true),
                    checksum = table.Column<string>(maxLength: 255, nullable: true),
                    extension = table.Column<string>(maxLength: 255, nullable: true),
                    current_file = table.Column<string>(maxLength: 255, nullable: true),
                    uploader_type = table.Column<string>(maxLength: 255, nullable: true),
                    file_location = table.Column<string>(maxLength: 255, nullable: true),
                    file_name = table.Column<string>(maxLength: 255, nullable: true),
                    expiration_date = table.Column<DateTime>(nullable: true),
                    local = table.Column<short>(nullable: true),
                    transcription_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_data", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "uploaded_data_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    data_uploaded_id = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    uploader_id = table.Column<int>(nullable: true),
                    checksum = table.Column<string>(maxLength: 255, nullable: true),
                    extension = table.Column<string>(maxLength: 255, nullable: true),
                    current_file = table.Column<string>(maxLength: 255, nullable: true),
                    uploader_type = table.Column<string>(maxLength: 255, nullable: true),
                    file_location = table.Column<string>(maxLength: 255, nullable: true),
                    file_name = table.Column<string>(maxLength: 255, nullable: true),
                    expiration_date = table.Column<DateTime>(nullable: true),
                    local = table.Column<short>(nullable: true),
                    transcription_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_uploaded_data_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "worker_versions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    microting_uid = table.Column<int>(nullable: false),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    first_name = table.Column<string>(maxLength: 255, nullable: true),
                    last_name = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    worker_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "workers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    microting_uid = table.Column<int>(nullable: false),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    first_name = table.Column<string>(maxLength: 255, nullable: true),
                    last_name = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "units",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    microting_uid = table.Column<int>(nullable: true),
                    otp_code = table.Column<int>(nullable: true),
                    customer_no = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    site_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_units", x => x.id);
                    table.ForeignKey(
                        name: "FK_units_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "check_lists",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    custom = table.Column<string>(nullable: true),
                    parent_id = table.Column<int>(nullable: true),
                    repeated = table.Column<int>(nullable: true),
                    display_index = table.Column<int>(nullable: true),
                    case_type = table.Column<string>(maxLength: 255, nullable: true),
                    folder_name = table.Column<string>(maxLength: 255, nullable: true),
                    review_enabled = table.Column<short>(nullable: true),
                    manual_sync = table.Column<short>(nullable: true),
                    extra_fields_enabled = table.Column<short>(nullable: true),
                    done_button_enabled = table.Column<short>(nullable: true),
                    approval_enabled = table.Column<short>(nullable: true),
                    multi_approval = table.Column<short>(nullable: true),
                    fast_navigation = table.Column<short>(nullable: true),
                    download_entities = table.Column<short>(nullable: true),
                    field_1 = table.Column<int>(nullable: true),
                    field_2 = table.Column<int>(nullable: true),
                    field_3 = table.Column<int>(nullable: true),
                    field_4 = table.Column<int>(nullable: true),
                    field_5 = table.Column<int>(nullable: true),
                    field_6 = table.Column<int>(nullable: true),
                    field_7 = table.Column<int>(nullable: true),
                    field_8 = table.Column<int>(nullable: true),
                    field_9 = table.Column<int>(nullable: true),
                    field_10 = table.Column<int>(nullable: true),
                    quick_sync_enabled = table.Column<short>(nullable: true),
                    parentid = table.Column<int>(nullable: true)
                    //tagsid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_lists", x => x.id);
                    table.ForeignKey(
                        name: "FK_check_lists_check_lists_parentid",
                        column: x => x.parentid,
                        principalTable: "check_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_check_lists_tags_tagsid",
                    //    column: x => x.tagsid,
                    //    principalTable: "tags",
                    //    principalColumn: "id",
                    //    onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "site_workers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    site_id = table.Column<int>(nullable: true),
                    worker_id = table.Column<int>(nullable: true),
                    microting_uid = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_workers", x => x.id);
                    table.ForeignKey(
                        name: "FK_site_workers_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_site_workers_workers_worker_id",
                        column: x => x.worker_id,
                        principalTable: "workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cases",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    status = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    done_at = table.Column<DateTime>(nullable: true),
                    site_id = table.Column<int>(nullable: true),
                    unit_id = table.Column<int>(nullable: true),
                    done_by_user_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    type = table.Column<string>(maxLength: 255, nullable: true),
                    microting_uid = table.Column<string>(maxLength: 255, nullable: true),
                    microting_check_uid = table.Column<string>(maxLength: 255, nullable: true),
                    case_uid = table.Column<string>(maxLength: 255, nullable: true),
                    custom = table.Column<string>(nullable: true),
                    field_value_1 = table.Column<string>(nullable: true),
                    field_value_2 = table.Column<string>(nullable: true),
                    field_value_3 = table.Column<string>(nullable: true),
                    field_value_4 = table.Column<string>(nullable: true),
                    field_value_5 = table.Column<string>(nullable: true),
                    field_value_6 = table.Column<string>(nullable: true),
                    field_value_7 = table.Column<string>(nullable: true),
                    field_value_8 = table.Column<string>(nullable: true),
                    field_value_9 = table.Column<string>(nullable: true),
                    field_value_10 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cases", x => x.id);
                    table.ForeignKey(
                        name: "FK_cases_check_lists_check_list_id",
                        column: x => x.check_list_id,
                        principalTable: "check_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cases_workers_done_by_user_id",
                        column: x => x.done_by_user_id,
                        principalTable: "workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cases_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_cases_units_unit_id",
                        column: x => x.unit_id,
                        principalTable: "units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "check_list_sites",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    site_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    microting_uid = table.Column<string>(maxLength: 255, nullable: true),
                    last_check_id = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_check_list_sites", x => x.id);
                    table.ForeignKey(
                        name: "FK_check_list_sites_check_lists_check_list_id",
                        column: x => x.check_list_id,
                        principalTable: "check_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_check_list_sites_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "sites",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "fields",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    parent_field_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    field_type_id = table.Column<int>(nullable: true),
                    mandatory = table.Column<short>(nullable: true),
                    read_only = table.Column<short>(nullable: true),
                    label = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    color = table.Column<string>(maxLength: 255, nullable: true),
                    display_index = table.Column<int>(nullable: true),
                    dummy = table.Column<short>(nullable: true),
                    default_value = table.Column<string>(nullable: true),
                    unit_name = table.Column<string>(maxLength: 255, nullable: true),
                    min_value = table.Column<string>(maxLength: 255, nullable: true),
                    max_value = table.Column<string>(maxLength: 255, nullable: true),
                    max_length = table.Column<int>(nullable: true),
                    decimal_count = table.Column<int>(nullable: true),
                    multi = table.Column<int>(nullable: true),
                    optional = table.Column<short>(nullable: true),
                    selected = table.Column<short>(nullable: true),
                    split_screen = table.Column<short>(nullable: true),
                    geolocation_enabled = table.Column<short>(nullable: true),
                    geolocation_forced = table.Column<short>(nullable: true),
                    geolocation_hidden = table.Column<short>(nullable: true),
                    stop_on_save = table.Column<short>(nullable: true),
                    is_num = table.Column<short>(nullable: true),
                    barcode_enabled = table.Column<short>(nullable: true),
                    barcode_type = table.Column<string>(maxLength: 255, nullable: true),
                    query_type = table.Column<string>(maxLength: 255, nullable: true),
                    key_value_pair_list = table.Column<string>(nullable: true),
                    custom = table.Column<string>(nullable: true),
                    entity_group_id = table.Column<int>(nullable: true),
                    parentid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fields", x => x.id);
                    table.ForeignKey(
                        name: "FK_fields_check_lists_check_list_id",
                        column: x => x.check_list_id,
                        principalTable: "check_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fields_field_types_field_type_id",
                        column: x => x.field_type_id,
                        principalTable: "field_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_fields_fields_parentid",
                        column: x => x.parentid,
                        principalTable: "fields",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "taggings",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    tag_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    tagger_id = table.Column<int>(nullable: true),
                    version = table.Column<int>(nullable: true),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taggings", x => x.id);
                    table.ForeignKey(
                        name: "FK_taggings_check_lists_check_list_id",
                        column: x => x.check_list_id,
                        principalTable: "check_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_taggings_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "field_values",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                       .Annotation(autoIDGenStrategy, autoIDGenStrategyValue),
                    workflow_state = table.Column<string>(maxLength: 255, nullable: true),
                    version = table.Column<int>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: true),
                    updated_at = table.Column<DateTime>(nullable: true),
                    done_at = table.Column<DateTime>(nullable: true),
                    date = table.Column<DateTime>(nullable: true),
                    user_id = table.Column<int>(nullable: true),
                    case_id = table.Column<int>(nullable: true),
                    field_id = table.Column<int>(nullable: true),
                    check_list_id = table.Column<int>(nullable: true),
                    check_list_duplicate_id = table.Column<int>(nullable: true),
                    uploaded_data_id = table.Column<int>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    latitude = table.Column<string>(maxLength: 255, nullable: true),
                    longitude = table.Column<string>(maxLength: 255, nullable: true),
                    altitude = table.Column<string>(maxLength: 255, nullable: true),
                    heading = table.Column<string>(maxLength: 255, nullable: true),
                    accuracy = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_field_values", x => x.id);
                    table.ForeignKey(
                        name: "FK_field_values_check_lists_check_list_id",
                        column: x => x.check_list_id,
                        principalTable: "check_lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_values_fields_field_id",
                        column: x => x.field_id,
                        principalTable: "fields",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_values_uploaded_data_uploaded_data_id",
                        column: x => x.uploaded_data_id,
                        principalTable: "uploaded_data",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_field_values_workers_user_id",
                        column: x => x.user_id,
                        principalTable: "workers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cases_check_list_id",
                table: "cases",
                column: "check_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_cases_done_by_user_id",
                table: "cases",
                column: "done_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cases_site_id",
                table: "cases",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_cases_unit_id",
                table: "cases",
                column: "unit_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_list_sites_check_list_id",
                table: "check_list_sites",
                column: "check_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_list_sites_site_id",
                table: "check_list_sites",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_check_lists_parentid",
                table: "check_lists",
                column: "parentid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_check_lists_tagsid",
            //    table: "check_lists",
            //    column: "tagsid");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_check_list_id",
                table: "field_values",
                column: "check_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_field_id",
                table: "field_values",
                column: "field_id");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_uploaded_data_id",
                table: "field_values",
                column: "uploaded_data_id");

            migrationBuilder.CreateIndex(
                name: "IX_field_values_user_id",
                table: "field_values",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_fields_check_list_id",
                table: "fields",
                column: "check_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_fields_field_type_id",
                table: "fields",
                column: "field_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_fields_parentid",
                table: "fields",
                column: "parentid");

            migrationBuilder.CreateIndex(
                name: "IX_site_workers_site_id",
                table: "site_workers",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "IX_site_workers_worker_id",
                table: "site_workers",
                column: "worker_id");

            migrationBuilder.CreateIndex(
                name: "IX_taggings_check_list_id",
                table: "taggings",
                column: "check_list_id");

            migrationBuilder.CreateIndex(
                name: "IX_taggings_tag_id",
                table: "taggings",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_units_site_id",
                table: "units",
                column: "site_id");

            // MySQL Code - only for MySQL
            //if (!DbConfig.IsMSSQL)
            if (migrationBuilder.ActiveProvider == "Pomelo.EntityFrameworkCore.MySql")
            {
                SetAutoIncrementforTables(migrationBuilder);
            }

        }
        private void SetAutoIncrementforTables(MigrationBuilder migrationBuilder)
        {
            List<string> modelNames = new List<string>();
            modelNames.Add("case_versions");
            modelNames.Add("cases");
            modelNames.Add("field_value_versions");
            modelNames.Add("field_values");
            modelNames.Add("field_versions");
            modelNames.Add("fields");
            modelNames.Add("check_list_site_versions");
            modelNames.Add("check_list_sites");
            modelNames.Add("check_list_value_versions");
            modelNames.Add("check_list_values");
            modelNames.Add("taggings");
            modelNames.Add("tagging_versions");
            modelNames.Add("tags");
            modelNames.Add("tag_versions");
            modelNames.Add("check_list_versions");
            modelNames.Add("check_lists");
            modelNames.Add("entity_group_versions");
            modelNames.Add("entity_groups");
            modelNames.Add("entity_item_versions");
            modelNames.Add("entity_items");
            modelNames.Add("log_exceptions");
            modelNames.Add("logs");
            modelNames.Add("notifications");
            modelNames.Add("settings");
            modelNames.Add("unit_versions");
            modelNames.Add("units");
            modelNames.Add("site_worker_versions");
            modelNames.Add("site_workers");
            modelNames.Add("worker_versions");
            modelNames.Add("workers");
            modelNames.Add("site_versions");
            modelNames.Add("sites");
            modelNames.Add("uploaded_data");
            modelNames.Add("uploaded_data_versions");
            modelNames.Add("field_types");


            migrationBuilder.Sql("SET foreign_key_checks = 0");

            foreach (var modelName in modelNames)
            {

                try
                {

                    string sqlTableAlterCmd = "ALTER TABLE `{0}` MODIFY COLUMN `id` int auto_increment";
                    migrationBuilder.Sql(String.Format(sqlTableAlterCmd, modelName));

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            migrationBuilder.Sql("SET foreign_key_checks = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "case_versions");

            migrationBuilder.DropTable(
                name: "cases");

            migrationBuilder.DropTable(
                name: "check_list_site_versions");

            migrationBuilder.DropTable(
                name: "check_list_sites");

            migrationBuilder.DropTable(
                name: "check_list_value_versions");

            migrationBuilder.DropTable(
                name: "check_list_values");

            migrationBuilder.DropTable(
                name: "check_list_versions");

            migrationBuilder.DropTable(
                name: "entity_group_versions");

            migrationBuilder.DropTable(
                name: "entity_groups");

            migrationBuilder.DropTable(
                name: "entity_item_versions");

            migrationBuilder.DropTable(
                name: "entity_items");

            migrationBuilder.DropTable(
                name: "field_value_versions");

            migrationBuilder.DropTable(
                name: "field_values");

            migrationBuilder.DropTable(
                name: "field_versions");

            migrationBuilder.DropTable(
                name: "log_exceptions");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "site_versions");

            migrationBuilder.DropTable(
                name: "site_worker_versions");

            migrationBuilder.DropTable(
                name: "site_workers");

            migrationBuilder.DropTable(
                name: "tag_versions");

            migrationBuilder.DropTable(
                name: "tagging_versions");

            migrationBuilder.DropTable(
                name: "taggings");

            migrationBuilder.DropTable(
                name: "unit_versions");

            migrationBuilder.DropTable(
                name: "uploaded_data_versions");

            migrationBuilder.DropTable(
                name: "worker_versions");

            migrationBuilder.DropTable(
                name: "units");

            migrationBuilder.DropTable(
                name: "fields");

            migrationBuilder.DropTable(
                name: "uploaded_data");

            migrationBuilder.DropTable(
                name: "workers");

            migrationBuilder.DropTable(
                name: "sites");

            migrationBuilder.DropTable(
                name: "check_lists");

            migrationBuilder.DropTable(
                name: "field_types");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}