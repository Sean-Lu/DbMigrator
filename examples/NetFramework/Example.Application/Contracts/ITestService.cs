using Example.Domain.Entities;

namespace Example.Application.Contracts
{
    public interface ITestService
    {
        bool Add(TestEntity model);
        bool Delete(long id);
        TestEntity Get(long id);
    }
}