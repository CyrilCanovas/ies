using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.IO;
using IQToolkit;
using IQToolkit.Data;
using SageClassLibrary.DataModel;
using IQToolkit.Data.Common;
using IQToolkit.Data.Odbc;
using IQToolkit.Data.SqlClient;
using System.Data.Odbc;

namespace SageClassLibrary
{
    
    public class SageDataContext
    {
        public IEntityTable<Warehouse> Warehouses;
        public IEntityTable<Location> Locations;
        public IEntityTable<SalesUnit> SaleUnits;
        public IEntityTable<Item> Items;
        public IEntityTable<LotSerial> LotSerials;
        public IEntityTable<ItemInventory> ItemInventories;
        public IEntityTable<DocHeader> DocHeaders;
        public IEntityTable<DocLine> DocLines;
        public IEntityTable<DocNumManager> DocNumManagers;
        public IEntityTable<ShipToAddress> ShipToAddresses;
        public IEntityTable<CustomerVendor> CustomersVendors;
        public IEntityTable<ShippingAgent> ShippingAgents;
        public IEntityTable<ItemCustomer> ItemsCustomers;
        public IEntityTable<Memo> Memos;
        public IEntityTable<Country> Countries;
        public IEntityTable<ItemMemo> ItemMemos;
        public IEntityTable<GroupProductAccountant> GroupsProductAccountant;
        public IEntityTable<ItemAccountant> ItemsAccountant;
        public IEntityTable<Tax> Taxes;
        public IEntityTable<ItemMedia> ItemsMedias;
        public IEntityTable<IesRisque> IesRisques;
        public IEntityTable<ItemVendor> ItemVendors;

        private IEntityProvider provider;

        //public static QueryMapping StandardPolicyWithOdbc = new CustomMapping(new OdbcSqlLanguage());
        //public static QueryMapping StandardPolicyWithSql = new CustomMapping(new TSqlLanguage());

        //public SageDataContext(DbConnection connection, TextWriter log)
        //    : this(connection, log, StandardPolicyWithOdbc)
        //{
        //}
        //public SageDataContext(DbConnection connection, TextWriter log, EnumAccessibility accessibility )
        //    : this(connection, log, accessibility == EnumAccessibility.OnlyOdbc ? StandardPolicyWithOdbc : StandardPolicyWithSql)
        //{
        
        //}

        //public SageDataContext(DbConnection connection, TextWriter log, QueryMapping mapping)
        //    : this(new OdbcQueryProvider(connection as OdbcConnection, mapping,new QueryPolicy()))
        //{
        //}
        public static IEntityTable<T> GetEntityTable<T>(DbEntityProvider provider)
        {
            Type type = typeof(T);
            string entityId = RepositoryClassDefinition.Current.GetByClassId(type).TableID;
            var table = provider.GetTable<T>(entityId);
            return (IEntityTable<T>)table;
        }
        public IEntityTable<T> GetEntityTable<T>()
        {

            return (IEntityTable<T>)GetEntityTable<T>(this.Provider as DbEntityProvider);
        }

        public SageDataContext(IEntityProvider provider)
        {
            this.provider = provider;
            this.ItemVendors = GetEntityTable<ItemVendor>();
            this.Warehouses = GetEntityTable<Warehouse>();
            this.Locations = GetEntityTable<Location>();
            this.SaleUnits = GetEntityTable<SalesUnit>();
            this.LotSerials = GetEntityTable<LotSerial>();
            this.Items = GetEntityTable<Item>();
            this.ItemInventories = GetEntityTable<ItemInventory>();
            this.DocHeaders = GetEntityTable<DocHeader>();
            this.DocLines = GetEntityTable<DocLine>();
            this.DocNumManagers = GetEntityTable<DocNumManager>();
            this.ShippingAgents = GetEntityTable<ShippingAgent>();
            this.ShipToAddresses = GetEntityTable<ShipToAddress>();
            this.CustomersVendors = GetEntityTable<CustomerVendor>();
            this.ItemsCustomers = GetEntityTable<ItemCustomer>();
            this.Memos = GetEntityTable<Memo>();

            this.GroupsProductAccountant = GetEntityTable<GroupProductAccountant>();
            this.ItemsAccountant =GetEntityTable<ItemAccountant>();
            this.Taxes = GetEntityTable<Tax>();
            this.Countries = GetEntityTable<Country>();
            this.ItemMemos = GetEntityTable<ItemMemo>();
            this.ItemsMedias = GetEntityTable<ItemMedia>();
            this.IesRisques = GetEntityTable<IesRisque>();
        }

        public IQueryProvider Provider
        {
            get { return this.provider; }
        }

    }
}
