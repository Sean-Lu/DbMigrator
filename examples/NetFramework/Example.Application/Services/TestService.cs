using Example.Application.Contracts;
using Example.Domain.Contracts;
using Example.Domain.Entities;

namespace Example.Application.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;

        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public bool Add(TestEntity model)
        {
            return _testRepository.Add(model);
        }

        public bool Delete(long id)
        {
            return _testRepository.Delete(entity => entity.Id == id) > 0;
        }

        public TestEntity Get(long id)
        {
            return _testRepository.Get(entity => entity.Id == id);
        }
    }
}
