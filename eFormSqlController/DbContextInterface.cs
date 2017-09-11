using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormSqlController
{
    interface DbContextInterface : IDisposable
    {
        DbSet<a_interaction_cases> a_interaction_cases { get; set; }
        DbSet<a_interaction_case_versions> a_interaction_case_versions { get; set; }
        DbSet<a_interaction_case_lists> a_interaction_case_lists { get; set; }
        DbSet<a_interaction_case_list_versions> a_interaction_case_list_versions { get; set; }
        DbSet<cases> cases { get; set; }
        DbSet<check_list_sites> check_list_sites { get; set; }
        DbSet<check_list_values> check_list_values { get; set; }
        DbSet<check_lists> check_lists { get; set; }
        DbSet<uploaded_data> data_uploaded { get; set; }
        DbSet<entity_groups> entity_groups { get; set; }
        DbSet<entity_items> entity_items { get; set; }
        DbSet<field_types> field_types { get; set; }
        DbSet<field_values> field_values { get; set; }
        DbSet<fields> fields { get; set; }
        DbSet<log_exceptions> log_exceptions { get; set; }
        DbSet<logs> logs { get; set; }
        DbSet<notifications> notifications { get; set; }
        DbSet<settings> settings { get; set; }
        DbSet<sites> sites { get; set; }
        DbSet<units> units { get; set; }
        DbSet<workers> workers { get; set; }
        DbSet<site_workers> site_workers { get; set; }
        DbSet<case_versions> version_cases { get; set; }
        DbSet<check_list_site_versions> version_check_list_sites { get; set; }
        DbSet<check_list_value_versions> version_check_list_values { get; set; }
        DbSet<check_list_versions> version_check_lists { get; set; }
        DbSet<uploaded_data_versions> version_data_uploaded { get; set; }
        DbSet<entity_group_versions> version_entity_groups { get; set; }
        DbSet<entity_item_versions> version_entity_items { get; set; }
        DbSet<field_value_versions> version_field_values { get; set; }
        DbSet<field_versions> version_fields { get; set; }
        DbSet<site_versions> version_sites { get; set; }
        DbSet<unit_versions> version_units { get; set; }
        DbSet<worker_versions> version_workers { get; set; }
        DbSet<site_worker_versions> version_site_workers { get; set; }

        int SaveChanges();

        Database Database { get; }
    }
}