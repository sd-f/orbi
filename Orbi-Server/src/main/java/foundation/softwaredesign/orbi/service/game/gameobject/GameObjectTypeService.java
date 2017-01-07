package foundation.softwaredesign.orbi.service.game.gameobject;

import foundation.softwaredesign.orbi.model.game.gameobject.GameObjectType;
import foundation.softwaredesign.orbi.persistence.repo.game.gameobject.GameObjectTypeRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.persistence.NoResultException;
import javax.ws.rs.BadRequestException;
import java.util.List;
import java.util.stream.Collectors;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class GameObjectTypeService {

    public static final String GIFT_CHEST_OBJECT_TYPE_PREFAB = "ToonTreasureChest/ToonTreasureChestBlue";

    @Inject
    GameObjectTypeRepository repository;

    public GameObjectType load(Long objectTypeId) {
        try {
            return repository.findBy(objectTypeId);
        } catch (NoResultException ex) {
            throw new BadRequestException("Invalid prefab");
        }
    }

    public List<GameObjectType> loadAll() {
        return repository.findAllOrderByOrdering();
    }

    public List<GameObjectType> loadAllCraftable() {
        //return repository.findAllCraftable(true);
        return loadAll().stream()
                .filter(gameObjectType -> gameObjectType.getCategory().getCraftable())
                .collect(Collectors.toList());
    }

    public GameObjectType loadByPrefab(String prefab) {
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
    public boolean isGiftObject(GameObjectType type) {
        if (isNull(type) || (nonNull(type) && isNull(type)))
            return false;
        return type.getPrefab().equals(GIFT_CHEST_OBJECT_TYPE_PREFAB);
    }
}
