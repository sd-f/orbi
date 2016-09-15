package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.*;
import foundation.softwaredesign.orbi.persistence.entity.GameObjectTypeEntity;
import foundation.softwaredesign.orbi.persistence.entity.InventoryEntity;
import foundation.softwaredesign.orbi.persistence.repo.InventoryRepository;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.NotFoundException;
import java.util.Objects;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class InventoryService {

    private static final Long ALWAYS_RESTOCK_OBJECT_TYPE_ID = new Long(1);
    @Inject
    InventoryRepository repository;
    @Inject
    UserService userService;
    @Inject
    GameObjectTypeService gameObjectType;

    public void checkRestock() {
        InventoryEntity inventoryEntity =
                repository.findByIdentAndType(userService.getIdentity().getId(),
                        ALWAYS_RESTOCK_OBJECT_TYPE_ID);

        if (Objects.isNull(inventoryEntity) || (Objects.nonNull(inventoryEntity) && inventoryEntity.getAmount().longValue() <= 0)) {
            restock(inventoryEntity);
        }
    }

    private void restock(InventoryEntity inventoryEntity) {
        InventoryEntity inventoryEntityRestocked = inventoryEntity;
        if (Objects.isNull(inventoryEntityRestocked)) {
            inventoryEntityRestocked = new InventoryEntity();
            inventoryEntityRestocked.setGameObjectType(gameObjectType.load(ALWAYS_RESTOCK_OBJECT_TYPE_ID));
            inventoryEntityRestocked.setIdentity(userService.getIdentity());
            inventoryEntityRestocked.setAmount(new Long(0));
        }
        inventoryEntityRestocked.setAmount(inventoryEntityRestocked.getAmount() + 100);
        repository.saveAndFlush(inventoryEntityRestocked);
    }

    public Inventory getInventory() {
        Inventory inventory = new Inventory();
        for (InventoryEntity inventoryEntity: repository.findByIdentityId(userService.getIdentity().getId())) {
            inventory.getItems().add(new InventoryItem(inventoryEntity.getGameObjectType().getPrefab(), inventoryEntity.getAmount()));
        }
        return inventory;
    }

    public void use(GameObject gameObject) {
        InventoryEntity toUse = getInventoryItemsByPrefab(gameObject.getPrefab());
        if (Objects.isNull(toUse) || (Objects.nonNull(toUse) && (toUse.getAmount().longValue() <= 0))) {
            throw new NotFoundException("Item not in inventory");
        }
        toUse.setAmount(toUse.getAmount() - 1);
        repository.save(toUse);
    }

    private InventoryEntity getInventoryItemsByPrefab(String prefab) {
        GameObjectTypeEntity objectType = gameObjectType.loadByPrefab(prefab);
        InventoryEntity inventoryItems =
                repository.findByIdentAndType(userService.getIdentity().getId(), objectType.getId());
        return inventoryItems;
    }
}
