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
        DbSet<uploaded_data> uploaded_data { get; set; }
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
        DbSet<case_versions> case_versions { get; set; }
        DbSet<check_list_site_versions> check_list_site_versions { get; set; }
        DbSet<check_list_value_versions> check_list_value_versions { get; set; }
        DbSet<check_list_versions> check_list_versions { get; set; }
        DbSet<uploaded_data_versions> uploaded_data_versions { get; set; }
        DbSet<entity_group_versions> entity_group_versions { get; set; }
        DbSet<entity_item_versions> entity_item_versions { get; set; }
        DbSet<field_value_versions> field_value_versions { get; set; }
        DbSet<field_versions> field_versions { get; set; }
        DbSet<site_versions> site_versions { get; set; }
        DbSet<unit_versions> unit_versions { get; set; }
        DbSet<worker_versions> worker_versions { get; set; }
        DbSet<site_worker_versions> site_worker_versions { get; set; }

        int SaveChanges();

        Database Database { get; }
    }
}