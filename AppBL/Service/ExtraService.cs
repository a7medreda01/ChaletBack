using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBL.Service
{
    using AppBL.DTOs;
    using AppBL.IService;
    using AppDAL.Entities;
    using AppDAL.IRepo;
    using AutoMapper;

    public class ExtraService : IExtraService
    {
        private readonly IGenericRepository<Extra> _repo;
        private readonly IMapper _mapper;

        public ExtraService(IGenericRepository<Extra> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExtraDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ExtraDto>>(data);
        }

        public async Task<ExtraDto> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Extra not found");

            return _mapper.Map<ExtraDto>(entity);
        }

        public async Task<ExtraDto> CreateAsync(CreateExtraDto dto)
        {
            var entity = _mapper.Map<Extra>(dto);

            entity.IsActive = true; // default

            await _repo.AddAsync(entity);
            await _repo.SaveAsync();

            return _mapper.Map<ExtraDto>(entity);
        }

        public async Task<ExtraDto> UpdateAsync(int id, UpdateExtraDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Extra not found");

            _mapper.Map(dto, entity);

            _repo.Update(entity);
            await _repo.SaveAsync();

            return _mapper.Map<ExtraDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Extra not found");

            _repo.Delete(entity);
            await _repo.SaveAsync();
        }
    }
}
