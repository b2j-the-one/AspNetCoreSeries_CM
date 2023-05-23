using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
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

        [HttpGet("{id}", Name = "OwnerById")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);

                if (owner is null)
                {
                    _logger.LogError($"Le propriétaire dont l'identifiant est {id} n'a pas été trouvé dans la base de données.");
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
                    _logger.LogError($"Le propriétaire dont l'identifiant est {id} n'a pas été trouvé dans la base de données.");
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

        [HttpPost]
        public IActionResult CreateOwner([FromBody] OwnerForCreationDto owner)
        {
            try
            {
                if (owner is null)
                {
                    _logger.LogError("L'objet propriétaire envoyé par le client est nul.");
                    return BadRequest("L'objet propriétaire est nul");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Objet propriétaire invalide envoyé par le client.");
                    return BadRequest("Objet de modèle non valide");
                }

                var ownerEntity = _mapper.Map<Owner>(owner);

                _repository.Owner.CreateOwner(ownerEntity);
                _repository.Save();

                var createdOwner = _mapper.Map<OwnerDto>(ownerEntity);

                return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un problème s'est produit dans l'action CreateOwner : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOwner(Guid id, [FromBody] OwnerForUpdateDto owner)
        {
            try
            {
                if (owner is null)
                {
                    _logger.LogInfo("L'objet propriétaire envoyé par le client est nul.");
                    return BadRequest("L'objet propriétaire est nul");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Objet propriétaire invalide envoyé par le client.");
                    return BadRequest("Objet de modèle non valide");
                }

                var ownerEntity = _repository.Owner.GetOwnerById(id);
                if (ownerEntity is null)
                {
                    _logger.LogError($"Le propriétaire dont l'identifiant est {id} n'a pas été trouvé dans la base de données.");
                    return NotFound();
                }

                _mapper.Map(owner, ownerEntity);

                _repository.Owner.UpdateOwner(ownerEntity);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un problème s'est produit dans l'action UpdateOwner : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);
                if (owner == null)
                {
                    _logger.LogError($"Le propriétaire dont l'identifiant est {id} n'a pas été trouvé dans la base de données.");
                    return NotFound();
                }

                if (_repository.Account.AccountsByOwner(id).Any())
                {
                    _logger.LogError($"Impossible de supprimer le propriétaire avec l'identifiant : {id}. Il a des comptes liés. Supprimez d'abord ces comptes");
                    return BadRequest("Impossible de supprimer le propriétaire. Il a des comptes liés. Supprimez d'abord ces comptes");
                }

                _repository.Owner.DeleteOwner(owner);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Un problème s'est produit dans l'action UpdateOwner : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }
    }
}
