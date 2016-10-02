package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import foundation.softwaredesign.orbi.persistence.repo.GameObjectTypeRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.persistence.NoResultException;
import javax.ws.rs.BadRequestException;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class GameObjectTypeService {

    @Inject
    GameObjectTypeRepository repository;

    public GameObjectTypeEntity load(Long objectTypeId) {
        try {
            return repository.findBy(objectTypeId);
        } catch (NoResultException ex) {
            throw new BadRequestException("Invalid prefab");
        }
    }

    public List<GameObjectTypeEntity> loadAll() {
        return repository.findAll();
    }

    public GameObjectTypeEntity loadByPrefab(String prefab) {
        try {
            return repository.findByPrefab(prefab);
        } catch (NoResultException ex) {
            throw new BadRequestException("Invalid prefab");
        }
    }
}
