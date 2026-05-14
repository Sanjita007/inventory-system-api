using inventory_system_api.Application.Models.Inventory;

namespace inventory_system_api.Application.IRepository
{
    public interface IUnitRepository
    {
        /// <summary>
        /// Get all the units or unit list
        /// </summary>
        /// <returns></returns>
        public Task<List<Unit>> Get(CancellationToken cancellationToken);

        /// <summary>
        /// Get Unit by Unit Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Unit> Get(int id, CancellationToken cancellationToken);

        /// <summary>
        /// Add or update the unit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> AddEdit(Unit entity, CancellationToken cancellationToken, int userId);

        /// <summary>
        /// Delete a unit based on the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id, CancellationToken cancellationToken, int userId);

        /// <summary>
        /// convert from default unit to current unit based on relationship among these units
        /// </summary>
        /// <param name="defaultUnitID"></param>
        /// <param name="currentUnitID"></param>
        /// <param name="valueToConvert"></param>
        /// <returns></returns>
        public Task<decimal?> ConvertUnit(int defaultUnitID, int currentUnitID, decimal valueToConvert, CancellationToken cancellationToken);
        public Task<List<UnitDetails>> GetRelatedUnit(int BaseUnitID, CancellationToken cancellationToken);
        public Task<List<UnitDetails>> GetMultipleRelatedUnit(string baseUnits, CancellationToken cancellationToken);



    }
}
