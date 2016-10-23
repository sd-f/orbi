package foundation.softwaredesign.orbi.service.game.gameobject;

import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectTypeRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.persistence.NoResultException;
import javax.ws.rs.BadRequestException;
import java.util.List;
import java.util.stream.Collectors;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class GameObjectTypeService {

    public static final String GIFT_CHEST_OBJECT_TYPE_PREFAB = "ToonTreasureChest/ToonTreasureChestBlue";

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

    public List<GameObjectTypeEntity> loadAllCraftable() {
        //return repository.findAllCraftable(true);
        return loadAll().stream().filter(gameObjectTypeEntity -> gameObjectTypeEntity.getGameObjectTypeCategoryEntity().getCraftable()).collect(Collectors.toList());
    }

    public GameObjectTypeEntity loadByPrefab(String prefab) {
        try {
            return repository.findByPrefab(prefab);
        } catch (NoResultException ex) {
            throw new BadRequestException("Invalid prefab");
        }
    }

    public Long getNumberOfObjectTypes() {
        return repository.count();
    }

    // TODO contains, not only one item
    public boolean isGiftObject(String prefab) {
        return prefab.equals(GIFT_CHEST_OBJECT_TYPE_PREFAB);
    }
}
