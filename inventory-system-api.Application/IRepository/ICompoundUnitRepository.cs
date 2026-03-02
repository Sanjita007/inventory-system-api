using inventory_system_api.Models.Inventory;
using inventory_system_api.Models.System;
using System.Data;

namespace inventory_system_api.IRepository
{
    public interface ICompoundUnitRepository
    {
        /// <summary>
        /// Get all the units or unit list
        /// </summary>
        /// <returns></returns>
        public Task<List<CompoundUnit>> Get();

        /// <summary>
        /// Get Unit by Unit Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CompoundUnit> Get(int id);

        /// <summary>
        /// Add or update the unit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(CompoundUnit entity);

        /// <summary>
        /// Delete a unit based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id);

        /// <summary>
        /// convert from default unit to current unit based on relationship among these units
        /// </summary>
        /// <param name="defaultUnitID"></param>
        /// <param name="currentUnitID"></param>
        /// <param name="valueToConvert"></param>
        /// <returns></returns>
        public Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert);
        public Task<List<UnitDetails>> GetRelatedUnit(int BaseUnitID);
        public Task<List<UnitDetails>> GetMultipleRelatedUnit(string baseUnits);



    }
}
