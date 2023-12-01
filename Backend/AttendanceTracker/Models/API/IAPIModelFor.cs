namespace AttendanceTracker.Models.API
{
    public interface IAPIModelFor<T,DbType> where DbType:class
    {
        public T ConvertToAPI(DbCtx databaseContext, DbType entity);
    }
}
