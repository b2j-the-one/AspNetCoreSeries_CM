using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllOwners()
        {
            try
            {
                var owners = _repository.Owner.GetAllOwners();
                _logger.LogInfo($"Renvoi de tous les propriétaires de la base de données.");

                var ownersResult = _mapper.Map<IEnumerable<OwnerDto>>(owners);
                return Ok(ownersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un problème s'est produit dans l'action GetAllOwners : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);

                if (owner is null)
                {
                    _logger.LogInfo($"Le propriétaire dont l'identifiant est {id} n'a pas été trouvé dans la base de données.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Retourne le propriétaire avec l'identifiant : {id}");

                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un problème s'est produit dans l'action GetOwnerById : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpGet("{id}/account")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerWithDetails(id);

                if (owner == null)
                {
                    _logger.LogInfo($"Le propriétaire dont l'identifiant est {id} n'a pas été trouvé dans la base de données.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Retourne le propriétaire avec les détails pour l'identifiant : {id}");

                    var ownerResult = _mapper.Map<OwnerDto>(owner);
                    return Ok(ownerResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un problème s'est produit dans l'action GetOwnerWithDetails : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }
    }
}
